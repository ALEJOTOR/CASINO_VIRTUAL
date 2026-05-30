using BLL;
using BotApi.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<UsuarioServicio>();
builder.Services.AddTransient<TransaccionServicio>();
builder.Services.AddTransient<PartidaServicio>();
builder.Services.AddTransient<TelegramVinculoService>();

WebApplication app = builder.Build();

// Protege los endpoints privados del bot con una clave simple para que ngrok no deje la API abierta.
app.Use(async (context, next) =>
{
    string apiKey = app.Configuration["BotApi:ApiKey"] ?? "";
    bool requiereClave = context.Request.Path.StartsWithSegments("/api/bot")
        && !context.Request.Path.StartsWithSegments("/api/bot/ping");

    if (requiereClave && !string.IsNullOrWhiteSpace(apiKey))
    {
        string enviada = context.Request.Headers["x-casino-api-key"].ToString();
        if (!string.Equals(enviada, apiKey, StringComparison.Ordinal))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new { ok = false, mensaje = "Clave API invalida." });
            return;
        }
    }

    await next();
});

app.MapGet("/api/bot/ping", () => Results.Ok(new
{
    ok = true,
    mensaje = "API Casino Royal funcionando",
    fecha = DateTime.Now
}));

app.MapGet("/api/bot/faq", () => Results.Ok(new
{
    ok = true,
    preguntas = new[]
    {
        new { pregunta = "Como consulto mi saldo?", respuesta = "Vincula tu cuenta y escribe /saldo en Telegram." },
        new { pregunta = "Como recargo saldo?", respuesta = "Entra a Casino Royal, abre Billetera y usa la opcion de deposito." },
        new { pregunta = "Como retiro dinero?", respuesta = "Registra tus datos bancarios en Billetera y solicita el retiro." },
        new { pregunta = "Por que debo vincular Telegram?", respuesta = "Para que el bot sepa que cuenta del casino debe consultar." }
    }
}));

app.MapGet("/api/bot/reglas", () => Results.Ok(new
{
    ok = true,
    reglas = new
    {
        minas = "Elige apuesta, numero de minas y cuadricula. Destapa casillas seguras y cobra antes de caer en una mina.",
        ruleta = "Selecciona una ficha, apuesta a numero, color, docena o rango y gira la ruleta.",
        tragamonedas = "Elige una apuesta y gira. Dos simbolos iguales pagan x2 y tres simbolos iguales pagan x8."
    }
}));

app.MapPost("/api/bot/vincular", (VincularRequest request, TelegramVinculoService service) =>
{
    if (string.IsNullOrWhiteSpace(request.ChatId) || string.IsNullOrWhiteSpace(request.Username))
        return Results.BadRequest(new { ok = false, mensaje = "Debes enviar chatId y username." });

    VinculoResultado resultado = service.SolicitarVinculo(request.ChatId.Trim(), request.Username.Trim());
    return resultado.Ok
        ? Results.Ok(resultado)
        : Results.BadRequest(resultado);
});

app.MapGet("/api/bot/vinculo-pendiente/{idUsuario:int}", (int idUsuario, TelegramVinculoService service) =>
{
    VinculoPendiente? vinculo = service.ObtenerPendientePorUsuario(idUsuario);
    return vinculo is null
        ? Results.Ok(new { ok = true, tienePendiente = false })
        : Results.Ok(new { ok = true, tienePendiente = true, vinculo });
});

app.MapPost("/api/bot/confirmar-vinculo", (ConfirmarVinculoRequest request, TelegramVinculoService service) =>
{
    VinculoResultado resultado = service.ConfirmarVinculo(request.IdUsuario, request.Codigo?.Trim() ?? "");
    return resultado.Ok
        ? Results.Ok(resultado)
        : Results.BadRequest(resultado);
});

app.MapGet("/api/bot/saldo", (string chatId, TelegramVinculoService vinculos, UsuarioServicio usuarios) =>
{
    int? idUsuario = vinculos.ObtenerUsuarioVinculado(chatId);
    if (idUsuario is null)
        return Results.Unauthorized();

    ENTITY.Usuario? usuario = usuarios.ObtenerPorId(idUsuario.Value);
    if (usuario is null)
        return Results.NotFound(new { ok = false, mensaje = "Usuario no encontrado." });

    return Results.Ok(new
    {
        ok = true,
        usuario = usuario.Username,
        nombre = $"{usuario.Nombre1} {usuario.Apellido1}",
        saldo = usuario.Saldo
    });
});

app.MapGet("/api/bot/salir", (string chatId, TelegramVinculoService vinculos) =>
{
    try
    {
        VinculoResultado resultado = vinculos.CerrarSesionTelegram(chatId);
        return resultado.Ok
            ? Results.Ok(resultado)
            : Results.BadRequest(resultado);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { ok = false, mensaje = $"No se pudo cerrar la sesion: {ex.Message}" });
    }
});

app.MapGet("/api/bot/transacciones", (string chatId, TelegramVinculoService vinculos, TransaccionServicio transacciones) =>
{
    int? idUsuario = vinculos.ObtenerUsuarioVinculado(chatId);
    if (idUsuario is null)
        return Results.Unauthorized();

    var items = transacciones.ObtenerPorUsuario(idUsuario.Value)
        .Take(8)
        .Select(t => new
        {
            t.Tipo,
            t.Monto,
            t.Fecha,
            t.Descripcion
        });

    return Results.Ok(new { ok = true, transacciones = items });
});

app.MapGet("/api/bot/historial", (string chatId, TelegramVinculoService vinculos, PartidaServicio partidas) =>
{
    int? idUsuario;
    try
    {
        idUsuario = vinculos.ObtenerUsuarioVinculado(chatId);
        if (idUsuario is null)
            return Results.Unauthorized();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { ok = false, mensaje = $"No se pudo validar el vinculo de Telegram: {ex.Message}" });
    }

    try
    {
        var items = partidas.ObtenerHistorialPorUsuario(idUsuario.Value)
            .Take(8)
            .Select(p => new
            {
                p.IdPartida,
                Juego = p.NombreJuego,
                p.Estado,
                p.Fecha,
                p.Apuesta,
                p.Ganancia,
                GananciaNeta = p.Ganancia - p.Apuesta
            });

        return Results.Ok(new { ok = true, historial = items });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { ok = false, mensaje = $"No se pudo consultar el historial: {ex.Message}" });
    }
});

app.MapGet("/api/bot/historial-texto", (string chatId, TelegramVinculoService vinculos, PartidaServicio partidas) =>
{
    int? idUsuario;
    try
    {
        idUsuario = vinculos.ObtenerUsuarioVinculado(chatId);
        if (idUsuario is null)
            return Results.Unauthorized();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { ok = false, mensaje = $"No se pudo validar el vinculo de Telegram: {ex.Message}" });
    }

    try
    {
        var historial = partidas.ObtenerHistorialPorUsuario(idUsuario.Value).Take(8).ToList();

        if (historial.Count == 0)
            return Results.Ok(new { ok = true, mensaje = "No tienes partidas recientes." });

        var lineas = historial.Select((p, i) =>
        {
            string estado = p.Estado.ToLowerInvariant();
            string resultado = estado.Contains("gan") ? "Ganaste" :
                estado.Contains("perd") ? "Perdiste" :
                estado.Contains("cancel") ? "Cancelada" : p.Estado;
            return $"{i + 1}. {p.NombreJuego}\n" +
                   $"Resultado: {resultado} ({p.Estado})\n" +
                   $"Apuesta: ${p.Apuesta:N0}\n" +
                   $"Ganancia: ${p.Ganancia:N0}\n" +
                   $"Fecha: {p.Fecha:dd/MM/yyyy hh:mm tt}";
        });

        return Results.Ok(new { ok = true, mensaje = $"Historial de partidas:\n\n{string.Join("\n\n", lineas)}" });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { ok = false, mensaje = $"No se pudo consultar el historial: {ex.Message}" });
    }
});

app.Run();

public record VincularRequest(string ChatId, string Username);
public record ConfirmarVinculoRequest(int IdUsuario, string Codigo);
