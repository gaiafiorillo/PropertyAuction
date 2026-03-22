CREATE DATABASE WebDB;
GO
USE WebDB;
GO

CREATE TABLE Users (
    UserID           INT            PRIMARY KEY IDENTITY(1,1),
    Username         VARCHAR(50)    NOT NULL UNIQUE,
    FullName         VARCHAR(100)   NOT NULL,
    Email            VARCHAR(100)   NOT NULL UNIQUE,
    Phone            VARCHAR(20)    NULL,
    UserRole         VARCHAR(20)    NOT NULL DEFAULT 'Bidder',
    RegistrationDate DATETIME       NOT NULL DEFAULT GETDATE(),
    CONSTRAINT chk_role CHECK (UserRole IN ('Bidder', 'Admin'))
);

CREATE TABLE Auctions (
    AuctionID     INT            PRIMARY KEY IDENTITY(1,1),
    Address       VARCHAR(200)   NOT NULL,
    City          VARCHAR(100)   NOT NULL,
    PostCode      VARCHAR(10)    NOT NULL,
    PropertyType  VARCHAR(50)    NOT NULL,
    Bedrooms      INT            NOT NULL DEFAULT 0,
    SquareFootage DECIMAL(10,2)  NULL,
    GuidePrice    DECIMAL(12,2)  NOT NULL,
    AuctionDate   DATE           NOT NULL,
    Status        VARCHAR(20)    NOT NULL DEFAULT 'Active',
    Description   VARCHAR(500)   NULL,
    CONSTRAINT chk_price  CHECK (GuidePrice > 0),
    CONSTRAINT chk_beds   CHECK (Bedrooms >= 0),
    CONSTRAINT chk_status CHECK (Status IN ('Active', 'Sold', 'Withdrawn'))
);

CREATE TABLE Bids (
    BidID      INT            PRIMARY KEY IDENTITY(1,1),
    AuctionID  INT            NOT NULL,
    UserID     INT            NOT NULL,
    BidAmount  DECIMAL(12,2)  NOT NULL,
    BidTime    DATETIME       NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Bids_Auction FOREIGN KEY (AuctionID) REFERENCES Auctions(AuctionID),
    CONSTRAINT FK_Bids_User    FOREIGN KEY (UserID)    REFERENCES Users(UserID),
    CONSTRAINT chk_bid CHECK (BidAmount > 0)
);

CREATE NONCLUSTERED INDEX idx_username  ON Users (Username);
CREATE NONCLUSTERED INDEX idx_price     ON Auctions (GuidePrice);
CREATE NONCLUSTERED INDEX idx_bid_auc   ON Bids (AuctionID);
GO
