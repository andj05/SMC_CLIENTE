Base de datos::


-- Base de datos para Sistema de Consultorio Médico
CREATE DATABASE IF NOT EXISTS consultorio_medico;
USE consultorio_medico;

-- Tabla de usuarios del sistema
CREATE TABLE usuarios (
    id_usuario INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(50) UNIQUE NOT NULL,
    password VARCHAR(255) NOT NULL,
    nombre_completo VARCHAR(100) NOT NULL,
    rol ENUM('Recepcionista', 'Medico', 'Administrador') NOT NULL,
    email VARCHAR(100),
    telefono VARCHAR(20),
    activo BOOLEAN DEFAULT TRUE,
    fecha_creacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Tabla de pacientes
CREATE TABLE pacientes (
    id_paciente INT AUTO_INCREMENT PRIMARY KEY,
    nombre_completo VARCHAR(100) NOT NULL,
    fecha_nacimiento DATE NOT NULL,
    telefono VARCHAR(20),
    direccion TEXT,
    email VARCHAR(100),
    cedula VARCHAR(20) UNIQUE,
    genero ENUM('Masculino', 'Femenino', 'Otro'),
    estado_civil ENUM('Soltero', 'Casado', 'Divorciado', 'Viudo'),
    ocupacion VARCHAR(50),
    contacto_emergencia VARCHAR(100),
    telefono_emergencia VARCHAR(20),
    fecha_registro TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    activo BOOLEAN DEFAULT TRUE
);

-- Tabla de médicos
CREATE TABLE medicos (
    id_medico INT AUTO_INCREMENT PRIMARY KEY,
    id_usuario INT,
    nombre_completo VARCHAR(100) NOT NULL,
    especialidad VARCHAR(100),
    numero_colegiatura VARCHAR(50),
    telefono VARCHAR(20),
    email VARCHAR(100),
    horario_inicio TIME DEFAULT '08:00:00',
    horario_fin TIME DEFAULT '17:00:00',
    activo BOOLEAN DEFAULT TRUE,
    FOREIGN KEY (id_usuario) REFERENCES usuarios(id_usuario)
);

-- Tabla de citas
CREATE TABLE citas (
    id_cita INT AUTO_INCREMENT PRIMARY KEY,
    id_paciente INT NOT NULL,
    id_medico INT NOT NULL,
    fecha_cita DATE NOT NULL,
    hora_cita TIME NOT NULL,
    motivo_consulta TEXT,
    estado ENUM('Programada', 'Completada', 'Cancelada', 'No_Asistio') DEFAULT 'Programada',
    observaciones TEXT,
    fecha_registro TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    registrado_por INT,
    FOREIGN KEY (id_paciente) REFERENCES pacientes(id_paciente),
    FOREIGN KEY (id_medico) REFERENCES medicos(id_medico),
    FOREIGN KEY (registrado_por) REFERENCES usuarios(id_usuario)
);

-- Tabla de diagnósticos/historial médico
CREATE TABLE diagnosticos (
    id_diagnostico INT AUTO_INCREMENT PRIMARY KEY,
    id_paciente INT NOT NULL,
    id_medico INT NOT NULL,
    id_cita INT,
    fecha_consulta DATE NOT NULL,
    sintomas TEXT,
    diagnostico TEXT,
    tratamiento TEXT,
    medicamentos TEXT,
    observaciones TEXT,
    peso DECIMAL(5,2),
    altura DECIMAL(5,2),
    presion_arterial VARCHAR(20),
    temperatura DECIMAL(4,1),
    fecha_registro TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (id_paciente) REFERENCES pacientes(id_paciente),
    FOREIGN KEY (id_medico) REFERENCES medicos(id_medico),
    FOREIGN KEY (id_cita) REFERENCES citas(id_cita)
);

-- Tabla de pagos
CREATE TABLE pagos (
    id_pago INT AUTO_INCREMENT PRIMARY KEY,
    id_paciente INT NOT NULL,
    id_cita INT,
    fecha_pago DATE NOT NULL,
    monto DECIMAL(10,2) NOT NULL,
    metodo_pago ENUM('Efectivo', 'Tarjeta_Credito', 'Tarjeta_Debito', 'Transferencia', 'Cheque') NOT NULL,
    concepto VARCHAR(200),
    numero_recibo VARCHAR(50),
    estado ENUM('Pendiente', 'Pagado', 'Parcial') DEFAULT 'Pagado',
    observaciones TEXT,
    fecha_registro TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    registrado_por INT,
    FOREIGN KEY (id_paciente) REFERENCES pacientes(id_paciente),
    FOREIGN KEY (id_cita) REFERENCES citas(id_cita),
    FOREIGN KEY (registrado_por) REFERENCES usuarios(id_usuario)
);

-- Tabla de configuración del sistema
CREATE TABLE configuracion (
    id_config INT AUTO_INCREMENT PRIMARY KEY,
    nombre_consultorio VARCHAR(100),
    direccion TEXT,
    telefono VARCHAR(20),
    email VARCHAR(100),
    logo LONGBLOB,
    duracion_cita_minutos INT DEFAULT 30,
    costo_consulta DECIMAL(10,2) DEFAULT 1000.00
);

-- Insertar datos iniciales

-- Usuario administrador por defecto
INSERT INTO usuarios (username, password, nombre_completo, rol, email) VALUES
('admin', SHA2('admin123', 256), 'Administrador del Sistema', 'Administrador', 'admin@consultorio.com'),
('recepcion', SHA2('recep123', 256), 'María García', 'Recepcionista', 'recepcion@consultorio.com'),
('dr.rodriguez', SHA2('med123', 256), 'Dr. Carlos Rodríguez', 'Medico', 'carlos.rodriguez@consultorio.com');

-- Médicos de ejemplo
INSERT INTO medicos (id_usuario, nombre_completo, especialidad, numero_colegiatura, telefono, email) VALUES
(3, 'Dr. Carlos Rodríguez', 'Medicina General', 'MED-001', '809-555-0001', 'carlos.rodriguez@consultorio.com');

-- Configuración inicial
INSERT INTO configuracion (nombre_consultorio, direccion, telefono, email, duracion_cita_minutos, costo_consulta) VALUES
('Consultorio Médico San José', 'Av. Principal #123, Santo Domingo', '809-555-0000', 'info@consultorio.com', 30, 1500.00);

-- Pacientes de ejemplo
INSERT INTO pacientes (nombre_completo, fecha_nacimiento, telefono, direccion, email, cedula, genero) VALUES
('Juan Pérez Martínez', '1985-05-15', '809-555-1001', 'Calle Primera #45, Santo Domingo', 'juan.perez@email.com', '001-0012345-6', 'Masculino'),
('María González López', '1990-08-22', '809-555-1002', 'Av. Independencia #78, Santo Domingo', 'maria.gonzalez@email.com', '001-0023456-7', 'Femenino');

-- Índices para optimizar consultas
CREATE INDEX idx_pacientes_nombre ON pacientes(nombre_completo);
CREATE INDEX idx_pacientes_cedula ON pacientes(cedula);
CREATE INDEX idx_citas_fecha ON citas(fecha_cita, hora_cita);
CREATE INDEX idx_citas_paciente ON citas(id_paciente);
CREATE INDEX idx_citas_medico ON citas(id_medico);
CREATE INDEX idx_diagnosticos_paciente ON diagnosticos(id_paciente);
CREATE INDEX idx_pagos_paciente ON pagos(id_paciente);
CREATE INDEX idx_pagos_fecha ON pagos(fecha_pago);

-- Vistas útiles para reportes

-- Vista de citas con información completa
CREATE VIEW vista_citas_completa AS
SELECT 
    c.id_cita,
    c.fecha_cita,
    c.hora_cita,
    p.nombre_completo AS paciente,
    p.telefono AS telefono_paciente,
    m.nombre_completo AS medico,
    m.especialidad,
    c.motivo_consulta,
    c.estado,
    c.observaciones
FROM citas c
JOIN pacientes p ON c.id_paciente = p.id_paciente
JOIN medicos m ON c.id_medico = m.id_medico
WHERE c.estado != 'Cancelada';

-- Vista de historial médico completo
CREATE VIEW vista_historial_medico AS
SELECT 
    d.id_diagnostico,
    d.fecha_consulta,
    p.nombre_completo AS paciente,
    p.cedula,
    m.nombre_completo AS medico,
    d.sintomas,
    d.diagnostico,
    d.tratamiento,
    d.medicamentos,
    d.peso,
    d.altura,
    d.presion_arterial,
    d.temperatura
FROM diagnosticos d
JOIN pacientes p ON d.id_paciente = p.id_paciente
JOIN medicos m ON d.id_medico = m.id_medico
ORDER BY d.fecha_consulta DESC;

-- Vista de pagos con información del paciente
CREATE VIEW vista_pagos_completa AS
SELECT 
    pg.id_pago,
    pg.fecha_pago,
    p.nombre_completo AS paciente,
    p.cedula,
    pg.monto,
    pg.metodo_pago,
    pg.concepto,
    pg.numero_recibo,
    pg.estado
FROM pagos pg
JOIN pacientes p ON pg.id_paciente = p.id_paciente
ORDER BY pg.fecha_pago DESC;

