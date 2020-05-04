IF not exists (select * from sys.databases where Name = 'Battleship.Game')
BEGIN 
	CREATE DATABASE [Battleship.Game] 
	ALTER DATABASE [Battleship.Game]  SET ANSI_NULL_DEFAULT OFF 
	ALTER DATABASE [Battleship.Game]  SET ANSI_NULLS OFF 
END
GO

IF EXISTS (select * from sys.databases where Name = 'Battleship.Game')
BEGIN
USE [Battleship.Game]
	IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GamePlay]') AND type in (N'U'))
	BEGIN
		CREATE TABLE [dbo].[GamePlay](
			[PlayerId] [uniqueidentifier] NOT NULL,
			[ShipCoordinates] [varchar](max) NULL,
			[SessionToken] [varchar](100) NOT NULL,
			[SessionExpiry] [datetime2](7) NOT NULL,
			[IsCompleted] [bit] NOT NULL,
			[DateCreated] [datetime2](7) NOT NULL
		) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
		
		ALTER TABLE [dbo].[GamePlay] ADD  CONSTRAINT [DF_GamePlay_IsCompleted]  DEFAULT ((0)) FOR [IsCompleted]
		ALTER TABLE [dbo].[GamePlay] ADD  CONSTRAINT [DF_GamePlay_DateCreated]  DEFAULT (getutcdate()) FOR [DateCreated]
	END
END

USE [Battleship.Game]

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [dbo].[spGetShipCoordinates]
GO

DROP PROCEDURE IF EXISTS [dbo].[spStartGame]
GO

DROP PROCEDURE IF EXISTS [dbo].[spCreatePlayer]
GO

DROP PROCEDURE IF EXISTS [dbo].[spCheckPlayerStatus] 
GO

DROP PROCEDURE IF EXISTS [dbo].[spUpdateShipCoordinates] 
GO

DROP PROCEDURE IF EXISTS [dbo].[spPlayerLogout] 
GO

CREATE PROCEDURE [dbo].[spGetShipCoordinates] @sessionToken VARCHAR(50)
AS
    BEGIN
	IF ISNULL(@sessionToken, '') = ''
	BEGIN
		RAISERROR('Session Token cannot be empty', 16, 0);
		RETURN;
	END;

	DECLARE @endDateDay datetime = (SELECT CAST(CONVERT(VARCHAR(10), GETDATE(), 110) + ' 23:59:59' AS DATETIME))

        SELECT ShipCoordinates
        FROM GamePlay
        WHERE SessionToken = @sessionToken AND SessionExpiry <= @endDateDay
    END
GO

CREATE PROCEDURE [dbo].[spStartGame] 
	@sessionToken VARCHAR(50),
	@shipCoordinates VARCHAR(MAX)
AS
    BEGIN
	IF ISNULL(@sessionToken, '') = ''
	BEGIN
		RAISERROR('Session Token cannot be empty', 16, 0);
		RETURN;
	END

	IF ISNULL(@shipCoordinates, '') = ''
	BEGIN
		RAISERROR('Ship Coordinates cannot be empty', 16, 0);
		RETURN;
	END

	DECLARE @endDateDay datetime = (SELECT CAST(CONVERT(VARCHAR(10), GETDATE(), 110) + ' 23:59:59' AS DATETIME))

        UPDATE GamePlay
		SET ShipCoordinates = @shipCoordinates,
			SessionExpiry = @endDateDay
        WHERE SessionToken = @sessionToken
    END
GO

CREATE PROCEDURE [dbo].[spCreatePlayer]
@sessionToken VARCHAR(50),
@playerId UNIQUEIDENTIFIER
AS
 BEGIN
        IF ISNULL(@sessionToken , '') = ''
            BEGIN
                RAISERROR('Session Token cannot be empty' , 16 , 0);
                RETURN;
        END

        DECLARE @endDateDay DATETIME= ( SELECT CAST(CONVERT(VARCHAR(10) , GETDATE() , 110) + ' 23:59:59' AS DATETIME));

		IF NOT exists(SELECT * FROM dbo.GamePlay gp WHERE gp.SessionToken = @sessionToken)
		BEGIN
			INSERT INTO GamePlay ( PlayerId , SessionToken , SessionExpiry) 
			VALUES ( @playerId , @sessionToken , GETUTCDATE())
		END
        DECLARE @result BIT = 0;

        IF EXISTS ( SELECT * FROM GamePlay WHERE sessionToken = @sessionToken AND playerId = @playerId) 
            BEGIN
                SET @result = 1;
			END
	
        SELECT @result;
    END
GO
	
CREATE PROCEDURE [dbo].[spCheckPlayerStatus] 
	@sessionToken varchar(100)
AS
BEGIN
    DECLARE @status bit = 0;
	DECLARE @endDateDay DATETIME= ( SELECT CAST(CONVERT(VARCHAR(10) , GETDATE() , 110) + ' 23:59:59' AS DATETIME));

	IF exists(SELECT * FROM dbo.GamePlay gp WHERE gp.SessionToken = @sessionToken AND gp.SessionExpiry <= @endDateDay)
	BEGIN
	   SET @status = 1;
	END

	SELECT @status AS [Status]
END
GO

CREATE PROCEDURE spPlayerLogout
		@sessionToken varchar(100)
AS
BEGIN

    DECLARE @status bit = 0;
	DECLARE @endDateDay DATETIME= ( SELECT CAST(CONVERT(VARCHAR(10) , GETDATE() , 110) + ' 23:59:59' AS DATETIME));
	
UPDATE [dbo].[GamePlay]
   SET [SessionExpiry] = @endDateDay,
	   [IsCompleted] = 0
 WHERE [SessionToken] = @sessionToken
	  
END
GO


CREATE PROCEDURE [dbo].[spUpdateShipCoordinates] 
	@updateShipCoordinates VARCHAR(MAX), 
	@SessionToken    VARCHAR(100)
AS
    BEGIN
        UPDATE GamePlay
          SET 
              ShipCoordinates = @updateShipCoordinates
        WHERE SessionToken = @SessionToken;
    END


