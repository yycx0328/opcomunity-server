USE [Recruit]
GO
/****** Object:  Table [dbo].[Auth_User]    Script Date: 04/21/2017 20:22:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Auth_User](
	[UserId] [varchar](32) NOT NULL,
	[UserName] [varchar](32) NOT NULL,
	[Password] [varchar](32) NOT NULL,
	[PhoneNo] [varchar](15) NULL,
	[Email] [varchar](42) NULL,
	[Gender] [char](1) NULL,
	[IsAvaiable] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Auth_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Auth_Role]    Script Date: 04/21/2017 20:22:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Auth_Role](
	[RoleId] [varchar](32) NOT NULL,
	[RoleName] [varchar](32) NOT NULL,
	[Description] [varchar](64) NOT NULL,
	[RoleType] [int] NOT NULL,
	[IsAvaiable] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Auth_Role] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Auth_Permission]    Script Date: 04/21/2017 20:22:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Auth_Permission](
	[PermissionId] [varchar](32) NOT NULL,
	[ParentId] [varchar](32) NULL,
	[PermissionNo] [varchar](32) NOT NULL,
	[PermissionName] [varchar](64) NOT NULL,
	[PermissionType] [int] NOT NULL,
	[ResourceUrl] [varchar](256) NULL,
	[Target] [varchar](20) NOT NULL,
	[SortIndex] [int] NOT NULL,
	[IsVisible] [bit] NOT NULL,
	[IsAvaiable] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UPdateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Auth_Permission] PRIMARY KEY CLUSTERED 
(
	[PermissionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[User]    Script Date: 04/21/2017 20:22:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[User](
	[Id] [bigint] IDENTITY(100000,1) NOT NULL,
	[Name] [varchar](20) NOT NULL,
	[BirthDay] [datetime] NULL,
	[Gender] [char](1) NULL,
	[Address] [nvarchar](256) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User', @level2type=N'COLUMN',@level2name=N'Name'
GO
/****** Object:  Table [dbo].[Company]    Script Date: 04/21/2017 20:22:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Company](
	[Id] [bigint] IDENTITY(1000,1) NOT NULL,
	[Name] [nvarchar](256) NULL,
 CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'公司Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'Id'
GO
/****** Object:  Table [dbo].[UserThirdAccount]    Script Date: 04/21/2017 20:22:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserThirdAccount](
	[Id] [bigint] IDENTITY(1000,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[OpenId] [varchar](64) NOT NULL,
	[Platform] [varchar](32) NOT NULL,
	[RegistTime] [datetime] NOT NULL,
 CONSTRAINT [PK_UserThirdAccount] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserAccount]    Script Date: 04/21/2017 20:22:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserAccount](
	[Id] [bigint] IDENTITY(1000,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[LoginName] [varchar](32) NOT NULL,
	[LoginPwd] [varchar](32) NOT NULL,
	[RegistTime] [datetime] NOT NULL,
 CONSTRAINT [PK_UserAccount] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserAccount', @level2type=N'COLUMN',@level2name=N'UserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登录名（邮箱、手机、个性账号...）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserAccount', @level2type=N'COLUMN',@level2name=N'LoginName'
GO
/****** Object:  Table [dbo].[Auth_UserRole]    Script Date: 04/21/2017 20:22:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Auth_UserRole](
	[UserId] [varchar](32) NOT NULL,
	[RoleId] [varchar](32) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Auth_Access]    Script Date: 04/21/2017 20:22:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Auth_Access](
	[RoleId] [varchar](32) NOT NULL,
	[PermissionId] [varchar](32) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  ForeignKey [FK_Auth_Access_Auth_Permission]    Script Date: 04/21/2017 20:22:04 ******/
ALTER TABLE [dbo].[Auth_Access]  WITH CHECK ADD  CONSTRAINT [FK_Auth_Access_Auth_Permission] FOREIGN KEY([PermissionId])
REFERENCES [dbo].[Auth_Permission] ([PermissionId])
GO
ALTER TABLE [dbo].[Auth_Access] CHECK CONSTRAINT [FK_Auth_Access_Auth_Permission]
GO
/****** Object:  ForeignKey [FK_Auth_Access_Auth_Role]    Script Date: 04/21/2017 20:22:04 ******/
ALTER TABLE [dbo].[Auth_Access]  WITH CHECK ADD  CONSTRAINT [FK_Auth_Access_Auth_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Auth_Role] ([RoleId])
GO
ALTER TABLE [dbo].[Auth_Access] CHECK CONSTRAINT [FK_Auth_Access_Auth_Role]
GO
/****** Object:  ForeignKey [FK_Auth_UserRole_Auth_Role]    Script Date: 04/21/2017 20:22:04 ******/
ALTER TABLE [dbo].[Auth_UserRole]  WITH CHECK ADD  CONSTRAINT [FK_Auth_UserRole_Auth_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Auth_Role] ([RoleId])
GO
ALTER TABLE [dbo].[Auth_UserRole] CHECK CONSTRAINT [FK_Auth_UserRole_Auth_Role]
GO
/****** Object:  ForeignKey [FK_Auth_UserRole_Auth_User]    Script Date: 04/21/2017 20:22:04 ******/
ALTER TABLE [dbo].[Auth_UserRole]  WITH CHECK ADD  CONSTRAINT [FK_Auth_UserRole_Auth_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[Auth_User] ([UserId])
GO
ALTER TABLE [dbo].[Auth_UserRole] CHECK CONSTRAINT [FK_Auth_UserRole_Auth_User]
GO
/****** Object:  ForeignKey [FK_UserAccount_User]    Script Date: 04/21/2017 20:22:04 ******/
ALTER TABLE [dbo].[UserAccount]  WITH CHECK ADD  CONSTRAINT [FK_UserAccount_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[UserAccount] CHECK CONSTRAINT [FK_UserAccount_User]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'UserId引用User表中的Id作为外键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserAccount', @level2type=N'CONSTRAINT',@level2name=N'FK_UserAccount_User'
GO
/****** Object:  ForeignKey [FK_UserThirdAccount_User]    Script Date: 04/21/2017 20:22:04 ******/
ALTER TABLE [dbo].[UserThirdAccount]  WITH CHECK ADD  CONSTRAINT [FK_UserThirdAccount_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[UserThirdAccount] CHECK CONSTRAINT [FK_UserThirdAccount_User]
GO
