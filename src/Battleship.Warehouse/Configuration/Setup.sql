IF not exists (select * from sys.databases where Name = 'Battleship.WareHouse')
BEGIN 
	CREATE DATABASE [Battleship.WareHouse] 
	ALTER DATABASE [Battleship.WareHouse] SET ANSI_NULL_DEFAULT OFF 
	ALTER DATABASE [Battleship.WareHouse] SET ANSI_NULLS OFF 
END
GO
IF exists (select * from sys.databases where Name = 'Battleship.WareHouse')
BEGIN
USE [Battleship.WareHouse]
	IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PlayerWareHouse]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[PlayerWareHouse](
	[SessionToken] [varchar](100) NOT NULL,
	[Firstname] [varchar](100) NOT NULL,
	[Lastname] [varchar](100) NOT NULL,
	[Percentage] int NOT NULL,
	[IsDemo]  bit NOT NULL,
	[DateCreated] [datetime2](7) NOT NULL
) ON [PRIMARY]
END

END

USE [Battleship.WareHouse]

DROP PROCEDURE IF EXISTS [dbo].[spManagePlayerWareHouse]
GO

DROP PROCEDURE IF EXISTS [dbo].[spGetPlayerWareHouse]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--UNIQUE IDENTIFIER TEST
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ufnIsIdentifierValid]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[ufnIsIdentifierValid] 
(
	@Id UNIQUEIDENTIFIER null
)
RETURNS bit
AS
BEGIN
	DECLARE @result BIT = 0;
	DECLARE @emptyId UNIQUEIDENTIFIER = (SELECT CAST(CAST(0 AS BINARY) AS UNIQUEIDENTIFIER))

	IF(@Id IS NOT NULL AND @Id <> @emptyId)
	BEGIN
		SET @result = 1
	END

	RETURN @result

END
' 
END
GO

USE [Battleship.WareHouse]
IF EXISTS (SELECT * FROM sys.procedures WHERE Name = 'spGetTopTenPlayers')
BEGIN
	drop procedure spGetTopTenPlayers
END
GO

CREATE PROCEDURE [dbo].[spGetTopTenPlayers]
	@sessionToken varchar(50)
AS
BEGIN
IF ISNULL(@sessionToken, '') = ''
	BEGIN
		RAISERROR('Session cannot be empty', 16, 0);
		RETURN;
	END;

	-- se
	
	SELECT Top 10 * FROM PlayerWareHouse ORDER BY DateCreated
END




