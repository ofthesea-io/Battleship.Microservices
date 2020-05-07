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
	[Message] [text] NOT NULL,
	[AuditTypeId] [int] NOT NULL,
	[Username] [varchar](50) NOT NULL,
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

USE [Battleship.Auditing]
GO
SET IDENTITY_INSERT [dbo].[AuditType] ON 
GO
INSERT [dbo].[AuditType] ([AuditTypeId], [Name]) VALUES (1, N'Error')
GO
INSERT [dbo].[AuditType] ([AuditTypeId], [Name]) VALUES (2, N'Log')
GO
INSERT [dbo].[AuditType] ([AuditTypeId], [Name]) VALUES (3, N'Message')
GO
SET IDENTITY_INSERT [dbo].[AuditType] OFF
GO
