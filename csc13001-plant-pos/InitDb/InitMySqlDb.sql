-- Tạo user "crm" với mật khẩu "crm" nếu chưa tồn tại
CREATE USER IF NOT EXISTS 'plantstore'@'%' IDENTIFIED BY 'plantstore';

-- Tạo database "crm" nếu chưa tồn tại
CREATE DATABASE IF NOT EXISTS plantstore;

-- Gán quyền sở hữu database "crm" cho user "crm"
GRANT ALL PRIVILEGES ON plantstore.* TO 'plantstore'@'%';

-- Áp dụng các thay đổi quyền
FLUSH PRIVILEGES;