# Estructura de la capa GUI

Esta carpeta contiene la interfaz de usuario del casino. La pantalla principal actual es `Shell/MainForm.cs`.

## Carpetas principales

- `Shell`: ventana principal del sistema. Desde aqui se muestran Inicio, Transacciones, Billetera y los juegos dentro del mismo formulario.
- `Views/Games`: juegos actuales implementados como `UserControl`. Estas son las vistas que usa `MainForm`.
- `Forms/Auth`: formularios de acceso y registro de usuarios.
- `Forms/Admin`: formularios administrativos y ventanas auxiliares para ingresar datos.
- `Core`: estilos visuales y utilidades compartidas de interfaz.
- `Assets`: imagenes usadas en el lobby y tarjetas de juegos.

## Flujo actual

1. `Program.cs` inicia la aplicacion.
2. `FrmLogin` autentica al usuario.
3. `MainForm` actua como contenedor principal.
4. Los juegos se cargan dentro de `MainForm` usando:
   - `UcMinas`
   - `UcRuleta`
   - `UcTragamonedas`

## Nota para revision

Los formularios antiguos de juegos fueron retirados para evitar duplicidad. La experiencia principal esta concentrada en `MainForm` y en los `UserControl` de `Views/Games`.
