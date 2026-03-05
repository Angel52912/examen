SET NOCOUNT ON;
SET XACT_ABORT ON;
USE [Test_utm_AGRZ];
GO

PRINT 'Starting data seeding for [dbo].[Producto] table...';

BEGIN TRY
    BEGIN TRANSACTION;

    -- Limpiar la tabla y resetear el contador de identidad
    TRUNCATE TABLE [dbo].[Producto];
    PRINT 'Table [dbo].[Producto] truncated.';

    SET IDENTITY_INSERT [dbo].[Producto] ON;
    PRINT 'IDENTITY_INSERT for [dbo].[Producto] is ON.';

    --
    -- Datos de Productos (Se generarán en bloques)
    --
INSERT INTO [dbo].[Producto] (ProductoID, Nombre, SKU, Marca, Precio, Stock) VALUES
(1, N'Coca-Cola Original 600ml', '7501000100001', N'Coca-Cola', 18.5000, 150),
(2, N'Pepsi Regular 600ml', '7501000200002', N'Pepsi', 17.0000, 140),
(3, N'Sprite Lima-Lim�n 600ml', '7501000300003', N'Sprite', 17.0000, 130),
(4, N'Fanta Naranja 600ml', '7501000400004', N'Fanta', 16.5000, 120),
(5, N'Boing! Guayaba 500ml', '7501000500005', N'Boing!', 12.0000, 180),
(6, N'Jumex Tetra Brik Manzana 1L', '7501000600006', N'Jumex', 25.0000, 90),
(7, N'Squirt Toronja 600ml', '7501000700007', N'Squirt', 16.0000, 110),
(8, N'Jarrito Mandarina 600ml', '7501000800008', N'Jarritos', 14.5000, 160),
(9, N'Del Valle Naranja 450ml', '7501000900009', N'Del Valle', 15.0000, 100),
(10, N'Topo Chico Mineral 600ml', '7501001000010', N'Topo Chico', 19.0000, 80),
(11, N'Sabritas Original 45g', '7501001100011', N'Sabritas', 16.0000, 200),
(12, N'Doritos Nacho 62g', '7501001200012', N'Doritos', 18.0000, 190),
(13, N'Cheetos Puffs 50g', '7501001300013', N'Cheetos', 15.5000, 170),
(14, N'Takis Fuego 62g', '7501001400014', N'Takis', 19.5000, 160),
(15, N'Ruffles Queso 50g', '7501001500015', N'Ruffles', 17.0000, 180),
(16, N'Chips Saladas 45g', '7501001600016', N'Barcel', 14.0000, 150),
(17, N'Bimbo Pan Blanco Chico', '7501001700017', N'Bimbo', 38.0000, 70),
(18, N'Marinela Gansito Individual', '7501001800018', N'Marinela', 18.0000, 130),
(19, N'Lala Leche Entera 1L', '7501001900019', N'Lala', 29.0000, 100),
(20, N'Alpura Leche Deslactosada 1L', '7501002000020', N'Alpura', 31.0000, 90),
(21, N'Yoghurt Lala Bebible Fresa 220g', '7501002100021', N'Lala', 15.0000, 110),
(22, N'Danone Danonino Fresa 45g', '7501002200022', N'Danone', 8.5000, 250),
(23, N'Queso Panela Fud 200g', '7501002300023', N'Fud', 55.0000, 60),
(24, N'Tortillas de Harina T�a Rosa 12pzas', '7501002400024', N'T�a Rosa', 28.0000, 80),
(25, N'Pan Dulce Bimbo Conchas 2pzas', '7501002500025', N'Bimbo', 25.0000, 90),
(26, N'Nutella Untable 350g', '7501002600026', N'Ferrero', 85.0000, 40),
(27, N'Caf� Soluble Nescaf� Cl�sico 50g', '7501002700027', N'Nescaf�', 35.0000, 120),
(28, N'Az�car Est�ndar Zulka 1kg', '7501002800028', N'Zulka', 32.0000, 100),
(29, N'Aceite Comestible Nutrioli 946ml', '7501002900029', N'Nutrioli', 45.0000, 70),
(30, N'At�n Dolores en Agua 140g', '7501003000030', N'Dolores', 22.0000, 150),
(31, N'Sopa de Pasta La Moderna Fideo', '7501003100031', N'La Moderna', 8.0000, 200),
(32, N'Arroz Verde Valle Super Extra 1kg', '7501003200032', N'Verde Valle', 29.0000, 110),
(33, N'Frijoles Refritos La Coste�a 440g', '7501003300033', N'La Coste�a', 16.0000, 130),
(34, N'Sal Refinada La Fina 1kg', '7501003400034', N'La Fina', 10.0000, 180),
(35, N'Harina de Trigo Tres Estrellas 1kg', '7501003500035', N'Tres Estrellas', 20.0000, 90),
(36, N'Pasta Dental Colgate Triple Acci�n 75ml', '7501003600036', N'Colgate', 28.0000, 100),
(37, N'Jab�n de Tocador Zest Blanco 150g', '7501003700037', N'Zest', 15.0000, 120),
(38, N'Shampoo Head & Shoulders Cl�sico 180ml', '7501003800038', N'Head & Shoulders', 45.0000, 80),
(39, N'Desodorante Speed Stick Barra 60g', '7501003900039', N'Speed Stick', 38.0000, 90),
(40, N'Papel Higi�nico P�talo Doble Hoja 4 rollos', '7501004000040', N'P�talo', 35.0000, 70),
(41, N'Servilletas Faciales Kleenex Paq 100', '7501004100041', N'Kleenex', 20.0000, 110),
(42, N'Rastillo Desechable Gillette Prestobarba', '7501004200042', N'Gillette', 22.0000, 60),
(43, N'Leche Condensada La Lechera 387g', '7501004300043', N'La Lechera', 30.0000, 85),
(44, N'Galletas Mar�as Gamesa 170g', '7501004400044', N'Gamesa', 18.0000, 160),
(45, N'Refresco Mundet Manzana 600ml', '7501004500045', N'Mundet', 15.0000, 130),
(46, N'Chiles Jalape�os La Coste�a Enteros 220g', '7501004600046', N'La Coste�a', 19.0000, 95),
(47, N'Mayonesa McCormick 190g', '7501004700047', N'McCormick', 27.0000, 75),
(48, N'Cereal Zucaritas Kellogg''s 290g', '7501004800048', N'Kellogg''s', 48.0000, 50),
(49, N'Detergente Ariel L�quido 500ml', '7501004900049', N'Ariel', 55.0000, 45),
(50, N'Suavizante Downy Concentrado 400ml', '7501005000050', N'Downy', 42.0000, 55);

INSERT INTO [dbo].[Producto] (ProductoID, Nombre, SKU, Marca, Precio, Stock) VALUES
(51, N'Agua Ciel Mineralizada 600ml', '7501005100051', N'Ciel', 12.0000, 180),
(52, N'Powerade Moras 500ml', '7501005200052', N'Powerade', 22.0000, 100),
(53, N'Monster Energy Original 473ml', '7501005300053', N'Monster Energy', 35.0000, 70),
(54, N'Red Bull Energy Drink 250ml', '7501005400054', N'Red Bull', 42.0000, 60),
(55, N'Agua Bonafont Kids Durazno 300ml', '7501005500055', N'Bonafont', 10.0000, 150),
(56, N'Jugo V8 Vegetales 300ml', '7501005600056', N'V8', 19.0000, 90),
(57, N'Coca-Cola Sin Az�car 600ml', '7501005700057', N'Coca-Cola', 18.5000, 120),
(58, N'Chocomilk Polvo 350g', '7501005800058', N'Chocomilk', 45.0000, 80),
(59, N'Leche Sello Rojo Entera 1L', '7501005900059', N'Sello Rojo', 28.0000, 95),
(60, N'Yoghurt Griego Chobani Fresa 150g', '7501006000060', N'Chobani', 25.0000, 75),
(61, N'Cacahuates Mafer Salados 180g', '7501006100061', N'Mafer', 32.0000, 110),
(62, N'Papas Fritas Pringles Original 124g', '7501006200062', N'Pringles', 40.0000, 90),
(63, N'Galletas Oreo Cl�sicas 137g', '7501006300063', N'Oreo', 22.0000, 160),
(64, N'Barritas Marinela Fresa 60g', '7501006400064', N'Marinela', 12.0000, 200),
(65, N'Palomitas Act II Mantequilla 87g', '7501006500065', N'Act II', 17.0000, 130),
(66, N'Chocolate Carlos V Barra 30g', '7501006600066', N'Carlos V', 10.0000, 250),
(67, N'Crema Dental Sensodyne R�pido Alivio 75ml', '7501006700067', N'Sensodyne', 65.0000, 50),
(68, N'Enjuague Bucal Listerine Cool Mint 250ml', '7501006800068', N'Listerine', 50.0000, 70),
(69, N'Desodorante Dove Roll-on Original 50ml', '7501006900069', N'Dove', 40.0000, 85),
(70, N'Jab�n L�quido para Manos Palmolive Neutro Balance 220ml', '7501007000070', N'Palmolive', 28.0000, 110),
(71, N'Papel Higi�nico Charmin Ultra Suave Gigante 4 rollos', '7501007100071', N'Charmin', 55.0000, 60),
(72, N'Rastrillos Bic Twin Lady 5 pzas', '7501007200072', N'Bic', 30.0000, 80),
(73, N'Toallas Sanitarias Always Noche con Alas 8 pzas', '7501007300073', N'Always', 38.0000, 90),
(74, N'Detergente Foca 1kg', '7501007400074', N'Foca', 25.0000, 100),
(75, N'Suavizante Ensue�o Max Primaveral 850ml', '7501007500075', N'Ensue�o', 30.0000, 70),
(76, N'Cloro Cloralex Regular 950ml', '7501007600076', N'Cloralex', 18.0000, 120),
(77, N'Pinol Limpiador Multiusos 828ml', '7501007700077', N'Pinol', 22.0000, 110),
(78, N'Jabon en Barra Zote Blanco 400g', '7501007800078', N'Zote', 16.0000, 150),
(79, N'Leche Evaporada Carnation Clavel 360g', '7501007900079', N'Carnation', 27.0000, 80),
(80, N'Mermelada McCormick Fresa 270g', '7501008000080', N'McCormick', 35.0000, 60),
(81, N'Cereal Choco Krispis Kellogg''s 290g', '7501008100081', N'Kellogg''s', 48.0000, 50),
(82, N'Pan de Caja Wonder Integral 470g', '7501008200082', N'Wonder', 40.0000, 65),
(83, N'Galletas Saladas Gamesa Crackets 140g', '7501008300083', N'Gamesa', 15.0000, 180),
(84, N'Sopa Maruchan Instant�nea Res 64g', '7501008400084', N'Maruchan', 14.0000, 220),
(85, N'Mayonesa Hellmann''s Real 190g', '7501008500085', N'Hellmann''s', 30.0000, 70),
(86, N'Ketchup Heinz 397g', '7501008600086', N'Heinz', 28.0000, 80),
(87, N'Salsa Valentina Etiqueta Roja 370ml', '7501008700087', N'Valentina', 15.0000, 150),
(88, N'Chile en Polvo Taj�n Cl�sico 142g', '7501008800088', N'Taj�n', 20.0000, 130),
(89, N'Leche de Almendras Silk Unsweetened 946ml', '7501008900089', N'Silk', 38.0000, 55),
(90, N'Yoghurt Lala Entero Natural 1kg', '7501009000090', N'Lala', 40.0000, 60),
(91, N'Refresco Pe�afiel Naranja 600ml', '7501009100091', N'Pe�afiel', 16.0000, 140),
(92, N'Papas Sabritas Adobadas 45g', '7501009200092', N'Sabritas', 16.0000, 190),
(93, N'Churrumais con Limoncito 62g', '7501009300093', N'Sabritas', 18.0000, 170),
(94, N'Cereal Corn Flakes Kellogg''s 300g', '7501009400094', N'Kellogg''s', 45.0000, 60),
(95, N'Pan Tostado Bimbo Cl�sico 210g', '7501009500095', N'Bimbo', 30.0000, 80),
(96, N'Gelatina D''Gari Fresa 120g', '7501009600096', N'D''Gari', 10.0000, 200),
(97, N'Caf� Punta del Cielo Americano Molido 250g', '7501009700097', N'Punta del Cielo', 90.0000, 30),
(98, N'Jab�n de Ba�o Palmolive Naturals Oliva y Aloe 150g', '7501009800098', N'Palmolive', 17.0000, 110),
(99, N'Crema Corporal Nivea Milk Nutritiva 400ml', '7501009900099', N'Nivea', 70.0000, 40),
(100, N'Talco para Pies Rexona Men Xtra Cool 100g', '7501010000100', N'Rexona', 35.0000, 70);


INSERT INTO [dbo].[Producto] (ProductoID, Nombre, SKU, Marca, Precio, Stock) VALUES
(101, N'Agua Purificada Bonafont 1L', '7501010100101', N'Bonafont', 15.0000, 200),
(102, N'Gatorade Lima Lim�n 600ml', '7501010200102', N'Gatorade', 25.0000, 90),
(103, N'Refresco Sangr�a Se�orial 600ml', '7501010300103', N'Sangr�a Se�orial', 16.0000, 130),
(104, N'Leche Santa Clara Semidescremada 1L', '7501010400104', N'Santa Clara', 30.0000, 80),
(105, N'Yoghurt Lala Zero Fresa 220g', '7501010500105', N'Lala', 17.0000, 100),
(106, N'Queso Oaxaca Lala 400g', '7501010600106', N'Lala', 70.0000, 50),
(107, N'Crema Lala �cida 450ml', '7501010700107', N'Lala', 35.0000, 70),
(108, N'Paletas de Hielo Holanda Mango', '7501010800108', N'Holanda', 10.0000, 150),
(109, N'Nieve Holanda Vainilla 1L', '7501010900109', N'Holanda', 60.0000, 40),
(110, N'Pan Bimbo Multigrano 600g', '7501011000110', N'Bimbo', 45.0000, 60),
(111, N'Tortillinas T�a Rosa Integral 12pzas', '7501011100111', N'T�a Rosa', 30.0000, 70),
(112, N'Galletas Pr�ncipe Chocolate 150g', '7501011200112', N'Marinela', 18.0000, 140),
(113, N'Pastelito Submarino Marinela Vainilla', '7501011300113', N'Marinela', 15.0000, 180),
(114, N'Conchas Bimbo Az�car 2pzas', '7501011400114', N'Bimbo', 25.0000, 90),
(115, N'Roles de Canela Bimbo', '7501011500115', N'Bimbo', 28.0000, 80),
(116, N'Aceite Capullo Vegetal 946ml', '7501011600116', N'Capullo', 42.0000, 75),
(117, N'Arroz Schettino Precocido 1kg', '7501011700117', N'Schettino', 32.0000, 90),
(118, N'Frijoles La Sierra Bayos Refritos 400g', '7501011800118', N'La Sierra', 18.0000, 120),
(119, N'Lentejas Verde Valle 500g', '7501011900119', N'Verde Valle', 20.0000, 100),
(120, N'Garbanzos La Coste�a 400g', '7501012000120', N'La Coste�a', 17.0000, 110),
(121, N'Sopa Knorr Tomate con Pollo 90g', '7501012100121', N'Knorr', 10.0000, 180),
(122, N'Cubos Consom� de Pollo Knorr Suiza 8 pzas', '7501012200122', N'Knorr Suiza', 15.0000, 150),
(123, N'Sal de Mesa La Fina Yodada 500g', '7501012300123', N'La Fina', 8.0000, 200),
(124, N'Vinagre Blanco Clemente Jacques 500ml', '7501012400124', N'Clemente Jacques', 15.0000, 100),
(125, N'Salsa Inglesa Crosse & Blackwell 145ml', '7501012500125', N'Crosse & Blackwell', 25.0000, 80),
(126, N'Cloro Puro Cloralex Triple Acci�n 950ml', '7501012600126', N'Cloralex', 20.0000, 100),
(127, N'Detergente en Polvo Ace Regular 1kg', '7501012700127', N'Ace', 35.0000, 80),
(128, N'Suavizante de Telas Downy Libre Enjuague 800ml', '7501012800128', N'Downy', 48.0000, 60),
(129, N'Limpia Vidrios Windex Original 500ml', '7501012900129', N'Windex', 30.0000, 70),
(130, N'Insecticida Raid Casa y Jard�n 400ml', '7501013000130', N'Raid', 55.0000, 40),
(131, N'Desinfectante Lysol Aerosol Original 350ml', '7501013100131', N'Lysol', 60.0000, 35),
(132, N'Escoba Vileda Practica', '7501013200132', N'Vileda', 80.0000, 20),
(133, N'Recogedor Pl�stico Reynera', '7501013300133', N'Reynera', 40.0000, 30),
(134, N'Trapeador Microfibra Scotch-Brite', '7501013400134', N'Scotch-Brite', 120.0000, 15),
(135, N'Guantes de Limpieza Scotch-Brite Medianos', '7501013500135', N'Scotch-Brite', 25.0000, 50),
(136, N'Jabon para Ba�o Escudo Antibacterial 150g', '7501013600136', N'Escudo', 18.0000, 120),
(137, N'Shampoo Savil� Pulpa de S�bila 750ml', '7501013700137', N'Savil�', 35.0000, 80),
(138, N'Acondicionador Herbal Essences Hidrataci�n 300ml', '7501013800138', N'Herbal Essences', 40.0000, 70),
(139, N'Crema Dental Oral-B Complete 75ml', '7501013900139', N'Oral-B', 30.0000, 90),
(140, N'Cepillo Dental Colgate Triple Acci�n', '7501014000140', N'Colgate', 20.0000, 100),
(141, N'Enjuague Bucal Oral-B Menta Suave 250ml', '7501014100141', N'Oral-B', 45.0000, 60),
(142, N'Desodorante Axe Body Spray Black 150ml', '7501014200142', N'Axe', 50.0000, 50),
(143, N'Papel Higi�nico Suavel Doble Hoja 4 rollos', '7501014300143', N'Suavel', 32.0000, 80),
(144, N'Servilletas Blanca Nieves Paq 200', '7501014400144', N'Blanca Nieves', 25.0000, 90),
(145, N'Leche Alpura Cl�sica Entera 1L', '7501014500145', N'Alpura', 29.0000, 100),
(146, N'Yoghurt Griego Fage Total 0% 150g', '7501014600146', N'Fage', 30.0000, 60),
(147, N'Pan de Muerto Bimbo Individual', '7501014700147', N'Bimbo', 20.0000, 70), -- Seasonal
(148, N'Totopos Susalia Horneados 150g', '7501014800148', N'Susalia', 25.0000, 90),
(149, N'Salsa B�falo Cl�sica 150ml', '7501014900149', N'B�falo', 12.0000, 150),
(150, N'Chocolate en Polvo Abuelita 250g', '7501015000150', N'Abuelita', 40.0000, 70);


INSERT INTO [dbo].[Producto] (ProductoID, Nombre, SKU, Marca, Precio, Stock) VALUES
(151, N'Agua mineral Topo Chico Twist Lima 600ml', '7501015100151', N'Topo Chico', 19.5000, 100),
(152, N'Refresco Jarritos Tamarindo 600ml', '7501015200152', N'Jarritos', 14.5000, 150),
(153, N'Jugo del Valle Antiox Granada Ar�ndano 450ml', '7501015300153', N'Del Valle', 22.0000, 80),
(154, N'Coca-Cola Light 600ml', '7501015400154', N'Coca-Cola', 18.0000, 110),
(155, N'Leche Lala Deslactosada Light 1L', '7501015500155', N'Lala', 32.0000, 90),
(156, N'Yoghurt Yoplait Batido Frutas Rojas 145g', '7501015600156', N'Yoplait', 16.0000, 130),
(157, N'Queso Cotija Bafar Rallado 100g', '7501015700157', N'Bafar', 30.0000, 70),
(158, N'Paletas Magnum Chocolate', '7501015800158', N'Magnum', 35.0000, 60),
(159, N'Pan de Caja Oroweat 12 Granos 680g', '7501015900159', N'Oroweat', 55.0000, 45),
(160, N'Galletas Marias Gamesa Azucaradas 170g', '7501016000160', N'Gamesa', 19.0000, 140),
(161, N'Chips Fuego Barcel 62g', '7501016100161', N'Barcel', 17.5000, 160),
(162, N'Cacahuates Japoneses Nishikawa 180g', '7501016200162', N'Nishikawa', 28.0000, 120),
(163, N'Mazap�n de la Rosa Gigante 30g', '7501016300163', N'De la Rosa', 8.0000, 250),
(164, N'Chocolate Hershey''s Barra Leche 40g', '7501016400164', N'Hershey''s', 15.0000, 200),
(165, N'Caf� Punta del Cielo Espresso Molido 250g', '7501016500165', N'Punta del Cielo', 95.0000, 25),
(166, N'Harina de Ma�z Maseca 1kg', '7501016600166', N'Maseca', 22.0000, 100),
(167, N'Az�car Moreno Domino 1kg', '7501016700167', N'Domino', 35.0000, 90),
(168, N'Sal Yodada Elefante 1kg', '7501016800168', N'Elefante', 10.5000, 170),
(169, N'Aceite de Oliva Extra Virgen La Espa�ola 500ml', '7501016900169', N'La Espa�ola', 120.0000, 30),
(170, N'At�n Tuny en Aceite 140g', '7501017000170', N'Tuny', 23.0000, 140),
(171, N'Sopa de Lentejas La Moderna 200g', '7501017100171', N'La Moderna', 12.0000, 160),
(172, N'Frijoles Isadora Enteros Negros 400g', '7501017200172', N'Isadora', 17.0000, 110),
(173, N'Ch�charos Herdez Lata 220g', '7501017300173', N'Herdez', 15.0000, 130),
(174, N'Salsa Picante Cholula Original 150ml', '7501017400174', N'Cholula', 28.0000, 90),
(175, N'Aderezo Ranch Hidden Valley 400ml', '7501017500175', N'Hidden Valley', 40.0000, 60),
(176, N'Champ� Pantene Restauraci�n 400ml', '7501017600176', N'Pantene', 50.0000, 70),
(177, N'Acondicionador Sedal Reparaci�n Instant�nea 350ml', '7501017700177', N'Sedal', 38.0000, 80),
(178, N'Jab�n L�quido Corporal Grisi con Extractos Naturales 400ml', '7501017800178', N'Grisi', 32.0000, 90),
(179, N'Desodorante Lady Speed Stick Gel 65g', '7501017900179', N'Lady Speed Stick', 40.0000, 85),
(180, N'Crema Nivea Lata Azul 150ml', '7501018000180', N'Nivea', 45.0000, 60),
(181, N'Protector Solar Hawaiian Tropic FPS 50 120ml', '7501018100181', N'Hawaiian Tropic', 150.0000, 20),
(182, N'Pa�ales Huggies Supreme Etapa 3 40 pzas', '7501018200182', N'Huggies', 250.0000, 15),
(183, N'Toallitas H�medas Pampers Sensitive 80 pzas', '7501018300183', N'Pampers', 40.0000, 50),
(184, N'Detergente Mas Color Ropa Oscura 1.3L', '7501018400184', N'Mas Color', 70.0000, 40),
(185, N'Limpia Pisos Fabuloso Fresca Lavanda 1L', '7501018500185', N'Fabuloso', 30.0000, 100),
(186, N'Pastillas para Sanitario Harpic Power Plus 1 pza', '7501018600186', N'Harpic', 25.0000, 80),
(187, N'Jabon en Polvo Ariel Doble Poder 1kg', '7501018700187', N'Ariel', 38.0000, 90),
(188, N'Bolsas para Basura Reynera Negras Jumbo 10 pzas', '7501018800188', N'Reynera', 28.0000, 70),
(189, N'Guantes de L�tex Spontex Medianos 1 par', '7501018900189', N'Spontex', 20.0000, 60),
(190, N'Esponja para Trastes Scotch-Brite Cl�sica', '7501019000190', N'Scotch-Brite', 10.0000, 150),
(191, N'Refresco Ciel Exprim 600ml Mandarina', '7501019100191', N'Ciel', 17.0000, 120),
(192, N'Yoghurt Oikos Griego Natural 145g', '7501019200192', N'Oikos', 28.0000, 70),
(193, N'Leche de Coco A de Coco 946ml', '7501019300193', N'A de Coco', 45.0000, 40),
(194, N'Pan de Hot Dog Bimbo 8 pzas', '7501019400194', N'Bimbo', 30.0000, 60),
(195, N'Galletas Oreo Doble Crema 137g', '7501019500195', N'Oreo', 24.0000, 100),
(196, N'Papas Pakeko Saladas 100g', '7501019600196', N'Pakeko', 20.0000, 130),
(197, N'Cereal Nesquik Nestl� 330g', '7501019700197', N'Nestl�', 50.0000, 55),
(198, N'Mermelada Smucker''s Fresa 340g', '7501019800198', N'Smucker''s', 40.0000, 65),
(199, N'Nutella B-ready Barrita 2 pzas', '7501019900199', N'Ferrero', 25.0000, 90),
(200, N'Chocolate Turin Semiamargo 100g', '7501020000200', N'Turin', 55.0000, 40);


INSERT INTO [dbo].[Producto] (ProductoID, Nombre, SKU, Marca, Precio, Stock) VALUES
(201, N'Agua Mineral Pe�afiel Original 600ml', '7501020100201', N'Pe�afiel', 16.0000, 150),
(202, N'Refresco Manzanita Sol 600ml', '7501020200202', N'Manzanita Sol', 15.0000, 140),
(203, N'Jugo del Valle Antiox Mango Durazno 450ml', '7501020300203', N'Del Valle', 22.0000, 85),
(204, N'Coca-Cola Zero Az�car 355ml (lata)', '7501020400204', N'Coca-Cola', 16.0000, 130),
(205, N'Leche Lala Deslactosada Light Fibra 1L', '7501020500205', N'Lala', 33.0000, 80),
(206, N'Yoghurt Yoplait Batido Durazno 145g', '7501020600206', N'Yoplait', 16.0000, 135),
(207, N'Queso Crema Philadelphia Barra 190g', '7501020700207', N'Philadelphia', 40.0000, 60),
(208, N'Helado Holanda Chocolate 1L', '7501020800208', N'Holanda', 60.0000, 35),
(209, N'Pan para Hamburguesa Bimbo 8 pzas', '7501020900209', N'Bimbo', 32.0000, 55),
(210, N'Galletas Saladas Ritz 190g', '7501021000210', N'Ritz', 25.0000, 110),
(211, N'Frituras Sabritas Mix Variado 100g', '7501021100211', N'Sabritas', 28.0000, 100),
(212, N'Cacahuates Planters Salados 200g', '7501021200212', N'Planters', 35.0000, 80),
(213, N'Cereal Special K Original Kellogg''s 290g', '7501021300213', N'Kellogg''s', 52.0000, 45),
(214, N'Barras Multigrano Nature Valley Avena y Miel 6 pzas', '7501021400214', N'Nature Valley', 40.0000, 70),
(215, N'Caf� Soluble Legal Tostado y Molido 100g', '7501021500215', N'Legal', 40.0000, 80),
(216, N'Mermelada Clemente Jacques Durazno 270g', '7501021600216', N'Clemente Jacques', 32.0000, 65),
(217, N'At�n Dolores Premium en Aceite 140g', '7501021700217', N'Dolores', 28.0000, 120),
(218, N'Frijoles La Coste�a Enteros Negros 400g', '7501021800218', N'La Coste�a', 17.0000, 140),
(219, N'Chiles Chipotle Adobados La Coste�a 200g', '7501021900219', N'La Coste�a', 22.0000, 90),
(220, N'Elote Dorado Herdez Lata 220g', '7501022000220', N'Herdez', 16.0000, 100),
(221, N'Pur� de Tomate Del Fuerte 210g', '7501022100221', N'Del Fuerte', 12.0000, 150),
(222, N'Sopa de Champi�ones Campbell''s Lata 290g', '7501022200222', N'Campbell''s', 20.0000, 80),
(223, N'Mayonesa Best Foods Real 300g', '7501022300223', N'Best Foods', 35.0000, 70),
(224, N'Salsa Picante San Luis 145ml', '7501022400224', N'San Luis', 15.0000, 130),
(225, N'Mostaza French''s Cl�sica 226g', '7501022500225', N'French''s', 20.0000, 100),
(226, N'Jabon en Barra Palmolive Naturals Leche y Rosa 150g', '7501022600226', N'Palmolive', 17.0000, 120),
(227, N'Shampoo Head & Shoulders Control Caspa 375ml', '7501022700227', N'Head & Shoulders', 60.0000, 60),
(228, N'Acondicionador Pantene Pro-V Fuerza y Reconstrucci�n 300ml', '7501022800228', N'Pantene', 55.0000, 65),
(229, N'Crema Dental Colgate Total 12 Clean Mint 100ml', '7501022900229', N'Colgate', 35.0000, 90),
(230, N'Cepillo Dental Oral-B Indicator 35 Suave', '7501023000230', N'Oral-B', 22.0000, 110),
(231, N'Enjuague Bucal Colgate Plax Freshmint 250ml', '7501023100231', N'Colgate', 40.0000, 70),
(232, N'Desodorante Speed Stick Clinical Protection 45g', '7501023200232', N'Speed Stick', 70.0000, 40),
(233, N'Papel Higi�nico Regio Rinde M�s 6 rollos', '7501023300233', N'Regio', 45.0000, 80),
(234, N'Toallas Sanitarias Always Ultrafina Flujo Abundante 10 pzas', '7501023400234', N'Always', 42.0000, 70),
(235, N'Pa�ales Huggies UltraConfort Etapa 4 30 pzas', '7501023500235', N'Huggies', 200.0000, 20),
(236, N'Detergente Persil Doble Poder 1.4kg', '7501023600236', N'Persil', 80.0000, 35),
(237, N'Suavizante Suavitel Fresco Amanecer 1.3L', '7501023700237', N'Suavitel', 50.0000, 60),
(238, N'Limpiahornos Easy-Off Heavy Duty 340g', '7501023800238', N'Easy-Off', 65.0000, 30),
(239, N'Aromatizante Ambiental Glade Toque Floral 220ml', '7501023900239', N'Glade', 30.0000, 90),
(240, N'Servilletas Kleenex Grandes 100 pzas', '7501024000240', N'Kleenex', 30.0000, 80),
(241, N'Refresco Jarritos Tamarindo light 600ml', '7501024100241', N'Jarritos', 14.5000, 150),
(242, N'Yoghurt Griego Fage Total 2% Fresa 150g', '7501024200242', N'Fage', 32.0000, 55),
(243, N'Leche de Avena Silk Original 946ml', '7501024300243', N'Silk', 48.0000, 40),
(244, N'Pan de Molde sin Orillas Wonder 450g', '7501024400244', N'Wonder', 40.0000, 60),
(245, N'Galletas Ritz Crackers con Queso 130g', '7501024500245', N'Ritz', 28.0000, 90),
(246, N'Papas Sabritas Habanero 45g', '7501024600246', N'Sabritas', 17.0000, 180),
(247, N'Cereal Cheerios Avena Entera Nestl� 300g', '7501024700247', N'Nestl�', 50.0000, 50),
(248, N'Mermelada Alpura Fresa Light 250g', '7501024800248', N'Alpura', 30.0000, 70),
(249, N'Cajeta Coronado Quemada 330g', '7501024900249', N'Coronado', 45.0000, 60),
(250, N'Chocolate Hersheys Kisses Leche 120g', '7501025000250', N'Hersheys', 40.0000, 80);



    SET IDENTITY_INSERT [dbo].[Producto] OFF;
    PRINT 'IDENTITY_INSERT for [dbo].[Producto] is OFF.';

    COMMIT TRANSACTION;
    PRINT 'Transaction committed successfully.';

    -- Re-seed the identity column to synchronize with the last inserted ID
    DBCC CHECKIDENT ('[dbo].[Producto]', RESEED, 250);
    PRINT 'DBCC CHECKIDENT for [dbo].[Producto] completed, reseeded to 250.';

END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;
    PRINT ERROR_MESSAGE();
    PRINT 'Transaction rolled back due to an error.';
END CATCH;

PRINT 'Data seeding for [dbo].[Producto] table completed.';

