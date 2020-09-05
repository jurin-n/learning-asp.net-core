/* dev2データベース作成 */
USE master;
GO
IF DB_ID (N'dev2') IS NOT NULL
DROP DATABASE dev2;
GO
CREATE DATABASE dev2
COLLATE Japanese_CS_AS
WITH TRUSTWORTHY ON, DB_CHAINING ON;
GO
--Verifying collation and option settings.
SELECT name, collation_name, is_trustworthy_on, is_db_chaining_on
FROM sys.databases
WHERE name = N'dev2';
GO