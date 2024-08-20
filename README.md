# Configuraci�n Inicial del Proyecto

Este documento describe la configuraci�n inicial y las indicaciones del proyecto para la aplicaci�n web desarrollada en ASP.NET Core 8.0.

## Configuraci�n del Sistema

La aplicaci�n utiliza autenticaci�n b�sica generada por defecto en Visual Studio para cuentas individuales. Adem�s, est� configurada para HTTPS, por lo que se recomienda tener esta opci�n habilitada.

### Base de Datos

- **Tipo de Base de Datos**: SQLite (modo Debug)
- **Configuraci�n**: Se han descargado los paquetes NuGet necesarios y realizado las configuraciones correspondientes en los archivos `Program.cs` y `appsettings.json`.

## Autenticaci�n

La autenticaci�n est� autogenerada por ASP.NET Identity. Las modificaciones de idioma del ingl�s al espa�ol se han realizado para adecuar el proyecto al idioma requerido. Sin embargo, podr�an existir algunas l�neas o frases que no se modificaron durante las pruebas t�cnicas.

## Indicaciones del Proyecto

- **Acceso a Pantallas**: Todas las pantallas est�n bloqueadas y solo se mostrar�n si el usuario ha creado una cuenta. El proceso de registro de usuario no cuenta con validaci�n externa. Para activar una cuenta, lea cada pantalla y realice la activaci�n necesaria en la base de datos si es necesario.
- **Super Usuario (Administrador)**: El sistema est� configurado para generar autom�ticamente un superusuario con privilegios administrativos. Este usuario puede acceder a todas las tareas y asignarlas a otros usuarios. Los usuarios normales solo pueden asignarse tareas a s� mismos.

### Credenciales del Super Usuario

- **Correo Electr�nico**: `admin@mail.com`
- **Contrase�a**: `Qyvxdr58*`

La creaci�n del usuario se realiza autom�ticamente en `Data/SeedData.cs`.

## Ejecuci�n del Proyecto

1. **Descarga**: Descargue el proyecto desde Git.
2. **Ejecuci�n Inicial**: La primera ejecuci�n puede tardar un poco, ya que el sistema crea todos los datos necesarios.

En caso de dudas o errores durante el proceso, y si la empresa lo permite durante el reclutamiento, contacte al desarrollador:

- **Nombre**: Brandon Cambronero Z��iga
- **Tel�fono**: (506) 8375-5048
- **Correo Electr�nico**: [brancamzu14@gmail.com](mailto:brancamzu14@gmail.com)
