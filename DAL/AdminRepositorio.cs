using ENTITY;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class AdminRepositorio : OracleBase<ResumenAdmin>
    {
        public override IList<ResumenAdmin> Consultar()
        {
            var r = ObtenerResumenGeneral();
            return r != null ? new List<ResumenAdmin> { r } : new List<ResumenAdmin>();
        }

        public override string Guardar(ResumenAdmin e) => throw new NotSupportedException();

        public ResumenAdmin ObtenerResumenGeneral()
        {
            string sql = @"WITH u_na AS (SELECT * FROM usuarios WHERE id_rol != 1),
p_na AS (SELECT p.* FROM partidas p
    WHERE NOT EXISTS (SELECT 1 FROM usuarios WHERE id_usuario = p.id_usuario AND id_rol = 1)),
t_na AS (SELECT t.* FROM transacciones t
    WHERE NOT EXISTS (SELECT 1 FROM usuarios WHERE id_usuario = t.id_usuario AND id_rol = 1)),
ea AS (SELECT id_estado FROM estado_usuario WHERE nombre = 'activo'),
hoy AS (SELECT TRUNC(SYSDATE) AS d FROM DUAL)
SELECT
(SELECT COUNT(*) FROM u_na),
(SELECT COUNT(*) FROM u_na WHERE id_estado = (SELECT id_estado FROM ea)),
(SELECT COUNT(*) FROM p_na),
(SELECT COUNT(*) FROM p_na WHERE fecha>=(SELECT d FROM hoy) AND fecha<(SELECT d+1 FROM hoy)),
ROUND((SELECT NVL(SUM(apuesta-ganancia),0) FROM p_na),2),
ROUND((SELECT NVL(SUM(apuesta-ganancia),0) FROM p_na WHERE fecha>=(SELECT d FROM hoy) AND fecha<(SELECT d+1 FROM hoy)),2),
ROUND((SELECT NVL(SUM(monto),0) FROM t_na WHERE tipo='deposito'),2),
ROUND((SELECT NVL(SUM(monto),0) FROM t_na WHERE tipo='deposito' AND fecha>=(SELECT d FROM hoy) AND fecha<(SELECT d+1 FROM hoy)),2),
ROUND((SELECT NVL(AVG(apuesta),0) FROM p_na),4),
ROUND((SELECT NVL(AVG(apuesta),0) FROM p_na WHERE fecha>=(SELECT d FROM hoy) AND fecha<(SELECT d+1 FROM hoy)),4),
ROUND((SELECT NVL(SUM(apuesta),0) FROM p_na WHERE fecha>=(SELECT d FROM hoy) AND fecha<(SELECT d+1 FROM hoy)),2),
(SELECT nombre FROM (SELECT j.nombre,COUNT(*)cnt FROM p_na p JOIN juegos j ON p.id_juego=j.id_juego GROUP BY j.id_juego,j.nombre ORDER BY cnt DESC) WHERE ROWNUM=1),
(SELECT cnt FROM (SELECT COUNT(*)cnt FROM p_na GROUP BY id_juego ORDER BY cnt DESC) WHERE ROWNUM=1),
(SELECT nombre FROM (SELECT u.nombre_1||' '||u.apellido_1 nombre,COUNT(*)cnt FROM p_na p JOIN usuarios u ON p.id_usuario=u.id_usuario GROUP BY u.id_usuario,u.nombre_1,u.apellido_1 ORDER BY cnt DESC) WHERE ROWNUM=1),
(SELECT cnt FROM (SELECT COUNT(*)cnt FROM p_na GROUP BY id_usuario ORDER BY cnt DESC) WHERE ROWNUM=1),
(SELECT COUNT(*) FROM u_na WHERE id_estado = (SELECT id_estado FROM ea))
FROM DUAL";
            using (OracleDataReader reader = EjecutarConsulta(sql))
            {
                if (!reader.Read()) return null;
                return new ResumenAdmin
                {
                    TotalUsuarios = reader.GetInt32(0),
                    UsuariosActivos = reader.GetInt32(1),
                    PartidasTotal = reader.GetInt32(2),
                    PartidasHoy = reader.GetInt32(3),
                    GananciaCasaTotal = reader.GetDecimal(4),
                    GananciaCasaHoy = reader.GetDecimal(5),
                    IngresosTotal = reader.GetDecimal(6),
                    IngresosHoy = reader.GetDecimal(7),
                    PromedioApuesta = reader.GetDecimal(8),
                    PromedioApuestaHoy = reader.GetDecimal(9),
                    TotalApostadoHoy = reader.GetDecimal(10),
                    JuegoMasJugado = reader.IsDBNull(11) ? "N/A" : reader.GetString(11),
                    PartidasJuegoMasJugado = reader.GetInt32(12),
                    UsuarioMasActivo = reader.IsDBNull(13) ? "N/A" : reader.GetString(13),
                    PartidasUsuarioMasActivo = reader.GetInt32(14),
                    UsuariosActivosHoy = reader.GetInt32(15)
                };
            }
        }

        public IList<EstadisticasUsuario> ObtenerTopJugadores(int cantidad)
        {
            var lista = new List<EstadisticasUsuario>();
            string sql = @"SELECT * FROM (SELECT u.id_usuario,u.username,u.nombre_1||' '||u.apellido_1 nombre_completo,u.saldo,
COUNT(p.id_partida) total_partidas,
SUM(CASE WHEN p.id_estado=2 THEN 1 ELSE 0 END) partidas_ganadas,
SUM(CASE WHEN p.id_estado=3 THEN 1 ELSE 0 END) partidas_perdidas,
NVL(SUM(p.apuesta),0) total_apostado,NVL(SUM(p.ganancia),0) total_ganado,
NVL(SUM(p.ganancia),0)-NVL(SUM(p.apuesta),0) ganancia_neta
FROM usuarios u JOIN partidas p ON u.id_usuario=p.id_usuario
WHERE u.id_rol!=1
GROUP BY u.id_usuario,u.username,u.nombre_1,u.apellido_1,u.saldo
ORDER BY total_apostado DESC) WHERE ROWNUM<=:cantidad";
            using (OracleDataReader reader = EjecutarConsulta(sql, new[] { (":cantidad", (object)cantidad) }))
                while (reader.Read())
                    lista.Add(new EstadisticasUsuario
                    {
                        IdUsuario = reader.GetInt32(0),
                        Username = reader.GetString(1),
                        NombreUsuario = reader.GetString(2),
                        Saldo = reader.GetDecimal(3),
                        TotalPartidas = reader.GetInt32(4),
                        PartidasGanadas = reader.GetInt32(5),
                        PartidasPerdidas = reader.GetInt32(6),
                        TotalApostado = reader.GetDecimal(7),
                        TotalGanado = reader.GetDecimal(8),
                        GananciaNeta = reader.GetDecimal(9)
                    });
            return lista;
        }

        public IList<(string Juego, int Partidas, decimal TotalApostado, decimal GananciaCasa, decimal Margen)> ObtenerRentabilidadPorJuego()
        {
            var lista = new List<(string, int, decimal, decimal, decimal)>();
            string sql = @"SELECT j.nombre,COUNT(p.id_partida),NVL(SUM(p.apuesta),0),NVL(SUM(p.apuesta-p.ganancia),0),
ROUND(CASE WHEN NVL(SUM(p.apuesta),0)>0 THEN (SUM(p.apuesta-p.ganancia)/SUM(p.apuesta))*100 ELSE 0 END,2)
FROM juegos j LEFT JOIN partidas p ON j.id_juego=p.id_juego
AND NOT EXISTS (SELECT 1 FROM usuarios WHERE id_usuario=p.id_usuario AND id_rol=1)
GROUP BY j.id_juego,j.nombre ORDER BY 4 DESC";
            using (OracleDataReader reader = EjecutarConsulta(sql))
                while (reader.Read())
                    lista.Add((
                        reader.GetString(0),
                        reader.GetInt32(1),
                        reader.GetDecimal(2),
                        reader.GetDecimal(3),
                        reader.GetDecimal(4)
                    ));
            return lista;
        }

        public Dictionary<string, int> ObtenerPartidasPorJuego()
        {
            return ObtenerRentabilidadPorJuego()
                .ToDictionary(x => x.Juego, x => x.Partidas);
        }

        public IList<Partida> ObtenerPartidasRecientes(int cantidad)
        {
            var lista = new List<Partida>();
            string sql = @"SELECT * FROM (SELECT p.* FROM partidas p
WHERE NOT EXISTS (SELECT 1 FROM usuarios WHERE id_usuario=p.id_usuario AND id_rol=1)
ORDER BY p.fecha DESC) WHERE ROWNUM<=:cantidad";
            using (OracleDataReader reader = EjecutarConsulta(sql, new[] { (":cantidad", (object)cantidad) }))
                while (reader.Read())
                    lista.Add(new Partida
                    {
                        IdPartida = reader.GetInt32(0),
                        IdUsuario = reader.GetInt32(1),
                        IdJuego = reader.GetInt32(2),
                        IdEstado = reader.GetInt32(3),
                        Fecha = reader.GetDateTime(4),
                        Apuesta = reader.GetDecimal(5),
                        Ganancia = reader.GetDecimal(6)
                    });
            return lista;
        }

        public IList<(int IdUsuario, decimal TotalDepositado, int NumDepositos)> ObtenerTopDepositantes(int cantidad)
        {
            var lista = new List<(int, decimal, int)>();
            string sql = @"SELECT * FROM (SELECT u.id_usuario,u.nombre_1||' '||u.apellido_1,u.username,
NVL(SUM(t.monto),0),COUNT(t.id_transaccion)
FROM usuarios u JOIN transacciones t ON u.id_usuario=t.id_usuario
WHERE u.id_rol!=1 AND t.tipo='deposito'
GROUP BY u.id_usuario,u.nombre_1,u.apellido_1,u.username
ORDER BY 4 DESC) WHERE ROWNUM<=:cantidad";
            using (OracleDataReader reader = EjecutarConsulta(sql, new[] { (":cantidad", (object)cantidad) }))
                while (reader.Read())
                    lista.Add((reader.GetInt32(0), reader.GetDecimal(3), reader.GetInt32(4)));
            return lista;
        }

        public Dictionary<string, (decimal Depositos, decimal Retiros)> ObtenerMovimientosPorMes(int meses)
        {
            var resultado = new Dictionary<string, (decimal, decimal)>();
            string sql = @"WITH t_na AS (SELECT t.* FROM transacciones t
WHERE NOT EXISTS (SELECT 1 FROM usuarios u WHERE u.id_usuario=t.id_usuario AND u.id_rol=1)),
meses AS (SELECT ADD_MONTHS(TRUNC(SYSDATE,'MM'),-(LEVEL-1)) mes FROM DUAL CONNECT BY LEVEL<=:m)
SELECT TO_CHAR(m.mes,'MON YY'),
NVL((SELECT SUM(monto) FROM t_na WHERE tipo='deposito' AND TRUNC(fecha,'MM')=m.mes),0),
NVL((SELECT SUM(monto) FROM t_na WHERE tipo='retiro' AND TRUNC(fecha,'MM')=m.mes),0)
FROM meses m ORDER BY m.mes";
            using (OracleDataReader reader = EjecutarConsulta(sql, new[] { (":m", (object)meses) }))
                while (reader.Read())
                    resultado[reader.GetString(0)] = (reader.GetDecimal(1), reader.GetDecimal(2));
            return resultado;
        }

        public Dictionary<DateTime, decimal> ObtenerIngresosPorDia(int dias)
        {
            var resultado = new Dictionary<DateTime, decimal>();
            string sql = @"WITH p_na AS (SELECT p.* FROM partidas p
WHERE NOT EXISTS (SELECT 1 FROM usuarios WHERE id_usuario=p.id_usuario AND id_rol=1)),
t_na AS (SELECT t.* FROM transacciones t
WHERE NOT EXISTS (SELECT 1 FROM usuarios u WHERE u.id_usuario=t.id_usuario AND u.id_rol=1)),
dias AS (SELECT TRUNC(SYSDATE)-LEVEL+1 fecha FROM DUAL CONNECT BY LEVEL<=:d)
SELECT d.fecha,
NVL((SELECT SUM(monto) FROM t_na WHERE tipo='deposito' AND TRUNC(fecha)=d.fecha),0),
NVL((SELECT SUM(apuesta-ganancia) FROM p_na WHERE TRUNC(fecha)=d.fecha),0)
FROM dias d ORDER BY d.fecha";
            using (OracleDataReader reader = EjecutarConsulta(sql, new[] { (":d", (object)dias) }))
                while (reader.Read())
                    resultado[reader.GetDateTime(0)] = reader.GetDecimal(1);
            return resultado;
        }

        public Dictionary<DateTime, decimal> ObtenerGananciaCasaPorDia(int dias)
        {
            var resultado = new Dictionary<DateTime, decimal>();
            string sql = @"WITH p_na AS (SELECT p.* FROM partidas p
WHERE NOT EXISTS (SELECT 1 FROM usuarios WHERE id_usuario=p.id_usuario AND id_rol=1)),
t_na AS (SELECT t.* FROM transacciones t
WHERE NOT EXISTS (SELECT 1 FROM usuarios u WHERE u.id_usuario=t.id_usuario AND u.id_rol=1)),
dias AS (SELECT TRUNC(SYSDATE)-LEVEL+1 fecha FROM DUAL CONNECT BY LEVEL<=:d)
SELECT d.fecha,
NVL((SELECT SUM(monto) FROM t_na WHERE tipo='deposito' AND TRUNC(fecha)=d.fecha),0),
NVL((SELECT SUM(apuesta-ganancia) FROM p_na WHERE TRUNC(fecha)=d.fecha),0)
FROM dias d ORDER BY d.fecha";
            using (OracleDataReader reader = EjecutarConsulta(sql, new[] { (":d", (object)dias) }))
                while (reader.Read())
                    resultado[reader.GetDateTime(0)] = reader.GetDecimal(2);
            return resultado;
        }

        public decimal ObtenerTotalTransacciones(string tipo)
        {
            return EjecutarScalar<decimal>(
                @"SELECT NVL(SUM(monto), 0) FROM transacciones t
WHERE t.tipo = :tipo AND NOT EXISTS (SELECT 1 FROM usuarios u WHERE u.id_usuario = t.id_usuario AND u.id_rol = 1)",
                new[] { (":tipo", (object)tipo) });
        }
    }
}
