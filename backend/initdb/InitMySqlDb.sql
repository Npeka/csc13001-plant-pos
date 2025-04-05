CREATE USER IF NOT EXISTS 'plantpos'@'%' IDENTIFIED BY 'plantpos';

CREATE DATABASE IF NOT EXISTS plantpos;

GRANT ALL PRIVILEGES ON plantpos.* TO 'plantpos'@'%';

FLUSH PRIVILEGES;

USE plantpos;