USE [db-nmas-tests];
GO

/* Create Tables */
CREATE TABLE [dbo].[Worker](
    [ID] INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    [FirstName] VARCHAR(100) NOT NULL,
    [LastName] VARCHAR(100) NOT NULL,
    [PersonalIdentityCode] VARCHAR(100),
    [ContactEmail] VARCHAR(100),
    [ContactPhone] VARCHAR(100)
);
GO

CREATE TABLE [dbo].[AccommodationPlace](
    [ID] INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    [WorkerID] INT NOT NULL,
    [PlaceName] VARCHAR(100) NOT NULL,
    [Adress] VARCHAR(100) NOT NULL,
    [AccommodationCapacity] INT,
    [UsedAccommodationCapacity] INT,
    [CompanyCode] VARCHAR(100),
    [ContactPhone] VARCHAR(100),
    CONSTRAINT fk_Worker FOREIGN KEY (WorkerID) REFERENCES [dbo].[Worker](ID)
);
GO

CREATE TABLE [dbo].[IllegalMigrant](
    [ID] INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    [AccommodationPlaceID] INT,
    [PersonalIdentityCode] VARCHAR(100),
    [FirstName] VARCHAR(100) NOT NULL,
    [MiddleName] VARCHAR(100),
    [LastName] VARCHAR(100) NOT NULL,
    [Gender] VARCHAR(100) NOT NULL,
    [DateOfBirth] DATE,
    [OriginCountry] VARCHAR(100) NOT NULL,
    [Religion] VARCHAR(100),
    CONSTRAINT fk_AccomodationPlace FOREIGN KEY (AccommodationPlaceID) REFERENCES [dbo].[AccommodationPlace](ID)
);
GO