USE JMCars
GO

-- =========================
-- MARCAS NUEVAS
-- =========================
INSERT INTO Marca (NombreMarca)
SELECT N.NombreMarca
FROM (VALUES ('Renault'), ('Peugeot'), ('Fiat'), ('Nissan'), ('Honda'), ('Kia'), ('Suzuki'), ('Citroen')) AS N (NombreMarca)
WHERE NOT EXISTS (SELECT 1 FROM Marca M WHERE M.NombreMarca = N.NombreMarca)
GO

-- =========================
-- MODELOS NUEVOS
-- =========================
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
WHERE NOT EXISTS (SELECT 1 FROM Modelo X WHERE X.NombreModelo = N.NombreModelo AND X.IdMarca = M.IdMarca)
GO

-- =========================
-- CLIENTES DE PRUEBA (60)
-- contraseña en texto plano de todos: prueba123
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
       GETDATE()
FROM Nuevos X
WHERE NOT EXISTS (SELECT 1 FROM Usuario U WHERE U.Email = X.Email)

INSERT INTO Cliente (IdUsuario, Cedula)
SELECT U.IdUsuario, CAST(50000000 + U.IdUsuario AS VARCHAR(8))
FROM Usuario U
WHERE U.Email LIKE '%@prueba.com'
  AND NOT EXISTS (SELECT 1 FROM Cliente C WHERE C.IdUsuario = U.IdUsuario)
GO

-- =========================
-- VEHICULOS DE PRUEBA (400)
-- =========================
IF NOT EXISTS (SELECT 1
               FROM Vehiculo V
               INNER JOIN Usuario U ON U.IdUsuario = V.IdUsuarioVendedor
               WHERE U.Email LIKE '%@prueba.com')
BEGIN
    -- 1) Numeros al azar: se calculan UNA vez y se guardan en una tabla temporal
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
           ABS(CHECKSUM(NEWID()) % 1000000) AS r11
    INTO #Azar
    FROM Numeros

    -- 2) Listas numeradas de modelos y de vendedores (clientes de prueba activos)
    SELECT ROW_NUMBER() OVER (ORDER BY IdModelo) AS Pos, IdModelo
    INTO #ListaModelos
    FROM Modelo

    SELECT ROW_NUMBER() OVER (ORDER BY U.IdUsuario) AS Pos, U.IdUsuario
    INTO #ListaVendedores
    FROM Usuario U
    INNER JOIN Cliente C ON C.IdUsuario = U.IdUsuario
    WHERE U.Email LIKE '%@prueba.com' AND U.Estado = 1

    DECLARE @CantModelos INT = (SELECT COUNT(*) FROM #ListaModelos)
    DECLARE @CantVendedores INT = (SELECT COUNT(*) FROM #ListaVendedores)

    -- 3) Inserto los vehiculos
    INSERT INTO Vehiculo (Precio, Kilometraje, Ano, CajaDeCambios, Motorizacion, Descripcion,
                          IdEstadoPublicacion, Latitud, Longitud, IdModelo, IdUsuarioVendedor)
    SELECT
        ((4000 + (X.Anio - 2008) * 900 + A.r2 % 6000) / 100) * 100,
        (2026 - X.Anio) * (6000 + A.r3 % 9000),
        X.Anio,
        CASE WHEN A.r4 % 100 < 35 THEN 'Automatica' ELSE 'Manual' END,
        CASE WHEN A.r5 % 100 < 60 THEN 'Nafta' WHEN A.r5 % 100 < 90 THEN 'Gas Oil' ELSE 'Hibrido' END,
        MA.NombreMarca + ' ' + MO.NombreModelo +
            CHOOSE(A.r6 % 4 + 1, ' en excelente estado, service al dia.',
                                 ' muy bien cuidado, listo para transferir.',
                                 ' con documentacion al dia y poco uso.',
                                 ' impecable, se recibe vehiculo menor.'),
        CASE WHEN A.r7 % 100 < 70 THEN 2
             WHEN A.r7 % 100 < 82 THEN 1
             WHEN A.r7 % 100 < 90 THEN 3
             ELSE 4 END,
        CAST(-34.90 + (A.r8 % 3000 - 1500) / 10000.0 AS DECIMAL(9,6)),
        CAST(-56.16 + (A.r9 % 3000 - 1500) / 10000.0 AS DECIMAL(9,6)),
        ML.IdModelo,
        VL.IdUsuario
    FROM #Azar A
    CROSS APPLY (SELECT 2008 + A.r10 % 17 AS Anio) X
    INNER JOIN #ListaModelos ML ON ML.Pos = A.r1 % @CantModelos + 1
    INNER JOIN Modelo MO ON MO.IdModelo = ML.IdModelo
    INNER JOIN Marca MA ON MA.IdMarca = MO.IdMarca
    INNER JOIN #ListaVendedores VL ON VL.Pos = A.r11 % @CantVendedores + 1

    DROP TABLE #Azar, #ListaModelos, #ListaVendedores
END
GO

-- =========================
-- FOTOS DE LOS VEHICULOS DE PRUEBA
-- (se reutilizan las imagenes que ya estan en JMCarsWeb/wwwroot/images)
-- =========================
IF NOT EXISTS (SELECT 1
               FROM FotoVehiculo F
               INNER JOIN Vehiculo V ON V.IdVehiculo = F.IdVehiculo
               INNER JOIN Usuario U ON U.IdUsuario = V.IdUsuarioVendedor
               WHERE U.Email LIKE '%@prueba.com')
BEGIN
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
END
GO

