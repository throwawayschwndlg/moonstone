-- 0.0.1

-- create moonstone_ui_web login
CREATE LOGIN moonstone_ui WITH PASSWORD = 'moonstone_ui';

-- create moonstone_ui_web user
USE ::db::;
CREATE USER moonstone_ui_web FOR LOGIN moonstone_ui;

-- create core schema
EXEC('CREATE SCHEMA [core];');

-- grant permissions to moonstone_ui_web to core
GRANT SELECT ON SCHEMA::[core] TO moonstone_ui_web;
GRANT UPDATE ON SCHEMA::[core] TO moonstone_ui_web;
GRANT INSERT ON SCHEMA::[core] TO moonstone_ui_web;