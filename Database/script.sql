-- Creación de la tabla Departamento
CREATE TABLE Departamento (
    Id SERIAL PRIMARY KEY,
    Nombre VARCHAR(255) NOT NULL,
    FechaCreacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FechaModificacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Creación de la tabla Empleados
CREATE TABLE Empleado (
    Id SERIAL PRIMARY KEY,
    Nombres VARCHAR(255) NOT NULL,
    Apellidos VARCHAR(255) NOT NULL,
    Telefono VARCHAR(20),
    Correo VARCHAR(255),
    FechaContratacion TIMESTAMP,
    IdArea INT,
    FechaCreacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FechaModificacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (IdArea) REFERENCES Departamento(Id)
);

-- Creación de la tabla Usuarios
CREATE TABLE Usuario (
    Id SERIAL PRIMARY KEY,
    Usuario VARCHAR(255) UNIQUE NOT NULL,
    Password VARCHAR(255) NOT NULL,
    Estado BOOLEAN,
    IdEmpleado INT,
    FechaCreacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FechaModificacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (IdEmpleado) REFERENCES Empleado(Id)
);

-- Procedimiento almacenado para la creación de empleados
CREATE OR REPLACE PROCEDURE  CrearEmpleado(
    IN p_Nombres TEXT,
    IN p_Apellidos TEXT,
    IN p_Telefono TEXT,
    IN p_Correo TEXT,
    IN p_FechaContratacion TIMESTAMP WITH TIME ZONE,
    IN p_IdArea INT,
    OUT p_EmpleadoId INT
)
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO Empleado (Nombres, Apellidos, Telefono, Correo, FechaContratacion, IdArea)
    VALUES (p_Nombres, p_Apellidos, p_Telefono, p_Correo, p_FechaContratacion, p_IdArea)
    RETURNING Id INTO p_EmpleadoId;
END;
$$;

-- Procedimiento almacenado para la edición de empleados
CREATE OR REPLACE PROCEDURE EditarEmpleado(
    IN p_Id INT,
    IN p_Nombres TEXT,
    IN p_Apellidos TEXT,
    IN p_Telefono TEXT),
    IN p_Correo TEXT,
    IN p_FechaContratacion TIMESTAMP WITH TIME ZONE,
    IN p_IdArea INT
)
LANGUAGE plpgsql
AS $$
BEGIN
    UPDATE Empleado
    SET Nombres = p_Nombres,
        Apellidos = p_Apellidos,
        Telefono = p_Telefono,
        Correo = p_Correo,
        FechaContratacion = p_FechaContratacion,
        IdArea = p_IdArea,
        FechaModificacion = CURRENT_TIMESTAMP
    WHERE Id = p_Id;
END;
$$;

-- Creación de registros en la tabla Departamento
INSERT INTO Departamento (Nombre) VALUES ('Ventas'), ('Recursos Humanos'), ('Tecnología');

-- Ejecutar el procedimiento almacenado para agregar registros a la tabla Empleados
CALL CrearEmpleado('Juan', 'Gómez', '123456789', 'juan.gomez@prueba.com', '2023-01-01', 1, NULL);
CALL CrearEmpleado('María', 'Sánchez', '987654321', 'maria.sanchez@prueba.com', '2023-02-01', 2, NULL);
CALL CrearEmpleado('Miguel', 'López', '456789123', 'miguel.lopez@prueba.com', '2023-03-01', 1, NULL);
CALL CrearEmpleado('Ana', 'Martínez', '654321987', 'ana.martinez@prueba.com', '2023-04-01', 3, NULL);
CALL CrearEmpleado('Pedro', 'Ruiz', '789123456', 'pedro.ruiz@prueba.com', '2023-05-01', 2, NULL);
CALL CrearEmpleado('Laura', 'Hernández', '321654987', 'laura.hernandez@prueba.com', '2023-06-01', 1, NULL);
CALL CrearEmpleado('Carlos', 'García', '987123654', 'carlos.garcia@prueba.com', '2023-07-01', 2, NULL);
CALL CrearEmpleado('Sofía', 'Pérez', '159263487', 'sofia.perez@prueba.com', '2023-08-01', 3, NULL);
CALL CrearEmpleado('Diego', 'González', '369258147', 'diego.gonzalez@prueba.com', '2023-09-01', 1, NULL);
CALL CrearEmpleado('Valentina', 'López', '753159852', 'valentina.lopez@prueba.com', '2023-10-01', 2, NULL);
CALL CrearEmpleado('Martín', 'Fernández', '258963147', 'martin.fernandez@prueba.com', '2023-11-01', 1, NULL);
CALL CrearEmpleado('Isabella', 'Díaz', '147852369', 'isabella.diaz@prueba.com', '2023-12-01', 3, NULL);
CALL CrearEmpleado('Emilio', 'Martínez', '123987456', 'emilio.martinez@prueba.com', '2024-01-01', 1, NULL);
CALL CrearEmpleado('Lucía', 'Rodríguez', '987654123', 'lucia.rodriguez@prueba.com', '2024-02-01', 2, NULL);
CALL CrearEmpleado('Joaquín', 'Sánchez', '654123987', 'joaquin.sanchez@prueba.com', '2024-03-01', 3, NULL);
