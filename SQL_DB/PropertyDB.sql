USE PropertyDB;
GO

CREATE SCHEMA User_Authentication_data;
GO

CREATE SCHEMA Property_Catalog;
GO

CREATE SCHEMA Sales;
GO

CREATE SCHEMA Support_Enquiries;
GO

--SELECT schema_name, schema_owner 
--FROM INFORMATION_SCHEMA.SCHEMATA
--WHERE schema_name IN (
  --  'User_Authentication_data',
  --  'Property_Catalog', 
  --  'Sales', 
  --  'Support_Enquiries'
--);


---------------------------------------------------------------------------------------------------------------------------------------
--USER-AUTHENTICATION-DATA--

CREATE TABLE User_Authentication_data.Roles (
RoleID INT PRIMARY KEY,
RoleName NVARCHAR(10)
);

INSERT INTO User_Authentication_data.Roles VALUES (1, 'Admin'), (2, 'Customer');

--Create secondary roles "Buyer"/ "Seller"/"Both"--

CREATE TABLE User_Authentication_data.Users (
UserID INT IDENTITY(1,1) PRIMARY KEY,
RoleID INT NOT NULL,
UserName NVARCHAR(30) NOT NULL UNIQUE,
Forename NVARCHAR(20) NOT NULL,
Surname NVARCHAR(20) NOT NULL,
Email NVARCHAR(100) NOT NULL UNIQUE,
PasswordHash VARBINARY(64)  NOT NULL,
PasswordSalt NVARCHAR(50)   NOT NULL,
Age INT NOT NULL,
PhoneNumber VARCHAR(20) NOT NULL

    CONSTRAINT FK_Users_Roles FOREIGN KEY (RoleID)
        REFERENCES User_Authentication_data.Roles(RoleID)

);

--DROP TABLE User_Authentication_data.Users;

--ADD USER TEST
---------------------------------------------------------------------------------------------------------------------------------------
--INSERT INTO User_Authentication_data.Users (RoleID, UserName, Forename, Surname, Email, PasswordHash, PasswordSalt, Age, PhoneNumber)
--VALUES
--(2, 'Tester1', 'Test', 'Part1', 'Tester@Email.c1', HASHBYTES('SHA2_256', 'TestPassword123'), 'randomsalt123', 20, 0700012345),
--(2, 'Tester2', 'Test', 'Part2', 'Tester2@Email.c1', HASHBYTES('SHA2_256', 'TestPassword456'), 'randomsalt456', 20, 07012345678),
--(2, 'Tester3', 'Test', 'Part3', 'Tester3@Email.c1', HASHBYTES('SHA2_256', 'TestPassword789'), 'randomsalt789', 20, 07009876543)
---------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE User_Authentication_data.Buyer_Documentation(
BuyerDocID  INT IDENTITY(1,1) PRIMARY KEY,
UserID INT NOT NULL
--ADD "VerificationStatus", "PaymentDetails" (CardName, CardNumber, ExpiryDate, CardCVC), "ValidIDType", "ValidIDCode", "UploadDate--
    CONSTRAINT FK_Users_ID FOREIGN KEY (UserID)
        REFERENCES User_Authentication_data.Users(UserID)
--TBC--
);

CREATE TABLE User_Authentication_data.Seller_Documentation(
SellerDocID  INT IDENTITY(1,1) PRIMARY KEY,
UserID INT NOT NULL
--ADD "VerificationStatus", "PaymentDetails" (CardName, CardNumber, ExpiryDate, CardCVC), "ValidIDType", "ValidIDCode", "UploadDate--
--FK ADD "PropertyID"
FOREIGN KEY (UserID)
        REFERENCES User_Authentication_data.Users(UserID)
--TBC--
);


---------------------------------------------------------------------------------------------------------------------------------------
--PROPERTY-CATALOGE--

CREATE TABLE Property_Catalog.Properties(
PropertyID INT IDENTITY(1,1) PRIMARY KEY,
PropertyTitle NVARCHAR(50) NOT NULL,
PropertyDescription NVARCHAR(400) NOT NULL,
PropertyPrice INT NOT NULL,
--TBC--
);

--DROP TABLE Property_Catalog.Properties;

CREATE TABLE Property_Catalog.Addresses(
AddressID INT IDENTITY(1,1) PRIMARY KEY,
PropertyID INT NOT NULL,
Country NVARCHAR(50) NOT NULL,
City NVARCHAR(100) NOT NULL,
StreetAddress NVARCHAR(100) NOT NULL,
PostalCode NVARCHAR(10) NOT NULL

    CONSTRAINT FK_Property_ID FOREIGN KEY (PropertyID)
        REFERENCES Property_Catalog.Properties(PropertyID)
--TBC--
);

--DROP TABLE Property_Catalog.Addresses;

CREATE TABLE Property_Catalog.Details(
DetailsID INT IDENTITY(1,1) PRIMARY KEY,
PropertyID INT NOT NULL,
RoomNumber INT NOT NULL,
BathroomNumber INT NOT NULL,
PropertySize NVARCHAR NOT NULL,
FloorAreaFS NVARCHAR NOT NULL,
PropertyCondition NVARCHAR NOT NULL,
DateUploaded DATETIME2 
--ADD "Features"

   FOREIGN KEY (PropertyID)
        REFERENCES Property_Catalog.Properties(PropertyID)
--TBC--
);

CREATE TABLE Property_Catalog.Images(
ImageID INT IDENTITY(1,1) PRIMARY KEY,
PropertyID INT NOT NULL,
--TBC--
);


CREATE TABLE Property_Catalog.Bid(
BiddingID INT IDENTITY(1,1) PRIMARY KEY,
PropertyID INT NOT NULL,
--ADD "BidStartTime", "BidEndTime", "BidStartPrice", "BidFinalPrice", "BidOutcome"--
--ADD FK "UserID"
   FOREIGN KEY (PropertyID)
        REFERENCES Property_Catalog.Properties(PropertyID)
--TBC--
);

---------------------------------------------------------------------------------------------------------------------------------------
--SALES--

CREATE TABLE Sales.Offers(
OfferID INT IDENTITY(1,1) PRIMARY KEY,
PropertyID INT NOT NULL,
--ADD "SaleStatus"
--ADD FK "UserID" ("BuyerID" + "SellerID"), "BidOutcome", "BidEndTime"
   FOREIGN KEY (PropertyID)
        REFERENCES Property_Catalog.Properties(PropertyID)
--TBC--
);

---------------------------------------------------------------------------------------------------------------------------------------
--SUPPORT-ENQUIRUIES--

CREATE TABLE Support_Enquiries.Customer_Service_Query(
QueryID INT IDENTITY(1,1) PRIMARY KEY,
UserID INT NOT NULL, --Both Customer and Admin--
QueryTitle NVARCHAR NOT NULL,
QueryStatus NVARCHAR NOT NULL,
DateSent DATETIME2
--FK ADD "PreferredContactInfo"
FOREIGN KEY (UserID)
        REFERENCES User_Authentication_data.Users(UserID)
--TBC--
);


--SELECT * FROM Property_Catalog.Details;








---------------------------------------------------------------------------------------------------------------------------------------
--TO-DO-LIST--

-----NF-----
--Team Discussion-> Other necessary Data and other possible Data to be added.
--Connection to Backend.
--Create Admin account .
--Create appropreate sample Data.
--Retrieve and sanitise user input.
--Create User sessions/ tokens.

-----PF-----
--Indexing frequently used data.
--Auto soft deleting unused data.
--Audits
--Slugs


