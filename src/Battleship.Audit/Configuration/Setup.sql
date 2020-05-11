IF not exists (select * from sys.databases where Name = 'Battleship.Auditing')
BEGIN 
	CREATE DATABASE [Battleship.Auditing] 
	ALTER DATABASE [Battleship.Auditing]  SET ANSI_NULL_DEFAULT OFF 
	ALTER DATABASE [Battleship.Auditing]  SET ANSI_NULLS OFF 
END
GO


USE [Battleship.Auditing]
GO
/****** Object:  Table [dbo].[Audit]    Script Date: 07/05/2020 15:04:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Audit]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Audit](
	[AuditId] [int] IDENTITY(1,1) NOT NULL,
	[Timestamp] [datetime2](7) NOT NULL,
	[Content] [text] NOT NULL,
	[AuditTypeId] [int] NOT NULL,
 CONSTRAINT [PK_Audit] PRIMARY KEY CLUSTERED 
(
	[AuditId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[AuditType]    Script Date: 07/05/2020 15:04:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AuditType]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AuditType](
	[AuditTypeId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
 CONSTRAINT [PK_AuditType] PRIMARY KEY CLUSTERED 
(
	[AuditTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Audit_Timestamp]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Audit] ADD  CONSTRAINT [DF_Audit_Timestamp]  DEFAULT (getdate()) FOR [Timestamp]
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Audit_Audit]') AND parent_object_id = OBJECT_ID(N'[dbo].[Audit]'))
ALTER TABLE [dbo].[Audit]  WITH CHECK ADD  CONSTRAINT [FK_Audit_Audit] FOREIGN KEY([AuditId])
REFERENCES [dbo].[AuditType] ([AuditTypeId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Audit_Audit]') AND parent_object_id = OBJECT_ID(N'[dbo].[Audit]'))
ALTER TABLE [dbo].[Audit] CHECK CONSTRAINT [FK_Audit_Audit]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AuditType_AuditType]') AND parent_object_id = OBJECT_ID(N'[dbo].[AuditType]'))
ALTER TABLE [dbo].[AuditType]  WITH CHECK ADD  CONSTRAINT [FK_AuditType_AuditType] FOREIGN KEY([AuditTypeId])
REFERENCES [dbo].[AuditType] ([AuditTypeId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AuditType_AuditType]') AND parent_object_id = OBJECT_ID(N'[dbo].[AuditType]'))
ALTER TABLE [dbo].[AuditType] CHECK CONSTRAINT [FK_AuditType_AuditType]
GO

--PRESETS
USE [Battleship.Auditing]
GO
SET IDENTITY_INSERT [dbo].[AuditType] ON 
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditType] WHERE [Name] = N'Error')
BEGIN
INSERT [dbo].[AuditType] ([AuditTypeId], [Name]) VALUES (1, N'Error')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditType] WHERE [Name] = N'Warning')
BEGIN
INSERT [dbo].[AuditType] ([AuditTypeId], [Name]) VALUES (2, N'Warning')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditType] WHERE [Name] = N'Info')
BEGIN
INSERT [dbo].[AuditType] ([AuditTypeId], [Name]) VALUES (3, N'Info')
END
GO
SET IDENTITY_INSERT [dbo].[AuditType] OFF
GO


USE [Battleship.Auditing]
GO
DROP PROCEDURE IF EXISTS [dbo].[spSaveAuditContent]
GO
DROP PROCEDURE IF EXISTS [dbo].[spGetAuditContentByAuditType]
GO
DROP PROCEDURE IF EXISTS [dbo].[spGetAuditContent]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jakes Potgieter
-- Create date: 08-05-202
-- Description:	Gets all audit Content
-- =============================================
CREATE PROCEDURE [dbo].[spGetAuditContent] 
AS
BEGIN
	SELECT a.Timestamp, a.Content, a.AuditTypeId
	FROM [dbo].[Audit] a
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jakes Potgieter
-- Create date: 08-05-202
-- Description:	Gets audit Content by audit type
-- =============================================
CREATE PROCEDURE [dbo].[spGetAuditContentByAuditType]
@auditTypeId int
AS
BEGIN
	SELECT a.Timestamp, a.Content, a.AuditTypeId
	FROM [dbo].[Audit] a
	WHERE a.AuditTypeId = @auditTypeId
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jakes Potgieter
-- Create date: 08-05-2020
--				11-05-2020 : Removed username field from the audit table
-- Description:	Save audit Content
-- =============================================
CREATE PROCEDURE [dbo].[spSaveAuditContent] 
	@AuditTypeId int,
	@Content text,
	@Timestamp datetime2
AS
BEGIN
	INSERT INTO [dbo].[Audit]
           ([AuditTypeId]
		   ,[Content]
		   ,[Timestamp])
     VALUES
           (@AuditTypeId
           ,@Content
           ,@Timestamp)
END
GO
