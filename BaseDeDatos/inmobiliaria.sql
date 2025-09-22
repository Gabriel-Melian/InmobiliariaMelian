-- phpMyAdmin SQL Dump
-- version 5.2.2
-- https://www.phpmyadmin.net/
--
-- Servidor: localhost:3306
-- Tiempo de generación: 22-09-2025 a las 13:32:36
-- Versión del servidor: 8.0.30
-- Versión de PHP: 8.1.10

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `inmobiliaria`
--
CREATE DATABASE IF NOT EXISTS `inmobiliaria` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci;
USE `inmobiliaria`;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contrato`
--

CREATE TABLE `contrato` (
  `id` int NOT NULL,
  `idInquilino` int NOT NULL,
  `idInmueble` int NOT NULL,
  `idUsuarioCreador` int NOT NULL,
  `fechaCreacion` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `desde` date NOT NULL,
  `hasta` date NOT NULL,
  `precio` decimal(10,0) NOT NULL,
  `estado` int NOT NULL DEFAULT '1',
  `idUsuarioAnulador` int DEFAULT NULL,
  `fechaAnulacion` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Volcado de datos para la tabla `contrato`
--

INSERT INTO `contrato` (`id`, `idInquilino`, `idInmueble`, `idUsuarioCreador`, `fechaCreacion`, `desde`, `hasta`, `precio`, `estado`, `idUsuarioAnulador`, `fechaAnulacion`) VALUES
(2, 1, 1, 2, '2025-09-10 16:26:33', '2025-10-15', '2026-04-15', 300000, 1, NULL, NULL),
(3, 3, 3, 2, '2025-09-18 22:00:06', '2026-01-01', '2026-01-14', 95000, 1, NULL, NULL),
(4, 3, 3, 2, '2025-09-20 17:25:55', '2026-02-12', '2026-04-12', 96000, 1, NULL, NULL),
(5, 4, 2, 2, '2025-09-20 17:31:45', '2026-06-12', '2026-08-12', 120000, 1, NULL, NULL),
(6, 5, 4, 2, '2025-09-21 20:19:13', '2025-12-20', '2026-01-03', 175000, 1, NULL, NULL),
(7, 2, 5, 2, '2025-09-22 10:15:09', '2025-10-01', '2025-10-31', 250000, 1, NULL, NULL);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmueble`
--

CREATE TABLE `inmueble` (
  `id` int NOT NULL,
  `direccion` varchar(255) NOT NULL,
  `latitud` varchar(200) NOT NULL,
  `longitud` varchar(200) NOT NULL,
  `idPropietario` int NOT NULL,
  `idUsoInmueble` int NOT NULL,
  `idTipoInmueble` int NOT NULL,
  `ambientes` int NOT NULL,
  `precio` decimal(10,0) NOT NULL,
  `estado` int NOT NULL DEFAULT '1'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Volcado de datos para la tabla `inmueble`
--

INSERT INTO `inmueble` (`id`, `direccion`, `latitud`, `longitud`, `idPropietario`, `idUsoInmueble`, `idTipoInmueble`, `ambientes`, `precio`, `estado`) VALUES
(1, 'Los Moyes 543', 'sfesfrs21', 'sfsdaawwr22', 2, 1, 1, 4, 100000, 1),
(2, 'Santa Rosa 276', 'asdwa23', 'adsrrt44', 5, 2, 1, 3, 75000, 1),
(3, 'Carpinteria 504', 'ghfjry44', 'fjdliyu41', 4, 1, 4, 4, 66000, 1),
(4, 'Rio Cuarto 276', 'adwaasef', 'rtrgfdgtfh', 6, 2, 3, 8, 175000, 1),
(5, 'Tilisarao 267', 'asdsadaw', 'asdwad', 7, 1, 1, 7, 250000, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilino`
--

CREATE TABLE `inquilino` (
  `id` int NOT NULL,
  `nombre` varchar(80) NOT NULL,
  `apellido` varchar(80) NOT NULL,
  `dni` varchar(20) NOT NULL,
  `email` varchar(100) NOT NULL,
  `telefono` varchar(40) NOT NULL,
  `estado` int NOT NULL DEFAULT '1'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Volcado de datos para la tabla `inquilino`
--

INSERT INTO `inquilino` (`id`, `nombre`, `apellido`, `dni`, `email`, `telefono`, `estado`) VALUES
(1, 'Santino', 'Rueda', '32758463', 'santinoR@gmail.com', '2664577867', 1),
(2, 'Jorge', 'Guzman', '40278564', 'jguzman@gmail.com', '2664788690', 1),
(3, 'Martin', 'Martinez', '42758847', 'mMartinez@gmail.com', '2664378576', 1),
(4, 'Milena', 'Gonzalez', '42865578', 'mileeG@gmail.com', '2664667589', 1),
(5, 'Gustavo', 'Orqueida', '47387765', 'Gustavito@gmail.com', '2664378829', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `multa`
--

CREATE TABLE `multa` (
  `id` int NOT NULL,
  `idContrato` int NOT NULL,
  `fechaAviso` date NOT NULL,
  `fechaTerminacion` date NOT NULL,
  `monto` decimal(10,0) NOT NULL,
  `estado` int NOT NULL DEFAULT '1'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Volcado de datos para la tabla `multa`
--

INSERT INTO `multa` (`id`, `idContrato`, `fechaAviso`, `fechaTerminacion`, `monto`, `estado`) VALUES
(1, 3, '2025-09-20', '2026-12-07', 95000, 1),
(2, 6, '2025-09-21', '2025-12-29', 175000, 1),
(3, 6, '2025-09-22', '2025-12-25', 350000, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pago`
--

CREATE TABLE `pago` (
  `id` int NOT NULL,
  `idContrato` int NOT NULL,
  `numero` int NOT NULL,
  `importe` decimal(10,0) NOT NULL,
  `fecha` date NOT NULL,
  `detalles` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `idUsuarioCrea` int DEFAULT NULL,
  `idUsuarioAnula` int DEFAULT NULL,
  `estado` int NOT NULL DEFAULT '1'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Volcado de datos para la tabla `pago`
--

INSERT INTO `pago` (`id`, `idContrato`, `numero`, `importe`, `fecha`, `detalles`, `idUsuarioCrea`, `idUsuarioAnula`, `estado`) VALUES
(1, 3, 1, 95000, '2025-09-19', 'Mes Septiembre', 2, 2, 2),
(3, 3, 2, 95000, '2025-10-19', 'Mes Octubre', 2, NULL, 1),
(5, 3, 3, 95000, '2025-11-19', 'Mes Noviembre', 2, NULL, 1),
(6, 3, 4, 95000, '2025-12-19', 'Mes Diciembre', 2, NULL, 1),
(7, 4, 1, 96000, '2026-02-12', 'Mes Febrero', 2, NULL, 1),
(8, 4, 2, 96000, '2026-03-12', 'Mes Marzo', 2, NULL, 1),
(9, 5, 1, 120000, '2026-06-12', 'Mes Junio', 2, NULL, 0),
(10, 2, 1, 300000, '2025-10-15', 'Mes de Octubre', 2, NULL, 1),
(11, 3, 5, 95000, '2026-01-19', 'Mes Enero', 2, NULL, 1),
(12, 4, 3, 96000, '2026-04-12', 'Mes Abril', 2, NULL, 1),
(13, 2, 2, 300000, '2025-11-15', 'Mes Noviembre', 2, NULL, 1),
(14, 6, 1, 87500, '2025-12-27', 'Semana 1 Diciembre', 2, NULL, 1),
(15, 6, 2, 87500, '2026-01-03', 'Principios Enero 2026', 2, NULL, 0),
(16, 6, 3, 87500, '2026-02-03', 'Mes Febrero (Adicional)', 2, 2, 2),
(17, 6, 4, 87500, '2026-04-03', 'Mes Marzo (Adicional)', 2, 3, 2);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietario`
--

CREATE TABLE `propietario` (
  `id` int NOT NULL,
  `nombre` varchar(80) NOT NULL,
  `apellido` varchar(80) NOT NULL,
  `dni` varchar(20) NOT NULL,
  `email` varchar(100) NOT NULL,
  `telefono` varchar(60) NOT NULL,
  `direccion` varchar(150) NOT NULL,
  `estado` int NOT NULL DEFAULT '1'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Volcado de datos para la tabla `propietario`
--

INSERT INTO `propietario` (`id`, `nombre`, `apellido`, `dni`, `email`, `telefono`, `direccion`, `estado`) VALUES
(1, 'Juan', 'Perez', '12345678', 'juan@mail.com', '1122334455', 'Calle Falsa 123', 1),
(2, 'Gonzalo', 'Loyola', '39678452', 'gonzalo@gmail.com', '2664783980', 'Av. Siempre Viva 742', 1),
(3, 'Oscar', 'Gomez', '38765783', 'oscar@gmail.com', '2664897657', 'Los Almendros 271', 1),
(4, 'Santiago', 'Calivares', '41765843', 'santi@gmail.com', '2664557843', 'El Rincon 265', 1),
(5, 'Jorge', 'Suarez', '36765821', 'jorge@gmail.com', '2664573877', 'Los Huarpes 2818', 1),
(6, 'Oscar', 'Mercedez', '36275783', 'OscarM@gmail.com', '2664598867', 'Las Catitas 436', 1),
(7, 'Jorge', 'Buscarolo', '34728867', 'JorgeB@gmail.com', '2664675894', 'Av. Siempre s 2645', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tipoinmueble`
--

CREATE TABLE `tipoinmueble` (
  `id` int NOT NULL,
  `valor` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Volcado de datos para la tabla `tipoinmueble`
--

INSERT INTO `tipoinmueble` (`id`, `valor`) VALUES
(1, 'Local'),
(2, 'Deposito'),
(3, 'Casa'),
(4, 'Terreno'),
(5, 'Departamento');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usoinmueble`
--

CREATE TABLE `usoinmueble` (
  `id` int NOT NULL,
  `valor` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Volcado de datos para la tabla `usoinmueble`
--

INSERT INTO `usoinmueble` (`id`, `valor`) VALUES
(1, 'Comercial'),
(2, 'Residencial');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuario`
--

CREATE TABLE `usuario` (
  `id` int NOT NULL,
  `nombre` varchar(100) NOT NULL,
  `apellido` varchar(100) NOT NULL,
  `email` varchar(100) NOT NULL,
  `password` varchar(200) NOT NULL,
  `rol` int NOT NULL,
  `dni` varchar(20) NOT NULL,
  `avatar` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `estado` int NOT NULL DEFAULT '1'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Volcado de datos para la tabla `usuario`
--

INSERT INTO `usuario` (`id`, `nombre`, `apellido`, `email`, `password`, `rol`, `dni`, `avatar`, `estado`) VALUES
(1, 'Gabriel', 'Melian', 'Gabriel86@gmail.com', '$2a$11$SyaiYedLMOUYHO7wNwEmjenCsDYWUS7boknuAOZq7cacLy6uJvxWq', 1, '43487566', '/img/usuarios/1_icono2.jpg', 1),
(2, 'Pedro', 'Gonzalez', 'Drope@gmail.com', '$2a$11$SzALJtlMBz8oAuiuyCSjheAFoWwzqLfG6L86n449rk.nIbIcDa8tq', 2, '44657388', '/img/usuarios/2_icono5.webp', 1),
(3, 'Franco', 'Lucero', 'Franco27@gmail.com', '$2a$11$SRl4Yejg0fGJOfnzF.T5Se03aWM9KIb8mT3kHbFheRC2DpnDcSYWu', 2, '42758490', '/img/usuarios/3_icono4.jpeg', 1);

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD PRIMARY KEY (`id`),
  ADD KEY `idInmueble_inmuebleId` (`idInmueble`),
  ADD KEY `idInquilino_inquilinoId` (`idInquilino`),
  ADD KEY `idUsuarioCreador_usuarioId` (`idUsuarioCreador`),
  ADD KEY `idUsuarioAnulador_usuarioId` (`idUsuarioAnulador`);

--
-- Indices de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD PRIMARY KEY (`id`),
  ADD KEY `idPropietario_propietarioId` (`idPropietario`),
  ADD KEY `idTipo_tipoinmuebleId` (`idTipoInmueble`),
  ADD KEY `idUso_usoinmuebleId` (`idUsoInmueble`);

--
-- Indices de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `dni` (`dni`);

--
-- Indices de la tabla `multa`
--
ALTER TABLE `multa`
  ADD PRIMARY KEY (`id`),
  ADD KEY `idContratoMulta_contratoId` (`idContrato`);

--
-- Indices de la tabla `pago`
--
ALTER TABLE `pago`
  ADD PRIMARY KEY (`id`),
  ADD KEY `idContrato_contratoId` (`idContrato`),
  ADD KEY `idUsuarioCrea_idUsuario` (`idUsuarioCrea`),
  ADD KEY `idUsuarioAnula_idUsuario` (`idUsuarioAnula`);

--
-- Indices de la tabla `propietario`
--
ALTER TABLE `propietario`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `dni` (`dni`);

--
-- Indices de la tabla `tipoinmueble`
--
ALTER TABLE `tipoinmueble`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `usoinmueble`
--
ALTER TABLE `usoinmueble`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `usuario`
--
ALTER TABLE `usuario`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `dni` (`dni`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contrato`
--
ALTER TABLE `contrato`
  MODIFY `id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  MODIFY `id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  MODIFY `id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT de la tabla `multa`
--
ALTER TABLE `multa`
  MODIFY `id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `pago`
--
ALTER TABLE `pago`
  MODIFY `id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=18;

--
-- AUTO_INCREMENT de la tabla `propietario`
--
ALTER TABLE `propietario`
  MODIFY `id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT de la tabla `tipoinmueble`
--
ALTER TABLE `tipoinmueble`
  MODIFY `id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT de la tabla `usoinmueble`
--
ALTER TABLE `usoinmueble`
  MODIFY `id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `usuario`
--
ALTER TABLE `usuario`
  MODIFY `id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD CONSTRAINT `idInmueble_inmuebleId` FOREIGN KEY (`idInmueble`) REFERENCES `inmueble` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `idInquilino_inquilinoId` FOREIGN KEY (`idInquilino`) REFERENCES `inquilino` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `idUsuarioAnulador_usuarioId` FOREIGN KEY (`idUsuarioAnulador`) REFERENCES `usuario` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `idUsuarioCreador_usuarioId` FOREIGN KEY (`idUsuarioCreador`) REFERENCES `usuario` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Filtros para la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD CONSTRAINT `idPropietario_propietarioId` FOREIGN KEY (`idPropietario`) REFERENCES `propietario` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `idTipo_tipoinmuebleId` FOREIGN KEY (`idTipoInmueble`) REFERENCES `tipoinmueble` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `idUso_usoinmuebleId` FOREIGN KEY (`idUsoInmueble`) REFERENCES `usoinmueble` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Filtros para la tabla `multa`
--
ALTER TABLE `multa`
  ADD CONSTRAINT `idContratoMulta_contratoId` FOREIGN KEY (`idContrato`) REFERENCES `contrato` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Filtros para la tabla `pago`
--
ALTER TABLE `pago`
  ADD CONSTRAINT `idContrato_contratoId` FOREIGN KEY (`idContrato`) REFERENCES `contrato` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `idUsuarioAnula_idUsuario` FOREIGN KEY (`idUsuarioAnula`) REFERENCES `usuario` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `idUsuarioCrea_idUsuario` FOREIGN KEY (`idUsuarioCrea`) REFERENCES `usuario` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
