IF not exists (select * from sys.databases where Name = 'Battleship.Statistics')
BEGIN 
	CREATE DATABASE [Battleship.Statistics] 
	ALTER DATABASE [Battleship.Statistics]  SET ANSI_NULL_DEFAULT OFF 
	ALTER DATABASE [Battleship.Statistics]  SET ANSI_NULLS OFF 
END
GO

IF EXISTS (select * from sys.databases where Name = 'Battleship.Statistics')
BEGIN
USE [Battleship.Statistics]
	IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Leaderboard]') AND type in (N'U'))
	BEGIN
		CREATE TABLE [dbo].[Leaderboard](
			[StatisticsId] [uniqueidentifier] NOT NULL,
			[FullName] [varchar](200) NOT NULL,
			[Email] [varchar](100) NOT NULL,
			[Percentage] [numeric](2, 2) NOT NULL,
			[CompletedGames] [int] NOT NULL,
			[CompletedOn] [datetime2](7) NOT NULL,
		 CONSTRAINT [PK_Leaderboard] PRIMARY KEY CLUSTERED 
		(
			[StatisticsId] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
		 CONSTRAINT [IX_Leaderboard] UNIQUE NONCLUSTERED 
		(
			[StatisticsId] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
		) ON [PRIMARY]
		ALTER TABLE [dbo].[Leaderboard] ADD  CONSTRAINT [DF_Leaderboard_StatisticsId]  DEFAULT (newid()) FOR [StatisticsId]
		ALTER TABLE [dbo].[Leaderboard] ADD  CONSTRAINT [DF_Leaderboard_Percentage]  DEFAULT ((0)) FOR [Percentage]
		ALTER TABLE [dbo].[Leaderboard] ADD  CONSTRAINT [DF_Leaderboard_CompletedGames]  DEFAULT ((0)) FOR [CompletedGames]
		ALTER TABLE [dbo].[Leaderboard] ADD  CONSTRAINT [DF_Leaderboard_LastPlayed]  DEFAULT (getdate()) FOR [CompletedOn]
	END
END
		
USE [Battleship.Statistics]

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


DROP PROCEDURE IF EXISTS [dbo].[spSaveStatistics]
GO

DROP PROCEDURE IF EXISTS [dbo].[spGetTopTenPlayers]
GO

DROP PROCEDURE IF EXISTS [dbo].[spGetPlayerByEmail]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spGetPlayerByEmail]
	@email varchar(100)
AS
BEGIN

	IF ISNULL(@email, '') = ''
	BEGIN
		RAISERROR('Email cannot be empty', 16, 0);
		RETURN;
	END;

	SELECT 
	TOP 10 l.FullName
		, l.Percentage
		, l.CompletedOn
		, l.CompletedGames
	FROM dbo.Leaderboard l
	WHERE l.Email = @email
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spGetTopTenPlayers]
AS
BEGIN
	SELECT 
	TOP 10 l.FullName
		, l.Percentage
		, l.CompletedOn
		, l.CompletedGames
	FROM dbo.Leaderboard l
	ORDER BY l.Percentage
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spSaveStatistics] 
				 @fullName varchar(200), @email varchar(100), @Percentage numeric, @CompletedGames int
AS
BEGIN
	IF ISNULL(@email, '') = ''
	BEGIN
		RAISERROR('Email cannot be empty', 16, 0);
		RETURN;
	END;

	-- Update or create new
	IF EXISTS(SELECT * FROM dbo.Leaderboard l WHERE l.Email = @Email)
	BEGIN
		UPDATE [dbo].[Leaderboard]
		   SET [FullName] = @FullName
			  ,[Percentage] = @Percentage
			  ,[CompletedGames] = @CompletedGames
			  ,[CompletedOn] = GetDate()
		 WHERE [Email] = @Email
	END
	ELSE
		BEGIN
			INSERT INTO [dbo].[Leaderboard]([FullName], [Email], [Percentage], [CompletedGames], [CompletedOn] )
			VALUES( @fullName, @email, @Percentage, @CompletedGames, GETDATE());
		END
END;
GO
