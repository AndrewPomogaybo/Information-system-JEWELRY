-- MySQL Script generated by MySQL Workbench
-- Fri Feb  3 16:57:12 2023
-- Model: New Model    Version: 1.0
-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema JEWELLERY
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema JEWELLERY
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `JEWELLERY` DEFAULT CHARACTER SET utf8 ;
USE `JEWELLERY` ;

create table if not exists `Status`
(
 StatusId int not null auto_increment primary key,
 StatusName VARCHAR(45) NOT NULL
);

CREATE TABLE IF NOT EXISTS `JEWELLERY`.`type_weaving` (
  `id_type_weaving` INT NOT NULL AUTO_INCREMENT,
  `name_weaving` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id_type_weaving`))
ENGINE = InnoDB;

CREATE TABLE IF NOT EXISTS `JEWELLERY`.`categories` (
  `id_category` INT NOT NULL AUTO_INCREMENT,
  `name_category` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id_category`))
ENGINE = InnoDB;
-- -----------------------------------------------------
-- Table `JEWELLERY`.`user`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `JEWELLERY`.`user` (
  `id_user` INT NOT NULL AUTO_INCREMENT,
  `login` VARCHAR(45) NOT NULL,
  `pwd` VARCHAR(45) NOT NULL,
  `role` ENUM('admin', 'user', 'master') NOT NULL,
  PRIMARY KEY (`id_user`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `JEWELLERY`.`basket`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `JEWELLERY`.`basket` (
  `id` INT NOT NULL,
  `name_prod` text NOT NULL ,
  `name_category` text NOT NULL,
  `amount` int not null,
  `cost` int not null,
  foreign key(`id`)  references `products`(`id_product`)
  )
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `JEWELLERY`.`order_buy`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `JEWELLERY`.`order_buy` (
  `id_order_buy` INT NOT NULL AUTO_INCREMENT UNIQUE,
  `Status` int not null,
  `ProductName` text not null,
  `ProductCategory` text not null,
  `Count` int not null,
  `Price` int not null,
  PRIMARY KEY (`id_order_buy`),
  CONSTRAINT `order_buy_cliets_id`
    FOREIGN KEY (`id_basket`)
    REFERENCES `JEWELLERY`.`basket` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
    foreign key(`StatusName`) REFERENCES `Status`(StatusId))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `JEWELLERY`.`order_create`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `JEWELLERY`.`order_create` (
  `id_order_create` INT NOT NULL AUTO_INCREMENT,
  `id_client` INT NOT NULL,
  `weight` INT NOT NULL,
  `price` INT NOT NULL,
  `name` VARCHAR(200) NOT NULL,
  PRIMARY KEY (`id_order_create`),
  CONSTRAINT `order_create_cliets_id`
    FOREIGN KEY (`id_client`)
    REFERENCES `JEWELLERY`.`clients` (`id_client`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `JEWELLERY`.`clients`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `JEWELLERY`.`clients` (
  `id_client` INT NOT NULL UNIQUE AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  `surname` VARCHAR(45) NOT NULL,
  `patronymic` VARCHAR(45) NOT NULL,
  `phone_number` VARCHAR(12) NOT NULL,
  PRIMARY KEY (`id_client`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `JEWELLERY`.`products`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `JEWELLERY`.`products` (
  `id_product` INT NOT NULL AUTO_INCREMENT,
  `id_category` INT NOT NULL,
  `id_type_weaving` INT NOT NULL,
  `name_product` VARCHAR(45) NOT NULL,
  `weight` INT NOT NULL,
  `price` INT NOT NULL,
  `count` int not null,
  PRIMARY KEY (`id_product`),
  foreign key (`id_type_weaving`) REFERENCES `JEWELLERY`.`type_weaving`(`id_type_weaving`),
  foreign key (`id_category`) REFERENCES `JEWELLERY`.`categories`(`id_category`)
  )
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `JEWELLERY`.`categories`
-- -----------------------------------------------------



-- -----------------------------------------------------
-- Table `JEWELLERY`.`type_weaving`
-- -----------------------------------------------------



SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
