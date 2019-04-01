/*
SQLyog Job Agent v11.33 (64 bit) Copyright(c) Webyog Inc. All Rights Reserved.


MySQL - 5.6.16-log : Database - facturador
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`facturador` /*!40100 DEFAULT CHARACTER SET utf8 COLLATE utf8_spanish_ci */;

USE `facturador`;

/*Table structure for table `alicuota` */

DROP TABLE IF EXISTS `alicuota`;

CREATE TABLE `alicuota` (
  `ali_Id` int(11) NOT NULL,
  `ali_Descripcion` varchar(50) DEFAULT NULL,
  `ali_Porcentaje` decimal(7,4) DEFAULT NULL,
  PRIMARY KEY (`ali_Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `alicuota` */

insert  into `alicuota`(`ali_Id`,`ali_Descripcion`,`ali_Porcentaje`) values (1,'No Gravado','0.0000'),(2,'Exento','0.0000'),(3,'0%','0.0000'),(4,'10,5%','10.5000'),(5,'21%','21.0000'),(6,'27%','27.0000');

/*Table structure for table `barrio` */

DROP TABLE IF EXISTS `barrio`;

CREATE TABLE `barrio` (
  `bar_Id` int(11) NOT NULL AUTO_INCREMENT,
  `ban_Nombre` varchar(50) DEFAULT NULL,
  `loc_Id` int(11) DEFAULT NULL,
  PRIMARY KEY (`bar_Id`),
  KEY `FK_barrio_localidad_loc_Id` (`loc_Id`),
  CONSTRAINT `FK_barrio_localidad_loc_Id` FOREIGN KEY (`loc_Id`) REFERENCES `localidad` (`loc_Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `barrio` */

/*Table structure for table `camposimpresion` */

DROP TABLE IF EXISTS `camposimpresion`;

CREATE TABLE `camposimpresion` (
  `cim_codigo` varchar(10) COLLATE utf8_spanish_ci NOT NULL,
  `cim_campo` varchar(30) COLLATE utf8_spanish_ci NOT NULL,
  `cim_descripcion` varchar(50) COLLATE utf8_spanish_ci DEFAULT NULL,
  `cim_tipo` varchar(30) COLLATE utf8_spanish_ci DEFAULT NULL,
  `cim_formato` varchar(30) COLLATE utf8_spanish_ci DEFAULT NULL,
  `cim_grupo` varchar(30) COLLATE utf8_spanish_ci DEFAULT NULL,
  PRIMARY KEY (`cim_codigo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

/*Data for the table `camposimpresion` */

insert  into `camposimpresion`(`cim_codigo`,`cim_campo`,`cim_descripcion`,`cim_tipo`,`cim_formato`,`cim_grupo`) values ('A00','cen_Id','ID interno comprobante','INT',NULL,NULL),('A01','tco_codigo','Código de tipo de comprobante',NULL,NULL,NULL),('A02','tco_descripcion','Descripción de tipo de comprobante',NULL,NULL,NULL),('A03','pvt_codigo','Código de punto de venta','INT','####',NULL),('A04','pvt_descripcion','Descripción de punto de venta',NULL,NULL,NULL),('A05','cen_numero','Número de comprobante','INT','########',NULL),('A06','cen_fecha','Fecha de comprobante','DATE','dd/MM/yyyy',NULL),('A07','tre_nombre','Nombre de Tipo de responsable',NULL,NULL,NULL),('A08','tre_codigo','Código de Tipo de responsable',NULL,NULL,NULL),('A09','cli_id','ID interno',NULL,NULL,NULL),('A10','cli_Codigo','Código de Cliente',NULL,NULL,NULL),('A11','cli_RazonSocial','Razón Social de Cliente',NULL,NULL,NULL),('A12','cli_CUIT','CUIT de Cliente','INT','##-########-#',NULL),('A13','cli_IngresosBrutos','IIBB de Cliente','INT',NULL,NULL),('A14','cli_direccion','Dirección de Cliente',NULL,NULL,NULL),('A15','cli_localidad','Localidad de Cliente',NULL,NULL,NULL),('A16','cli_provincia','Provincia de Cliente',NULL,NULL,NULL),('A17','lpr_Nombre','Nombre de lista de precios',NULL,NULL,NULL),('A18','cen_neto','Neto del comprobante','DECIMAL','$ 0.00',NULL),('A19','cen_IVA01','Monto IVA 01','DECIMAL','$ 0.00',NULL),('A20','cen_IVA01porc','Porcentaje IVA 01','DECIMAL','0.00 %',NULL),('A21','cen_IVA01neto','Neto IVA 01','DECIMAL','#########.##',NULL),('A22','cen_IVA02','Monto IVA 02','DECIMAL',NULL,NULL),('A23','cen_IVA02porc','Porcentaje IVA 02','DECIMAL',NULL,NULL),('A24','cen_IVA02neto','Neto IVA 02','DECIMAL',NULL,NULL),('A25','cen_IVA03','Monto IVA 03','DECIMAL',NULL,NULL),('A26','cen_IVA03porc','Porcentaje IVA 03','DECIMAL',NULL,NULL),('A27','cen_IVA03neto','Neto IVA 03','DECIMAL',NULL,NULL),('A28','cen_IVA04','Monto IVA 04','DECIMAL',NULL,NULL),('A29','cen_IVA04porc','Porcentaje IVA 04','DECIMAL',NULL,NULL),('A30','cen_IVA04neto','Neto IVA 04','DECIMAL',NULL,NULL),('A31','cen_total','Total comprobante','DECIMAL',NULL,NULL),('A32','cvt_codigo','Código de concepto de venta',NULL,NULL,NULL),('A33','cvt_nombre','Nombre de concepto de venta',NULL,NULL,NULL),('A34','tdo_codigo','Código de tipo de tipo de documento de cliente',NULL,NULL,NULL),('A35','tdo_nombre','Nombre de tipo de tipo de documento de cliente',NULL,NULL,NULL),('A36','cva_codigo','Código de condición de venta',NULL,NULL,NULL),('A37','cva_nombre','Nombre de condición de venta',NULL,NULL,NULL),('A38','cen_numerocompleto','Numero Completo del comprobante',NULL,NULL,NULL),('A39','tco_letra','Letra del Comprobante',NULL,NULL,NULL);

/*Table structure for table `cliente` */

DROP TABLE IF EXISTS `cliente`;

CREATE TABLE `cliente` (
  `cli_Id` int(11) NOT NULL AUTO_INCREMENT,
  `cli_RazonSocial` varchar(255) DEFAULT NULL,
  `cli_CUIT` bigint(20) DEFAULT NULL,
  `cli_IngresosBrutos` int(11) DEFAULT NULL,
  `tre_codigo` int(11) DEFAULT NULL,
  `cli_Activo` bit(1) DEFAULT b'1',
  `cli_FechaAlta` datetime DEFAULT NULL,
  `cli_Codigo` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`cli_Id`),
  KEY `FK_cliente_condicioniva_civ_Id` (`tre_codigo`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

/*Data for the table `cliente` */

insert  into `cliente`(`cli_Id`,`cli_RazonSocial`,`cli_CUIT`,`cli_IngresosBrutos`,`tre_codigo`,`cli_Activo`,`cli_FechaAlta`,`cli_Codigo`) values (1,'Prueba SA',2000000001,NULL,1,'','2014-03-21 20:20:26','0001');

/*Table structure for table `comprobanteencabezado` */

DROP TABLE IF EXISTS `comprobanteencabezado`;

CREATE TABLE `comprobanteencabezado` (
  `cen_Id` int(11) NOT NULL AUTO_INCREMENT,
  `tco_codigo` varchar(10) NOT NULL,
  `pvt_codigo` varchar(10) DEFAULT NULL,
  `cen_numero` int(11) DEFAULT NULL,
  `cen_fecha` datetime DEFAULT NULL,
  `tre_codigo` varchar(10) DEFAULT NULL,
  `tre_nombre` varchar(50) DEFAULT NULL,
  `cli_id` int(11) DEFAULT NULL,
  `cli_Codigo` varchar(255) DEFAULT NULL,
  `cli_RazonSocial` varchar(50) DEFAULT NULL,
  `cli_CUIT` bigint(20) DEFAULT NULL,
  `cli_IngresosBrutos` varchar(50) DEFAULT NULL,
  `cli_direccion` varchar(255) DEFAULT NULL,
  `cli_localidad` varchar(50) DEFAULT NULL,
  `cli_provincia` varchar(50) DEFAULT NULL,
  `lpr_Nombre` varchar(50) DEFAULT NULL,
  `cen_neto` decimal(12,4) DEFAULT NULL,
  `cen_IVA01` decimal(12,4) DEFAULT NULL COMMENT 'monto IVA 1',
  `cen_IVA01porc` decimal(7,4) DEFAULT NULL COMMENT 'porcentaje IVA 1',
  `cen_IVA01neto` decimal(12,4) DEFAULT NULL,
  `cen_IVA02` decimal(12,4) DEFAULT NULL,
  `cen_IVA02porc` decimal(7,4) DEFAULT NULL,
  `cen_IVA02neto` decimal(12,4) DEFAULT NULL,
  `cen_IVA03` decimal(12,4) DEFAULT NULL,
  `cen_IVA03porc` decimal(7,4) DEFAULT NULL,
  `cen_IVA03neto` decimal(12,4) DEFAULT NULL,
  `cen_IVA04` decimal(12,4) DEFAULT NULL,
  `cen_IVA04porc` decimal(7,4) DEFAULT NULL,
  `cen_IVA04neto` decimal(12,4) DEFAULT NULL,
  `cen_total` decimal(12,4) DEFAULT NULL,
  `cvt_codigo` varchar(10) DEFAULT NULL,
  `cvt_nombre` varchar(50) DEFAULT NULL,
  `tdo_codigo` varchar(10) DEFAULT NULL,
  `tdo_nombre` varchar(50) DEFAULT NULL,
  `cva_codigo` varchar(10) DEFAULT NULL,
  `cva_nombre` varchar(50) DEFAULT NULL,
  `cen_cae` varchar(30) DEFAULT NULL,
  `cen_caefechavencimiento` date DEFAULT NULL,
  `cen_afipreq` text,
  `cen_afipres` text,
  `cen_estadoimpresion` tinyint(4) DEFAULT NULL,
  `cen_fechaimpresion` datetime DEFAULT NULL,
  `cen_cae_i2o5` varbinary(150) DEFAULT NULL,
  PRIMARY KEY (`cen_Id`),
  KEY `FK_comprobanteencabezado_puntodeventa_pvt_codigo` (`pvt_codigo`),
  KEY `FK_comprobanteencabezado_tipocomprobante_tco_codigo` (`tco_codigo`),
  CONSTRAINT `FK_comprobanteencabezado_puntodeventa_pvt_codigo` FOREIGN KEY (`pvt_codigo`) REFERENCES `puntodeventa` (`pvt_codigo`),
  CONSTRAINT `FK_comprobanteencabezado_tipocomprobante_tco_codigo` FOREIGN KEY (`tco_codigo`) REFERENCES `tipocomprobante` (`tco_codigo`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8;

/*Data for the table `comprobanteencabezado` */

insert  into `comprobanteencabezado`(`cen_Id`,`tco_codigo`,`pvt_codigo`,`cen_numero`,`cen_fecha`,`tre_codigo`,`tre_nombre`,`cli_id`,`cli_Codigo`,`cli_RazonSocial`,`cli_CUIT`,`cli_IngresosBrutos`,`cli_direccion`,`cli_localidad`,`cli_provincia`,`lpr_Nombre`,`cen_neto`,`cen_IVA01`,`cen_IVA01porc`,`cen_IVA01neto`,`cen_IVA02`,`cen_IVA02porc`,`cen_IVA02neto`,`cen_IVA03`,`cen_IVA03porc`,`cen_IVA03neto`,`cen_IVA04`,`cen_IVA04porc`,`cen_IVA04neto`,`cen_total`,`cvt_codigo`,`cvt_nombre`,`tdo_codigo`,`tdo_nombre`,`cva_codigo`,`cva_nombre`,`cen_cae`,`cen_caefechavencimiento`,`cen_afipreq`,`cen_afipres`,`cen_estadoimpresion`,`cen_fechaimpresion`,`cen_cae_i2o5`) values (1,'01','1',1,'2014-03-21 20:21:40','01','IVA Responsable Inscripto',1,'0001','Prueba SA',20123456781,NULL,'Las palmas 25','cordoba','cordoba',NULL,'865.0000','168.0000','21.0000','800.0000','6.8250','10.5000','65.0000',NULL,NULL,NULL,NULL,NULL,NULL,'1039.8250','03','Productos y Servicios','80','CUIT','CDO','Contado',NULL,NULL,NULL,NULL,NULL,NULL,NULL),(6,'01','1',4,'2014-03-26 00:00:00','01','IVA Responsable Inscripto',0,NULL,'',20000000001,'','','','','Lista Normal','4.2000','2.1000','21.0000','10.0000','2.1000','10.5000','20.0000','0.0000','0.0000','0.0000','0.0000','0.0000','0.0000','34.2000','01','Productos','80','CUIT','CC','Cuenta Corriente','64131182707568','2014-04-05','<?xml version=\"1.0\" encoding=\"UTF-8\"?><FeCAEReq><FeCabReq><CantReg>1</CantReg><PtoVta>1</PtoVta><CbteTipo>01</CbteTipo></FeCabReq><FeDetReq><FECAEDetRequest><Concepto>01</Concepto><DocTipo>80</DocTipo><DocNro>20000000001</DocNro><CbteDesde>4</CbteDesde><CbteHasta>4</CbteHasta><CbteFch>20140326</CbteFch><ImpTotal>34,20</ImpTotal><ImpTotConc>0</ImpTotConc><ImpIVA>4,2000</ImpIVA><ImpOpEx>0</ImpOpEx><ImpNeto>30,0000</ImpNeto><ImpTrib>0</ImpTrib><FchServDesde></FchServDesde><FchServHasta></FchServHasta><FchVtoPago></FchVtoPago><MonId>PES</MonId><MonCotiz>1</MonCotiz><IVA><AlicIva><Id>5</Id><BaseImp>10</BaseImp><Importe>2,1000</Importe></AlicIva><AlicIva><Id>4</Id><BaseImp>20</BaseImp><Importe>2,1000</Importe></AlicIva></IVA></FECAEDetRequest></FeDetReq></FeCAEReq>','<?xml version=\"1.0\"?><FECAESolicitarResponse><FECAESolicitarResult><FeCabResp><Cuit>20170675771</Cuit><CbteTipo>1</CbteTipo><FchProceso>20140326160942</FchProceso><CantReg>1</CantReg><Resultado>A</Resultado></FeCabResp><FeDetResp /><FEDetResponse><Concepto>1</Concepto><DocTipo>80</DocTipo><DocNro>20000000001</DocNro><CbteDesde>4</CbteDesde><CbteHasta>4</CbteHasta><Resultado>A</Resultado><CAE>64131182707568</CAE><CbteFch>20140326</CbteFch><CAEFchVto>20140405</CAEFchVto></FEDetResponse><Obs><Observaciones><Code>10017</Code><Msg>Factura individual, DocTipo: 80, DocNro 20000000001 no se encuentra registrado en los padrones de AFIP.</Msg></Observaciones></Obs></FECAESolicitarResult></FECAESolicitarResponse>',NULL,NULL,NULL),(7,'01','1',10,'2014-03-26 00:00:00','01','IVA Responsable Inscripto',0,NULL,'',20000000001,'','','','','Lista Normal','4.0000','0.8400','21.0000','4.0000','0.0000','0.0000','0.0000','0.0000','0.0000','0.0000','0.0000','0.0000','0.0000','4.8400','01','Productos','80','CUIT','CC','Cuenta Corriente','64131182709293','2014-04-05','<?xml version=\"1.0\" encoding=\"UTF-8\"?><FeCAEReq><FeCabReq><CantReg>1</CantReg><PtoVta>1</PtoVta><CbteTipo>01</CbteTipo></FeCabReq><FeDetReq><FECAEDetRequest><Concepto>01</Concepto><DocTipo>80</DocTipo><DocNro>20000000001</DocNro><CbteDesde>10</CbteDesde><CbteHasta>10</CbteHasta><CbteFch>20140326</CbteFch><ImpTotal>4.84</ImpTotal><ImpTotConc>0</ImpTotConc><ImpIVA>0.84</ImpIVA><ImpOpEx>0</ImpOpEx><ImpNeto>4.00</ImpNeto><ImpTrib>0</ImpTrib><FchServDesde></FchServDesde><FchServHasta></FchServHasta><FchVtoPago></FchVtoPago><MonId>PES</MonId><MonCotiz>1</MonCotiz><IVA><AlicIva><Id>5</Id><BaseImp>4</BaseImp><Importe>0,84</Importe></AlicIva></IVA></FECAEDetRequest></FeDetReq></FeCAEReq>','<?xml version=\"1.0\"?><FECAESolicitarResponse><FECAESolicitarResult><FeCabResp><Cuit>20170675771</Cuit><CbteTipo>1</CbteTipo><FchProceso>20140326223623</FchProceso><CantReg>1</CantReg><Resultado>A</Resultado></FeCabResp><FeDetResp /><FEDetResponse><Concepto>1</Concepto><DocTipo>80</DocTipo><DocNro>20000000001</DocNro><CbteDesde>10</CbteDesde><CbteHasta>10</CbteHasta><Resultado>A</Resultado><CAE>64131182709293</CAE><CbteFch>20140326</CbteFch><CAEFchVto>20140405</CAEFchVto></FEDetResponse><Obs><Observaciones><Code>10017</Code><Msg>Factura individual, DocTipo: 80, DocNro 20000000001 no se encuentra registrado en los padrones de AFIP.</Msg></Observaciones></Obs></FECAESolicitarResult></FECAESolicitarResponse>',NULL,NULL,'(DA6ÙÛ::0@YOBK9MP1XXÇ)\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0'),(8,'01','1',11,'2014-03-26 00:00:00','01','IVA Responsable Inscripto',0,NULL,'',20000000001,'','','','','Lista Normal','4.0000','0.8400','21.0000','4.0000','0.0000','0.0000','0.0000','0.0000','0.0000','0.0000','0.0000','0.0000','0.0000','4.8400','01','Productos','80','CUIT','CC','Cuenta Corriente','64131182709311','2014-04-05','<?xml version=\"1.0\" encoding=\"UTF-8\"?><FeCAEReq><FeCabReq><CantReg>1</CantReg><PtoVta>1</PtoVta><CbteTipo>01</CbteTipo></FeCabReq><FeDetReq><FECAEDetRequest><Concepto>01</Concepto><DocTipo>80</DocTipo><DocNro>20000000001</DocNro><CbteDesde>11</CbteDesde><CbteHasta>11</CbteHasta><CbteFch>20140326</CbteFch><ImpTotal>4.84</ImpTotal><ImpTotConc>0</ImpTotConc><ImpIVA>0.84</ImpIVA><ImpOpEx>0</ImpOpEx><ImpNeto>4.00</ImpNeto><ImpTrib>0</ImpTrib><FchServDesde></FchServDesde><FchServHasta></FchServHasta><FchVtoPago></FchVtoPago><MonId>PES</MonId><MonCotiz>1</MonCotiz><IVA><AlicIva><Id>5</Id><BaseImp>4</BaseImp><Importe>0,84</Importe></AlicIva></IVA></FECAEDetRequest></FeDetReq></FeCAEReq>','<?xml version=\"1.0\"?><FECAESolicitarResponse><FECAESolicitarResult><FeCabResp><Cuit>20170675771</Cuit><CbteTipo>1</CbteTipo><FchProceso>20140326224212</FchProceso><CantReg>1</CantReg><Resultado>A</Resultado></FeCabResp><FeDetResp /><FEDetResponse><Concepto>1</Concepto><DocTipo>80</DocTipo><DocNro>20000000001</DocNro><CbteDesde>11</CbteDesde><CbteHasta>11</CbteHasta><Resultado>A</Resultado><CAE>64131182709311</CAE><CbteFch>20140326</CbteFch><CAEFchVto>20140405</CAEFchVto></FEDetResponse><Obs><Observaciones><Code>10017</Code><Msg>Factura individual, DocTipo: 80, DocNro 20000000001 no se encuentra registrado en los padrones de AFIP.</Msg></Observaciones></Obs></FECAESolicitarResult></FECAESolicitarResponse>',NULL,NULL,'KERBNsOZw5s6OjBAWU9CSzlPPDFYWMOIKQ=='),(9,'01','1',12,'2014-03-26 00:00:00','01','IVA Responsable Inscripto',0,NULL,'',20000000001,'','','','','Lista Normal','4.0000','0.8400','21.0000','4.0000','0.0000','0.0000','0.0000','0.0000','0.0000','0.0000','0.0000','0.0000','0.0000','4.8400','01','Productos','80','CUIT','CC','Cuenta Corriente','64131182709382','2014-04-05','<?xml version=\"1.0\" encoding=\"UTF-8\"?><FeCAEReq><FeCabReq><CantReg>1</CantReg><PtoVta>1</PtoVta><CbteTipo>01</CbteTipo></FeCabReq><FeDetReq><FECAEDetRequest><Concepto>01</Concepto><DocTipo>80</DocTipo><DocNro>20000000001</DocNro><CbteDesde>12</CbteDesde><CbteHasta>12</CbteHasta><CbteFch>20140326</CbteFch><ImpTotal>4.84</ImpTotal><ImpTotConc>0</ImpTotConc><ImpIVA>0.84</ImpIVA><ImpOpEx>0</ImpOpEx><ImpNeto>4.00</ImpNeto><ImpTrib>0</ImpTrib><FchServDesde></FchServDesde><FchServHasta></FchServHasta><FchVtoPago></FchVtoPago><MonId>PES</MonId><MonCotiz>1</MonCotiz><IVA><AlicIva><Id>5</Id><BaseImp>4</BaseImp><Importe>0,84</Importe></AlicIva></IVA></FECAEDetRequest></FeDetReq></FeCAEReq>','<?xml version=\"1.0\"?><FECAESolicitarResponse><FECAESolicitarResult><FeCabResp><Cuit>20170675771</Cuit><CbteTipo>1</CbteTipo><FchProceso>20140326225915</FchProceso><CantReg>1</CantReg><Resultado>A</Resultado></FeCabResp><FeDetResp /><FEDetResponse><Concepto>1</Concepto><DocTipo>80</DocTipo><DocNro>20000000001</DocNro><CbteDesde>12</CbteDesde><CbteHasta>12</CbteHasta><Resultado>A</Resultado><CAE>64131182709382</CAE><CbteFch>20140326</CbteFch><CAEFchVto>20140405</CAEFchVto></FEDetResponse><Obs><Observaciones><Code>10017</Code><Msg>Factura individual, DocTipo: 80, DocNro 20000000001 no se encuentra registrado en los padrones de AFIP.</Msg></Observaciones></Obs></FECAESolicitarResult></FECAESolicitarResponse>',NULL,NULL,'(DA6��::0@YOBK9VF1XX�)');

/*Table structure for table `comprobanteitem` */

DROP TABLE IF EXISTS `comprobanteitem`;

CREATE TABLE `comprobanteitem` (
  `cim_Id` int(11) NOT NULL AUTO_INCREMENT,
  `cen_id` int(11) DEFAULT NULL,
  `pro_Codigo` varchar(255) DEFAULT NULL,
  `pro_Descripcion` varchar(255) DEFAULT NULL,
  `pro_Costo` decimal(12,4) DEFAULT NULL,
  `pro_Precio` decimal(12,4) DEFAULT NULL,
  `cim_Cantidad` decimal(12,4) DEFAULT NULL,
  `cim_descuento` decimal(7,4) DEFAULT NULL,
  `ali_id` int(11) DEFAULT NULL,
  `ali_porcentaje` decimal(7,4) DEFAULT NULL,
  `cim_neto` decimal(12,4) DEFAULT NULL,
  `cim_total` decimal(12,4) DEFAULT NULL,
  PRIMARY KEY (`cim_Id`),
  KEY `FK_comprobanteitem_comprobanteencabezado_cen_Id` (`cen_id`),
  CONSTRAINT `FK_comprobanteitem_comprobanteencabezado_cen_Id` FOREIGN KEY (`cen_id`) REFERENCES `comprobanteencabezado` (`cen_Id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8;

/*Data for the table `comprobanteitem` */

insert  into `comprobanteitem`(`cim_Id`,`cen_id`,`pro_Codigo`,`pro_Descripcion`,`pro_Costo`,`pro_Precio`,`cim_Cantidad`,`cim_descuento`,`ali_id`,`ali_porcentaje`,`cim_neto`,`cim_total`) values (1,1,NULL,'Servicio de reparacion de antena','300.0000','300.0000','1.0000',NULL,5,'21.0000','300.0000','363.0000'),(2,1,NULL,'convertidor de oxido','10.0000','10.0000','2.0000',NULL,4,'10.5000','20.0000','22.1000'),(3,NULL,NULL,'mouse inalambrico','15.0000','15.0000','3.0000',NULL,4,'10.5000','45.0000','49.7250'),(4,NULL,NULL,'limpieza general piso','500.0000','500.0000','1.0000',NULL,5,'21.0000','500.0000','605.0000'),(5,6,'','d','10.0000','10.0000','1.0000','0.0000',0,'21.0000','10.0000','12.1000'),(6,6,'','dd','10.0000','10.0000','2.0000','0.0000',0,'10.5000','20.0000','22.1000'),(7,8,'','2','2.0000','2.0000','2.0000','0.0000',0,'21.0000','4.0000','4.8400');

/*Table structure for table `conceptoventa` */

DROP TABLE IF EXISTS `conceptoventa`;

CREATE TABLE `conceptoventa` (
  `cvt_codigo` varchar(10) COLLATE utf8_spanish_ci NOT NULL,
  `cvt_nombre` varchar(50) COLLATE utf8_spanish_ci DEFAULT NULL,
  PRIMARY KEY (`cvt_codigo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

/*Data for the table `conceptoventa` */

insert  into `conceptoventa`(`cvt_codigo`,`cvt_nombre`) values ('01','Productos'),('02','Servicios'),('03','Productos y Servicios');

/*Table structure for table `condicionventa` */

DROP TABLE IF EXISTS `condicionventa`;

CREATE TABLE `condicionventa` (
  `cva_codigo` varchar(10) NOT NULL,
  `cva_nombre` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`cva_codigo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `condicionventa` */

insert  into `condicionventa`(`cva_codigo`,`cva_nombre`) values ('CC','Cuenta Corriente'),('CDO','Contado');

/*Table structure for table `contacto` */

DROP TABLE IF EXISTS `contacto`;

CREATE TABLE `contacto` (
  `con_Id` int(11) NOT NULL AUTO_INCREMENT,
  `con_Nombre` varchar(50) DEFAULT NULL,
  `tco_Id` int(11) DEFAULT NULL,
  `cli_Id` int(11) DEFAULT NULL,
  PRIMARY KEY (`con_Id`),
  KEY `FK_contacto_cliente_cli_Id` (`cli_Id`),
  KEY `FK_contacto_tipocontacto_tco_Id` (`tco_Id`),
  CONSTRAINT `FK_contacto_cliente_cli_Id` FOREIGN KEY (`cli_Id`) REFERENCES `cliente` (`cli_Id`),
  CONSTRAINT `FK_contacto_tipocontacto_tco_Id` FOREIGN KEY (`tco_Id`) REFERENCES `tipocontacto` (`tco_Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `contacto` */

/*Table structure for table `direccion` */

DROP TABLE IF EXISTS `direccion`;

CREATE TABLE `direccion` (
  `dir_Id` int(11) NOT NULL AUTO_INCREMENT,
  `dir_Calle` varchar(255) DEFAULT NULL,
  `dir_Altura` varchar(255) DEFAULT NULL,
  `dir_Torre` varchar(255) DEFAULT NULL,
  `dir_Piso` varchar(255) DEFAULT NULL,
  `dir_Departamento` varchar(255) DEFAULT NULL,
  `dir_CodigoPostal` varchar(255) DEFAULT NULL,
  `bar_Id` int(11) DEFAULT NULL,
  PRIMARY KEY (`dir_Id`),
  KEY `FK_direccion_barrio_bar_Id` (`bar_Id`),
  CONSTRAINT `FK_direccion_barrio_bar_Id` FOREIGN KEY (`bar_Id`) REFERENCES `barrio` (`bar_Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `direccion` */

/*Table structure for table `direccionxcliente` */

DROP TABLE IF EXISTS `direccionxcliente`;

CREATE TABLE `direccionxcliente` (
  `dir_Id` int(11) NOT NULL,
  `cli_Id` int(11) NOT NULL,
  `dxc_observaciones` text,
  `dxc_principal` bit(1) DEFAULT NULL,
  PRIMARY KEY (`dir_Id`,`cli_Id`),
  KEY `FK_direccionxcliente_cliente_cli_Id` (`cli_Id`),
  CONSTRAINT `FK_direccionxcliente_cliente_cli_Id` FOREIGN KEY (`cli_Id`) REFERENCES `cliente` (`cli_Id`),
  CONSTRAINT `FK_direccionxcliente_direccion_dir_Id` FOREIGN KEY (`dir_Id`) REFERENCES `direccion` (`dir_Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `direccionxcliente` */

/*Table structure for table `listaprecio` */

DROP TABLE IF EXISTS `listaprecio`;

CREATE TABLE `listaprecio` (
  `lpr_Id` int(11) NOT NULL AUTO_INCREMENT,
  `lpr_Nombre` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`lpr_Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

/*Data for the table `listaprecio` */

insert  into `listaprecio`(`lpr_Id`,`lpr_Nombre`) values (1,'Lista Normal'),(2,'Publico 1'),(3,'Mayorista 1');

/*Table structure for table `localidad` */

DROP TABLE IF EXISTS `localidad`;

CREATE TABLE `localidad` (
  `loc_Id` int(11) NOT NULL AUTO_INCREMENT,
  `loc_Nombre` varchar(50) DEFAULT NULL,
  `pro_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`loc_Id`),
  KEY `FK_localidad_provincia_pro_Id` (`pro_id`),
  CONSTRAINT `FK_localidad_provincia_pro_Id` FOREIGN KEY (`pro_id`) REFERENCES `provincia` (`pro_Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `localidad` */

/*Table structure for table `numeroxpuntoventa` */

DROP TABLE IF EXISTS `numeroxpuntoventa`;

CREATE TABLE `numeroxpuntoventa` (
  `tco_codigo` varchar(10) COLLATE utf8_spanish_ci NOT NULL,
  `pvt_codigo` varchar(10) COLLATE utf8_spanish_ci NOT NULL,
  `npv_ultimo` int(11) DEFAULT NULL,
  PRIMARY KEY (`tco_codigo`,`pvt_codigo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

/*Data for the table `numeroxpuntoventa` */

/*Table structure for table `pais` */

DROP TABLE IF EXISTS `pais`;

CREATE TABLE `pais` (
  `pai_Id` int(11) NOT NULL AUTO_INCREMENT,
  `pai_Nombre` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`pai_Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `pais` */

/*Table structure for table `producto` */

DROP TABLE IF EXISTS `producto`;

CREATE TABLE `producto` (
  `pro_Id` int(11) NOT NULL,
  `pro_Codigo` varchar(255) DEFAULT NULL,
  `pro_CodigoFabricante` varchar(255) DEFAULT NULL,
  `pro_Descripcion` varchar(255) DEFAULT NULL,
  `pro_Costo` decimal(12,4) DEFAULT NULL,
  PRIMARY KEY (`pro_Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `producto` */

/*Table structure for table `productoxlistaprecio` */

DROP TABLE IF EXISTS `productoxlistaprecio`;

CREATE TABLE `productoxlistaprecio` (
  `lpr_Id` int(11) NOT NULL,
  `pro_Id` int(11) NOT NULL,
  `pxl_Precio` decimal(12,4) DEFAULT NULL,
  PRIMARY KEY (`lpr_Id`,`pro_Id`),
  KEY `FK_productoxlistaprecio_producto_pro_Id` (`pro_Id`),
  CONSTRAINT `FK_productoxlistaprecio_listaprecio_lpr_Id` FOREIGN KEY (`lpr_Id`) REFERENCES `listaprecio` (`lpr_Id`),
  CONSTRAINT `FK_productoxlistaprecio_producto_pro_Id` FOREIGN KEY (`pro_Id`) REFERENCES `producto` (`pro_Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `productoxlistaprecio` */

/*Table structure for table `provincia` */

DROP TABLE IF EXISTS `provincia`;

CREATE TABLE `provincia` (
  `pro_Id` int(11) NOT NULL AUTO_INCREMENT,
  `pro_Nombre` varchar(50) DEFAULT NULL,
  `pai_Id` int(11) DEFAULT NULL,
  PRIMARY KEY (`pro_Id`),
  KEY `FK_provincia_pais_pai_Id` (`pai_Id`),
  CONSTRAINT `FK_provincia_pais_pai_Id` FOREIGN KEY (`pai_Id`) REFERENCES `pais` (`pai_Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `provincia` */

/*Table structure for table `puntodeventa` */

DROP TABLE IF EXISTS `puntodeventa`;

CREATE TABLE `puntodeventa` (
  `pvt_codigo` varchar(10) NOT NULL,
  `pvt_descripcion` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`pvt_codigo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `puntodeventa` */

insert  into `puntodeventa`(`pvt_codigo`,`pvt_descripcion`) values ('1','Casa Central'),('2','Sucursal 1');

/*Table structure for table `tipocomprobante` */

DROP TABLE IF EXISTS `tipocomprobante`;

CREATE TABLE `tipocomprobante` (
  `tco_codigo` varchar(10) NOT NULL,
  `tco_descripcion` varchar(255) DEFAULT NULL,
  `tco_letra` char(1) DEFAULT NULL,
  `tco_activo` bit(1) DEFAULT NULL,
  PRIMARY KEY (`tco_codigo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `tipocomprobante` */

insert  into `tipocomprobante`(`tco_codigo`,`tco_descripcion`,`tco_letra`,`tco_activo`) values ('01','Facturas A','A',''),('02','Notas de Débito A','A','\0'),('03','Notas de Crédito A','A','\0'),('04','Recibos A','A','\0'),('05','Notas de Venta al contado A','A','\0'),('06','Facturas B','B',''),('07','Notas de Débito B','B','\0'),('08','Notas de Crédito B','B','\0'),('09','Recibos B','B','\0'),('10','Notas de Venta al contado B','B','\0'),('11','Facturas C','C','\0'),('12','Notas de Débito C','C','\0'),('13','Notas de Crédito C','C','\0'),('14','Documento aduanero',NULL,'\0'),('15','Recibos C','C','\0'),('16','Notas de Venta al contado C','C','\0'),('19','Facturas de Exportación',NULL,'\0'),('20','Notas de Débito por operaciones con el exterior',NULL,'\0'),('21','Notas de Crédito por operaciones con el exterior',NULL,'\0'),('22','Facturas  Permiso Exportación simplificado  Dto. 855/97',NULL,'\0'),('30','Comprobantes de compra de bienes usados',NULL,'\0'),('34','Comprobantes A del Anexo I, Apartado A, inc. f), R.G. N° 1415','A','\0'),('35','Comprobantes B del Anexo I, Apartado A, inc. f), R.G. N° 1415','B','\0'),('36','Comprobantes C del Anexo I, Apartado A, inc. f), R.G. N° 1415','C','\0'),('37','Notas de Débito o documento equivalente que cumplan con la R.G. N° 1415',NULL,'\0'),('38','Notas de Crédito o documento equivalente que cumplan con la R.G. N° 1415',NULL,'\0'),('39','Otros comprobantes A que cumplan con la R.G. N° 1415','A','\0'),('40','Otros comprobantes B que cumplan con la R.G. N° 1415','B','\0'),('41','Otros comprobantes C que cumplan con la R.G. N° 1415','C','\0'),('51','Facturas M','M','\0'),('52','Notas de Débito M','M','\0'),('53','Notas de Crédito M','M','\0'),('54','Recibos M','M','\0'),('55','Notas de Venta al contado M','M','\0'),('56','Comprobantes M del Anexo I, Apartado A, inc. f), R.G. N° 1415','M','\0'),('57','Otros comprobantes M que cumplan con la R.G. N° 1415','M','\0'),('58','Cuenta de Venta y Líquido producto M','M','\0'),('59','Liquidación M','M','\0'),('60','Cuenta de Venta y Líquido producto A','A','\0'),('61','Cuenta de Venta y Líquido producto B','B','\0'),('62','Cuenta de Venta y Líquido producto C','C','\0'),('63','Liquidación A','A','\0'),('64','Liquidación B','B','\0'),('65','Liquidación C','C','\0'),('80','Comprobante diario de cierre (zeta)',NULL,'\0'),('81','TiqueFactura \"A\"','A','\0'),('82','TiqueFactura \"B\"','B','\0'),('83','Tique',NULL,'\0'),('84','Comprobante/Factura de servicios públicos',NULL,'\0'),('85','Nota de Crédito  servicios públicos',NULL,'\0'),('86','Nota de Débito  servicios públicos',NULL,'\0'),('87','Otros comprobantes  servicios del exterior',NULL,'\0'),('88','Otros comprobantes  documentos exceptuados',NULL,'\0'),('89','Otros comprobantes  documentos exceptuados  Notas de Débito',NULL,'\0'),('90','Otros comprobantes  documentos exceptuados  Notas de Crédito',NULL,'\0'),('92','Ajustes contables que incrementan el débito fiscal',NULL,'\0'),('93','Ajustes contables que disminuyen el débito fiscal',NULL,'\0'),('94','Ajustes contables que incrementan el crédito fiscal',NULL,'\0'),('95','Ajustes contables que disminuyen el crédito fiscal',NULL,'\0'),('96','Formulario 1116/B',NULL,'\0'),('97','Formulario 1116/C',NULL,'\0');

/*Table structure for table `tipocontacto` */

DROP TABLE IF EXISTS `tipocontacto`;

CREATE TABLE `tipocontacto` (
  `tco_Id` int(11) NOT NULL AUTO_INCREMENT,
  `tco_Nombre` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`tco_Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `tipocontacto` */

/*Table structure for table `tipodocumento` */

DROP TABLE IF EXISTS `tipodocumento`;

CREATE TABLE `tipodocumento` (
  `tdo_codigo` varchar(10) COLLATE utf8_spanish_ci NOT NULL,
  `tdo_nombre` varchar(50) COLLATE utf8_spanish_ci DEFAULT NULL,
  PRIMARY KEY (`tdo_codigo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

/*Data for the table `tipodocumento` */

insert  into `tipodocumento`(`tdo_codigo`,`tdo_nombre`) values ('00','CI Policía Federal'),('01','CI Buenos Aires'),('02','CI Catamarca'),('03','CI Córdoba'),('04','CI Corrientes'),('05','CI Entre Ríos'),('06','CI Jujuy'),('07','CI Mendoza'),('08','CI La Rioja'),('09','CI Salta'),('10','CI San Juan'),('11','CI San Luis'),('12','CI Santa Fe'),('13','CI Santiago del Estero'),('14','CI Tucumán'),('16','CI Chaco'),('17','CI Chubut'),('18','CI Formosa'),('19','CI Misiones'),('20','CI Neuquén'),('21','CI La Pampa'),('22','CI Río Negro'),('23','CI Santa Cruz'),('24','CI Tierra del Fuego'),('80','CUIT'),('86','CUIL'),('87','CDI'),('89','LE'),('90','LC'),('91','CI extranjera'),('92','en trámite'),('93','Acta nacimiento'),('94','Pasaporte'),('95','CI Bs. As. RNP'),('96','DNI'),('99','Sin identificar/venta global diaria');

/*Table structure for table `tiporesponsable` */

DROP TABLE IF EXISTS `tiporesponsable`;

CREATE TABLE `tiporesponsable` (
  `tre_codigo` varchar(10) COLLATE utf8_spanish_ci NOT NULL,
  `tre_nombre` varchar(50) COLLATE utf8_spanish_ci DEFAULT NULL,
  PRIMARY KEY (`tre_codigo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

/*Data for the table `tiporesponsable` */

insert  into `tiporesponsable`(`tre_codigo`,`tre_nombre`) values ('01','IVA Responsable Inscripto'),('02','IVA Responsable no Inscripto'),('03','IVA no Responsable'),('04','IVA Sujeto Exento'),('05','Consumidor Final'),('06','Responsable Monotributo'),('07','Sujeto no Categorizado'),('08','Importador del Exterior'),('09','Cliente del Exterior'),('10','IVA Liberado - Ley N 19.640'),('11','IVA Responsable Inscripto - Agente de Percepción');

/*Table structure for table `vw_comprobanteencabezado` */

DROP TABLE IF EXISTS `vw_comprobanteencabezado`;

/*!50001 DROP VIEW IF EXISTS `vw_comprobanteencabezado` */;
/*!50001 DROP TABLE IF EXISTS `vw_comprobanteencabezado` */;

/*!50001 CREATE TABLE  `vw_comprobanteencabezado`(
 `cen_Id` int(11) ,
 `tco_codigo` varchar(10) ,
 `pvt_descripcion` varchar(255) ,
 `pvt_codigo` varchar(10) ,
 `cen_numero` int(11) ,
 `cen_fecha` datetime ,
 `tre_codigo` varchar(10) ,
 `tre_nombre` varchar(50) ,
 `cli_id` int(11) ,
 `cli_Codigo` varchar(255) ,
 `cli_RazonSocial` varchar(50) ,
 `cli_CUIT` bigint(20) ,
 `cli_IngresosBrutos` varchar(50) ,
 `cli_direccion` varchar(255) ,
 `cli_localidad` varchar(50) ,
 `cli_provincia` varchar(50) ,
 `lpr_Nombre` varchar(50) ,
 `cen_neto` decimal(12,4) ,
 `cen_IVA01` decimal(12,4) ,
 `cen_IVA01porc` decimal(7,4) ,
 `cen_IVA01neto` decimal(12,4) ,
 `cen_IVA02` decimal(12,4) ,
 `cen_IVA02porc` decimal(7,4) ,
 `cen_IVA02neto` decimal(12,4) ,
 `cen_IVA03` decimal(12,4) ,
 `cen_IVA03porc` decimal(7,4) ,
 `cen_IVA03neto` decimal(12,4) ,
 `cen_IVA04` decimal(12,4) ,
 `cen_IVA04porc` decimal(7,4) ,
 `cen_IVA04neto` decimal(12,4) ,
 `cen_total` decimal(12,4) ,
 `cvt_codigo` varchar(10) ,
 `cvt_nombre` varchar(50) ,
 `tdo_codigo` varchar(10) ,
 `tdo_nombre` varchar(50) ,
 `cva_codigo` varchar(10) ,
 `cva_nombre` varchar(50) ,
 `tco_descripcion` varchar(255) ,
 `tco_letra` char(1) ,
 `cen_cae` varchar(30) ,
 `cen_caefechavencimiento` date ,
 `cen_numerocompleto` varchar(13) 
)*/;

/*View structure for view vw_comprobanteencabezado */

/*!50001 DROP TABLE IF EXISTS `vw_comprobanteencabezado` */;
/*!50001 DROP VIEW IF EXISTS `vw_comprobanteencabezado` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vw_comprobanteencabezado` AS select `comprobanteencabezado`.`cen_Id` AS `cen_Id`,`comprobanteencabezado`.`tco_codigo` AS `tco_codigo`,`puntodeventa`.`pvt_descripcion` AS `pvt_descripcion`,`comprobanteencabezado`.`pvt_codigo` AS `pvt_codigo`,`comprobanteencabezado`.`cen_numero` AS `cen_numero`,`comprobanteencabezado`.`cen_fecha` AS `cen_fecha`,`comprobanteencabezado`.`tre_codigo` AS `tre_codigo`,`comprobanteencabezado`.`tre_nombre` AS `tre_nombre`,`comprobanteencabezado`.`cli_id` AS `cli_id`,`comprobanteencabezado`.`cli_Codigo` AS `cli_Codigo`,`comprobanteencabezado`.`cli_RazonSocial` AS `cli_RazonSocial`,`comprobanteencabezado`.`cli_CUIT` AS `cli_CUIT`,`comprobanteencabezado`.`cli_IngresosBrutos` AS `cli_IngresosBrutos`,`comprobanteencabezado`.`cli_direccion` AS `cli_direccion`,`comprobanteencabezado`.`cli_localidad` AS `cli_localidad`,`comprobanteencabezado`.`cli_provincia` AS `cli_provincia`,`comprobanteencabezado`.`lpr_Nombre` AS `lpr_Nombre`,`comprobanteencabezado`.`cen_neto` AS `cen_neto`,`comprobanteencabezado`.`cen_IVA01` AS `cen_IVA01`,`comprobanteencabezado`.`cen_IVA01porc` AS `cen_IVA01porc`,`comprobanteencabezado`.`cen_IVA01neto` AS `cen_IVA01neto`,`comprobanteencabezado`.`cen_IVA02` AS `cen_IVA02`,`comprobanteencabezado`.`cen_IVA02porc` AS `cen_IVA02porc`,`comprobanteencabezado`.`cen_IVA02neto` AS `cen_IVA02neto`,`comprobanteencabezado`.`cen_IVA03` AS `cen_IVA03`,`comprobanteencabezado`.`cen_IVA03porc` AS `cen_IVA03porc`,`comprobanteencabezado`.`cen_IVA03neto` AS `cen_IVA03neto`,`comprobanteencabezado`.`cen_IVA04` AS `cen_IVA04`,`comprobanteencabezado`.`cen_IVA04porc` AS `cen_IVA04porc`,`comprobanteencabezado`.`cen_IVA04neto` AS `cen_IVA04neto`,`comprobanteencabezado`.`cen_total` AS `cen_total`,`comprobanteencabezado`.`cvt_codigo` AS `cvt_codigo`,`comprobanteencabezado`.`cvt_nombre` AS `cvt_nombre`,`comprobanteencabezado`.`tdo_codigo` AS `tdo_codigo`,`comprobanteencabezado`.`tdo_nombre` AS `tdo_nombre`,`comprobanteencabezado`.`cva_codigo` AS `cva_codigo`,`comprobanteencabezado`.`cva_nombre` AS `cva_nombre`,`tipocomprobante`.`tco_descripcion` AS `tco_descripcion`,`tipocomprobante`.`tco_letra` AS `tco_letra`,`comprobanteencabezado`.`cen_cae` AS `cen_cae`,`comprobanteencabezado`.`cen_caefechavencimiento` AS `cen_caefechavencimiento`,concat(lpad(`comprobanteencabezado`.`pvt_codigo`,4,'0'),'-',lpad(`comprobanteencabezado`.`cen_numero`,8,'0')) AS `cen_numerocompleto` from ((`comprobanteencabezado` join `puntodeventa` on((`comprobanteencabezado`.`pvt_codigo` = `puntodeventa`.`pvt_codigo`))) join `tipocomprobante` on((`comprobanteencabezado`.`tco_codigo` = `tipocomprobante`.`tco_codigo`))) */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
