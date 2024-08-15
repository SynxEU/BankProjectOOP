------------------------------------------------
-- Drops old DB if it exists
------------------------------------------------
USE [master]
GO
IF EXISTS (SELECT * FROM sysdatabases WHERE name='Bank')
BEGIN
	ALTER DATABASE [Bank] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
	DROP database Bank
END
GO

------------------------------------------------
-- New DB
------------------------------------------------
CREATE DATABASE Bank
GO
------------------------------------------------
-- Tables
------------------------------------------------
USE Bank

--Drop if exists
DROP TABLE IF EXISTS UserTable
DROP TABLE IF EXISTS AccountTable
DROP TABLE IF EXISTS AccountsToUser

--Create 
CREATE TABLE UserTable (
	[User ID] INT IDENTITY PRIMARY KEY,
	[Name] NVARCHAR(255) NOT NULL,
	[Age] INT NOT NULL,
	[E-Mail] NVARCHAR(255) NOT NULL,
	[Password] VARBINARY(64) NOT NULL,
	[Registration Date] DATE NOT NULL,
)
CREATE TABLE AccountTable (
	[Account Number] INT IDENTITY PRIMARY KEY,
	[TitleID] INT NOT NULL,
	[Balance] INT DEFAULT 0,
	[Interest Rate] INT DEFAULT 0
)
CREATE TABLE AccountsToUser (
	[User ID] INT,
	[Account Number] INT,
	FOREIGN KEY ([User ID]) REFERENCES UserTable([User ID]),
	FOREIGN KEY ([Account Number]) REFERENCES AccountTable([Account Number])
)
GO
------------------------------------------------
-- View
------------------------------------------------
CREATE OR ALTER VIEW GetUserID AS
SELECT [User ID] FROM (
	SELECT *, ROW_NUMBER() OVER (ORDER BY [User ID] DESC) AS rn
	FROM UserTable
) AS subquery
WHERE rn = 1;
GO
CREATE OR ALTER VIEW GetAccountNumber AS
SELECT [Account Number] FROM (
	SELECT *, ROW_NUMBER() OVER (ORDER BY [Account Number] DESC) AS rn
	FROM AccountTable
) AS subquery
WHERE rn = 1;
GO
------------------------------------------------
-- Stored Procedure
------------------------------------------------

--Create
CREATE OR ALTER PROCEDURE sp_CreateUser
(@Name NVARCHAR(255), @Age INT, @Mail NVARCHAR(255), @Password NVARCHAR(50))
AS
Begin
	DECLARE @HashedPassword VARBINARY(64) = HASHBYTES('SHA2_256', @Password);
	DECLARE @Date DATE = CONVERT(DATE, getutcdate());
	INSERT INTO [UserTable]
	([Name], [Age], [E-Mail], [Password], [Registration Date]) 
	VALUES 
	(@Name, @Age, @Mail, @HashedPassword, @Date)
END
GO
CREATE OR ALTER PROCEDURE sp_CreateAccount
(@TitleID INT, @Interest INT)
AS
BEGIN
	INSERT INTO [AccountTable]
	([TitleID],[Interest Rate])
	VALUES
	(@TitleID, @Interest)
END
GO
CREATE OR ALTER PROCEDURE sp_CreateAccountUserShip
(@UserID INT)
AS
BEGIN
	IF @UserID IS NULL
		BEGIN
			INSERT INTO [AccountsToUser]
			([User ID], [Account Number])
			VALUES
			((SELECT * FROM GetUserID),(SELECT * FROM GetAccountNumber))
		END
	ELSE
		BEGIN
			INSERT INTO [AccountsToUser]
			([User ID], [Account Number])
			VALUES
			(@UserID, (SELECT * FROM GetAccountNumber))
		END
END
GO

--Logon
CREATE OR ALTER PROCEDURE sp_UserLogOn
(@Mail NVARCHAR(255), @Password NVARCHAR(50))
AS
BEGIN
	DECLARE @StoredHashedPassword VARBINARY(64);
        SELECT @StoredHashedPassword = [Password]
    FROM [UserTable]
    WHERE [E-Mail] = @Mail;
    IF HASHBYTES('SHA2_256', @Password) = @StoredHashedPassword
	BEGIN
        SELECT 'Login successful' AS Result;
		SELECT * FROM [UserTable] WHERE [E-Mail] = @Mail 
    END
    ELSE
    BEGIN
        SELECT 'Login failed' AS Result;
    END
END
GO

--WithDraw/Deposit
CREATE OR ALTER PROCEDURE sp_WithDraw
(@AccountNumber INT, @Withdraw DECIMAL)
AS
BEGIN
    DECLARE @Balance DECIMAL = (SELECT [Balance] FROM [AccountTable] WHERE [Account Number] = @AccountNumber)
    IF @Balance IS NULL
    BEGIN
        SELECT 'Account does not exist' AS Result;
		RETURN
    END
    IF @Balance < @Withdraw
    BEGIN
        SELECT 'You can not overdraw' AS Result;
		RETURN
    END
    UPDATE [AccountTable]
    SET [Balance] = (@Balance - @Withdraw)
    WHERE [Account Number] = @AccountNumber
    SELECT 'You have now withdrawen' AS Result;
END
GO
CREATE OR ALTER PROCEDURE sp_Deposit
(@AccountNumber INT, @Deposit DECIMAL)
AS
BEGIN
    DECLARE @Balance DECIMAL = (SELECT [Balance] FROM [AccountTable] WHERE [Account Number] = @AccountNumber)
    IF @Balance IS NULL
    BEGIN
        SELECT 'Account does not exist' AS Result;
		RETURN
    END
    UPDATE [AccountTable]
    SET [Balance] = (@Balance - @Deposit)
    WHERE [Account Number] = @AccountNumber
    SELECT 'You have now deposited' AS Result;
END
GO

--Get alls
CREATE PROCEDURE sp_GetAllUsers
AS
BEGIN
	SELECT * FROM [UserTable]
END
GO
CREATE PROCEDURE sp_GetAllAccounts
AS
BEGIN
	SELECT * FROM [AccountTable]
END
GO

--Get By
CREATE PROCEDURE sp_GetAccountByNumber
(@AccountNumber INT)
AS
BEGIN
	SELECT * FROM [AccountTable] WHERE [Account Number] = @AccountNumber
END
GO
CREATE PROCEDURE sp_GetAccountsByUserID
(@UserId INT)
AS
BEGIN
	SELECT * FROM [AccountsToUser] WHERE [User ID] = @UserId
END
GO
CREATE PROCEDURE sp_GetUserByMail
(@Mail NVARCHAR(255))
AS
BEGIN
	SELECT * FROM [UserTable] WHERE [E-Mail] = @Mail
END
GO
CREATE PROCEDURE sp_GetUserByID
(@UserId INT)
AS
BEGIN
	SELECT * FROM [UserTable] WHERE [User ID] = @UserId
END
GO

--Delete
CREATE PROCEDURE sp_DeleteUser
(@UserId INT)
AS
BEGIN
	DELETE FROM [UserTable] WHERE [User ID] = @UserId
	DELETE FROM [AccountsToUser] WHERE [User ID] = @UserId
END
GO
CREATE PROCEDURE sp_DeleteAccount
(@AccountNumber INT)
AS
BEGIN
	DELETE FROM [AccountTable] WHERE [Account Number] = @AccountNumber
	DELETE FROM [AccountsToUser] WHERE [Account Number] = @AccountNumber
END
GO