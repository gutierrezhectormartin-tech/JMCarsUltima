--si la base de datos existe la borro------------------------------------------------------
USE master
GO
 
IF EXISTS(SELECT * FROM SysDataBases WHERE name='JMCars')
BEGIN
	DROP DATABASE JMCars
END
go
 
--creo la base de datos---------------------------------------------------------------------
CREATE DATABASE JMCars
GO

USE JMCars
go
 
---------------------------------
-- TABLAS
---------------------------------
CREATE TABLE Usuario
(
	IdUsuario INT IDENTITY PRIMARY KEY, 
	NombreCompleto  VARCHAR(100) NOT NULL,
	Telefono VARCHAR(20) NOT NULL,
	Email VARCHAR(255) NOT NULL, 
	Contrasena VARCHAR(255) NOT NULL check (LEN(Contrasena) >= 3),
	Estado BIT NOT NULL,
	FechaAceptacionTerminos DATETIME NULL
)
GO

-- >>> Como te logueas con el email / contraseña, este índice hace que solo exista un único usuario
--	   activo por mail. Pero si borras el usuario (estado = 0) entonces te deja reutilizar el mail.
CREATE UNIQUE INDEX UX_Usuario_Email_Activo
ON Usuario(Email)
WHERE Estado = 1;
GO


CREATE TABLE Cliente
(
	IdUsuario INT FOREIGN KEY REFERENCES Usuario(IdUsuario),
	Cedula VARCHAR(8) NOT NULL CHECK (Cedula NOT LIKE '%[^0-9]%' AND LEN(Cedula) BETWEEN 7 AND 8), 
	PRIMARY KEY (IdUsuario)
)
GO

CREATE TABLE Escribano
(
	IdUsuario INT FOREIGN KEY REFERENCES Usuario(IdUsuario), 
	NumCajaProf VARCHAR(50) NOT NULL,
	DireccionEstudio VARCHAR(200) NOT NULL,
	PRIMARY KEY (IdUsuario)
)
GO

CREATE TABLE Administrador
(
	IdUsuario INT FOREIGN KEY REFERENCES Usuario(IdUsuario), 
	PRIMARY KEY (IdUsuario)
)
GO


CREATE TABLE Marca
(
	IdMarca INT IDENTITY PRIMARY KEY,
	NombreMarca VARCHAR(50) NOT NULL
)
GO

CREATE TABLE Modelo
(
	IdModelo INT IDENTITY PRIMARY KEY,
	NombreModelo VARCHAR(50) NOT NULL,
	IdMarca INT NOT NULL FOREIGN KEY REFERENCES Marca(IdMarca)
)
GO



CREATE TABLE Vehiculo
(
	IdVehiculo INT IDENTITY(1,1) PRIMARY KEY,
	Precio DECIMAL(10,2) NOT NULL CHECK (Precio > 0),
	Kilometraje INT NOT NULL CHECK (Kilometraje >= 0),
	Ano INT NOT NULL,
	CajaDeCambios VARCHAR(30) NOT NULL,
	Motorizacion VARCHAR(30) NOT NULL,
	Descripcion VARCHAR(MAX) NOT NULL,
	Publicado BIT NOT NULL,
	--Ubicacion VARCHAR(100), ---- <<< si son coordenadas, nos conviene guardarlas separadas en 2 columnas de tipo POINT (ahi vemos en sql server el concepto de spatial)
	Latitud DECIMAL(9,6),
	Longitud DECIMAL(9,6),
	IdModelo INT NOT NULL,
	IdUsuarioVendedor INT NOT NULL,

	CONSTRAINT VehiculoModelo FOREIGN KEY (IdModelo) REFERENCES Modelo(IdModelo),
	CONSTRAINT VehiculoUsuario FOREIGN KEY (IdUsuarioVendedor) REFERENCES Cliente(IdUsuario)
)
GO


CREATE TABLE FotoVehiculo
(
	IdFoto INT IDENTITY(1,1) PRIMARY KEY,
	UrlFoto VARCHAR(MAX) NOT NULL,
	IdVehiculo INT NOT NULL,
	
	CONSTRAINT FK_Vehiculo FOREIGN KEY (IdVehiculo) REFERENCES Vehiculo(IdVehiculo)
)
GO


CREATE TABLE EstadosSolicitudNotarial
(
	IdEstadoSolicitud INT NOT NULL PRIMARY KEY,
	NombreEstado VARCHAR(20)
)
GO

INSERT INTO EstadosSolicitudNotarial VALUES (1, 'Pendiente'),
											(2, 'Aceptada'),
											(3, 'Rechazada'),
											(4, 'Finalizada')
GO


CREATE TABLE SolicitudNotarial
(
	IdSolicitud INT IDENTITY(1,1) PRIMARY KEY,
	FechaSolicitud DATETIME DEFAULT GETDATE(),
	EstadoSolicitud INT NOT NULL,
	IdUsuarioCliente INT NOT NULL,
	IdVehiculo INT NOT NULL,
	
	CONSTRAINT SolicitudEstado FOREIGN KEY (EstadoSolicitud)REFERENCES EstadosSolicitudNotarial(IdEstadoSolicitud),
	CONSTRAINT SolicitudCliente FOREIGN KEY (IdUsuarioCliente) REFERENCES Cliente(IdUsuario),
	CONSTRAINT SolicitudVehiculo FOREIGN KEY (IdVehiculo) REFERENCES Vehiculo(IdVehiculo)
)
GO


CREATE TABLE SolicitudEscribano
(
	IdSolicitud INT NOT NULL,
	IdUsuarioEscribano INT NOT NULL,
	
	PRIMARY KEY (IdSolicitud, IdUsuarioEscribano),
	CONSTRAINT Solicitud FOREIGN KEY (IdSolicitud) REFERENCES SolicitudNotarial(IdSolicitud),
	CONSTRAINT SeEscribano FOREIGN KEY (IdUsuarioEscribano) REFERENCES Escribano(IdUsuario)
)
GO

CREATE TABLE EstadoCompraVenta
(
	IdEstadoCompraVenta INT NOT NULL PRIMARY KEY,
	NombreEstado VARCHAR(20)
)
GO

INSERT INTO EstadoCompraVenta VALUES (1, 'Pendiente'),
									 (2, 'Aceptada'),
									 (3, 'Rechazada'),
									 (4, 'Inactiva')
GO


CREATE TABLE CompraVenta (
    IdCompraVenta INT IDENTITY(1,1) PRIMARY KEY,
    FechaInicio DATETIME DEFAULT GETDATE(),
    EstadoCompraVenta INT NOT NULL,
    IdSolicitud INT NOT NULL UNIQUE,

	CONSTRAINT CompraVentaEstado FOREIGN KEY (EstadoCompraVenta)REFERENCES EstadoCompraVenta(IdEstadoCompraVenta),
    CONSTRAINT CompraVentaSolicitud FOREIGN KEY (IdSolicitud) REFERENCES SolicitudNotarial(IdSolicitud)
)
GO




CREATE TABLE Chat (
    IdChat INT IDENTITY(1,1) PRIMARY KEY,
    FechaInicio DATETIME DEFAULT GETDATE(),
    IdVehiculo INT NOT NULL,
    CONSTRAINT ChatVehiculo FOREIGN KEY (IdVehiculo) REFERENCES Vehiculo(IdVehiculo)
)
GO

CREATE TABLE ChatParticipante (
    IdChat INT NOT NULL,
    IdUsuario INT NOT NULL,
    FechaIngreso DATETIME NOT NULL DEFAULT GETDATE(),-- esto es opcional si queres lo borramos jorge

    CONSTRAINT ParticipanteChat PRIMARY KEY (IdChat, IdUsuario),
	CONSTRAINT FK_Chat FOREIGN KEY (IdChat) REFERENCES Chat(IdChat),
    CONSTRAINT FK_Usuario FOREIGN KEY (IdUsuario) REFERENCES Usuario(IdUsuario)
);

CREATE TABLE Mensaje (
    IdMensaje INT IDENTITY(1,1) PRIMARY KEY,
    IdChat INT NOT NULL,
	IdUsuarioEmisor INT NOT NULL,
	Contenido VARCHAR(MAX) NOT NULL,
    FechaHora DATETIME DEFAULT GETDATE(),    
    CONSTRAINT MensajeChat FOREIGN KEY (IdChat) REFERENCES Chat(IdChat),
    CONSTRAINT MensajeUsuario FOREIGN KEY (IdUsuarioEmisor) REFERENCES Usuario(IdUsuario)
)
GO

CREATE TABLE TokenRecuperacion (
    IdToken INT IDENTITY(1,1) PRIMARY KEY,
    IdUsuario INT NOT NULL,
    Token VARCHAR(255) NOT NULL,
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
    FechaExpiracion DATETIME NOT NULL,
    Usado BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_TokenRecuperacion_Usuario FOREIGN KEY (IdUsuario) REFERENCES Usuario(IdUsuario)
);
GO

---------------------- SP-------------------------

-- Login
-- >>> Ya no recibe @Contrasena: el SP solo busca por Email/Estado y devuelve el hash guardado.
--	   La verificacion real del hash (BCrypt) se hace en la capa Logica (LogicaUsuario.Login).
CREATE PROC sp_Usuario_Login
    @Email VARCHAR(255) 
AS
BEGIN
    SELECT U.IdUsuario, U.NombreCompleto, U.Contrasena, U.Estado, U.FechaAceptacionTerminos,
           CASE 
               WHEN A.IdUsuario IS NOT NULL THEN 1 -- Admin
               WHEN E.IdUsuario IS NOT NULL THEN 2 -- Escribano
               WHEN C.IdUsuario IS NOT NULL THEN 3 -- Cliente
           END AS IdRol
    FROM Usuario U
    LEFT JOIN Administrador A ON U.IdUsuario = A.IdUsuario
    LEFT JOIN Escribano E ON U.IdUsuario = E.IdUsuario
    LEFT JOIN Cliente C ON U.IdUsuario = C.IdUsuario
    WHERE U.Email = @Email AND U.Estado = 1;
END
GO

-- Registrar Cliente
-- >>> @Contrasena llega ya hasheado con BCrypt desde la capa Logica (LogicaCliente.Registrar).
-- >>> @FechaAceptacionTerminos se completa con la fecha/hora en que el usuario aceptó los Terminos y Condiciones en el formulario de registro.
CREATE PROCEDURE sp_Usuario_RegistrarCliente
    @NombreCompleto VARCHAR(100), @Telefono VARCHAR(20), @Email VARCHAR(255), 
    @Contrasena VARCHAR(255), @Cedula VARCHAR(8), @FechaAceptacionTerminos DATETIME
AS
BEGIN
    BEGIN TRANSACTION;
    BEGIN TRY
        INSERT INTO Usuario (NombreCompleto, Telefono, Email, Contrasena, Estado, FechaAceptacionTerminos)
        VALUES (@NombreCompleto, @Telefono, @Email, @Contrasena, 1, @FechaAceptacionTerminos);
        INSERT INTO Cliente (IdUsuario, Cedula) VALUES (SCOPE_IDENTITY(), @Cedula);
        COMMIT;
    END TRY
    BEGIN CATCH ROLLBACK; THROW; END CATCH
END
GO

-- Registrar Escribano (Inactivo por defecto para aprobación)
-- >>> @Contrasena llega ya hasheado con BCrypt desde la capa Logica (LogicaEscribano.Registrar).
-- >>> @FechaAceptacionTerminos se completa con la fecha/hora en que el usuario aceptó los Terminos y Condiciones en el formulario de registro.
CREATE PROCEDURE sp_Usuario_RegistrarEscribano
    @NombreCompleto VARCHAR(100), @Telefono VARCHAR(20), @Email VARCHAR(255), 
    @Contrasena VARCHAR(255), @NumCajaProf VARCHAR(50), @DireccionEstudio VARCHAR(200), @FechaAceptacionTerminos DATETIME
AS
BEGIN
    BEGIN TRANSACTION;
    BEGIN TRY
        INSERT INTO Usuario (NombreCompleto, Telefono, Email, Contrasena, Estado, FechaAceptacionTerminos)
        VALUES (@NombreCompleto, @Telefono, @Email, @Contrasena, 0, @FechaAceptacionTerminos);
        INSERT INTO Escribano (IdUsuario, NumCajaProf, DireccionEstudio) 
        VALUES (SCOPE_IDENTITY(), @NumCajaProf, @DireccionEstudio);
        COMMIT;
    END TRY
    BEGIN CATCH ROLLBACK; THROW; END CATCH
END
GO

-- Recuperar contraseña 
CREATE PROCEDURE sp_Usuario_ExisteEmail
    @Email VARCHAR(255)
AS
BEGIN
    SELECT COUNT(*) 
    FROM Usuario
    WHERE Email = @Email
    AND Estado = 1
END
GO

create proc TokenRecuperacionCrear
@IdUsuario int,
@Token varchar(255),
@FechaExpiracion datetime
as
begin

    insert into TokenRecuperacion (IdUsuario, Token, FechaExpiracion)
    values (@IdUsuario, @Token, @FechaExpiracion);
end
go

create proc TokenRecuperacionObtener
@Token VARCHAR(255)
as
begin

    select IdToken, IdUsuario, Token, FechaCreacion, FechaExpiracion, Usado
    from TokenRecuperacion
    where Token = @Token and Usado = 0 and FechaExpiracion > GETDATE();
end
go

create proc TokenRecuperacionUsado
@IdToken int
as
begin
    update TokenRecuperacion
    set Usado = 1
    where IdToken = @IdToken;
end
go

create proc ObtenerUsuarioxMail
@Email varchar(255)
as
begin
    select U.IdUsuario, U.NombreCompleto, U.Telefono, U.Email, U.Estado, U.FechaAceptacionTerminos,
        case
            when A.IdUsuario is not null then 1
            when E.IdUsuario is not null then 2
            when C.IdUsuario is not null then 3
        end as IdRol
    from Usuario U
    left join Administrador A on U.IdUsuario = A.IdUsuario
    left join Escribano E on U.IdUsuario = E.IdUsuario
    left join Cliente C on U.IdUsuario = C.IdUsuario
    where U.Email = @Email and U.Estado = 1
end
go

-- >>> @NuevaContrasena llega ya hasheado con BCrypt desde la capa Logica (LogicaUsuario.ResetearContrasena).
create proc ActualizarContrasenaUsuario
@IdUsuario int,
@NuevaContrasena varchar(255)
as
begin
    update Usuario set Contrasena = @NuevaContrasena where IdUsuario = @IdUsuario
end
go

-- Crear Vehículo
CREATE PROCEDURE sp_Vehiculo_Crear
    @Precio DECIMAL(10,2), @Kilometraje INT, @Ano INT, @Caja VARCHAR(30), 
    @Motor VARCHAR(30), @Desc VARCHAR(MAX), @Lat DECIMAL(9,6), @Lon DECIMAL(9,6), 
    @IdModelo INT, @IdVendedor INT
AS
BEGIN
    INSERT INTO Vehiculo (Precio, Kilometraje, Ano, CajaDeCambios, Motorizacion, Descripcion, Publicado, Latitud, Longitud, IdModelo, IdUsuarioVendedor)
    VALUES (@Precio, @Kilometraje, @Ano, @Caja, @Motor, @Desc, 0, @Lat, @Lon, @IdModelo, @IdVendedor);
    SELECT SCOPE_IDENTITY() AS IdVehiculo;
END
GO

-- Listar Vehículos con datos de Marca/Modelo

CREATE PROCEDURE sp_Vehiculo_Listar
AS
BEGIN
    SELECT 
        V.IdVehiculo,
        V.Precio,
        V.Kilometraje,
        V.Ano,
        V.CajaDeCambios,
        V.Motorizacion,
        V.Descripcion,
        V.Publicado,
        V.Latitud,
        V.Longitud,

        M.IdModelo,
        M.NombreModelo,

        MA.IdMarca,
        MA.NombreMarca,

        U.IdUsuario,
        U.NombreCompleto

    FROM Vehiculo V

    INNER JOIN Modelo M
        ON V.IdModelo = M.IdModelo

    INNER JOIN Marca MA
        ON M.IdMarca = MA.IdMarca

    INNER JOIN Usuario U
        ON V.IdUsuarioVendedor = U.IdUsuario
END
GO

-- Búsqueda por Radio (Geolocalización)
CREATE PROCEDURE sp_Vehiculo_BuscarGeneral
    @LatCli DECIMAL(9,6), @LonCli DECIMAL(9,6), @RadioKM INT,
    @IdMarca INT = NULL, @PrecioMax DECIMAL(10,2) = NULL
AS
BEGIN
    SELECT V.*, M.NombreModelo, MA.NombreMarca,
    (6371 * acos(cos(radians(@LatCli)) * cos(radians(V.Latitud)) * cos(radians(V.Longitud) - radians(@LonCli)) + sin(radians(@LatCli)) * sin(radians(V.Latitud)))) AS Distancia
    FROM Vehiculo V
    JOIN Modelo M ON V.IdModelo = M.IdModelo
    JOIN Marca MA ON M.IdMarca = MA.IdMarca
    WHERE V.Publicado = 1
    AND (@IdMarca IS NULL OR MA.IdMarca = @IdMarca)
    AND (@PrecioMax IS NULL OR V.Precio <= @PrecioMax)
    AND (6371 * acos(cos(radians(@LatCli)) * cos(radians(V.Latitud)) * cos(radians(V.Longitud) - radians(@LonCli)) + sin(radians(@LatCli)) * sin(radians(V.Latitud)))) <= @RadioKM
    ORDER BY Distancia ASC;
END
GO

-- Obtener o Crear Chat
CREATE PROCEDURE sp_Chat_Obtener
    @IdVehiculo INT, @IdComprador INT, @IdVendedor INT
AS
BEGIN
    -- Validación: El comprador no puede ser el mismo que el vendedor
    IF (@IdComprador = @IdVendedor)
    BEGIN
        RAISERROR ('No puedes iniciar un chat sobre tu propio vehículo.', 16, 1);
        RETURN;
    END

    DECLARE @IdChat INT = (SELECT TOP 1 CP1.IdChat FROM ChatParticipante CP1 
                           JOIN ChatParticipante CP2 ON CP1.IdChat = CP2.IdChat
                           JOIN Chat C ON C.IdChat = CP1.IdChat
                           WHERE CP1.IdUsuario = @IdComprador AND CP2.IdUsuario = @IdVendedor AND C.IdVehiculo = @IdVehiculo);
    
    IF @IdChat IS NULL
    BEGIN
        INSERT INTO Chat (IdVehiculo) VALUES (@IdVehiculo); SET @IdChat = SCOPE_IDENTITY();
        INSERT INTO ChatParticipante (IdChat, IdUsuario) VALUES (@IdChat, @IdComprador), (@IdChat, @IdVendedor);
    END
    
    SELECT @IdChat AS IdChat;
END
GO

-- Enviar Mensaje
CREATE PROCEDURE sp_Mensaje_Enviar 
    @IdChat INT, 
    @IdEmisor INT, 
    @Texto VARCHAR(MAX) 
AS 
BEGIN 
    -- Validamos que el emisor sea parte de este chat
    IF NOT EXISTS (SELECT 1 FROM ChatParticipante WHERE IdChat = @IdChat AND IdUsuario = @IdEmisor)
    BEGIN
        RAISERROR ('No tienes permiso para enviar mensajes en este chat.', 16, 1);
        RETURN;
    END

    INSERT INTO Mensaje (IdChat, IdUsuarioEmisor, Contenido) 
    VALUES (@IdChat, @IdEmisor, @Texto);
END
GO

-- Solicitar Escribano
CREATE PROCEDURE sp_Notarial_CrearSolicitud
    @IdCliente INT, @IdVehiculo INT, @IdEscribano INT
AS
BEGIN
    INSERT INTO SolicitudNotarial (EstadoSolicitud, IdUsuarioCliente, IdVehiculo) VALUES (1, @IdCliente, @IdVehiculo);
    INSERT INTO SolicitudEscribano (IdSolicitud, IdUsuarioEscribano) VALUES (SCOPE_IDENTITY(), @IdEscribano);
END
GO

-- Finalizar Venta (Cierre total)
CREATE PROCEDURE sp_Notarial_FinalizarVenta
    @IdSolicitud INT
AS
BEGIN
    BEGIN TRANSACTION;
    BEGIN TRY
        -- Validamos que la solicitud esté en estado "Aceptada/En Proceso" (Estado 2) 
        -- y no esté ya Finalizada (4) o Cancelada (3)
        IF NOT EXISTS (SELECT 1 FROM SolicitudNotarial WHERE IdSolicitud = @IdSolicitud AND EstadoSolicitud = 2)
        BEGIN
            RAISERROR ('La solicitud no se encuentra en un estado válido para ser finalizada.', 16, 1);
        END

        -- Finaliza solicitud
        UPDATE SolicitudNotarial SET EstadoSolicitud = 4 WHERE IdSolicitud = @IdSolicitud;
        
        -- Crea registro en CompraVenta
        INSERT INTO CompraVenta (EstadoCompraVenta, IdSolicitud) VALUES (2, @IdSolicitud);
        
        -- Quitamos el auto de la venta pública
        DECLARE @IdV INT = (SELECT IdVehiculo FROM SolicitudNotarial WHERE IdSolicitud = @IdSolicitud);
        UPDATE Vehiculo SET Publicado = 0 WHERE IdVehiculo = @IdV;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH 
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW; 
    END CATCH
END
GO

-- Moderar Publicación
CREATE PROCEDURE sp_Admin_AprobarVehiculo @Id INT, @Publicado BIT
AS BEGIN UPDATE Vehiculo SET Publicado = @Publicado WHERE IdVehiculo = @Id END
GO

-- Activar/Inactivar Usuario
CREATE PROCEDURE sp_Admin_SetEstadoUsuario @Id INT, @Estado BIT 
AS BEGIN UPDATE Usuario SET Estado = @Estado WHERE IdUsuario = @Id END
GO

-- Alerta de 24 horas para Escribanos
CREATE PROCEDURE sp_Admin_AlertaEscribanos
AS
BEGIN
    SELECT S.IdSolicitud, U.NombreCompleto AS Escribano, S.FechaSolicitud
    FROM SolicitudNotarial S
    JOIN SolicitudEscribano SE ON S.IdSolicitud = SE.IdSolicitud
    JOIN Usuario U ON SE.IdUsuarioEscribano = U.IdUsuario
    WHERE S.EstadoSolicitud = 1 AND DATEDIFF(HOUR, S.FechaSolicitud, GETDATE()) > 24;
END
GO


-- Marcas (Alta y Modificación)
CREATE PROCEDURE sp_Marca_Guardar
    @IdMarca INT = 0, -- Si es 0 inserta, si es > 0 actualiza
    @NombreMarca VARCHAR(50)
AS
BEGIN
    IF @IdMarca = 0
        INSERT INTO Marca (NombreMarca) VALUES (@NombreMarca);
    ELSE
        UPDATE Marca SET NombreMarca = @NombreMarca WHERE IdMarca = @IdMarca;
END
GO

-- Modelos (Alta y Modificación)
CREATE PROCEDURE sp_Modelo_Guardar
    @IdModelo INT = 0,
    @NombreModelo VARCHAR(50),
    @IdMarca INT
AS
BEGIN
    IF @IdModelo = 0
        INSERT INTO Modelo (NombreModelo, IdMarca) VALUES (@NombreModelo, @IdMarca);
    ELSE
        UPDATE Modelo SET NombreModelo = @NombreModelo, IdMarca = @IdMarca WHERE IdModelo = @IdModelo;
END
GO

-- Modificar datos básicos de Usuario
CREATE PROCEDURE sp_Usuario_ActualizarPerfil
    @IdUsuario INT,
    @NombreCompleto VARCHAR(100),
    @Telefono VARCHAR(20),
    @Email VARCHAR(255)
AS
BEGIN
    UPDATE Usuario 
    SET NombreCompleto = @NombreCompleto, 
        Telefono = @Telefono, 
        Email = @Email
    WHERE IdUsuario = @IdUsuario;
END
GO

-- Modificar datos específicos del Escribano
CREATE PROCEDURE sp_Escribano_ActualizarDatos
    @IdUsuario INT,
    @NumCajaProf VARCHAR(50),
    @DireccionEstudio VARCHAR(200)
AS
BEGIN
    UPDATE Escribano
    SET NumCajaProf = @NumCajaProf,
        DireccionEstudio = @DireccionEstudio
    WHERE IdUsuario = @IdUsuario;
END
GO

-- Obtener Cliente por Id (para Gestionar Perfil)
CREATE PROCEDURE sp_Cliente_ObtenerPorId
    @IdUsuario INT
AS
BEGIN
    SELECT U.IdUsuario, U.NombreCompleto, U.Telefono, U.Email, U.Estado, U.FechaAceptacionTerminos, C.Cedula
    FROM Usuario U
    INNER JOIN Cliente C ON U.IdUsuario = C.IdUsuario
    WHERE U.IdUsuario = @IdUsuario;
END
GO

-- Obtener Escribano por Id (para Gestionar Perfil)
CREATE PROCEDURE sp_Escribano_ObtenerPorId
    @IdUsuario INT
AS
BEGIN
    SELECT U.IdUsuario, U.NombreCompleto, U.Telefono, U.Email, U.Estado, U.FechaAceptacionTerminos, E.NumCajaProf, E.DireccionEstudio
    FROM Usuario U
    INNER JOIN Escribano E ON U.IdUsuario = E.IdUsuario
    WHERE U.IdUsuario = @IdUsuario;
END
GO

-- Modificar Vehículo (Solo si no está en proceso de venta)
CREATE PROCEDURE sp_Vehiculo_Modificar
    @IdVehiculo INT,
    @Precio DECIMAL(10,2),
    @Kilometraje INT,
    @Descripcion VARCHAR(MAX)
AS
BEGIN
    -- Verificamos que no haya una solicitud notarial activa/aceptada
    IF NOT EXISTS (SELECT 1 FROM SolicitudNotarial WHERE IdVehiculo = @IdVehiculo AND EstadoSolicitud IN (1, 2))
    BEGIN
        UPDATE Vehiculo 
        SET Precio = @Precio, 
            Kilometraje = @Kilometraje, 
            Descripcion = @Descripcion 
        WHERE IdVehiculo = @IdVehiculo;
    END
    ELSE
    BEGIN
        RAISERROR ('No se puede modificar un vehículo con un proceso de venta en curso.', 16, 1);
    END
END
GO

-- Inactivar Publicación (Baja Lógica)
CREATE PROCEDURE sp_Vehiculo_Inactivar
    @IdVehiculo INT
AS
BEGIN
    UPDATE Vehiculo SET Publicado = 0 WHERE IdVehiculo = @IdVehiculo;
END
GO

-- Obtener un Vehículo completo con sus fotos
CREATE PROCEDURE sp_Vehiculo_ObtenerDetalle
    @IdVehiculo INT
AS
BEGIN
    SELECT V.*, M.NombreModelo, MA.NombreMarca
    FROM Vehiculo V
    JOIN Modelo M ON V.IdModelo = M.IdModelo
    JOIN Marca MA ON M.IdMarca = MA.IdMarca
    WHERE V.IdVehiculo = @IdVehiculo;

    SELECT UrlFoto FROM FotoVehiculo WHERE IdVehiculo = @IdVehiculo;
END
GO

-- Listar Escribanos aprobados (Para que el cliente elija uno)
CREATE PROCEDURE sp_Escribano_ListarActivos
AS
BEGIN
    SELECT U.IdUsuario, U.NombreCompleto, E.DireccionEstudio, E.NumCajaProf
    FROM Usuario U
    JOIN Escribano E ON U.IdUsuario = E.IdUsuario
    WHERE U.Estado = 1;
END
GO

-- Índice para la búsqueda por ubicación (Latitud/Longitud) y estado de publicación
CREATE INDEX IX_Vehiculo_Ubicacion_Publicado ON Vehiculo (Publicado, Latitud, Longitud);

-- Índice para filtrar por precio rápidamente
CREATE INDEX IX_Vehiculo_Precio ON Vehiculo (Precio) WHERE Publicado = 1;

-- Índice en la tabla Usuario para el Login (Email es lo que más se busca)
CREATE UNIQUE INDEX IX_Usuario_Email ON Usuario (Email) WHERE Estado = 1;

-- Índice para los mensajes de chat (para que el historial cargue rápido)
CREATE INDEX IX_Mensaje_Chat ON Mensaje (IdChat, FechaHora);


---------------------------------
-- DATOS DE PRUEBA
---------------------------------

-- >>> Las contraseñas de los datos de prueba estan hasheadas con BCrypt (BCrypt.Net-Next),
--	   con el mismo algoritmo que usa la capa Logica al registrar o resetear una contraseña real.
--	   La contraseña en texto plano de cada usuario de prueba se indica en el comentario de cada INSERT.

-- =========================
-- USUARIOS
-- =========================

-- ADMIN
-- contraseña en texto plano: admin123
INSERT INTO Usuario (NombreCompleto, Telefono, Email, Contrasena, Estado, FechaAceptacionTerminos)
VALUES ('Administrador General', '099111111', 'admin@jmcars.com', '$2a$11$RnZq9IMIumUFyCbX.jJiNOzpResv2DDHS8qIAeKbNzLd1.hG8Z1Eq', 1, GETDATE())

INSERT INTO Administrador VALUES (SCOPE_IDENTITY())
GO


-- CLIENTES
-- contraseña en texto plano: juan123
INSERT INTO Usuario (NombreCompleto, Telefono, Email, Contrasena, Estado, FechaAceptacionTerminos)
VALUES ('Juan Perez', '099222222', 'juan@gmail.com', '$2a$11$f3Z36OTiTXCp3uNvdGTn/.u4/l1a.Z4A07QOEDt4xRFvmltxFjMy6', 1, GETDATE())

INSERT INTO Cliente VALUES (SCOPE_IDENTITY(), '45678901')
GO

-- contraseña en texto plano: maria123
INSERT INTO Usuario (NombreCompleto, Telefono, Email, Contrasena, Estado, FechaAceptacionTerminos)
VALUES ('Maria Rodriguez', '099333333', 'maria@gmail.com', '$2a$11$X0P55g8U.n62nmY382g2Mevc/PYUy9QfOZRc5SH0NRcvrvpbSTFEW', 1, GETDATE())

INSERT INTO Cliente VALUES (SCOPE_IDENTITY(), '47896521')
GO

-- contraseña en texto plano: carlos123
INSERT INTO Usuario (NombreCompleto, Telefono, Email, Contrasena, Estado, FechaAceptacionTerminos)
VALUES ('Carlos Lopez', '099444444', 'carlos@gmail.com', '$2a$11$f00kcEXB1qQuvreMtcdU8OFNpzWL.KHzyQNJnCoz835fpWJ7B/6f2', 1, GETDATE())

INSERT INTO Cliente VALUES (SCOPE_IDENTITY(), '51234789')
GO

-- contraseña en texto plano: ana123
INSERT INTO Usuario (NombreCompleto, Telefono, Email, Contrasena, Estado, FechaAceptacionTerminos) VALUES ('Ana Silva','099555001','ana@gmail.com','$2a$11$gBCgqfKmQddhfbXMRGshMe4nF69Mk9a7jrHd.D/y9u.GKSU/X.Ve.',1, GETDATE())
INSERT INTO Cliente VALUES (SCOPE_IDENTITY(),'52345671')
GO

-- contraseña en texto plano: pedro123
INSERT INTO Usuario (NombreCompleto, Telefono, Email, Contrasena, Estado, FechaAceptacionTerminos) VALUES ('Pedro Gomez','099555002','pedro@gmail.com','$2a$11$goZSAVgW6V3v0Bg1znpb8.uAqZE6NyQhepL.ApLB8XknTIB.iFCbS',1, GETDATE())
INSERT INTO Cliente VALUES (SCOPE_IDENTITY(),'52345672')
GO

-- contraseña en texto plano: lucia123
INSERT INTO Usuario (NombreCompleto, Telefono, Email, Contrasena, Estado, FechaAceptacionTerminos) VALUES ('Lucia Torres','099555003','lucia@gmail.com','$2a$11$2PboZShybaCIs7ScvEB1le5ZjwsumR0Kubxuq5cNRSuatDmCp3Koy',1, GETDATE())
INSERT INTO Cliente VALUES (SCOPE_IDENTITY(),'52345673')
GO

-- contraseña en texto plano: martin123
INSERT INTO Usuario (NombreCompleto, Telefono, Email, Contrasena, Estado, FechaAceptacionTerminos) VALUES ('Martin Suarez','099555004','martin@gmail.com','$2a$11$oeCrp9oFsWt6k3opcbaL4uSPAvWsIPrWx8evwWkvQ5tohrOag9Lv.',1, GETDATE())
INSERT INTO Cliente VALUES (SCOPE_IDENTITY(),'52345674')
GO

-- contraseña en texto plano: vale123
INSERT INTO Usuario (NombreCompleto, Telefono, Email, Contrasena, Estado, FechaAceptacionTerminos) VALUES ('Valentina Castro','099555005','vale@gmail.com','$2a$11$ax4Gx8F/HqL5SJcnU6Q8QeRSq2THhwPVo87FVoVI0P79l3DGMOEma',1, GETDATE())
INSERT INTO Cliente VALUES (SCOPE_IDENTITY(),'52345675')
GO

-- contraseña en texto plano: nico123
INSERT INTO Usuario (NombreCompleto, Telefono, Email, Contrasena, Estado, FechaAceptacionTerminos) VALUES ('Nicolas Pereira','099555006','nico@gmail.com','$2a$11$11Rjnp/8JtVD1cLBC4Ze2ef1GYwikELJWe3YxSLbTHQmQX0SXi1du',1, GETDATE())
INSERT INTO Cliente VALUES (SCOPE_IDENTITY(),'52345676')
GO

-- contraseña en texto plano: sofia123
INSERT INTO Usuario (NombreCompleto, Telefono, Email, Contrasena, Estado, FechaAceptacionTerminos) VALUES ('Sofia Fernandez','099555007','sofia@gmail.com','$2a$11$FCGAxt1fCgnI8.bhrFCSJeV8YUjIk0EEUscyx1dSQHpW.tDkrRxY.',1, GETDATE())
INSERT INTO Cliente VALUES (SCOPE_IDENTITY(),'52345677')
GO

-- contraseña en texto plano: diego123
INSERT INTO Usuario (NombreCompleto, Telefono, Email, Contrasena, Estado, FechaAceptacionTerminos) VALUES ('Diego Alvarez','099555008','diegoa@gmail.com','$2a$11$SVsxbYkDlS8U35NpiDxO4OjsKyWEFUIaNRbOBMHyN6Yso9NxS6Bmu',1, GETDATE())
INSERT INTO Cliente VALUES (SCOPE_IDENTITY(),'52345678')
GO

-- contraseña en texto plano: camila123
INSERT INTO Usuario (NombreCompleto, Telefono, Email, Contrasena, Estado, FechaAceptacionTerminos) VALUES ('Camila Rodriguez','099555009','camila@gmail.com','$2a$11$PsDxWKANCqoOK1fyY2YhnOhRKF5/qsOUc7Cm0j6a..uZRrCaB4ZiO',1, GETDATE())
INSERT INTO Cliente VALUES (SCOPE_IDENTITY(),'52345679')
GO

-- contraseña en texto plano: fede123
INSERT INTO Usuario (NombreCompleto, Telefono, Email, Contrasena, Estado, FechaAceptacionTerminos) VALUES ('Federico Ramos','099555010','fede@gmail.com','$2a$11$8IfVpKVp23e8b5V2xFGUhO7DwiAb.bRfuPCkLAqpNSRTcQNIVH3.6',1, GETDATE())
INSERT INTO Cliente VALUES (SCOPE_IDENTITY(),'52345680')
GO



-- ESCRIBANOS
-- contraseña en texto plano: laura123
INSERT INTO Usuario (NombreCompleto, Telefono, Email, Contrasena, Estado, FechaAceptacionTerminos)
VALUES ('Laura Fernandez', '098111111', 'laura@estudio.com', '$2a$11$llXXvtghK8unawt5eMYblOcvHLjIJMm0.Iztqdl58peMXJ6vexHxu', 1, GETDATE())

INSERT INTO Escribano VALUES (SCOPE_IDENTITY(), 'CP1001', '18 de Julio 1234')
GO

-- contraseña en texto plano: diego123
INSERT INTO Usuario (NombreCompleto, Telefono, Email, Contrasena, Estado, FechaAceptacionTerminos)
VALUES ('Diego Martinez', '098222222', 'diego@estudio.com', '$2a$11$SVsxbYkDlS8U35NpiDxO4OjsKyWEFUIaNRbOBMHyN6Yso9NxS6Bmu', 0, GETDATE())

INSERT INTO Escribano VALUES (SCOPE_IDENTITY(), 'CP1002', 'Bulevar Artigas 456')
GO


-- =========================
-- MARCAS
-- =========================

INSERT INTO Marca (NombreMarca) VALUES 
('Toyota'),
('Chevrolet'),
('Volkswagen'),
('Hyundai'),
('Ford')
GO


-- =========================
-- MODELOS
-- =========================

INSERT INTO Modelo (NombreModelo, IdMarca) VALUES
('Corolla', 1),
('Hilux', 1),

('Onix', 2),
('Cruze', 2),

('Gol', 3),
('Vento', 3),

('HB20', 4),
('Tucson', 4),

('Fiesta', 5),
('Ranger', 5)
GO


-- =========================
-- VEHICULOS
-- =========================

-- Juan Perez vende Corolla
INSERT INTO Vehiculo
(
    Precio,
    Kilometraje,
    Ano,
    CajaDeCambios,
    Motorizacion,
    Descripcion,
    Publicado,
    Latitud,
    Longitud,
    IdModelo,
    IdUsuarioVendedor
)
VALUES
(
    18500,
    54000,
    2020,
    'Automatica',
    'Gas Oil',
    'Toyota Corolla en excelente estado.',
    1,
    -34.9011,
    -56.1645,
    1,
    2
)
GO


-- Maria vende Onix
INSERT INTO Vehiculo
(
    Precio,
    Kilometraje,
    Ano,
    CajaDeCambios,
    Motorizacion,
    Descripcion,
    Publicado,
    Latitud,
    Longitud,
    IdModelo,
    IdUsuarioVendedor
)
VALUES
(
    14500,
    32000,
    2021,
    'Manual',
    'Nafta',
    'Chevrolet Onix muy economico.',
    1,
    -34.9032,
    -56.1881,
    3,
    3
)
GO


-- Carlos vende Ranger
INSERT INTO Vehiculo
(
    Precio,
    Kilometraje,
    Ano,
    CajaDeCambios,
    Motorizacion,
    Descripcion,
    Publicado,
    Latitud,
    Longitud,
    IdModelo,
    IdUsuarioVendedor
)
VALUES
(
    32000,
    75000,
    2019,
    'Automatica',
    'Electrico',
    'Ford Ranger doble cabina.',
    1,
    -34.8870,
    -56.1312,
    10,
    4
)
GO

INSERT INTO Vehiculo
(Precio,Kilometraje,Ano,CajaDeCambios,Motorizacion,Descripcion,Publicado,Latitud,Longitud,IdModelo,IdUsuarioVendedor)
VALUES
(19500,45000,2021,'Automatica','Nafta','Toyota Corolla SEG.',1,-34.901,-56.164,1,7);

INSERT INTO Vehiculo
(Precio,Kilometraje,Ano,CajaDeCambios,Motorizacion,Descripcion,Publicado,Latitud,Longitud,IdModelo,IdUsuarioVendedor)
VALUES
(28000,60000,2020,'Manual','Gas Oil','Toyota Hilux SR.',1,-34.902,-56.165,2,8);

INSERT INTO Vehiculo
(Precio,Kilometraje,Ano,CajaDeCambios,Motorizacion,Descripcion,Publicado,Latitud,Longitud,IdModelo,IdUsuarioVendedor)
VALUES
(15500,30000,2022,'Manual','Nafta','Chevrolet Onix LT.',1,-34.903,-56.166,3,9);

INSERT INTO Vehiculo
(Precio,Kilometraje,Ano,CajaDeCambios,Motorizacion,Descripcion,Publicado,Latitud,Longitud,IdModelo,IdUsuarioVendedor)
VALUES
(21000,55000,2020,'Automatica','Nafta','Chevrolet Cruze LTZ.',1,-34.904,-56.167,4,10);

INSERT INTO Vehiculo
(Precio,Kilometraje,Ano,CajaDeCambios,Motorizacion,Descripcion,Publicado,Latitud,Longitud,IdModelo,IdUsuarioVendedor)
VALUES
(14500,70000,2019,'Manual','Nafta','Volkswagen Gol Trend.',1,-34.905,-56.168,5,11);

INSERT INTO Vehiculo
(Precio,Kilometraje,Ano,CajaDeCambios,Motorizacion,Descripcion,Publicado,Latitud,Longitud,IdModelo,IdUsuarioVendedor)
VALUES
(23000,50000,2021,'Automatica','Nafta','Volkswagen Vento Comfortline.',1,-34.906,-56.169,6,12);

INSERT INTO Vehiculo
(Precio,Kilometraje,Ano,CajaDeCambios,Motorizacion,Descripcion,Publicado,Latitud,Longitud,IdModelo,IdUsuarioVendedor)
VALUES
(17500,42000,2022,'Manual','Nafta','Hyundai HB20 impecable.',1,-34.907,-56.170,7,13);

INSERT INTO Vehiculo
(Precio,Kilometraje,Ano,CajaDeCambios,Motorizacion,Descripcion,Publicado,Latitud,Longitud,IdModelo,IdUsuarioVendedor)
VALUES
(32000,35000,2022,'Automatica','Nafta','Hyundai Tucson Full.',1,-34.908,-56.171,8,14);






-- SOLICITUDES NOTARIALES


-- Maria quiere comprar el Corolla de Juan
INSERT INTO SolicitudNotarial
(
    EstadoSolicitud,
    IdUsuarioCliente,
    IdVehiculo
)
VALUES
(
    1,
    3,
    1
)
GO

INSERT INTO SolicitudEscribano
(
    IdSolicitud,
    IdUsuarioEscribano
)
VALUES
(
    1,
    15
)
GO



-- COMPRA VENTA


-- Solicitud aceptada/finalizada de ejemplo
INSERT INTO SolicitudNotarial
(
    EstadoSolicitud,
    IdUsuarioCliente,
    IdVehiculo
)
VALUES
(
    4,
    2,
    2
)
GO

INSERT INTO SolicitudEscribano
(
    IdSolicitud,
    IdUsuarioEscribano
)
VALUES
(
    2,
    15
)
GO

INSERT INTO CompraVenta
(
    EstadoCompraVenta,
    IdSolicitud
)
VALUES
(
    2,
    2
)
GO




-- CHAT


-- Chat entre Maria y Juan por el Corolla
INSERT INTO Chat (IdVehiculo)
VALUES (1)
GO

INSERT INTO ChatParticipante (IdChat, IdUsuario)
VALUES
(1, 2),
(1, 3)
GO


-- MENSAJES


INSERT INTO Mensaje
(
    IdChat,
    IdUsuarioEmisor,
    Contenido
)
VALUES
(1, 3, 'Hola, el vehiculo sigue disponible?'),

(1, 2, 'Si, sigue disponible.'),

(1, 3, 'Perfecto, me interesa coordinar una visita.')
GO
