USE JMCars
GO

-- =========================
-- MARCAS Y MODELOS NUEVOS
-- =========================
INSERT INTO Marca (NombreMarca)
VALUES ('Renault'), ('Peugeot'), ('Fiat'), ('Nissan'), ('Honda'), ('Kia'), ('Suzuki'), ('Citroen')
GO

INSERT INTO Modelo (NombreModelo, IdMarca)
SELECT N.NombreModelo, M.IdMarca
FROM (VALUES
    ('Sandero', 'Renault'), ('Duster', 'Renault'), ('Kwid', 'Renault'),
    ('208', 'Peugeot'), ('2008', 'Peugeot'), ('Partner', 'Peugeot'),
    ('Cronos', 'Fiat'), ('Argo', 'Fiat'), ('Toro', 'Fiat'),
    ('Versa', 'Nissan'), ('Kicks', 'Nissan'), ('Frontier', 'Nissan'),
    ('Fit', 'Honda'), ('HR-V', 'Honda'), ('Civic', 'Honda'),
    ('Rio', 'Kia'), ('Sportage', 'Kia'), ('Picanto', 'Kia'),
    ('Swift', 'Suzuki'), ('Vitara', 'Suzuki'), ('Jimny', 'Suzuki'),
    ('C3', 'Citroen'), ('C4 Cactus', 'Citroen'), ('Berlingo', 'Citroen')
) AS N (NombreModelo, NombreMarca)
INNER JOIN Marca M ON M.NombreMarca = N.NombreMarca
GO

-- =========================
-- CLIENTES DE PRUEBA (60)
-- contraseña en texto plano de todos: prueba123
-- Fecha de registro repartida entre enero y octubre de 2026
-- =========================
DECLARE @Hash VARCHAR(255) = '$2a$11$Az5f/iOCxm6urw4ZhcsQHumUaZCZ/1vVPZCQvmflbIQ7DYWgY0/TW'

;WITH Numeros AS
(
    SELECT TOP (60) ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS n
    FROM sys.all_objects
),
Nuevos AS
(
    SELECT
        N.n,
        P.Nombre + ' ' + A.Apellido AS NombreCompleto,
        LOWER(P.Nombre) + '.' + LOWER(A.Apellido) + CAST(N.n AS VARCHAR(3)) + '@prueba.com' AS Email
    FROM Numeros N
    INNER JOIN (VALUES (1,'Sofia'),(2,'Martin'),(3,'Valentina'),(4,'Santiago'),(5,'Camila'),(6,'Mateo'),
                       (7,'Florencia'),(8,'Nicolas'),(9,'Agustina'),(10,'Diego'),(11,'Julieta'),(12,'Federico')) AS P (Id, Nombre)
        ON P.Id = (N.n - 1) % 12 + 1
    INNER JOIN (VALUES (1,'Fernandez'),(2,'Garcia'),(3,'Martinez'),(4,'Rodriguez'),(5,'Suarez'),
                       (6,'Acosta'),(7,'Benitez'),(8,'Castro'),(9,'Dominguez'),(10,'Ferreira')) AS A (Id, Apellido)
        ON A.Id = (N.n - 1) / 12 % 10 + 1
)
INSERT INTO Usuario (NombreCompleto, Telefono, Email, Contrasena, Estado, FechaAceptacionTerminos)
SELECT X.NombreCompleto,
       '099' + CAST(600000 + X.n AS VARCHAR(6)),
       X.Email,
       @Hash,
       CASE WHEN X.n % 10 = 0 THEN 0 ELSE 1 END,
       DATEADD(MINUTE, (X.n * 7919) % 400000, '2026-01-01')
FROM Nuevos X

INSERT INTO Cliente (IdUsuario, Cedula)
SELECT U.IdUsuario, CAST(50000000 + U.IdUsuario AS VARCHAR(8))
FROM Usuario U
WHERE U.Email LIKE '%@prueba.com'
GO

-- =========================
-- ESCRIBANOS DE PRUEBA (12, cada 4to inactivo)
-- =========================
DECLARE @HashEsc VARCHAR(255) = '$2a$11$Az5f/iOCxm6urw4ZhcsQHumUaZCZ/1vVPZCQvmflbIQ7DYWgY0/TW'

;WITH Numeros AS
(
    SELECT TOP 12 ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS n
    FROM sys.all_objects
)
INSERT INTO Usuario (NombreCompleto, Telefono, Email, Contrasena, Estado, FechaAceptacionTerminos)
SELECT
    'Esc. ' + CHOOSE((n % 6) + 1, 'Gabriela', 'Ricardo', 'Silvana', 'Alberto', 'Mariana', 'Federico')
            + ' ' + CHOOSE((n % 4) + 1, 'Fernandez', 'Rodriguez', 'Gomez', 'Perez'),
    '098' + CAST(700000 + n AS VARCHAR(10)),
    'escribano' + CAST(n AS VARCHAR(10)) + '@prueba.com',
    @HashEsc,
    CASE WHEN n % 4 = 0 THEN 0 ELSE 1 END,
    DATEADD(MINUTE, (n * 6029) % 330000, '2026-01-01')
FROM Numeros

INSERT INTO Escribano (IdUsuario, NumCajaProf, DireccionEstudio)
SELECT
    U.IdUsuario,
    'CP' + CAST(2000 + CAST(REPLACE(REPLACE(U.Email, 'escribano', ''), '@prueba.com', '') AS INT) AS VARCHAR(10)),
    CHOOSE((CAST(REPLACE(REPLACE(U.Email, 'escribano', ''), '@prueba.com', '') AS INT) % 4) + 1,
           'Av. 18 de Julio 1200', 'Bvar. Artigas 800', 'Colonia 950', 'Av. Italia 3100')
FROM Usuario U
WHERE U.Email LIKE 'escribano%@prueba.com'
GO

-- =========================
-- VEHICULOS DE PRUEBA (400) + fecha de publicacion en RegistroActividad
-- La publicacion es posterior al registro del vendedor.
-- Las Pendientes son recientes (ultimos 9 dias); el resto, repartidas en el año.
-- =========================
;WITH Numeros AS
(
    SELECT TOP (400) ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS n
    FROM sys.all_objects
)
SELECT n,
       ABS(CHECKSUM(NEWID()) % 1000000) AS r1,
       ABS(CHECKSUM(NEWID()) % 1000000) AS r2,
       ABS(CHECKSUM(NEWID()) % 1000000) AS r3,
       ABS(CHECKSUM(NEWID()) % 1000000) AS r4,
       ABS(CHECKSUM(NEWID()) % 1000000) AS r5,
       ABS(CHECKSUM(NEWID()) % 1000000) AS r6,
       ABS(CHECKSUM(NEWID()) % 1000000) AS r7,
       ABS(CHECKSUM(NEWID()) % 1000000) AS r8,
       ABS(CHECKSUM(NEWID()) % 1000000) AS r9,
       ABS(CHECKSUM(NEWID()) % 1000000) AS r10,
       ABS(CHECKSUM(NEWID()) % 1000000) AS r11,
       ABS(CHECKSUM(NEWID()) % 1000000) AS r12
INTO #Azar
FROM Numeros

SELECT ROW_NUMBER() OVER (ORDER BY IdModelo) AS Pos, IdModelo
INTO #ListaModelos
FROM Modelo

SELECT ROW_NUMBER() OVER (ORDER BY U.IdUsuario) AS Pos, U.IdUsuario, U.FechaAceptacionTerminos AS Registro
INTO #ListaVendedores
FROM Usuario U
INNER JOIN Cliente C ON C.IdUsuario = U.IdUsuario
WHERE U.Email LIKE '%@prueba.com' AND U.Estado = 1

DECLARE @CantModelos INT = (SELECT COUNT(*) FROM #ListaModelos)
DECLARE @CantVendedores INT = (SELECT COUNT(*) FROM #ListaVendedores)

-- Plan completo de cada vehiculo (todavia no se inserta nada en Vehiculo)
SELECT
    A.n,
    ((4000 + (X.Anio - 2008) * 900 + A.r2 % 6000) / 100) * 100 AS Precio,
    (2026 - X.Anio) * (6000 + A.r3 % 9000) AS Km,
    X.Anio,
    CASE WHEN A.r4 % 100 < 35 THEN 'Automatica' ELSE 'Manual' END AS Caja,
    CASE WHEN A.r5 % 100 < 60 THEN 'Nafta' WHEN A.r5 % 100 < 90 THEN 'Gas Oil' ELSE 'Hibrido' END AS Motor,
    MA.NombreMarca + ' ' + MO.NombreModelo +
        CHOOSE(A.r6 % 4 + 1, ' en excelente estado, service al dia.',
                             ' muy bien cuidado, listo para transferir.',
                             ' con documentacion al dia y poco uso.',
                             ' impecable, se recibe vehiculo menor.') AS Descripcion,
    X.Estado,
    CAST(-34.90 + (A.r8 % 3000 - 1500) / 10000.0 AS DECIMAL(9,6)) AS Lat,
    CAST(-56.16 + (A.r9 % 3000 - 1500) / 10000.0 AS DECIMAL(9,6)) AS Lon,
    ML.IdModelo,
    VL.IdUsuario AS IdUsuarioVendedor,
    F.FechaPub
INTO #PlanVeh
FROM #Azar A
CROSS APPLY (SELECT 2008 + A.r10 % 17 AS Anio,
                    CASE WHEN A.r7 % 100 < 70 THEN 2
                         WHEN A.r7 % 100 < 82 THEN 1
                         WHEN A.r7 % 100 < 90 THEN 3
                         ELSE 4 END AS Estado) X
INNER JOIN #ListaModelos ML ON ML.Pos = A.r1 % @CantModelos + 1
INNER JOIN Modelo MO ON MO.IdModelo = ML.IdModelo
INNER JOIN Marca MA ON MA.IdMarca = MO.IdMarca
INNER JOIN #ListaVendedores VL ON VL.Pos = A.r11 % @CantVendedores + 1
CROSS APPLY (SELECT CASE WHEN X.Estado = 1
                         THEN DATEADD(MINUTE, -(A.r12 % 12960), GETDATE())
                         ELSE DATEADD(MINUTE, 1440 + A.r12 % (DATEDIFF(MINUTE, VL.Registro, GETDATE()) - 2880), VL.Registro)
                    END AS FechaPub) F

-- Inserto los vehiculos y me guardo que IdVehiculo le toco a cada plan
CREATE TABLE #Mapa (IdVehiculo INT, IdVendedor INT, FechaPub DATETIME)

MERGE Vehiculo AS V
USING #PlanVeh AS P ON 1 = 0
WHEN NOT MATCHED THEN
    INSERT (Precio, Kilometraje, Ano, CajaDeCambios, Motorizacion, Descripcion,
            IdEstadoPublicacion, Latitud, Longitud, IdModelo, IdUsuarioVendedor)
    VALUES (P.Precio, P.Km, P.Anio, P.Caja, P.Motor, P.Descripcion,
            P.Estado, P.Lat, P.Lon, P.IdModelo, P.IdUsuarioVendedor)
OUTPUT inserted.IdVehiculo, P.IdUsuarioVendedor, P.FechaPub INTO #Mapa (IdVehiculo, IdVendedor, FechaPub);

-- Fecha de publicacion: igual que la app, una fila "Crea Publicacion" por vehiculo
INSERT INTO RegistroActividad (FechaHora, IdUsuario, TipoAccion, Detalle)
SELECT FechaPub, IdVendedor, 'Crea Publicacion', 'Vehiculo Id ' + CAST(IdVehiculo AS VARCHAR(10)) + ' creado'
FROM #Mapa

DROP TABLE #Azar, #ListaModelos, #ListaVendedores, #PlanVeh, #Mapa
GO

-- =========================
-- FOTOS DE LOS VEHICULOS DE PRUEBA
-- (se reutilizan las imagenes que ya estan en JMCarsWeb/wwwroot/images)
-- =========================
SELECT V.IdVehiculo,
       ABS(CHECKSUM(NEWID()) % 1000000) AS r1,
       ABS(CHECKSUM(NEWID()) % 1000000) AS r2,
       ABS(CHECKSUM(NEWID()) % 1000000) AS r3
INTO #AzarFoto
FROM Vehiculo V
INNER JOIN Usuario U ON U.IdUsuario = V.IdUsuarioVendedor
WHERE U.Email LIKE '%@prueba.com'

SELECT L.Pos, L.UrlFoto
INTO #ListaFotos
FROM (VALUES
    (1, 'images/toyotacorolla2020.jpg'),
    (2, 'images/toyotacorolla2021.jpg'),
    (3, 'images/Hilux2020.jpg'),
    (4, 'images/onix2022.jpg'),
    (5, 'images/cruze2020.jpg'),
    (6, 'images/gol2019.jpg'),
    (7, 'images/vento2021.jpg'),
    (8, 'images/hb202022.jpg'),
    (9, 'images/tucson2022.jpg')
) AS L (Pos, UrlFoto)

INSERT INTO FotoVehiculo (UrlFoto, IdVehiculo)
SELECT L.UrlFoto, A.IdVehiculo
FROM #AzarFoto A
INNER JOIN (VALUES (1), (2), (3)) AS K (k)
    ON K.k <= CASE WHEN A.r1 % 20 = 0 THEN 0 ELSE 1 + A.r2 % 3 END
INNER JOIN #ListaFotos L
    ON L.Pos = (A.r3 + K.k) % 9 + 1

DROP TABLE #AzarFoto, #ListaFotos
GO

-- =====================================================
-- SOLICITUDES NOTARIALES Y COMPRAVENTAS (120)
-- Un vehiculo Aprobada distinto por solicitud (publicado hace 5+ dias)
-- =====================================================
SELECT ROW_NUMBER() OVER (ORDER BY U.IdUsuario) AS Pos, U.IdUsuario, U.FechaAceptacionTerminos AS Registro
INTO #Clientes
FROM Usuario U JOIN Cliente C ON C.IdUsuario = U.IdUsuario
WHERE U.Email LIKE '%@prueba.com' AND U.Estado = 1

SELECT ROW_NUMBER() OVER (ORDER BY U.IdUsuario) AS Pos, U.IdUsuario, U.FechaAceptacionTerminos AS Registro
INTO #Escribanos
FROM Usuario U JOIN Escribano E ON E.IdUsuario = U.IdUsuario
WHERE U.Email LIKE 'escribano%@prueba.com' AND U.Estado = 1

-- 120 vehiculos Aprobada de vendedores de prueba, con su fecha de publicacion
SELECT TOP 120
    ROW_NUMBER() OVER (ORDER BY NEWID()) AS n,
    V.IdVehiculo, V.IdUsuarioVendedor, R.FechaHora AS Pub,
    ABS(CHECKSUM(NEWID()) % 1000000) AS r1,
    ABS(CHECKSUM(NEWID()) % 1000000) AS r2
INTO #Elegidos
FROM Vehiculo V
JOIN Usuario UV ON UV.IdUsuario = V.IdUsuarioVendedor
JOIN RegistroActividad R ON R.TipoAccion = 'Crea Publicacion'
                        AND R.Detalle = 'Vehiculo Id ' + CAST(V.IdVehiculo AS VARCHAR(10)) + ' creado'
WHERE V.IdEstadoPublicacion = 2
  AND UV.Email LIKE '%@prueba.com'
  AND R.FechaHora <= DATEADD(DAY, -5, GETDATE())
ORDER BY NEWID()

DECLARE @K INT = (SELECT COUNT(*) FROM #Clientes)
DECLARE @E INT = (SELECT COUNT(*) FROM #Escribanos)

-- Quien compra y que escribano atiende (el comprador nunca es el vendedor)
SELECT
    X.n, X.IdVehiculo, X.Pub, X.r1, X.r2,
    CASE WHEN C1.IdUsuario <> X.IdUsuarioVendedor THEN C1.IdUsuario ELSE C2.IdUsuario END AS IdCliente,
    CASE WHEN C1.IdUsuario <> X.IdUsuarioVendedor THEN C1.Registro ELSE C2.Registro END AS RegCliente,
    ES.IdUsuario AS IdEscribano,
    ES.Registro AS RegEscribano,
    X.n % 24 AS Grupo
INTO #Base
FROM #Elegidos X
JOIN #Clientes C1 ON C1.Pos = ((X.n * 7) % @K) + 1
JOIN #Clientes C2 ON C2.Pos = ((X.n * 7 + 1) % @K) + 1
JOIN #Escribanos ES ON ES.Pos = ((X.n - 1) % @E) + 1

-- Fechas: la solicitud nunca es anterior a la publicacion ni al registro de las personas
SELECT
    B.*,
    CASE WHEN B.Grupo <= 3
         THEN DATEADD(MINUTE, -(B.r1 % 1200), GETDATE())
         ELSE DATEADD(MINUTE, B.r1 % (DATEDIFF(MINUTE, D.Desde, GETDATE()) - 1500), D.Desde)
    END AS FechaSol
INTO #Plan
FROM #Base B
CROSS APPLY (SELECT MAX(V) AS Desde
             FROM (VALUES (B.Pub), (B.RegCliente), (B.RegEscribano)) T (V)) D

-- Solicitudes segun Grupo (n % 24):
--  0-3 Pendiente reciente | 4-5 Pendiente vencida | 6-9 Aceptada | 10-17 Finalizada
--  18-20 Rechazada | 21-23 Rechazada tras aceptar
INSERT INTO SolicitudNotarial (FechaSolicitud, EstadoSolicitud, IdUsuarioCliente, IdVehiculo)
SELECT
    FechaSol,
    CASE WHEN Grupo <= 5 THEN 1
         WHEN Grupo <= 9 THEN 2
         WHEN Grupo <= 17 THEN 4
         ELSE 3 END,
    IdCliente, IdVehiculo
FROM #Plan

INSERT INTO SolicitudEscribano (IdSolicitud, IdUsuarioEscribano)
SELECT S.IdSolicitud, P.IdEscribano
FROM #Plan P JOIN SolicitudNotarial S ON S.IdVehiculo = P.IdVehiculo

-- CompraVenta: Aceptada = En Proceso(1), Finalizada = Finalizada(3), grupo 21-23 = Rechazada(2)
INSERT INTO CompraVenta (FechaInicio, EstadoCompraVenta, IdSolicitud)
SELECT
    DATEADD(MINUTE, 60 + P.r2 % 1380, P.FechaSol),
    CASE WHEN P.Grupo <= 9 THEN 1 WHEN P.Grupo <= 17 THEN 3 ELSE 2 END,
    S.IdSolicitud
FROM #Plan P JOIN SolicitudNotarial S ON S.IdVehiculo = P.IdVehiculo
WHERE P.Grupo BETWEEN 6 AND 17 OR P.Grupo >= 21

-- Estado del vehiculo: En Tramite (5) o Vendido (6)
UPDATE V
SET IdEstadoPublicacion = CASE WHEN P.Grupo <= 9 THEN 5 ELSE 6 END
FROM Vehiculo V JOIN #Plan P ON P.IdVehiculo = V.IdVehiculo
WHERE P.Grupo BETWEEN 6 AND 17

-- Actividad del escribano: acepto o rechazo la solicitud (un poco despues de recibirla)
INSERT INTO RegistroActividad (FechaHora, IdUsuario, TipoAccion, Detalle)
SELECT
    DATEADD(MINUTE, 60 + P.r2 % 1380, P.FechaSol),
    P.IdEscribano,
    CASE WHEN P.Grupo BETWEEN 18 AND 20 THEN 'Solicitud Rechazada' ELSE 'Aceptar Solicitud' END,
    CASE WHEN P.Grupo BETWEEN 18 AND 20
         THEN 'La solicitud ' + CAST(S.IdSolicitud AS VARCHAR(10)) + ' fue rechazada'
         ELSE 'Solicitud ' + CAST(S.IdSolicitud AS VARCHAR(10)) + ' aceptada' END
FROM #Plan P JOIN SolicitudNotarial S ON S.IdVehiculo = P.IdVehiculo
WHERE P.Grupo >= 6

DROP TABLE #Clientes, #Escribanos, #Elegidos, #Base, #Plan
GO

-- =====================================================
-- CHATS Y MENSAJES (150 chats, 2 a 8 mensajes c/u)
-- El chat empieza despues de la publicacion y del registro del comprador
-- =====================================================
SELECT ROW_NUMBER() OVER (ORDER BY U.IdUsuario) AS Pos, U.IdUsuario, U.FechaAceptacionTerminos AS Registro
INTO #Clientes
FROM Usuario U JOIN Cliente C ON C.IdUsuario = U.IdUsuario
WHERE U.Email LIKE '%@prueba.com' AND U.Estado = 1

DECLARE @K INT = (SELECT COUNT(*) FROM #Clientes)

-- 150 vehiculos con publicacion de hace 5+ dias (Aprobada, En Tramite o Vendido)
SELECT TOP 150
    ROW_NUMBER() OVER (ORDER BY NEWID()) AS n,
    V.IdVehiculo, V.IdUsuarioVendedor, R.FechaHora AS Pub,
    ABS(CHECKSUM(NEWID()) % 1000000) AS r1,
    ABS(CHECKSUM(NEWID()) % 1000000) AS r2
INTO #Elegidos
FROM Vehiculo V
JOIN Usuario UV ON UV.IdUsuario = V.IdUsuarioVendedor
JOIN RegistroActividad R ON R.TipoAccion = 'Crea Publicacion'
                        AND R.Detalle = 'Vehiculo Id ' + CAST(V.IdVehiculo AS VARCHAR(10)) + ' creado'
WHERE V.IdEstadoPublicacion IN (2, 5, 6)
  AND UV.Email LIKE '%@prueba.com'
  AND R.FechaHora <= DATEADD(DAY, -5, GETDATE())
ORDER BY NEWID()

-- Comprador interesado, cantidad de mensajes y fecha de inicio del chat
SELECT
    X.n, X.IdVehiculo, X.IdUsuarioVendedor, X.r2,
    CASE WHEN C1.IdUsuario <> X.IdUsuarioVendedor THEN C1.IdUsuario ELSE C2.IdUsuario END AS IdComprador,
    2 + (X.n % 7) AS CantMensajes,
    DATEADD(MINUTE, X.r1 % (DATEDIFF(MINUTE, D.Desde, GETDATE()) - 2880), D.Desde) AS Inicio
INTO #Plan
FROM #Elegidos X
JOIN #Clientes C1 ON C1.Pos = ((X.n * 5) % @K) + 1
JOIN #Clientes C2 ON C2.Pos = ((X.n * 5 + 1) % @K) + 1
CROSS APPLY (SELECT MAX(V) AS Desde
             FROM (VALUES (X.Pub),
                          (CASE WHEN C1.IdUsuario <> X.IdUsuarioVendedor THEN C1.Registro ELSE C2.Registro END)) T (V)) D

INSERT INTO Chat (FechaInicio, IdVehiculo)
SELECT Inicio, IdVehiculo FROM #Plan

INSERT INTO ChatParticipante (IdChat, IdUsuario, FechaIngreso)
SELECT C.IdChat, P.IdUsuarioVendedor, P.Inicio
FROM #Plan P JOIN Chat C ON C.IdVehiculo = P.IdVehiculo
UNION ALL
SELECT C.IdChat, P.IdComprador, P.Inicio
FROM #Plan P JOIN Chat C ON C.IdVehiculo = P.IdVehiculo

-- Mensajes: k impar escribe el comprador, k par el vendedor, con pausas distintas entre uno y otro
;WITH K AS
(
    SELECT TOP 8 ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS k FROM sys.all_objects
)
INSERT INTO Mensaje (IdChat, IdUsuarioEmisor, Contenido, FechaHora)
SELECT
    C.IdChat,
    CASE WHEN K.k % 2 = 1 THEN P.IdComprador ELSE P.IdUsuarioVendedor END,
    CASE WHEN K.k % 2 = 1
         THEN CHOOSE(((P.n + K.k) % 6) + 1,
              'Hola, sigue disponible?', 'Cual es el ultimo precio?',
              'Tiene los papeles al dia?', 'Se puede ver el fin de semana?',
              'Acepta permuta?', 'Me interesa, donde se puede ver?')
         ELSE CHOOSE(((P.n + K.k) % 6) + 1,
              'Si, sigue disponible.', 'El precio es conversable.',
              'Si, esta todo al dia.', 'Si, coordinamos por aca.',
              'No acepto permuta.', 'Esta en Montevideo, te paso la direccion.')
    END,
    DATEADD(MINUTE, K.k * (10 + P.r2 % 50), P.Inicio)
FROM #Plan P
JOIN Chat C ON C.IdVehiculo = P.IdVehiculo
JOIN K ON K.k <= P.CantMensajes

DROP TABLE #Clientes, #Elegidos, #Plan
GO

-- =====================================================
-- LOGINS: entre 1 y 6 por usuario activo, siempre despues de su registro
-- =====================================================
;WITH K AS
(
    SELECT TOP 6 ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS k FROM sys.all_objects
)
INSERT INTO RegistroActividad (FechaHora, IdUsuario, TipoAccion, Detalle)
SELECT
    DATEADD(MINUTE, ABS(CHECKSUM(NEWID())) % (DATEDIFF(MINUTE, U.FechaAceptacionTerminos, GETDATE()) - 60),
            U.FechaAceptacionTerminos),
    U.IdUsuario, 'Login', NULL
FROM Usuario U
JOIN K ON K.k <= 1 + (U.IdUsuario % 6)
WHERE U.Email LIKE '%@prueba.com' AND U.Estado = 1
GO

---- =====================================================
---- VERIFICACION FINAL
---- =====================================================
---- 1) Cantidades generales
--SELECT (SELECT COUNT(*) FROM Usuario)            AS Usuarios,
--       (SELECT COUNT(*) FROM Vehiculo)           AS Vehiculos,
--       (SELECT COUNT(*) FROM FotoVehiculo)       AS Fotos,
--       (SELECT COUNT(*) FROM SolicitudNotarial)  AS Solicitudes,
--       (SELECT COUNT(*) FROM CompraVenta)        AS CompraVentas,
--       (SELECT COUNT(*) FROM Chat)               AS Chats,
--       (SELECT COUNT(*) FROM Mensaje)            AS Mensajes,
--       (SELECT COUNT(*) FROM RegistroActividad)  AS Actividad

---- 2) Vehiculos por estado
--SELECT EP.NombreEstado, COUNT(*) AS Vehiculos
--FROM Vehiculo V JOIN EstadoPublicacion EP ON EP.IdEstadoPublicacion = V.IdEstadoPublicacion
--GROUP BY EP.NombreEstado, EP.IdEstadoPublicacion ORDER BY EP.IdEstadoPublicacion

---- 3) Solicitudes por estado, con compraventas y vencidas
--SELECT E.NombreEstado, COUNT(*) AS Solicitudes, COUNT(CV.IdCompraVenta) AS ConCompraVenta,
--       SUM(CASE WHEN S.EstadoSolicitud = 1 AND DATEDIFF(HOUR, S.FechaSolicitud, GETDATE()) >= 24 THEN 1 ELSE 0 END) AS Vencidas
--FROM SolicitudNotarial S
--JOIN EstadosSolicitudNotarial E ON E.IdEstadoSolicitud = S.EstadoSolicitud
--LEFT JOIN CompraVenta CV ON CV.IdSolicitud = S.IdSolicitud
--GROUP BY E.NombreEstado, E.IdEstadoSolicitud ORDER BY E.IdEstadoSolicitud

---- 4) Distribucion por mes (publicaciones, solicitudes, registros)
--SELECT M.Mes,
--       (SELECT COUNT(*) FROM RegistroActividad WHERE TipoAccion = 'Crea Publicacion' AND MONTH(FechaHora) = M.Mes AND YEAR(FechaHora) = 2026) AS Publicaciones,
--       (SELECT COUNT(*) FROM SolicitudNotarial WHERE MONTH(FechaSolicitud) = M.Mes AND YEAR(FechaSolicitud) = 2026) AS Solicitudes,
--       (SELECT COUNT(*) FROM Usuario WHERE MONTH(FechaAceptacionTerminos) = M.Mes AND YEAR(FechaAceptacionTerminos) = 2026) AS Registros
--FROM (VALUES (1),(2),(3),(4),(5),(6),(7),(8),(9),(10)) AS M (Mes)
--ORDER BY M.Mes

---- 5) Control de coherencia: debe devolver 0 filas
--SELECT 'Solicitud antes de la publicacion' AS Problema, S.IdSolicitud AS Id
--FROM SolicitudNotarial S
--JOIN RegistroActividad R ON R.TipoAccion = 'Crea Publicacion'
--                        AND R.Detalle = 'Vehiculo Id ' + CAST(S.IdVehiculo AS VARCHAR(10)) + ' creado'
--WHERE S.FechaSolicitud < R.FechaHora
--UNION ALL
--SELECT 'Compraventa antes de su solicitud', CV.IdCompraVenta
--FROM CompraVenta CV JOIN SolicitudNotarial S ON S.IdSolicitud = CV.IdSolicitud
--WHERE CV.FechaInicio < S.FechaSolicitud
--UNION ALL
--SELECT 'Mensaje en el futuro', M.IdMensaje FROM Mensaje M WHERE M.FechaHora > GETDATE()
--UNION ALL
--SELECT 'Actividad en el futuro', R.IdRegistro FROM RegistroActividad R WHERE R.FechaHora > GETDATE()
--GO