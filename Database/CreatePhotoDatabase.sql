-- Create a new database if not exists
IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'photo_app')
    CREATE DATABASE photo_app;

-- Use the newly created database
USE photo_app;

-- Create a table to store user accounts (if not already created)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[users]') AND type in (N'U'))
CREATE TABLE [dbo].[users] (
    [user_id] INT IDENTITY(1,1) PRIMARY KEY,
    [username] VARCHAR(50) NOT NULL,
    [password] VARCHAR(255) NOT NULL
);

-- Create a table to store photos
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[photos]') AND type in (N'U'))
CREATE TABLE [dbo].[photos] (
    [photo_id] INT IDENTITY(1,1) PRIMARY KEY,
    [user_id] INT NOT NULL,
    [photo_name] VARCHAR(100) NOT NULL,
    [photo_path] VARCHAR(255) NOT NULL,
    -- Add a foreign key constraint to ensure each photo is associated with a valid user
    CONSTRAINT [fk_user_id]
        FOREIGN KEY ([user_id])
        REFERENCES [dbo].[users]([user_id])
        ON DELETE CASCADE
        ON UPDATE CASCADE
);

-- Create a unique index on photo_name to ensure each photo name is unique per user
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name='unique_photo_name_per_user' AND object_id = OBJECT_ID(N'[dbo].[photos]'))
CREATE UNIQUE INDEX [unique_photo_name_per_user] ON [dbo].[photos] ([user_id], [photo_name]);

-- Create a unique index on photo_path to ensure each photo path is unique
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name='unique_photo_path' AND object_id = OBJECT_ID(N'[dbo].[photos]'))
CREATE UNIQUE INDEX [unique_photo_path] ON [dbo].[photos] ([photo_path]);
