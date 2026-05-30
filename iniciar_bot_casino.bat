@echo off
cd /d "%~dp0"

echo Iniciando Casino Royal Bot API en http://localhost:5055 ...
start "Casino Bot API" cmd /k dotnet run --project BotApi\BotApi.csproj --urls http://localhost:5055

echo Esperando a que la API arranque...
timeout /t 5 /nobreak >nul

echo Iniciando ngrok hacia el puerto 5055 ...
start "Ngrok Casino Bot" cmd /k ngrok http 5055

echo.
echo Listo. Deja abiertas las dos ventanas.
echo API local: http://localhost:5055/api/bot/ping
echo Ngrok: revisa la ventana de ngrok para confirmar la URL publica.
pause
