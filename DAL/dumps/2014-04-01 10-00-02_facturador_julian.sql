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
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

/*Data for the table `barrio` */

insert  into `barrio`(`bar_Id`,`ban_Nombre`,`loc_Id`) values (1,'Buenos Aires',1),(2,'Vicente Lopez',1);

/*Table structure for table `camposimpresion` */

DROP TABLE IF EXISTS `camposimpresion`;

CREATE TABLE `camposimpresion` (
  `cim_codigo` varchar(10) COLLATE utf8_spanish_ci NOT NULL,
  `cim_campo` varchar(30) COLLATE utf8_spanish_ci NOT NULL,
  `cim_descripcion` varchar(50) COLLATE utf8_spanish_ci DEFAULT NULL,
  `cim_tipo` varchar(30) COLLATE utf8_spanish_ci DEFAULT NULL,
  `cim_formato` varchar(30) COLLATE utf8_spanish_ci DEFAULT NULL,
  `gim_id` int(11) DEFAULT NULL,
  `cim_alineacion` char(1) COLLATE utf8_spanish_ci DEFAULT 'd',
  PRIMARY KEY (`cim_codigo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

/*Data for the table `camposimpresion` */

insert  into `camposimpresion`(`cim_codigo`,`cim_campo`,`cim_descripcion`,`cim_tipo`,`cim_formato`,`gim_id`,`cim_alineacion`) values ('A00','cen_Id','ID interno comprobante','INT',NULL,1,'d'),('A01','tco_codigo','Código de tipo de comprobante',NULL,NULL,1,'i'),('A02','tco_descripcion','Descripción de tipo de comprobante',NULL,NULL,1,'d'),('A03','pvt_codigo','Código de punto de venta','INT','####',1,'d'),('A04','pvt_descripcion','Descripción de punto de venta',NULL,NULL,1,'d'),('A05','cen_numero','Número de comprobante','INT','########',1,'d'),('A06','cen_fecha','Fecha de comprobante','DATE','dd/MM/yyyy',1,'i'),('A07','tre_nombre','Nombre de Tipo de responsable',NULL,NULL,1,'i'),('A08','tre_codigo','Código de Tipo de responsable',NULL,NULL,1,'i'),('A09','cli_id','ID interno',NULL,NULL,1,'i'),('A10','cli_Codigo','Código de Cliente',NULL,NULL,1,'d'),('A11','cli_RazonSocial','Razón Social de Cliente',NULL,NULL,1,'i'),('A12','cli_CUIT','CUIT de Cliente','INT','##-########-#',1,'i'),('A13','cli_IngresosBrutos','IIBB de Cliente','INT',NULL,1,'i'),('A14','cli_direccion','Dirección de Cliente',NULL,NULL,1,'i'),('A15','cli_localidad','Localidad de Cliente',NULL,NULL,1,'i'),('A16','cli_provincia','Provincia de Cliente',NULL,NULL,1,'i'),('A17','lpr_Nombre','Nombre de lista de precios',NULL,NULL,1,'d'),('A18','cen_neto','Neto del comprobante','DECIMAL','0.00',1,'d'),('A19','cen_IVA01','Monto IVA 01','DECIMAL','0.00',1,'d'),('A20','cen_IVA01porc','Porcentaje IVA 01','DECIMAL','0.00',1,'d'),('A21','cen_IVA01neto','Neto IVA 01','DECIMAL','#########.##',1,'d'),('A22','cen_IVA02','Monto IVA 02','DECIMAL',NULL,1,'d'),('A23','cen_IVA02porc','Porcentaje IVA 02','DECIMAL',NULL,1,'d'),('A24','cen_IVA02neto','Neto IVA 02','DECIMAL',NULL,1,'d'),('A25','cen_IVA03','Monto IVA 03','DECIMAL',NULL,1,'d'),('A26','cen_IVA03porc','Porcentaje IVA 03','DECIMAL',NULL,1,'d'),('A27','cen_IVA03neto','Neto IVA 03','DECIMAL',NULL,1,'d'),('A28','cen_IVA04','Monto IVA 04','DECIMAL',NULL,1,'d'),('A29','cen_IVA04porc','Porcentaje IVA 04','DECIMAL',NULL,1,'d'),('A30','cen_IVA04neto','Neto IVA 04','DECIMAL',NULL,1,'d'),('A31','cen_total','Total comprobante','DECIMAL','0.00',1,'i'),('A32','cvt_codigo','Código de concepto de venta',NULL,NULL,1,'i'),('A33','cvt_nombre','Nombre de concepto de venta',NULL,NULL,1,'i'),('A34','tdo_codigo','Código de tipo de tipo de documento de cliente',NULL,NULL,1,'i'),('A35','tdo_nombre','Nombre de tipo de tipo de documento de cliente',NULL,NULL,1,'i'),('A36','cva_codigo','Código de condición de venta',NULL,NULL,1,'i'),('A37','cva_nombre','Nombre de condición de venta',NULL,NULL,1,'i'),('A38','cen_numerocompleto','Numero Completo del comprobante',NULL,NULL,1,'i'),('A39','tco_letra','Letra del Comprobante',NULL,NULL,1,'d'),('A40','cen_cae','AFIP CAE',NULL,NULL,1,'d'),('A41','cen_caefechavencimiento','AFIP CAE Fecha Vencimiento','DATE','dd/MM/yyyy',1,'d'),('A42','cen_cae_i2o5','AFIP CAE Texto codificado barra I2O5',NULL,'*',1,'d'),('A43','cen_cae_texto_barra','AFIP CAE Texto plano Barra',NULL,NULL,1,'d'),('A44','cen_totalIva','Suma de todos los importes de IVA','DECIMAL','0.00',1,'d'),('A45','pro_codigo','Código de producto de item de comprobante',NULL,NULL,1,'d'),('A46','cim_cantidad','Cantidad de producto de item de comprobante','DECIMAL','0.00',1,'d'),('A47','pro_descripcion','Descripcion de producto de item de comprobante',NULL,NULL,1,'i'),('A48','pro_precio','Precio Unitario de producto de item de comprobante','DECIMAL','0.00',1,'d'),('A49','cim_neto','Monto neto de producto de item de comprobante','DECIMAL','0.00',1,'d'),('A50','cim_total','Monto total de producto de item de comprobante','DECIMAL','0.00',1,'d');

/*Table structure for table `cliente` */

DROP TABLE IF EXISTS `cliente`;

CREATE TABLE `cliente` (
  `cli_Id` int(11) NOT NULL AUTO_INCREMENT,
  `cli_RazonSocial` varchar(255) DEFAULT NULL,
  `cli_CUIT` bigint(20) DEFAULT NULL,
  `cli_IngresosBrutos` varchar(20) DEFAULT NULL,
  `tre_codigo` int(11) DEFAULT NULL,
  `cli_Activo` bit(1) DEFAULT b'1',
  `cli_FechaAlta` datetime DEFAULT NULL,
  `cli_Codigo` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`cli_Id`),
  KEY `FK_cliente_condicioniva_civ_Id` (`tre_codigo`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;

/*Data for the table `cliente` */

insert  into `cliente`(`cli_Id`,`cli_RazonSocial`,`cli_CUIT`,`cli_IngresosBrutos`,`tre_codigo`,`cli_Activo`,`cli_FechaAlta`,`cli_Codigo`) values (1,'AMX ARGENTINA S.A.',30663288497,'642020',1,'','2014-03-31 00:00:00','0001'),(2,'TELMEX ARGENTINA S.A.',33695098419,'642099',1,'','2014-03-31 00:00:00','0002'),(3,'COMPAÑIA ERICSSON S.A.C.I',30526058190,'9029105801',1,'','2014-03-31 00:00:00','0003'),(4,'CERAGON ARGENTINA S.A.',30707493999,NULL,1,'','2014-03-31 00:00:00','0004');

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
  `cen_cae_texto_barra` varchar(100) DEFAULT NULL,
  `gim_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`cen_Id`),
  KEY `FK_comprobanteencabezado_puntodeventa_pvt_codigo` (`pvt_codigo`),
  KEY `FK_comprobanteencabezado_tipocomprobante_tco_codigo` (`tco_codigo`),
  CONSTRAINT `FK_comprobanteencabezado_puntodeventa_pvt_codigo` FOREIGN KEY (`pvt_codigo`) REFERENCES `puntodeventa` (`pvt_codigo`),
  CONSTRAINT `FK_comprobanteencabezado_tipocomprobante_tco_codigo` FOREIGN KEY (`tco_codigo`) REFERENCES `tipocomprobante` (`tco_codigo`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8;

/*Data for the table `comprobanteencabezado` */

insert  into `comprobanteencabezado`(`cen_Id`,`tco_codigo`,`pvt_codigo`,`cen_numero`,`cen_fecha`,`tre_codigo`,`tre_nombre`,`cli_id`,`cli_Codigo`,`cli_RazonSocial`,`cli_CUIT`,`cli_IngresosBrutos`,`cli_direccion`,`cli_localidad`,`cli_provincia`,`lpr_Nombre`,`cen_neto`,`cen_IVA01`,`cen_IVA01porc`,`cen_IVA01neto`,`cen_IVA02`,`cen_IVA02porc`,`cen_IVA02neto`,`cen_IVA03`,`cen_IVA03porc`,`cen_IVA03neto`,`cen_IVA04`,`cen_IVA04porc`,`cen_IVA04neto`,`cen_total`,`cvt_codigo`,`cvt_nombre`,`tdo_codigo`,`tdo_nombre`,`cva_codigo`,`cva_nombre`,`cen_cae`,`cen_caefechavencimiento`,`cen_afipreq`,`cen_afipres`,`cen_estadoimpresion`,`cen_fechaimpresion`,`cen_cae_i2o5`,`cen_cae_texto_barra`,`gim_id`) values (9,'01','1',12,'2014-03-26 00:00:00','01','IVA Responsable Inscripto',0,'5465','Cliente S.R.L.',20000000001,'28888888887','las palmas 45','cordoba','5000cba','Lista Normal','12000.5800','88873.2500','21.0000','4.0000','0.0000','0.0000','0.0000','0.0000','0.0000','0.0000','0.0000','0.0000','0.0000','888888.5600','01','Productos','80','CUIT','CC','Cuenta Corriente','64131182709382','2014-04-05','<?xml version=\"1.0\" encoding=\"UTF-8\"?><FeCAEReq><FeCabReq><CantReg>1</CantReg><PtoVta>1</PtoVta><CbteTipo>01</CbteTipo></FeCabReq><FeDetReq><FECAEDetRequest><Concepto>01</Concepto><DocTipo>80</DocTipo><DocNro>20000000001</DocNro><CbteDesde>12</CbteDesde><CbteHasta>12</CbteHasta><CbteFch>20140326</CbteFch><ImpTotal>4.84</ImpTotal><ImpTotConc>0</ImpTotConc><ImpIVA>0.84</ImpIVA><ImpOpEx>0</ImpOpEx><ImpNeto>4.00</ImpNeto><ImpTrib>0</ImpTrib><FchServDesde></FchServDesde><FchServHasta></FchServHasta><FchVtoPago></FchVtoPago><MonId>PES</MonId><MonCotiz>1</MonCotiz><IVA><AlicIva><Id>5</Id><BaseImp>4</BaseImp><Importe>0,84</Importe></AlicIva></IVA></FECAEDetRequest></FeDetReq></FeCAEReq>','<?xml version=\"1.0\"?><FECAESolicitarResponse><FECAESolicitarResult><FeCabResp><Cuit>20170675771</Cuit><CbteTipo>1</CbteTipo><FchProceso>20140326225915</FchProceso><CantReg>1</CantReg><Resultado>A</Resultado></FeCabResp><FeDetResp /><FEDetResponse><Concepto>1</Concepto><DocTipo>80</DocTipo><DocNro>20000000001</DocNro><CbteDesde>12</CbteDesde><CbteHasta>12</CbteHasta><Resultado>A</Resultado><CAE>64131182709382</CAE><CbteFch>20140326</CbteFch><CAEFchVto>20140405</CAEFchVto></FEDetResponse><Obs><Observaciones><Code>10017</Code><Msg>Factura individual, DocTipo: 80, DocNro 20000000001 no se encuentra registrado en los padrones de AFIP.</Msg></Observaciones></Obs></FECAESolicitarResult></FECAESolicitarResponse>',1,'2014-03-31 13:34:04','(DA6��::0@YOBK9VF1XX�)','2017067577101000164131182709382201404058',1),(10,'01','1',34,'2014-03-31 00:00:00','01','IVA Responsable Inscripto',0,NULL,'AMX ARGENTINA S.A.',30663288497,'642020','AV. DE MAYO 878','Buenos Aires','','Lista Normal','67.7329','14.2200','21.0000','67.7329','0.0000','0.0000','0.0000','0.0000','0.0000','0.0000','0.0000','0.0000','0.0000','81.9568','01','Productos','80','CUIT','CC','Cuenta Corriente','64131182882035','2014-04-10','<?xml version=\"1.0\" encoding=\"UTF-8\"?><FeCAEReq><FeCabReq><CantReg>1</CantReg><PtoVta>1</PtoVta><CbteTipo>01</CbteTipo></FeCabReq><FeDetReq><FECAEDetRequest><Concepto>01</Concepto><DocTipo>80</DocTipo><DocNro>30663288497</DocNro><CbteDesde>34</CbteDesde><CbteHasta>34</CbteHasta><CbteFch>20140331</CbteFch><ImpTotal>81.96</ImpTotal><ImpTotConc>0</ImpTotConc><ImpIVA>14.22</ImpIVA><ImpOpEx>0</ImpOpEx><ImpNeto>67.73</ImpNeto><ImpTrib>0</ImpTrib><FchServDesde></FchServDesde><FchServHasta></FchServHasta><FchVtoPago></FchVtoPago><MonId>PES</MonId><MonCotiz>1</MonCotiz><IVA><AlicIva><Id>5</Id><BaseImp>67.73</BaseImp><Importe>14.22</Importe></AlicIva></IVA></FECAEDetRequest></FeDetReq></FeCAEReq>','<?xml version=\"1.0\"?><FECAESolicitarResponse><FECAESolicitarResult><FeCabResp><Cuit>20170675771</Cuit><CbteTipo>1</CbteTipo><FchProceso>20140331160200</FchProceso><CantReg>1</CantReg><Resultado>A</Resultado></FeCabResp><FeDetResp /><FEDetResponse><Concepto>1</Concepto><DocTipo>80</DocTipo><DocNro>30663288497</DocNro><CbteDesde>34</CbteDesde><CbteHasta>34</CbteHasta><Resultado>A</Resultado><CAE>64131182882035</CAE><CbteFch>20140331</CbteFch><CAEFchVto>20140410</CAEFchVto></FEDetResponse></FECAESolicitarResult></FECAESolicitarResponse>',1,'2014-03-31 20:52:32','(DA6��::0@YOBL�3�1XY9)',NULL,1),(11,'01','1',35,'2014-03-31 00:00:00','01','IVA Responsable Inscripto',0,NULL,'AMX ARGENTINA S.A.',30663288497,'642020','AV. DE MAYO 878','Buenos Aires','','Lista Normal','491.7000','103.2600','21.0000','491.7000','0.0000','0.0000','0.0000','0.0000','0.0000','0.0000','0.0000','0.0000','0.0000','594.9570','01','Productos','80','CUIT','CC','Cuenta Corriente','64131182882051','2014-04-10','<?xml version=\"1.0\" encoding=\"UTF-8\"?><FeCAEReq><FeCabReq><CantReg>1</CantReg><PtoVta>1</PtoVta><CbteTipo>01</CbteTipo></FeCabReq><FeDetReq><FECAEDetRequest><Concepto>01</Concepto><DocTipo>80</DocTipo><DocNro>30663288497</DocNro><CbteDesde>35</CbteDesde><CbteHasta>35</CbteHasta><CbteFch>20140331</CbteFch><ImpTotal>594.96</ImpTotal><ImpTotConc>0</ImpTotConc><ImpIVA>103.26</ImpIVA><ImpOpEx>0</ImpOpEx><ImpNeto>491.70</ImpNeto><ImpTrib>0</ImpTrib><FchServDesde></FchServDesde><FchServHasta></FchServHasta><FchVtoPago></FchVtoPago><MonId>PES</MonId><MonCotiz>1</MonCotiz><IVA><AlicIva><Id>5</Id><BaseImp>491.70</BaseImp><Importe>103.26</Importe></AlicIva></IVA></FECAEDetRequest></FeDetReq></FeCAEReq>','<?xml version=\"1.0\"?><FECAESolicitarResponse><FECAESolicitarResult><FeCabResp><Cuit>20170675771</Cuit><CbteTipo>1</CbteTipo><FchProceso>20140331160456</FchProceso><CantReg>1</CantReg><Resultado>A</Resultado></FeCabResp><FeDetResp /><FEDetResponse><Concepto>1</Concepto><DocTipo>80</DocTipo><DocNro>30663288497</DocNro><CbteDesde>35</CbteDesde><CbteHasta>35</CbteHasta><Resultado>A</Resultado><CAE>64131182882051</CAE><CbteFch>20140331</CbteFch><CAEFchVto>20140410</CAEFchVto></FEDetResponse></FECAESolicitarResult></FECAESolicitarResponse>',NULL,NULL,'(DA6��::0@YOBL�5<1XY9)',NULL,1),(12,'01','1',36,'2014-03-31 00:00:00','01','IVA Responsable Inscripto',0,NULL,'AMX ARGENTINA S.A.',30663288497,'642020','AV. DE MAYO 878','Buenos Aires','','Lista Normal','222.5000','46.7200','21.0000','222.5000','0.0000','0.0000','0.0000','0.0000','0.0000','0.0000','0.0000','0.0000','0.0000','269.2250','01','Productos','80','CUIT','CC','Cuenta Corriente','64131182882080','2014-04-10','<?xml version=\"1.0\" encoding=\"UTF-8\"?><FeCAEReq><FeCabReq><CantReg>1</CantReg><PtoVta>1</PtoVta><CbteTipo>01</CbteTipo></FeCabReq><FeDetReq><FECAEDetRequest><Concepto>01</Concepto><DocTipo>80</DocTipo><DocNro>30663288497</DocNro><CbteDesde>36</CbteDesde><CbteHasta>36</CbteHasta><CbteFch>20140331</CbteFch><ImpTotal>269.23</ImpTotal><ImpTotConc>0</ImpTotConc><ImpIVA>46.73</ImpIVA><ImpOpEx>0</ImpOpEx><ImpNeto>222.50</ImpNeto><ImpTrib>0</ImpTrib><FchServDesde></FchServDesde><FchServHasta></FchServHasta><FchVtoPago></FchVtoPago><MonId>PES</MonId><MonCotiz>1</MonCotiz><IVA><AlicIva><Id>5</Id><BaseImp>222.50</BaseImp><Importe>46.72</Importe></AlicIva></IVA></FECAEDetRequest></FeDetReq></FeCAEReq>','<?xml version=\"1.0\"?><FECAESolicitarResponse><FECAESolicitarResult><FeCabResp><Cuit>20170675771</Cuit><CbteTipo>1</CbteTipo><FchProceso>20140331160726</FchProceso><CantReg>1</CantReg><Resultado>A</Resultado></FeCabResp><FeDetResp /><FEDetResponse><Concepto>1</Concepto><DocTipo>80</DocTipo><DocNro>30663288497</DocNro><CbteDesde>36</CbteDesde><CbteHasta>36</CbteHasta><Resultado>A</Resultado><CAE>64131182882080</CAE><CbteFch>20140331</CbteFch><CAEFchVto>20140410</CAEFchVto></FEDetResponse></FECAESolicitarResult></FECAESolicitarResponse>',NULL,NULL,'(DA6��::0@YOBL�821XY9)',NULL,1),(13,'03','1',1,'2014-03-31 00:00:00','01','IVA Responsable Inscripto',0,NULL,'TELMEX ARGENTINA S.A.',33695098419,'642099','AV. DE MAYO 878','Buenos Aires','','Lista Normal','495.0000','103.9500','21.0000','495.0000','0.0000','0.0000','0.0000','0.0000','0.0000','0.0000','0.0000','0.0000','0.0000','598.9500','01','Productos','80','CUIT','CC','Cuenta Corriente','64131182882111','2014-04-10','<?xml version=\"1.0\" encoding=\"UTF-8\"?><FeCAEReq><FeCabReq><CantReg>1</CantReg><PtoVta>1</PtoVta><CbteTipo>03</CbteTipo></FeCabReq><FeDetReq><FECAEDetRequest><Concepto>01</Concepto><DocTipo>80</DocTipo><DocNro>33695098419</DocNro><CbteDesde>1</CbteDesde><CbteHasta>1</CbteHasta><CbteFch>20140331</CbteFch><ImpTotal>598.95</ImpTotal><ImpTotConc>0</ImpTotConc><ImpIVA>103.95</ImpIVA><ImpOpEx>0</ImpOpEx><ImpNeto>495.00</ImpNeto><ImpTrib>0</ImpTrib><FchServDesde></FchServDesde><FchServHasta></FchServHasta><FchVtoPago></FchVtoPago><MonId>PES</MonId><MonCotiz>1</MonCotiz><IVA><AlicIva><Id>5</Id><BaseImp>495.00</BaseImp><Importe>103.95</Importe></AlicIva></IVA></FECAEDetRequest></FeDetReq></FeCAEReq>','<?xml version=\"1.0\"?><FECAESolicitarResponse><FECAESolicitarResult><FeCabResp><Cuit>20170675771</Cuit><CbteTipo>3</CbteTipo><FchProceso>20140331160914</FchProceso><CantReg>1</CantReg><Resultado>A</Resultado></FeCabResp><FeDetResp /><FEDetResponse><Concepto>1</Concepto><DocTipo>80</DocTipo><DocNro>33695098419</DocNro><CbteDesde>1</CbteDesde><CbteHasta>1</CbteHasta><Resultado>A</Resultado><CAE>64131182882111</CAE><CbteFch>20140331</CbteFch><CAEFchVto>20140410</CAEFchVto></FEDetResponse></FECAESolicitarResult></FECAESolicitarResponse>',NULL,NULL,'(DA6��::0@YOBL�;<1XY0)',NULL,1);

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
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8;

/*Data for the table `comprobanteitem` */

insert  into `comprobanteitem`(`cim_Id`,`cen_id`,`pro_Codigo`,`pro_Descripcion`,`pro_Costo`,`pro_Precio`,`cim_Cantidad`,`cim_descuento`,`ali_id`,`ali_porcentaje`,`cim_neto`,`cim_total`) values (1,9,'cod1','Servicio de reparacion de antena','300.0000','300.0000','1.2580',NULL,5,'21.0000','300.0000','363.0000'),(2,9,'cod2','convertidor de oxido','10.0000','10.0000','2.5500',NULL,4,'10.5000','20.0000','22.1000'),(3,9,'cod3','mouse inalambrico','15.0000','15.0000','6.8901',NULL,4,'10.5000','45.0000','49.7250'),(4,9,'cod4','limpieza general piso','500.0000','500.0000','1.0000',NULL,5,'21.0000','500.0000','605.0000'),(5,9,'cod5','d','10.0000','10.0000','7000.2500','0.0000',0,'21.0000','10000.3600','10000.3600'),(6,9,'cod6','dd','10.0000','10.0000','3000.0000','0.0000',0,'10.5000','999999.2540','999999.2540'),(7,9,'cod7','2','2.0000','2.0000','255.3600','0.0000',0,'21.0000','4.0000','4.8400'),(8,10,'','tetetet','21.5600','21.5600','3.1416','0.0000',0,'21.0000','67.7329','81.9568'),(9,11,'','22','22.3500','22.3500','22.0000','0.0000',0,'21.0000','491.7000','594.9570'),(10,12,'','tetet','10.0000','10.0000','22.2500','0.0000',0,'21.0000','222.5000','269.2250'),(11,13,'','test','22.5000','22.5000','22.0000','0.0000',0,'21.0000','495.0000','598.9500');

/*Table structure for table `conceptoventa` */

DROP TABLE IF EXISTS `conceptoventa`;

CREATE TABLE `conceptoventa` (
  `cvt_codigo` varchar(10) COLLATE utf8_spanish_ci NOT NULL,
  `cvt_nombre` varchar(50) COLLATE utf8_spanish_ci DEFAULT NULL,
  `cvt_activo` bit(1) DEFAULT b'0',
  PRIMARY KEY (`cvt_codigo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

/*Data for the table `conceptoventa` */

insert  into `conceptoventa`(`cvt_codigo`,`cvt_nombre`,`cvt_activo`) values ('01','Productos',''),('02','Servicios','\0'),('03','Productos y Servicios','\0');

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
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;

/*Data for the table `direccion` */

insert  into `direccion`(`dir_Id`,`dir_Calle`,`dir_Altura`,`dir_Torre`,`dir_Piso`,`dir_Departamento`,`dir_CodigoPostal`,`bar_Id`) values (1,'AV. DE MAYO','878',NULL,NULL,NULL,'C1084AAQ',1),(2,'AV. DE MAYO','878',NULL,NULL,NULL,'C1084AAQ',1),(3,'GENERAL GÜEMES','676',NULL,NULL,NULL,'B1638CJF',2),(4,'MAIPU','1300',NULL,'23',NULL,'C1006ACT',1);

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

insert  into `direccionxcliente`(`dir_Id`,`cli_Id`,`dxc_observaciones`,`dxc_principal`) values (1,1,NULL,''),(2,2,NULL,''),(3,3,NULL,''),(4,4,NULL,'');

/*Table structure for table `grupoimpresion` */

DROP TABLE IF EXISTS `grupoimpresion`;

CREATE TABLE `grupoimpresion` (
  `gim_id` int(11) NOT NULL AUTO_INCREMENT,
  `gim_copias` tinyint(4) DEFAULT NULL,
  `gim_impresora` varchar(255) COLLATE utf8_spanish_ci DEFAULT NULL,
  PRIMARY KEY (`gim_id`),
  UNIQUE KEY `UK_GrupoImpresion` (`gim_impresora`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

/*Data for the table `grupoimpresion` */

insert  into `grupoimpresion`(`gim_id`,`gim_copias`,`gim_impresora`) values (1,1,'casacentral');

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
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

/*Data for the table `localidad` */

insert  into `localidad`(`loc_Id`,`loc_Nombre`,`pro_id`) values (1,'Buenos Aires',1);

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
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

/*Data for the table `pais` */

insert  into `pais`(`pai_Id`,`pai_Nombre`) values (1,'Argentina');

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
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

/*Data for the table `provincia` */

insert  into `provincia`(`pro_Id`,`pro_Nombre`,`pai_Id`) values (1,'Buenos Aires',1);

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
  `tco_nombre_sin_letra` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`tco_codigo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `tipocomprobante` */

insert  into `tipocomprobante`(`tco_codigo`,`tco_descripcion`,`tco_letra`,`tco_activo`,`tco_nombre_sin_letra`) values ('01','Facturas A','A','','Factura'),('02','Notas de Débito A','A','\0','Notas de Débito A'),('03','Notas de Crédito A','A','','Nota de Crédito'),('04','Recibos A','A','\0','Recibos A'),('05','Notas de Venta al contado A','A','\0','Notas de Venta al contado A'),('06','Facturas B','B','\0','Facturas B'),('07','Notas de Débito B','B','\0','Notas de Débito B'),('08','Notas de Crédito B','B','\0','Notas de Crédito B'),('09','Recibos B','B','\0','Recibos B'),('10','Notas de Venta al contado B','B','\0','Notas de Venta al contado B'),('11','Facturas C','C','\0','Facturas C'),('12','Notas de Débito C','C','\0','Notas de Débito C'),('13','Notas de Crédito C','C','\0','Notas de Crédito C'),('14','Documento aduanero',NULL,'\0','Documento aduanero'),('15','Recibos C','C','\0','Recibos C'),('16','Notas de Venta al contado C','C','\0','Notas de Venta al contado C'),('19','Facturas de Exportación',NULL,'\0','Facturas de Exportación'),('20','Notas de Débito por operaciones con el exterior',NULL,'\0','Notas de Débito por operaciones con el exterior'),('21','Notas de Crédito por operaciones con el exterior',NULL,'\0','Notas de Crédito por operaciones con el exterior'),('22','Facturas  Permiso Exportación simplificado  Dto. 855/97',NULL,'\0','Facturas  Permiso Exportación simplificado  Dto. 855/97'),('30','Comprobantes de compra de bienes usados',NULL,'\0','Comprobantes de compra de bienes usados'),('34','Comprobantes A del Anexo I, Apartado A, inc. f), R.G. N° 1415','A','\0','Comprobantes A del Anexo I, Apartado A, inc. f), R.G. N° 1415'),('35','Comprobantes B del Anexo I, Apartado A, inc. f), R.G. N° 1415','B','\0','Comprobantes B del Anexo I, Apartado A, inc. f), R.G. N° 1415'),('36','Comprobantes C del Anexo I, Apartado A, inc. f), R.G. N° 1415','C','\0','Comprobantes C del Anexo I, Apartado A, inc. f), R.G. N° 1415'),('37','Notas de Débito o documento equivalente que cumplan con la R.G. N° 1415',NULL,'\0','Notas de Débito o documento equivalente que cumplan con la R.G. N° 1415'),('38','Notas de Crédito o documento equivalente que cumplan con la R.G. N° 1415',NULL,'\0','Notas de Crédito o documento equivalente que cumplan con la R.G. N° 1415'),('39','Otros comprobantes A que cumplan con la R.G. N° 1415','A','\0','Otros comprobantes A que cumplan con la R.G. N° 1415'),('40','Otros comprobantes B que cumplan con la R.G. N° 1415','B','\0','Otros comprobantes B que cumplan con la R.G. N° 1415'),('41','Otros comprobantes C que cumplan con la R.G. N° 1415','C','\0','Otros comprobantes C que cumplan con la R.G. N° 1415'),('51','Facturas M','M','\0','Facturas M'),('52','Notas de Débito M','M','\0','Notas de Débito M'),('53','Notas de Crédito M','M','\0','Notas de Crédito M'),('54','Recibos M','M','\0','Recibos M'),('55','Notas de Venta al contado M','M','\0','Notas de Venta al contado M'),('56','Comprobantes M del Anexo I, Apartado A, inc. f), R.G. N° 1415','M','\0','Comprobantes M del Anexo I, Apartado A, inc. f), R.G. N° 1415'),('57','Otros comprobantes M que cumplan con la R.G. N° 1415','M','\0','Otros comprobantes M que cumplan con la R.G. N° 1415'),('58','Cuenta de Venta y Líquido producto M','M','\0','Cuenta de Venta y Líquido producto M'),('59','Liquidación M','M','\0','Liquidación M'),('60','Cuenta de Venta y Líquido producto A','A','\0','Cuenta de Venta y Líquido producto A'),('61','Cuenta de Venta y Líquido producto B','B','\0','Cuenta de Venta y Líquido producto B'),('62','Cuenta de Venta y Líquido producto C','C','\0','Cuenta de Venta y Líquido producto C'),('63','Liquidación A','A','\0','Liquidación A'),('64','Liquidación B','B','\0','Liquidación B'),('65','Liquidación C','C','\0','Liquidación C'),('80','Comprobante diario de cierre (zeta)',NULL,'\0','Comprobante diario de cierre (zeta)'),('81','TiqueFactura \"A\"','A','\0','TiqueFactura \"A\"'),('82','TiqueFactura \"B\"','B','\0','TiqueFactura \"B\"'),('83','Tique',NULL,'\0','Tique'),('84','Comprobante/Factura de servicios públicos',NULL,'\0','Comprobante/Factura de servicios públicos'),('85','Nota de Crédito  servicios públicos',NULL,'\0','Nota de Crédito  servicios públicos'),('86','Nota de Débito  servicios públicos',NULL,'\0','Nota de Débito  servicios públicos'),('87','Otros comprobantes  servicios del exterior',NULL,'\0','Otros comprobantes  servicios del exterior'),('88','Otros comprobantes  documentos exceptuados',NULL,'\0','Otros comprobantes  documentos exceptuados'),('89','Otros comprobantes  documentos exceptuados  Notas de Débito',NULL,'\0','Otros comprobantes  documentos exceptuados  Notas de Débito'),('90','Otros comprobantes  documentos exceptuados  Notas de Crédito',NULL,'\0','Otros comprobantes  documentos exceptuados  Notas de Crédito'),('92','Ajustes contables que incrementan el débito fiscal',NULL,'\0','Ajustes contables que incrementan el débito fiscal'),('93','Ajustes contables que disminuyen el débito fiscal',NULL,'\0','Ajustes contables que disminuyen el débito fiscal'),('94','Ajustes contables que incrementan el crédito fiscal',NULL,'\0','Ajustes contables que incrementan el crédito fiscal'),('95','Ajustes contables que disminuyen el crédito fiscal',NULL,'\0','Ajustes contables que disminuyen el crédito fiscal'),('96','Formulario 1116/B',NULL,'\0','Formulario 1116/B'),('97','Formulario 1116/C',NULL,'\0','Formulario 1116/C');

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

/*Table structure for table `vw_cliente` */

DROP TABLE IF EXISTS `vw_cliente`;

/*!50001 DROP VIEW IF EXISTS `vw_cliente` */;
/*!50001 DROP TABLE IF EXISTS `vw_cliente` */;

/*!50001 CREATE TABLE  `vw_cliente`(
 `pai_Id` int(11) ,
 `pai_Nombre` varchar(50) ,
 `pro_Id` int(11) ,
 `pro_Nombre` varchar(50) ,
 `loc_Nombre` varchar(50) ,
 `loc_Id` int(11) ,
 `bar_Id` int(11) ,
 `ban_Nombre` varchar(50) ,
 `dir_Id` int(11) ,
 `dir_Calle` varchar(255) ,
 `dir_Altura` varchar(255) ,
 `dir_Torre` varchar(255) ,
 `dir_Piso` varchar(255) ,
 `dir_Departamento` varchar(255) ,
 `dir_CodigoPostal` varchar(255) ,
 `cli_Id` int(11) ,
 `cli_RazonSocial` varchar(255) ,
 `cli_CUIT` bigint(20) ,
 `cli_IngresosBrutos` varchar(20) ,
 `tre_codigo` int(11) ,
 `cli_Activo` bit(1) ,
 `cli_FechaAlta` datetime ,
 `cli_Codigo` varchar(255) ,
 `dxc_principal` bit(1) ,
 `dxc_observaciones` text 
)*/;

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
 `tco_letra` char(1) ,
 `cen_cae` varchar(30) ,
 `cen_caefechavencimiento` date ,
 `cen_cae_i2o5` varbinary(150) ,
 `cen_estadoimpresion` tinyint(4) ,
 `cen_fechaimpresion` datetime ,
 `gim_id` int(11) ,
 `gim_copias` tinyint(4) ,
 `gim_impresora` varchar(255) ,
 `tco_descripcion` varchar(255) ,
 `cen_cae_texto_barra` varchar(74) ,
 `cen_totalIva` decimal(15,4) ,
 `cen_numerocompleto` varchar(13) 
)*/;

/*View structure for view vw_cliente */

/*!50001 DROP TABLE IF EXISTS `vw_cliente` */;
/*!50001 DROP VIEW IF EXISTS `vw_cliente` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vw_cliente` AS (select `pais`.`pai_Id` AS `pai_Id`,`pais`.`pai_Nombre` AS `pai_Nombre`,`provincia`.`pro_Id` AS `pro_Id`,`provincia`.`pro_Nombre` AS `pro_Nombre`,`localidad`.`loc_Nombre` AS `loc_Nombre`,`localidad`.`loc_Id` AS `loc_Id`,`barrio`.`bar_Id` AS `bar_Id`,`barrio`.`ban_Nombre` AS `ban_Nombre`,`direccion`.`dir_Id` AS `dir_Id`,`direccion`.`dir_Calle` AS `dir_Calle`,`direccion`.`dir_Altura` AS `dir_Altura`,`direccion`.`dir_Torre` AS `dir_Torre`,`direccion`.`dir_Piso` AS `dir_Piso`,`direccion`.`dir_Departamento` AS `dir_Departamento`,`direccion`.`dir_CodigoPostal` AS `dir_CodigoPostal`,`cliente`.`cli_Id` AS `cli_Id`,`cliente`.`cli_RazonSocial` AS `cli_RazonSocial`,`cliente`.`cli_CUIT` AS `cli_CUIT`,`cliente`.`cli_IngresosBrutos` AS `cli_IngresosBrutos`,`cliente`.`tre_codigo` AS `tre_codigo`,`cliente`.`cli_Activo` AS `cli_Activo`,`cliente`.`cli_FechaAlta` AS `cli_FechaAlta`,`cliente`.`cli_Codigo` AS `cli_Codigo`,`direccionxcliente`.`dxc_principal` AS `dxc_principal`,`direccionxcliente`.`dxc_observaciones` AS `dxc_observaciones` from ((((((`cliente` left join `direccionxcliente` on((`direccionxcliente`.`cli_Id` = `cliente`.`cli_Id`))) left join `direccion` on((`direccion`.`dir_Id` = `direccionxcliente`.`dir_Id`))) left join `barrio` on((`direccion`.`bar_Id` = `barrio`.`bar_Id`))) left join `localidad` on((`barrio`.`loc_Id` = `localidad`.`loc_Id`))) left join `provincia` on((`localidad`.`pro_id` = `provincia`.`pro_Id`))) left join `pais` on((`provincia`.`pai_Id` = `pais`.`pai_Id`)))) */;

/*View structure for view vw_comprobanteencabezado */

/*!50001 DROP TABLE IF EXISTS `vw_comprobanteencabezado` */;
/*!50001 DROP VIEW IF EXISTS `vw_comprobanteencabezado` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vw_comprobanteencabezado` AS select `comprobanteencabezado`.`cen_Id` AS `cen_Id`,`comprobanteencabezado`.`tco_codigo` AS `tco_codigo`,`puntodeventa`.`pvt_descripcion` AS `pvt_descripcion`,`comprobanteencabezado`.`pvt_codigo` AS `pvt_codigo`,`comprobanteencabezado`.`cen_numero` AS `cen_numero`,`comprobanteencabezado`.`cen_fecha` AS `cen_fecha`,`comprobanteencabezado`.`tre_codigo` AS `tre_codigo`,`comprobanteencabezado`.`tre_nombre` AS `tre_nombre`,`comprobanteencabezado`.`cli_id` AS `cli_id`,`comprobanteencabezado`.`cli_Codigo` AS `cli_Codigo`,`comprobanteencabezado`.`cli_RazonSocial` AS `cli_RazonSocial`,`comprobanteencabezado`.`cli_CUIT` AS `cli_CUIT`,`comprobanteencabezado`.`cli_IngresosBrutos` AS `cli_IngresosBrutos`,`comprobanteencabezado`.`cli_direccion` AS `cli_direccion`,`comprobanteencabezado`.`cli_localidad` AS `cli_localidad`,`comprobanteencabezado`.`cli_provincia` AS `cli_provincia`,`comprobanteencabezado`.`lpr_Nombre` AS `lpr_Nombre`,`comprobanteencabezado`.`cen_neto` AS `cen_neto`,`comprobanteencabezado`.`cen_IVA01` AS `cen_IVA01`,`comprobanteencabezado`.`cen_IVA01porc` AS `cen_IVA01porc`,`comprobanteencabezado`.`cen_IVA01neto` AS `cen_IVA01neto`,`comprobanteencabezado`.`cen_IVA02` AS `cen_IVA02`,`comprobanteencabezado`.`cen_IVA02porc` AS `cen_IVA02porc`,`comprobanteencabezado`.`cen_IVA02neto` AS `cen_IVA02neto`,`comprobanteencabezado`.`cen_IVA03` AS `cen_IVA03`,`comprobanteencabezado`.`cen_IVA03porc` AS `cen_IVA03porc`,`comprobanteencabezado`.`cen_IVA03neto` AS `cen_IVA03neto`,`comprobanteencabezado`.`cen_IVA04` AS `cen_IVA04`,`comprobanteencabezado`.`cen_IVA04porc` AS `cen_IVA04porc`,`comprobanteencabezado`.`cen_IVA04neto` AS `cen_IVA04neto`,`comprobanteencabezado`.`cen_total` AS `cen_total`,`comprobanteencabezado`.`cvt_codigo` AS `cvt_codigo`,`comprobanteencabezado`.`cvt_nombre` AS `cvt_nombre`,`comprobanteencabezado`.`tdo_codigo` AS `tdo_codigo`,`comprobanteencabezado`.`tdo_nombre` AS `tdo_nombre`,`comprobanteencabezado`.`cva_codigo` AS `cva_codigo`,`comprobanteencabezado`.`cva_nombre` AS `cva_nombre`,`tipocomprobante`.`tco_letra` AS `tco_letra`,`comprobanteencabezado`.`cen_cae` AS `cen_cae`,`comprobanteencabezado`.`cen_caefechavencimiento` AS `cen_caefechavencimiento`,`comprobanteencabezado`.`cen_cae_i2o5` AS `cen_cae_i2o5`,`comprobanteencabezado`.`cen_estadoimpresion` AS `cen_estadoimpresion`,`comprobanteencabezado`.`cen_fechaimpresion` AS `cen_fechaimpresion`,`grupoimpresion`.`gim_id` AS `gim_id`,`grupoimpresion`.`gim_copias` AS `gim_copias`,`grupoimpresion`.`gim_impresora` AS `gim_impresora`,`tipocomprobante`.`tco_nombre_sin_letra` AS `tco_descripcion`,concat(`comprobanteencabezado`.`cli_CUIT`,`comprobanteencabezado`.`cvt_codigo`,lpad(`comprobanteencabezado`.`cvt_codigo`,2,'0'),lpad(`comprobanteencabezado`.`pvt_codigo`,4,'0'),`comprobanteencabezado`.`cen_cae`,date_format(`comprobanteencabezado`.`cen_caefechavencimiento`,'%Y%m%d')) AS `cen_cae_texto_barra`,(((`comprobanteencabezado`.`cen_IVA01` + `comprobanteencabezado`.`cen_IVA02`) + `comprobanteencabezado`.`cen_IVA03`) + `comprobanteencabezado`.`cen_IVA04`) AS `cen_totalIva`,concat(lpad(`comprobanteencabezado`.`pvt_codigo`,4,'0'),'-',lpad(`comprobanteencabezado`.`cen_numero`,8,'0')) AS `cen_numerocompleto` from (((`comprobanteencabezado` join `puntodeventa` on((`comprobanteencabezado`.`pvt_codigo` = `puntodeventa`.`pvt_codigo`))) join `tipocomprobante` on((`comprobanteencabezado`.`tco_codigo` = `tipocomprobante`.`tco_codigo`))) join `grupoimpresion` on((`comprobanteencabezado`.`gim_id` = `grupoimpresion`.`gim_id`))) */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
