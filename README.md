# MainsAuction

> Blazor Server web application for property auctions : browse listings, place bids, manage properties, and chat with an AI assistant

**CST2550 Group Coursework Middlesex University**

---

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- SQL Server or SQL Server Express

---

## Running the Application

### Clone the repository

```bash
git clone https://github.com/gaiafiorillo/PropertyAuction.git
```


### Run the application

```bash
dotnet run --project "PropertyAuction/"
```

The app will be available at the URL shown in the terminal

---

## AI Assistant

The built in AI chat assistant is powered by GPT-4o-mini routed through a proxy server deployed on Railway , so  **no API key is required**. The proxy URL is preconfigured in `appsettings.json` under `OpenAI:ProxyUrl` and works out of the box.

---

## Running the Tests

```bash
dotnet test
```

---

## Repository Structure

```
PropertyAuction/                        # Main Blazor server application
│   Components/
│   │   ├── Pages/                      # Razor pages (Home, Listings, Admin, etc.)
│   │   ├── Layout/                     # Navigation and layout components
│   │   └── Shared/                     # Shared components (BidModal, ChatAgent)
│   ├── wwwroot/                        # Static assets
│   ├── Program.cs                      # App entry point and DI configuration
│   └── appsettings.json                # Configuration (connection string, proxy URL)
│
PropertyAuction.Proxy/                  # AI proxy server (deployed to Railway)
│
src/
│   ├── PropertyAuction.Core/           # Models, interfaces, enums
│   │   ├── Models/                     # Auction, Bid, User
│   │   ├── Interfaces/                 # IUserRepository, IUserService, IEmailService
│   │   └── Enums/                      # UserRole
│   ├── PropertyAuction.Data/           # Database context and repositories
│   ├── PropertyAuction.DataStructures/ # Custom data structures
│   │   ├── BST/                        # AuctionBST
│   │   └── HashTable/                  # UserHashTable, UserRepository
│   └── PropertyAuction.Services/       # Business logic
│       └── Services/                   # AdminService, AuctionService, BidService, UserService
│
tests/
│   └── PropertyAuction.Tests.Unit/
│       ├── DataStructures/             # AuctionBSTTests, UserHashTableTests
│       ├── Models/                     # UserModelTests
│       ├── Fakes/                      # FakeUserRepository
│       └── Services/                   # AuctionServiceTests, BidTests, UserServiceTests
│
SQL_DB/                                 # SQL scripts
│   └── PropertyDB.sql                  # Database schema and seed data
│
docs/
│   ├── Report/                         # PDF report
│   └── DailyStandups/                  # daily standup logs
│
media/
    └── demo/                           # video demonstration
```

---

## Features

- **Property listings** : browse active auctions with filtering by price, location, and property type
- **Bidding** : place bids with a live countdown timer per auction
- **Admin panel** : approve or reject property submissions and manage users
- **Property submission** : registered users can submit properties for auction
- **AI chat assistant** : ask questions about properties and the auction process
- **User authentication** : register, log in, and manage your account

## Pages

| Route | Description |
|---|---|
| `/` | Home. browse active auction listings |
| `/login` | Log in to your account |
| `/register` | Create a new account |
| `/submit` | Submit a property for auction |
| `/auction/{id}` | Auction detail and bidding page |
| `/admin` | Admin panel : approve listings sent from/submit|
---

## Data Structures

All data structures are custom implementations — no third-party or standard library collections are used.

| Structure | Used For |
|---|---|
| Binary Search Tree | Storing and searching auction listings |
| Hash Table (separate chaining) | User lookup and authentication |
| Linked List | Bid history per auction |

---

## Notes for Markers

- The AI proxy server is already deployed and live. no local setup needed
- All custom data structures are in `src/PropertyAuction.DataStructures/`
- Unit tests are in `tests/PropertyAuction.Tests.Unit/`
- The report is in `docs/Report/`
- The video demonstration is in `media/demo/`


## Authors

- [@Gaia](https://www.github.com/gaiafiorillo) -Developer
- [@Lee](https://github.com/devlene) - Tester
- [@Elysia](https://github.com/OrchidRural) - Secretary
- [@Sell](https://github.com/MadameNana) - SCRUM 
- [@Khalid](https://github.com/khxlrd20) - Developer
