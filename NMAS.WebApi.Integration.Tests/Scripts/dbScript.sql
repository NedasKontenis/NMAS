USE [db-nmas-tests];
GO

/* Create Tables */
CREATE TABLE [dbo].[Worker](
    [Id] INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    [FirstName] VARCHAR(100) NOT NULL,
    [LastName] VARCHAR(100) NOT NULL,
    [PersonalIdentityCode] VARCHAR(100),
    [ContactEmail] VARCHAR(100),
    [ContactPhone] VARCHAR(100)
);
GO

CREATE TABLE [dbo].[AccommodationPlace](
    [Id] INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    [WorkerId] INT NOT NULL,
    [PlaceName] VARCHAR(100) NOT NULL,
    [Adress] VARCHAR(100) NOT NULL,
    [AccommodationCapacity] INT,
    [UsedAccommodationCapacity] INT,
    [CompanyCode] VARCHAR(100),
    [ContactPhone] VARCHAR(100),
    CONSTRAINT fk_Worker FOREIGN KEY (WorkerId) REFERENCES [dbo].[Worker](Id)
);
GO

CREATE TABLE [dbo].[IllegalMigrant](
    [Id] INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    [AccommodationPlaceId] INT,
    [PersonalIdentityCode] VARCHAR(100),
    [FirstName] VARCHAR(100) NOT NULL,
    [MiddleName] VARCHAR(100),
    [LastName] VARCHAR(100) NOT NULL,
    [Gender] VARCHAR(100) NOT NULL,
    [DateOfBirth] DATE,
    [OriginCountry] VARCHAR(100) NOT NULL,
    [Religion] VARCHAR(100),
    CONSTRAINT fk_AccomodationPlace FOREIGN KEY (AccommodationPlaceId) REFERENCES [dbo].[AccommodationPlace](Id)
);
GO