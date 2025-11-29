-- MySQL Initialization Script untuk Employee Manager
-- Jalankan script ini di MySQL untuk setup awal database

-- Buat database jika belum ada
CREATE DATABASE IF NOT EXISTS `db_sample` 
CHARACTER SET utf8mb4 
COLLATE utf8mb4_unicode_ci;

-- Gunakan database
USE `db_sample`;

-- Buat table employee jika belum ada
CREATE TABLE IF NOT EXISTS `employee` (
    `row_id` INT AUTO_INCREMENT PRIMARY KEY,
    `id` VARCHAR(6) NOT NULL UNIQUE,
    `name` VARCHAR(100) NOT NULL,
    `salary` DECIMAL(15, 2) NOT NULL,
    `status` INT NOT NULL DEFAULT 1,
    `created_at` TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    `updated_at` TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    INDEX `idx_id` (`id`),
    INDEX `idx_name` (`name`),
    INDEX `idx_status` (`status`)
) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- Insert sample data (optional)
INSERT INTO `employee` (`id`, `name`, `salary`, `status`) VALUES
('001', 'John Doe', 5000000, 1),
('002', 'Jane Smith', 6500000, 1),
('003', 'Bob Johnson', 4500000, 0),
('004', 'Alice Williams', 7000000, 1),
('005', 'Charlie Brown', 5500000, 1);

-- Check data
SELECT * FROM `employee`;

