USE [HotelBookingDb];
GO

-- Create the database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'HotelBookingDb')
BEGIN
    CREATE DATABASE [HotelBookingDb];
END
GO

USE [HotelBookingDb];
GO

-- 1. Create Users Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Users](
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [Username] NVARCHAR(100) NOT NULL,
        [PasswordHash] NVARCHAR(MAX) NULL, -- Nullable for social logins
        [Role] NVARCHAR(50) NOT NULL, -- 'Owner' or 'Customer'
        [Provider] NVARCHAR(50) NOT NULL, -- 'Local', 'MockGoogle', 'MockTwitter'
        [ProviderId] NVARCHAR(255) NULL -- To store social login ID
    );
END
GO

-- 2. Create Rooms Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rooms]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Rooms](
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [RoomNo] NVARCHAR(50) NOT NULL UNIQUE,
        [RoomType] NVARCHAR(50) NOT NULL, -- 'Single', 'Double'
        [IsAC] BIT NOT NULL DEFAULT 0, -- AC or Non-AC
        [Price] DECIMAL(18,2) NOT NULL,
        [IsActive] BIT NOT NULL DEFAULT 1
    );
END
GO

-- 3. Create Bookings Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Bookings]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Bookings](
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [RoomId] INT NOT NULL,
        [UserId] INT NOT NULL,
        [FromDate] DATE NOT NULL,
        [ToDate] DATE NOT NULL,
        [Status] NVARCHAR(50) NOT NULL, -- 'Reserved', 'Booked', 'Completed'
        CONSTRAINT [FK_Bookings_Rooms] FOREIGN KEY ([RoomId]) REFERENCES [dbo].[Rooms] ([Id]),
        CONSTRAINT [FK_Bookings_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
    );
END
GO

-- 4. Create Stored Procedure for Getting Rooms Status by Date
CREATE OR ALTER PROCEDURE [dbo].[sp_GetRoomsStatusByDate]
    @SelectedDate DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        r.Id,
        r.RoomNo,
        r.RoomType,
        r.IsAC,
        r.Price,
        r.IsActive,
        CASE 
            WHEN b.Id IS NOT NULL THEN 'Booked'
            ELSE 'Not Booked'
        END AS [Status]
    FROM [dbo].[Rooms] r
    LEFT JOIN [dbo].[Bookings] b 
        ON r.Id = b.RoomId 
        AND b.Status IN ('Reserved', 'Booked') 
        AND @SelectedDate BETWEEN b.FromDate AND b.ToDate
    WHERE r.IsActive = 1
    ORDER BY r.RoomNo;
END
GO

-- 5. Create Stored Procedure for Getting User Bookings
CREATE OR ALTER PROCEDURE [dbo].[sp_GetUserBookings]
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        b.Id AS BookingId,
        r.RoomNo,
        r.RoomType,
        b.FromDate,
        b.ToDate,
        b.Status
    FROM [dbo].[Bookings] b
    INNER JOIN [dbo].[Rooms] r ON b.RoomId = r.Id
    WHERE b.UserId = @UserId
    ORDER BY b.FromDate DESC;
END
GO

-- 6. Insert Default Hotel Owner
IF NOT EXISTS (SELECT * FROM [dbo].[Users] WHERE [Username] = 'owner' AND [Role] = 'Owner')
BEGIN
    -- Bcrypt hash for password 'admin'
    INSERT INTO [dbo].[Users] ([Username], [PasswordHash], [Role], [Provider], [ProviderId])
    VALUES ('nithish', '$2a$11$Y7P14j3PqU.h/Vp.JqDk1OOO.p1y5l65d.BpsR9e/1v1Uv2YJ9hOS', 'Owner', 'Local', NULL);
END
GO


ALTER TABLE [dbo].[Rooms] 
ADD [RoomStatus] NVARCHAR(50) NOT NULL DEFAULT 'Active';
GO
