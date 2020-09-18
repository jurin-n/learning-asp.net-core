/* 接続文字列 */
Server=localhost\SQLEXPRESS;Database=master;Trusted_Connection=True;


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


/*
Windows認証でSQL Serverにログインして行う作業
*/
--SQL Server DeveloperのログインモードをSQL Server認証に変更
https://docs.microsoft.com/ja-jp/sql/database-engine/configure-windows/change-server-authentication-mode?view=sql-server-ver15


--ユーザ作成
USE master
--CREATE LOGIN user01 WITH PASSWORD = '&DiJ652j*e';
CREATE LOGIN user01 WITH PASSWORD = 'test123';
CREATE USER user01 FOR LOGIN user01;  
GO 

--ロール付与
EXEC master..sp_addsrvrolemember @loginame = N'user01', @rolename = N'sysadmin'
GO
