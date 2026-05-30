using BLL;
using DAL;
using ENTITY;
using Oracle.ManagedDataAccess.Client;

namespace BotApi.Services;

public class TelegramVinculoService
{
    private readonly UsuarioServicio _usuarios;

    public TelegramVinculoService(UsuarioServicio usuarios)
    {
        _usuarios = usuarios;
    }

    // Crea una solicitud pendiente para que el usuario confirme desde Casino Royal.
    public VinculoResultado SolicitarVinculo(string chatId, string username)
    {
        Usuario? usuario;
        try
        {
            usuario = _usuarios.ObtenerTodos()
                .FirstOrDefault(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));
        }
        catch (Exception ex)
        {
            return VinculoResultado.Error($"No se pudo consultar Oracle: {ex.Message}");
        }

        if (usuario is null)
            return VinculoResultado.Error("Usuario no encontrado.");

        string codigo = Random.Shared.Next(100000, 999999).ToString();
        DateTime expira = DateTime.Now.AddMinutes(10);

        using OracleConnection cn = ConexionOracle.Abrir();
        using OracleTransaction tx = cn.BeginTransaction();

        try
        {
            // Cancela solicitudes anteriores para evitar varios codigos activos del mismo chat o usuario.
            using (OracleCommand cancelar = cn.CreateCommand())
            {
                cancelar.Transaction = tx;
                cancelar.CommandText = @"
                    UPDATE telegram_vinculos
                       SET estado = 'CANCELADO',
                           fecha_cancelacion = SYSTIMESTAMP
                     WHERE estado = 'PENDIENTE'
                       AND (chat_id_telegram = :chat_id OR id_usuario = :id_usuario)";
                cancelar.Parameters.Add(":chat_id", chatId);
                cancelar.Parameters.Add(":id_usuario", usuario.IdUsuario);
                cancelar.ExecuteNonQuery();
            }

            using (OracleCommand insertar = cn.CreateCommand())
            {
                insertar.Transaction = tx;
                insertar.CommandText = @"
                    INSERT INTO telegram_vinculos (
                        id_vinculo, chat_id_telegram, id_usuario, username_casino,
                        codigo, estado, fecha_creacion, fecha_expiracion
                    ) VALUES (
                        seq_telegram_vinculos.NEXTVAL, :chat_id, :id_usuario, :username,
                        :codigo, 'PENDIENTE', SYSTIMESTAMP, :fecha_expiracion
                    )";
                insertar.Parameters.Add(":chat_id", chatId);
                insertar.Parameters.Add(":id_usuario", usuario.IdUsuario);
                insertar.Parameters.Add(":username", usuario.Username);
                insertar.Parameters.Add(":codigo", codigo);
                insertar.Parameters.Add(":fecha_expiracion", expira);
                insertar.ExecuteNonQuery();
            }

            tx.Commit();
            return VinculoResultado.Exito("Solicitud creada. Abre Casino Royal en Soporte para confirmar.", codigo, usuario.IdUsuario);
        }
        catch (Exception ex)
        {
            tx.Rollback();
            return VinculoResultado.Error($"Error al crear vinculo: {ex.Message}");
        }
    }

    // Busca el codigo pendiente que la vista Soporte debe mostrar al usuario logueado.
    public VinculoPendiente? ObtenerPendientePorUsuario(int idUsuario)
    {
        using OracleConnection cn = ConexionOracle.Abrir();
        using OracleCommand cmd = cn.CreateCommand();
        cmd.CommandText = @"
            SELECT id_vinculo, chat_id_telegram, id_usuario, username_casino, codigo,
                   estado, fecha_creacion, fecha_expiracion
              FROM telegram_vinculos
             WHERE id_usuario = :id_usuario
               AND estado = 'PENDIENTE'
               AND fecha_expiracion > SYSTIMESTAMP
             ORDER BY fecha_creacion DESC
             FETCH FIRST 1 ROWS ONLY";
        cmd.Parameters.Add(":id_usuario", idUsuario);

        using OracleDataReader r = cmd.ExecuteReader();
        if (!r.Read()) return null;

        return new VinculoPendiente(
            r.GetInt32(0),
            r.GetString(1),
            r.GetInt32(2),
            r.GetString(3),
            r.GetString(4),
            r.GetString(5),
            r.GetDateTime(6),
            r.GetDateTime(7));
    }

    // Confirma el codigo desde Casino Royal y deja ese chat autorizado para consultar datos.
    public VinculoResultado ConfirmarVinculo(int idUsuario, string codigo)
    {
        if (string.IsNullOrWhiteSpace(codigo))
            return VinculoResultado.Error("Debes enviar el codigo.");

        using OracleConnection cn = ConexionOracle.Abrir();
        using OracleCommand cmd = cn.CreateCommand();
        cmd.CommandText = @"
            UPDATE telegram_vinculos
               SET estado = 'VINCULADO',
                   fecha_confirmacion = SYSTIMESTAMP
             WHERE id_usuario = :id_usuario
               AND codigo = :codigo
               AND estado = 'PENDIENTE'
               AND fecha_expiracion > SYSTIMESTAMP";
        cmd.Parameters.Add(":id_usuario", idUsuario);
        cmd.Parameters.Add(":codigo", codigo);

        int filas = cmd.ExecuteNonQuery();
        return filas > 0
            ? VinculoResultado.Exito("Cuenta vinculada correctamente.", codigo, idUsuario)
            : VinculoResultado.Error("Codigo invalido o expirado.");
    }

    // Resuelve el usuario asociado a un chat de Telegram para comandos privados como /saldo.
    public int? ObtenerUsuarioVinculado(string chatId)
    {
        using OracleConnection cn = ConexionOracle.Abrir();
        using OracleCommand cmd = cn.CreateCommand();
        cmd.CommandText = @"
            SELECT id_usuario
              FROM telegram_vinculos
             WHERE chat_id_telegram = :chat_id
               AND estado = 'VINCULADO'
             ORDER BY fecha_confirmacion DESC
             FETCH FIRST 1 ROWS ONLY";
        cmd.Parameters.Add(":chat_id", chatId);

        object? valor = cmd.ExecuteScalar();
        if (valor is null || valor == DBNull.Value) return null;
        return Convert.ToInt32(valor);
    }

    // Cierra la sesion del bot cancelando el vinculo activo de ese chat de Telegram.
    public VinculoResultado CerrarSesionTelegram(string chatId)
    {
        if (string.IsNullOrWhiteSpace(chatId))
            return VinculoResultado.Error("No se recibio el chat de Telegram.");

        using OracleConnection cn = ConexionOracle.Abrir();
        using OracleCommand cmd = cn.CreateCommand();
        cmd.CommandText = @"
            UPDATE telegram_vinculos
               SET estado = 'CANCELADO',
                   fecha_cancelacion = SYSTIMESTAMP
             WHERE chat_id_telegram = :chat_id
               AND estado = 'VINCULADO'";
        cmd.Parameters.Add(":chat_id", chatId);

        int filas = cmd.ExecuteNonQuery();
        return filas > 0
            ? VinculoResultado.Exito("Sesion cerrada correctamente. Para usar datos privados de nuevo escribe /vincular tu_usuario.")
            : VinculoResultado.Error("No habia una sesion activa para cerrar.");
    }
}

public record VinculoResultado(bool Ok, string Mensaje, string? Codigo = null, int? IdUsuario = null)
{
    public static VinculoResultado Exito(string mensaje, string? codigo = null, int? idUsuario = null)
        => new(true, mensaje, codigo, idUsuario);

    public static VinculoResultado Error(string mensaje)
        => new(false, mensaje);
}

public record VinculoPendiente(
    int IdVinculo,
    string ChatIdTelegram,
    int IdUsuario,
    string UsernameCasino,
    string Codigo,
    string Estado,
    DateTime FechaCreacion,
    DateTime FechaExpiracion);
