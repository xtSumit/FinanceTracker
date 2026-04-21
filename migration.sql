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
CREATE TABLE [Users] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [Email] nvarchar(150) NOT NULL,
    [PasswordHash] nvarchar(max) NOT NULL,
    [IsEmailVerified] bit NOT NULL,
    [Role] nvarchar(max) NOT NULL DEFAULT N'User',
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);

CREATE UNIQUE INDEX [IX_Users_Email] ON [Users] ([Email]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260414161352_InitialCreate', N'10.0.5');

COMMIT;
GO

