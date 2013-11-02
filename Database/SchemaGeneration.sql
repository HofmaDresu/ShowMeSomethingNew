USE [master]
GO
/****** Object:  Database [ShowMeSomethingNew]    Script Date: 11/1/2013 8:08:58 PM ******/
CREATE DATABASE [ShowMeSomethingNew]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ShowMeSomethingNew', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS2012\MSSQL\DATA\ShowMeSomethingNew.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'ShowMeSomethingNew_log', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS2012\MSSQL\DATA\ShowMeSomethingNew_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [ShowMeSomethingNew] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ShowMeSomethingNew].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ShowMeSomethingNew] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ShowMeSomethingNew] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ShowMeSomethingNew] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ShowMeSomethingNew] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ShowMeSomethingNew] SET ARITHABORT OFF 
GO
ALTER DATABASE [ShowMeSomethingNew] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ShowMeSomethingNew] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [ShowMeSomethingNew] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ShowMeSomethingNew] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ShowMeSomethingNew] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ShowMeSomethingNew] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ShowMeSomethingNew] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ShowMeSomethingNew] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ShowMeSomethingNew] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ShowMeSomethingNew] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ShowMeSomethingNew] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ShowMeSomethingNew] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ShowMeSomethingNew] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ShowMeSomethingNew] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ShowMeSomethingNew] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ShowMeSomethingNew] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ShowMeSomethingNew] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ShowMeSomethingNew] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ShowMeSomethingNew] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [ShowMeSomethingNew] SET  MULTI_USER 
GO
ALTER DATABASE [ShowMeSomethingNew] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ShowMeSomethingNew] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ShowMeSomethingNew] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ShowMeSomethingNew] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [ShowMeSomethingNew]
GO
/****** Object:  Table [dbo].[TableOfContent]    Script Date: 11/1/2013 8:08:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TableOfContent](
	[SiteId] [bigint] IDENTITY(1,1) NOT NULL,
	[SiteName] [varchar](max) NOT NULL,
	[URL] [varchar](800) NOT NULL,
	[TopSiteId] [bigint] NULL,
 CONSTRAINT [PK_TableOfContent] PRIMARY KEY CLUSTERED 
(
	[SiteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [AK_URL] UNIQUE NONCLUSTERED 
(
	[URL] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[User]    Script Date: 11/1/2013 8:08:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[User](
	[UserId] [bigint] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [AK_UserName] UNIQUE NONCLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserVisits]    Script Date: 11/1/2013 8:08:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserVisits](
	[UserId] [bigint] NOT NULL,
	[SiteId] [bigint] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  View [dbo].[UserProgress]    Script Date: 11/1/2013 8:08:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE View [dbo].[UserProgress]
as


with tocCount as (select toc.TopSiteId, count(1) SiteCount
		from TableOfContent toc
		group by toc.TopSiteId),
	userProgress as (select u.UserName, u.UserId, toc.TopSiteId, count(1) progressCount
		from [User] u
		inner join UserVisits up on u.UserId = up.UserId
		inner join TableOfContent toc on toc.SiteId = up.SiteId
		group by toc.TopSiteId, u.UserId, u.UserName
		),
	userSiteProgress as (
		select toc.SiteId, toc.SiteName, up.UserName, convert(decimal(28, 2), ISNULL((convert(decimal(28, 2), up.progressCount) / convert(decimal(28, 2), tc.SiteCount)) * 100, 0)) UserPercentProgress
		from tocCount tc
		inner join TableOfContent toc on tc.TopSiteId = toc.SiteId
		left outer join userProgress up on tc.TopSiteId = up.TopSiteId
		),
	userUnstartedSites as (
		select x.SiteId, toc.SiteName, x.UserName, 0 as UserPercentProgress
		from (
			select toc.SiteId, u.UserName
			from TableOfContent toc, [User] u
			where toc.TopSiteId is NULL
			except
			select SiteId, UserName
			from userSiteProgress
			) as x
		inner join TableOfContent toc on x.SiteId = toc.SiteId
	)

	select * 
	from userSiteProgress usp
	union
	select * 
	from userUnstartedSites
 

GO
ALTER TABLE [dbo].[TableOfContent]  WITH CHECK ADD  CONSTRAINT [FK_TableOfContent_TableOfContent] FOREIGN KEY([TopSiteId])
REFERENCES [dbo].[TableOfContent] ([SiteId])
GO
ALTER TABLE [dbo].[TableOfContent] CHECK CONSTRAINT [FK_TableOfContent_TableOfContent]
GO
USE [master]
GO
ALTER DATABASE [ShowMeSomethingNew] SET  READ_WRITE 
GO
