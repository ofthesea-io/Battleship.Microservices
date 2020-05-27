IF not exists (select * from sys.databases where Name = 'Battleship.ScoreCard')
BEGIN 
	CREATE DATABASE [Battleship.ScoreCard] 
	ALTER DATABASE [Battleship.ScoreCard] SET ANSI_NULL_DEFAULT OFF 
	ALTER DATABASE [Battleship.ScoreCard] SET ANSI_NULLS OFF 
END
GO
IF exists (select * from sys.databases where Name = 'Battleship.ScoreCard')
BEGIN
USE [Battleship.ScoreCard]
	IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PlayerScoreCard]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[PlayerScoreCard](
	[SessionToken] [varchar](100) NOT NULL,
	[ScoreCard] [varchar](1000) NULL
) ON [PRIMARY]
END
END

USE [Battleship.ScoreCard]

DROP PROCEDURE IF EXISTS [dbo].[spManagePlayerScoreCard]
GO

DROP PROCEDURE IF EXISTS [dbo].[spGetPlayerScoreCard]
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

CREATE PROCEDURE [dbo].[spGetPlayerScoreCard]
	@sessionToken varchar(50)
AS
BEGIN

IF ISNULL(@sessionToken, '') = ''
	BEGIN
		RAISERROR('Session cannot be empty', 16, 0);
		RETURN;
	END;

	SELECT ScoreCard FROM PlayerSCoreCard 
	WHERE SessionToken = @sessionToken
END
GO

CREATE PROCEDURE [dbo].[spManagePlayerScoreCard]
	@sessionToken varchar(50),
	@scoreCard varchar(1000)
AS
BEGIN



IF ISNULL(@sessionToken, '') = ''
	BEGIN
		RAISERROR('Session cannot be empty', 16, 0);
		RETURN;
	END;

IF ISNULL(@scoreCard, '') = ''
	BEGIN
		RAISERROR('Score Card cannot be empty', 16, 0);
		RETURN;
	END;

	IF exists(SELECT * FROM dbo.PlayerScoreCard psc WHERE psc.SessionToken = @sessionToken)
	BEGIN
		UPDATE PlayerScoreCard
		SET ScoreCard  = @ScoreCard
		WHERE sessionToken = @sessionToken
	END
	ELSE
	BEGIN
		INSERT INTO [dbo].[PlayerScoreCard]
           ([SessionToken]
           ,[ScoreCard])
     VALUES
           (@sessionToken
           ,@scoreCard)
	END

	 SELECT psc.ScoreCard FROM dbo.PlayerScoreCard psc WHERE psc.SessionToken = @sessionToken
END


