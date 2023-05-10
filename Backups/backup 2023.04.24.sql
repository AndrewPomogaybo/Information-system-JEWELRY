-- MySqlBackup.NET 2.3.6
-- Dump Time: 2023-04-24 21:07:14
-- --------------------------------------
-- Server version 8.0.31 MySQL Community Server - GPL


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb3 */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- 
-- Definition of basket
-- 

DROP TABLE IF EXISTS `basket`;
CREATE TABLE IF NOT EXISTS `basket` (
  `id` int NOT NULL,
  `name_category` text NOT NULL,
  `name_weaving` text NOT NULL,
  `count` int NOT NULL,
  `price` int NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- 
-- Dumping data for table basket
-- 

/*!40000 ALTER TABLE `basket` DISABLE KEYS */;

/*!40000 ALTER TABLE `basket` ENABLE KEYS */;

-- 
-- Definition of categories
-- 

DROP TABLE IF EXISTS `categories`;
CREATE TABLE IF NOT EXISTS `categories` (
  `id_category` int NOT NULL AUTO_INCREMENT,
  `name_category` varchar(45) NOT NULL,
  PRIMARY KEY (`id_category`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb3;

-- 
-- Dumping data for table categories
-- 

/*!40000 ALTER TABLE `categories` DISABLE KEYS */;
INSERT INTO `categories`(`id_category`,`name_category`) VALUES(1,'Кресты'),(2,'Цепи');
/*!40000 ALTER TABLE `categories` ENABLE KEYS */;

-- 
-- Definition of clients
-- 

DROP TABLE IF EXISTS `clients`;
CREATE TABLE IF NOT EXISTS `clients` (
  `id_client` int NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  `surname` varchar(45) NOT NULL,
  `patronymic` varchar(45) NOT NULL,
  `phone_number` varchar(12) NOT NULL,
  PRIMARY KEY (`id_client`),
  UNIQUE KEY `id_client` (`id_client`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- 
-- Dumping data for table clients
-- 

/*!40000 ALTER TABLE `clients` DISABLE KEYS */;

/*!40000 ALTER TABLE `clients` ENABLE KEYS */;

-- 
-- Definition of order_create
-- 

DROP TABLE IF EXISTS `order_create`;
CREATE TABLE IF NOT EXISTS `order_create` (
  `id_order_create` int NOT NULL AUTO_INCREMENT,
  `id_client` int NOT NULL,
  `weight` int NOT NULL,
  `price` int NOT NULL,
  `name` varchar(200) NOT NULL,
  PRIMARY KEY (`id_order_create`),
  KEY `order_create_cliets_id` (`id_client`),
  CONSTRAINT `order_create_cliets_id` FOREIGN KEY (`id_client`) REFERENCES `clients` (`id_client`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- 
-- Dumping data for table order_create
-- 

/*!40000 ALTER TABLE `order_create` DISABLE KEYS */;

/*!40000 ALTER TABLE `order_create` ENABLE KEYS */;

-- 
-- Definition of products
-- 

DROP TABLE IF EXISTS `products`;
CREATE TABLE IF NOT EXISTS `products` (
  `id_product` int NOT NULL AUTO_INCREMENT,
  `id_category` int NOT NULL,
  `id_type_weaving` int NOT NULL,
  `name_product` varchar(45) NOT NULL,
  `weight` int NOT NULL,
  `price` int NOT NULL,
  `count` int NOT NULL,
  PRIMARY KEY (`id_product`),
  KEY `id_type_weaving` (`id_type_weaving`),
  KEY `id_category` (`id_category`),
  CONSTRAINT `products_ibfk_1` FOREIGN KEY (`id_type_weaving`) REFERENCES `type_weaving` (`id_type_weaving`),
  CONSTRAINT `products_ibfk_2` FOREIGN KEY (`id_category`) REFERENCES `categories` (`id_category`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb3;

-- 
-- Dumping data for table products
-- 

/*!40000 ALTER TABLE `products` DISABLE KEYS */;

/*!40000 ALTER TABLE `products` ENABLE KEYS */;

-- 
-- Definition of status
-- 

DROP TABLE IF EXISTS `status`;
CREATE TABLE IF NOT EXISTS `status` (
  `StatusId` int NOT NULL AUTO_INCREMENT,
  `StatusName` varchar(45) NOT NULL,
  PRIMARY KEY (`StatusId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- 
-- Dumping data for table status
-- 

/*!40000 ALTER TABLE `status` DISABLE KEYS */;

/*!40000 ALTER TABLE `status` ENABLE KEYS */;

-- 
-- Definition of order_buy
-- 

DROP TABLE IF EXISTS `order_buy`;
CREATE TABLE IF NOT EXISTS `order_buy` (
  `id_order_buy` int NOT NULL AUTO_INCREMENT,
  `id_basket` int NOT NULL,
  `StatusName` int NOT NULL,
  PRIMARY KEY (`id_order_buy`),
  UNIQUE KEY `id_order_buy` (`id_order_buy`),
  UNIQUE KEY `id_basket` (`id_basket`),
  KEY `StatusName` (`StatusName`),
  CONSTRAINT `order_buy_cliets_id` FOREIGN KEY (`id_basket`) REFERENCES `basket` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `order_buy_ibfk_1` FOREIGN KEY (`StatusName`) REFERENCES `status` (`StatusId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- 
-- Dumping data for table order_buy
-- 

/*!40000 ALTER TABLE `order_buy` DISABLE KEYS */;

/*!40000 ALTER TABLE `order_buy` ENABLE KEYS */;

-- 
-- Definition of type_weaving
-- 

DROP TABLE IF EXISTS `type_weaving`;
CREATE TABLE IF NOT EXISTS `type_weaving` (
  `id_type_weaving` int NOT NULL AUTO_INCREMENT,
  `name_weaving` varchar(45) NOT NULL,
  PRIMARY KEY (`id_type_weaving`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb3;

-- 
-- Dumping data for table type_weaving
-- 

/*!40000 ALTER TABLE `type_weaving` DISABLE KEYS */;
INSERT INTO `type_weaving`(`id_type_weaving`,`name_weaving`) VALUES(2,'Корда'),(3,'Сингапур'),(4,'Якорь'),(5,'Снейк'),(7,'Бисмарк');
/*!40000 ALTER TABLE `type_weaving` ENABLE KEYS */;

-- 
-- Definition of user
-- 

DROP TABLE IF EXISTS `user`;
CREATE TABLE IF NOT EXISTS `user` (
  `id_user` int NOT NULL AUTO_INCREMENT,
  `login` varchar(45) NOT NULL,
  `pwd` varchar(45) NOT NULL,
  `role` enum('admin','user','master') NOT NULL,
  PRIMARY KEY (`id_user`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- 
-- Dumping data for table user
-- 

/*!40000 ALTER TABLE `user` DISABLE KEYS */;

/*!40000 ALTER TABLE `user` ENABLE KEYS */;


/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;


-- Dump completed on 2023-04-24 21:07:14
-- Total time: 0:0:0:0:175 (d:h:m:s:ms)
