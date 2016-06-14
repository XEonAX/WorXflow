
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 06/13/2016 21:17:44
-- Generated from EDMX file: J:\Root\DOT.NET\WorXflow\WorXflow.Data\DataModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [WorXflow];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_WorkTaskAssignedToUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WorkTasks] DROP CONSTRAINT [FK_WorkTaskAssignedToUser];
GO
IF OBJECT_ID(N'[dbo].[FK_WorkTaskAssignedByUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WorkTasks] DROP CONSTRAINT [FK_WorkTaskAssignedByUser];
GO
IF OBJECT_ID(N'[dbo].[FK_WorkTaskStatus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StatusMessages] DROP CONSTRAINT [FK_WorkTaskStatus];
GO
IF OBJECT_ID(N'[dbo].[FK_StatusUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StatusMessages] DROP CONSTRAINT [FK_StatusUser];
GO
IF OBJECT_ID(N'[dbo].[FK_WorkTaskFollowingUser_WorkTask]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WorkTaskFollowingUser] DROP CONSTRAINT [FK_WorkTaskFollowingUser_WorkTask];
GO
IF OBJECT_ID(N'[dbo].[FK_WorkTaskFollowingUser_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WorkTaskFollowingUser] DROP CONSTRAINT [FK_WorkTaskFollowingUser_User];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[WorkTasks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[WorkTasks];
GO
IF OBJECT_ID(N'[dbo].[StatusMessages]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StatusMessages];
GO
IF OBJECT_ID(N'[dbo].[WorkTaskFollowingUser]', 'U') IS NOT NULL
    DROP TABLE [dbo].[WorkTaskFollowingUser];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Login] nvarchar(max)  NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'WorkTasks'
CREATE TABLE [dbo].[WorkTasks] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [AssignedToUser_Id] int  NULL,
    [AssignedByUser_Id] int  NULL
);
GO

-- Creating table 'StatusMessages'
CREATE TABLE [dbo].[StatusMessages] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [WorkTaskId] int  NOT NULL,
    [UserId] int  NOT NULL,
    [Message] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'WebMessages'
CREATE TABLE [dbo].[WebMessages] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Text] nvarchar(max)  NOT NULL,
    [Type] int  NOT NULL
);
GO

-- Creating table 'WorkTaskFollowingUser'
CREATE TABLE [dbo].[WorkTaskFollowingUser] (
    [Following_Id] int  NOT NULL,
    [Followers_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'WorkTasks'
ALTER TABLE [dbo].[WorkTasks]
ADD CONSTRAINT [PK_WorkTasks]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'StatusMessages'
ALTER TABLE [dbo].[StatusMessages]
ADD CONSTRAINT [PK_StatusMessages]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'WebMessages'
ALTER TABLE [dbo].[WebMessages]
ADD CONSTRAINT [PK_WebMessages]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Following_Id], [Followers_Id] in table 'WorkTaskFollowingUser'
ALTER TABLE [dbo].[WorkTaskFollowingUser]
ADD CONSTRAINT [PK_WorkTaskFollowingUser]
    PRIMARY KEY CLUSTERED ([Following_Id], [Followers_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [AssignedToUser_Id] in table 'WorkTasks'
ALTER TABLE [dbo].[WorkTasks]
ADD CONSTRAINT [FK_WorkTaskAssignedToUser]
    FOREIGN KEY ([AssignedToUser_Id])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WorkTaskAssignedToUser'
CREATE INDEX [IX_FK_WorkTaskAssignedToUser]
ON [dbo].[WorkTasks]
    ([AssignedToUser_Id]);
GO

-- Creating foreign key on [AssignedByUser_Id] in table 'WorkTasks'
ALTER TABLE [dbo].[WorkTasks]
ADD CONSTRAINT [FK_WorkTaskAssignedByUser]
    FOREIGN KEY ([AssignedByUser_Id])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WorkTaskAssignedByUser'
CREATE INDEX [IX_FK_WorkTaskAssignedByUser]
ON [dbo].[WorkTasks]
    ([AssignedByUser_Id]);
GO

-- Creating foreign key on [WorkTaskId] in table 'StatusMessages'
ALTER TABLE [dbo].[StatusMessages]
ADD CONSTRAINT [FK_WorkTaskStatus]
    FOREIGN KEY ([WorkTaskId])
    REFERENCES [dbo].[WorkTasks]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WorkTaskStatus'
CREATE INDEX [IX_FK_WorkTaskStatus]
ON [dbo].[StatusMessages]
    ([WorkTaskId]);
GO

-- Creating foreign key on [UserId] in table 'StatusMessages'
ALTER TABLE [dbo].[StatusMessages]
ADD CONSTRAINT [FK_StatusUser]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StatusUser'
CREATE INDEX [IX_FK_StatusUser]
ON [dbo].[StatusMessages]
    ([UserId]);
GO

-- Creating foreign key on [Following_Id] in table 'WorkTaskFollowingUser'
ALTER TABLE [dbo].[WorkTaskFollowingUser]
ADD CONSTRAINT [FK_WorkTaskFollowingUser_WorkTask]
    FOREIGN KEY ([Following_Id])
    REFERENCES [dbo].[WorkTasks]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Followers_Id] in table 'WorkTaskFollowingUser'
ALTER TABLE [dbo].[WorkTaskFollowingUser]
ADD CONSTRAINT [FK_WorkTaskFollowingUser_User]
    FOREIGN KEY ([Followers_Id])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WorkTaskFollowingUser_User'
CREATE INDEX [IX_FK_WorkTaskFollowingUser_User]
ON [dbo].[WorkTaskFollowingUser]
    ([Followers_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------