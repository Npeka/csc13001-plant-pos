CREATE USER IF NOT EXISTS 'plantstore'@'%' IDENTIFIED BY 'plantstore';

CREATE DATABASE IF NOT EXISTS plantstore;

GRANT ALL PRIVILEGES ON plantstore.* TO 'plantstore'@'%';

FLUSH PRIVILEGES;

USE plantstore;

-- Drop tables if exists
DROP TABLE IF EXISTS users;
DROP TABLE IF EXISTS staffs;
DROP TABLE IF EXISTS staff_work_history;
DROP TABLE IF EXISTS customers;
DROP TABLE IF EXISTS orders;
DROP TABLE IF EXISTS order_details;
DROP TABLE IF EXISTS categories;
DROP TABLE IF EXISTS products;
DROP TABLE IF EXISTS inventories;
DROP TABLE IF EXISTS discount_programs;
DROP TABLE IF EXISTS discount_products;

-- Create tables if not exists
CREATE TABLE IF NOT EXISTS users (
    user_id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(255) NOT NULL,
    password VARCHAR(255) NOT NULL,
    email VARCHAR(255),
    is_admin BOOLEAN NOT NULL DEFAULT FALSE
);

CREATE TABLE IF NOT EXISTS staffs (
    staff_id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT NOT NULL,
    name VARCHAR(255) NOT NULL,
    phone VARCHAR(20),
    email VARCHAR(255),
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES users(user_id)
);

CREATE TABLE IF NOT EXISTS staff_work_history (
    history_id INT AUTO_INCREMENT PRIMARY KEY,
    staff_id INT NOT NULL,
    timestamp DATETIME NOT NULL,
    FOREIGN KEY (staff_id) REFERENCES staffs(staff_id)
);

CREATE TABLE IF NOT EXISTS customers (
    customer_id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    phone VARCHAR(20) NOT NULL,
    email VARCHAR(255),
    address TEXT,
    loyalty_points INT DEFAULT 0,
    loyalty_card_type VARCHAR(50)
);

CREATE TABLE IF NOT EXISTS categories (
    category_id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    description TEXT
);

CREATE TABLE IF NOT EXISTS products (
    product_id INT AUTO_INCREMENT PRIMARY KEY,
    category_id INT NOT NULL,
    name VARCHAR(255) NOT NULL,
    description TEXT,
    price DECIMAL(10, 2) NOT NULL,
    stock INT NOT NULL,
    care_level INT NOT NULL,
    environment_type VARCHAR(50),
    size VARCHAR(50),
    light_requirement INT NOT NULL,
    watering_schedule INT NOT NULL,
    FOREIGN KEY (category_id) REFERENCES categories(category_id)
);

CREATE TABLE IF NOT EXISTS discount_programs (
    discount_id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    start_date DATETIME NOT NULL,
    end_date DATETIME NOT NULL,
    applicable_customer_type VARCHAR(50)
);

CREATE TABLE IF NOT EXISTS orders (
    order_id INT AUTO_INCREMENT PRIMARY KEY,
    customer_id INT NOT NULL,
    staff_id INT NOT NULL,
    order_date DATETIME NOT NULL,
    total_price DECIMAL(10, 2) NOT NULL,
    status ENUM('Pending', 'Completed', 'Canceled') NOT NULL DEFAULT 'Pending',
    FOREIGN KEY (customer_id) REFERENCES customers(customer_id),
    FOREIGN KEY (staff_id) REFERENCES staffs(staff_id)
);

CREATE TABLE IF NOT EXISTS order_details (
    order_detail_id INT AUTO_INCREMENT PRIMARY KEY,
    order_id INT NOT NULL,
    product_id INT NOT NULL,
    quantity INT NOT NULL,
    unit_price DECIMAL(10, 2) NOT NULL,
    discount_id INT,
    FOREIGN KEY (order_id) REFERENCES orders(order_id),
    FOREIGN KEY (product_id) REFERENCES products(product_id),
    FOREIGN KEY (discount_id) REFERENCES discount_programs(discount_id)
);

CREATE TABLE IF NOT EXISTS inventories (
    batch_id INT AUTO_INCREMENT PRIMARY KEY,
    supplier VARCHAR(255),
    product_id INT NOT NULL,
    quantity INT NOT NULL,
    purchase_price DECIMAL(10, 2) NOT NULL,
    purchase_date DATETIME NOT NULL,
    FOREIGN KEY (product_id) REFERENCES products(product_id)
);

CREATE TABLE IF NOT EXISTS discount_products (
    discount_id INT NOT NULL,
    product_id INT NOT NULL,
    discount_percentage DECIMAL(5, 2) NOT NULL,
    PRIMARY KEY (discount_id, product_id),
    FOREIGN KEY (discount_id) REFERENCES discount_programs(discount_id),
    FOREIGN KEY (product_id) REFERENCES products(product_id)
);

-- Insert data into users
INSERT INTO users (username, password, email, is_admin) VALUES
('admin', 'adminpass', 'admin@plantstore.com', TRUE),
('staff1', 'staffpass1', 'staff1@plantstore.com', FALSE),
('staff2', 'staffpass2', 'staff2@plantstore.com', FALSE);

-- Insert data into staff
INSERT INTO staffs (user_id, name, phone, email) VALUES
(2, 'John Doe', '1234567890', 'john.doe@plantstore.com'),
(3, 'Jane Smith', '0987654321', 'jane.smith@plantstore.com');

-- Insert data into customers
INSERT INTO customers (name, phone, email, address, loyalty_points, loyalty_card_type) VALUES
('Alice Johnson', '1112223333', 'alice.johnson@example.com', '123 Maple St', 100, 'Gold'),
('Bob Brown', '4445556666', 'bob.brown@example.com', '456 Oak St', 50, 'Silver');

-- Insert data into categories
INSERT INTO categories (name, description) VALUES
('Indoor Plants', 'Plants that thrive indoors'),
('Outdoor Plants', 'Plants that thrive outdoors');

-- Insert data into products
INSERT INTO products (category_id, name, description, price, stock, care_level, environment_type, size, light_requirement, watering_schedule) VALUES
(1, 'Fiddle Leaf Fig', 'A popular indoor plant', 49.99, 20, 3, 'Indoor', 'Medium', 4, 7),
(2, 'Rose Bush', 'A beautiful outdoor plant', 29.99, 15, 2, 'Outdoor', 'Large', 5, 3);

-- Insert data into inventorys
INSERT INTO inventorys (supplier, product_id, quantity, purchase_price, purchase_date) VALUES
('Plant Supplier Inc.', 1, 50, 25.00, '2023-01-15'),
('Garden Supplies Co.', 2, 30, 15.00, '2023-01-20');

-- Insert data into discount_programs
INSERT INTO discount_programs (name, start_date, end_date, applicable_customer_type) VALUES
('Spring Sale', '2023-03-01', '2023-03-31', 'All'),
('Loyalty Discount', '2023-01-01', '2023-12-31', 'Gold');

-- Insert data into discount_products
INSERT INTO discount_products (discount_id, product_id, discount_percentage) VALUES
(1, 1, 10.00),
(2, 2, 15.00);

-- Insert data into orders
INSERT INTO orders (customer_id, staff_id, order_date, total_price, status) VALUES
(1, 2, '2023-02-03', 79.98, 'Completed'),
(2, 2, '2023-02-04', 59.98, 'Pending');

-- Insert data into order_details
INSERT INTO order_details (order_id, product_id, quantity, unit_price, discount_id) VALUES
(1, 1, 1, 49.99, 1),
(2, 2, 1, 29.99, 2), 
(2, 1, 1, 49.99, NULL),
(2, 2, 1, 29.99, NULL),
(2, 1, 1, 49.99, NULL);