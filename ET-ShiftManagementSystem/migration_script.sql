IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221213060530_initial Migration')
BEGIN
    CREATE TABLE [projects] (
        [ProjectId] int NOT NULL IDENTITY,
        [ProjectName] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [ClientName] nvarchar(max) NOT NULL,
        [CreatedBy] nvarchar(max) NOT NULL,
        [CreatedDate] datetime2 NOT NULL,
        [ModifieBy] nvarchar(max) NOT NULL,
        [ModifieDate] datetime2 NOT NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_projects] PRIMARY KEY ([ProjectId])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221213060530_initial Migration')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221213060530_initial Migration', N'7.0.1');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221225172318_Added Login')
BEGIN
    CREATE TABLE [Comments] (
        [CommentID] int NOT NULL IDENTITY,
        [CommentText] nvarchar(max) NOT NULL,
        [ShiftID] int NOT NULL,
        [UserID] int NOT NULL,
        [Shared] bit NOT NULL,
        [CreatedDate] datetime2 NOT NULL,
        CONSTRAINT [PK_Comments] PRIMARY KEY ([CommentID])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221225172318_Added Login')
BEGIN
    CREATE TABLE [projectDetails] (
        [ProjectDetailsID] int NOT NULL IDENTITY,
        [ProjectId] int NOT NULL,
        [UserID] int NOT NULL,
        [CreatedBy] nvarchar(max) NOT NULL,
        [CreatedDate] datetime2 NOT NULL,
        [ModifiedBy] nvarchar(max) NOT NULL,
        [ModifiedDate] datetime2 NOT NULL,
        [IsActive] bit NOT NULL,
        [ShiftID] int NULL,
        CONSTRAINT [PK_projectDetails] PRIMARY KEY ([ProjectDetailsID])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221225172318_Added Login')
BEGIN
    CREATE TABLE [roles] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_roles] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221225172318_Added Login')
BEGIN
    CREATE TABLE [users] (
        [id] uniqueidentifier NOT NULL,
        [username] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [password] nvarchar(max) NOT NULL,
        [FirstName] nvarchar(max) NOT NULL,
        [LastName] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_users] PRIMARY KEY ([id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221225172318_Added Login')
BEGIN
    CREATE TABLE [Shifts] (
        [ShiftId] int NOT NULL,
        [ShiftName] nvarchar(max) NOT NULL,
        [StartTime] datetime2 NOT NULL,
        [EndTime] datetime2 NOT NULL,
        CONSTRAINT [PK_Shifts] PRIMARY KEY ([ShiftId]),
        CONSTRAINT [FK_Shifts_Comments_ShiftId] FOREIGN KEY ([ShiftId]) REFERENCES [Comments] ([CommentID]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221225172318_Added Login')
BEGIN
    CREATE TABLE [usersRoles] (
        [Id] uniqueidentifier NOT NULL,
        [Userid] uniqueidentifier NOT NULL,
        [RoleId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_usersRoles] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_usersRoles_roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [roles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_usersRoles_users_Userid] FOREIGN KEY ([Userid]) REFERENCES [users] ([id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221225172318_Added Login')
BEGIN
    CREATE INDEX [IX_usersRoles_RoleId] ON [usersRoles] ([RoleId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221225172318_Added Login')
BEGIN
    CREATE INDEX [IX_usersRoles_Userid] ON [usersRoles] ([Userid]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221225172318_Added Login')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221225172318_Added Login', N'7.0.1');
END;
GO

COMMIT;
GO

