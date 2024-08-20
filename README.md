# Configuración Inicial del Proyecto

Este documento describe la configuración inicial y las indicaciones del proyecto para la aplicación web desarrollada en ASP.NET Core 8.0.

## Configuración del Sistema

La aplicación utiliza autenticación básica generada por defecto en Visual Studio para cuentas individuales. Además, está configurada para HTTPS, por lo que se recomienda tener esta opción habilitada.

### Base de Datos

- **Tipo de Base de Datos**: SQLite (modo Debug)
- **Configuración**: Se han descargado los paquetes NuGet necesarios y realizado las configuraciones correspondientes en los archivos `Program.cs` y `appsettings.json`.

## Autenticación

La autenticación está autogenerada por ASP.NET Identity. Las modificaciones de idioma del inglés al español se han realizado para adecuar el proyecto al idioma requerido. Sin embargo, podrían existir algunas líneas o frases que no se modificaron durante las pruebas técnicas.

## Indicaciones del Proyecto

- **Acceso a Pantallas**: Todas las pantallas están bloqueadas y solo se mostrarán si el usuario ha creado una cuenta. El proceso de registro de usuario no cuenta con validación externa. Para activar una cuenta, lea cada pantalla y realice la activación necesaria en la base de datos si es necesario.
- **Super Usuario (Administrador)**: El sistema está configurado para generar automáticamente un superusuario con privilegios administrativos. Este usuario puede acceder a todas las tareas y asignarlas a otros usuarios. Los usuarios normales solo pueden asignarse tareas a sí mismos.

### Credenciales del Super Usuario

- **Correo Electrónico**: `admin@mail.com`
- **Contraseña**: `Qyvxdr58*`

La creación del usuario se realiza automáticamente en `Data/SeedData.cs`.

## Ejecución del Proyecto

1. **Descarga**: Descargue el proyecto desde Git.
2. **Ejecución Inicial**: La primera ejecución puede tardar un poco, ya que el sistema crea todos los datos necesarios.

En caso de dudas o errores durante el proceso, y si la empresa lo permite durante el reclutamiento, contacte al desarrollador:

- **Nombre**: Brandon Cambronero Zúñiga
- **Teléfono**: (506) 8375-5048
- **Correo Electrónico**: [brancamzu14@gmail.com](mailto:brancamzu14@gmail.com)
