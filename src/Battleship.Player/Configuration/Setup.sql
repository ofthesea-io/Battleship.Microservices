IF not exists (select * from sys.databases where Name = 'Battleship.Player')
BEGIN 
	CREATE DATABASE [Battleship.Player] 
	ALTER DATABASE [Battleship.Player] SET ANSI_NULL_DEFAULT OFF 
	ALTER DATABASE [Battleship.Player] SET ANSI_NULLS OFF 
END
GO

IF exists (select * from sys.databases where Name = 'Battleship.Player')
BEGIN
USE [Battleship.Player]
	IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Player]') AND type in (N'U'))
BEGIN
			CREATE TABLE [dbo].[Player](
			[PlayerId] [uniqueidentifier] NOT NULL,
			[Firstname] [varchar](50) NOT NULL,
			[Lastname] [varchar](50) NOT NULL,
			[Email] [varchar](100) NOT NULL,
			[Password] [varchar](16) NOT NULL,
			[IsDemo] [bit] NOT NULL,
			[CurrentLevel] [int] NOT NULL,
			[DateCreated] [datetime2](7) NOT NULL,
		 CONSTRAINT [PK_Player] PRIMARY KEY CLUSTERED 
		(
			[PlayerId] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
		) ON [PRIMARY]

		ALTER TABLE [dbo].[Player] ADD  CONSTRAINT [DF_Player_PlayerId]  DEFAULT (newid()) FOR [PlayerId]
		
		ALTER TABLE [dbo].[Player] ADD  CONSTRAINT [DF_Player_IsDemo]  DEFAULT ((0)) FOR [IsDemo]

		ALTER TABLE [dbo].[Player] ADD  CONSTRAINT [DF_Player_CurrentLevel]  DEFAULT ((1)) FOR [CurrentLevel]

		ALTER TABLE [dbo].[Player] ADD  CONSTRAINT [DF_Player_DateCreated]  DEFAULT (getutcdate()) FOR [DateCreated]
 END		
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [dbo].[spGetPlayers]
GO

DROP PROCEDURE IF EXISTS [dbo].[spGetDemoPlayers]
GO

DROP PROCEDURE IF EXISTS [dbo].[spGetPlayerByPlayerId]
GO

DROP PROCEDURE IF EXISTS [dbo].[spCreatePlayer]
GO

DROP PROCEDURE IF EXISTS [dbo].[spPlayerLogin]
GO

DROP PROCEDURE IF EXISTS [dbo].[spDemoLogin]
GO

DROP PROCEDURE IF EXISTS [dbo].[spPlayerLogout]
GO


CREATE PROCEDURE [dbo].[spCreatePlayer]
@firstname varchar(50),
@lastname varchar(50),
@email varchar(100),
@password varchar(50)
AS
BEGIN
	IF ISNULL(@firstname, '') = ''
	BEGIN
		RAISERROR('First name cannot be empty', 16, 0);
		RETURN;
	END;

	IF ISNULL(@lastname, '') = ''
	BEGIN
		RAISERROR('Last name cannot be empty', 16, 0);
		RETURN;
	END;

	IF ISNULL(@email, '') = ''
	BEGIN
		RAISERROR('Email cannot be empty', 16, 0);
		RETURN;
	END;

	IF ISNULL(@password, '') = ''
	BEGIN
		RAISERROR('Password cannot be empty', 16, 0);
		RETURN;
	END;

	DECLARE @playerId uniqueidentifier = NEWID();
	INSERT INTO dbo.Player(PlayerId, Firstname, Lastname, Email, Password)
	VALUES(@playerId, @Firstname, @Lastname, @email, @password);

	SELECT @playerId
END
GO

DROP FUNCTION IF EXISTS [dbo].[ufnIsIdentifierValid]
GO

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

CREATE PROCEDURE [dbo].[spGetPlayerByPlayerId]
	@playerId varchar(50)
AS
BEGIN
	 IF ((SELECT dbo.ufnIsIdentifierValid(@playerId)) = 0)
        BEGIN
            RAISERROR('Invalid parameter: PlayerId cannot be NULL or empty', 18, 0)
            RETURN
        END


	SELECT  p.PlayerId, p.Firstname, p.Lastname, P.Email, p.IsDemo, p.DateCReated  
	FROM [Battleship.Player].dbo.Player p where PlayerId = @playerId
END
GO


CREATE PROCEDURE [dbo].[spGetPlayers]
	@sessionToken varchar(50)
AS
BEGIN
	select * from Player
END
GO

CREATE PROCEDURE [dbo].[spGetDemoPlayers]
AS
BEGIN
	select * from Player where IsDemo = 1
END
GO

CREATE PROCEDURE spPlayerLogin @email    VARCHAR(100), 
                               @password VARCHAR(16)
AS
    BEGIN

	IF ISNULL(@email, '') = ''
	BEGIN
		RAISERROR('Email cannot be empty', 16, 0);
		RETURN;
	END;

	IF ISNULL(@password, '') = ''
	BEGIN
		RAISERROR('Password cannot be empty', 16, 0);
		RETURN;
	END;

        DECLARE @sessionToken UNIQUEIDENTIFIER =  NEWID();
        
            SELECT PlayerId, 
				   Email,
                   Firstname, 
                   Lastname,
				   @sessionToken as SessionGuid
            FROM Player
            WHERE Email = @email
                  AND Password = @password

			select @sessionToken 
   
    END
GO

CREATE PROCEDURE spPlayerLogout
	@PlayerId uniqueidentifier
AS
BEGIN
	declare @sessionToken uniqueidentifier = NEWID();


	 IF ((SELECT dbo.ufnIsIdentifierValid(@playerId)) = 0)
        BEGIN
            RAISERROR('Invalid parameter: PlayerId cannot be NULL or empty', 18, 0)
            RETURN
        END
	SELECT  p.PlayerId, p.Firstname, p.Lastname, P.Email, p.IsDemo, p.DateCReated  
	FROM [Battleship.Player].dbo.Player p where PlayerId = @playerId 
END
GO

CREATE PROCEDURE spDemoLogin
	@PlayerId uniqueidentifier
AS
BEGIN
	declare @sessionToken uniqueidentifier = NEWID();

	     SELECT PlayerId, 
                Firstname, 
                Lastname,
				 @sessionToken as SessionGuid
            FROM Player
            where PlayerId = @PlayerId

			select @sessionToken
END
GO

IF NOT EXISTS(SELECT * FROM Player WHERE Email = 'john@battleship.com' )
BEGIN
	INSERT INTO dbo.Player(Firstname, Lastname, Email, Password, IsDemo) VALUES('John', 'Doe', 'john@battleship.com', 'password1', 1);
END
GO

IF NOT EXISTS(SELECT * FROM Player WHERE Email = 'jane@battleship.com' )
BEGIN
	INSERT INTO dbo.Player(Firstname, Lastname, Email, Password, IsDemo) VALUES('Jane', 'Doe', 'jane@battleship.com', 'password2', 1);
END
GO
