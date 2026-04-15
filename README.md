# MainsAuction

> Blazor Server web application for property auctions : browse listings, place bids, manage properties, and chat with an AI assistant

**CST2550 Group Coursework Middlesex University**

[📄 View Report](docs/Report/Mains_Report.pdf)
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

The app will be available at the URL shown in the terminal (normally http://localhost:5212/)

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



## Data Structures

All data structures are custom implementations — no third-party or standard library collections are used.

| Structure | Used For |
|---|---|
| Binary Search Tree | Storing and searching auction listings |
| Hash Table (separate chaining) | User lookup and authentication |
| Linked List | Bid history per auction |

---


## Screenshots

| | |
|---|---|
| ![Home](media/screenshots/websitescreenshots/Home.png) | ![Listings](media/screenshots/websitescreenshots/listingsinhome.png) |
| *Home page* | *Property listings* |
| ![Listing page](media/screenshots/websitescreenshots/listingpage.png) | ![Place bid](media/screenshots/websitescreenshots/placebid.png) |
| *Auction detail page* | *Place a bid* |
| ![Register](media/screenshots/websitescreenshots/Register.png) | ![Verify email](media/screenshots/websitescreenshots/verifyemail.png) |
| *Register* | *Email verification* |
| ![Login](media/screenshots/websitescreenshots/Login.png) | ![Submit property](media/screenshots/websitescreenshots/submitproperty.png) |
| *Login* | *Submit a property* |
| ![Admin access](media/screenshots/websitescreenshots/Adminaccess.png) | ![Admin listings](media/screenshots/websitescreenshots/adminpanellistings.png) |
| *Admin login* | *Admin panel : listings* |
| ![Admin approved](media/screenshots/websitescreenshots/adminpanelafterlisingisapproved.png) | ![Admin enquiries](media/screenshots/websitescreenshots/adminpanelenquiries.png) |
| *Admin panel — approved listing* | *Admin panel : contact enquiries* |
| ![Contact](media/screenshots/websitescreenshots/ContactUs.png) | ![AI Agent](media/screenshots/websitescreenshots/AIagentcomponent.png) |
| *Contact us* | *AI chat assistant* |
| ![About](media/screenshots/websitescreenshots/Aboutus.png) | ![Terms](media/screenshots/websitescreenshots/T%26C.png) |
| *About us* | *Terms & Conditions* |

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

### Initial Design Sketches

| | | | |
|---|---|---|---|
| ![Notes 1](media/screenshots/notes/leeinitialnotespage1.jpg) | ![Notes 2](media/screenshots/notes/leeinitialnotespage2.jpg) | ![Notes 3](media/screenshots/notes/leeinitialnotespage3.jpg) | ![Notes 4](media/screenshots/notes/leeinitialnotespage4.jpg) |
| ![Notes 5](media/screenshots/notes/leeinitialnotespage5.jpg) | ![Notes 6](media/screenshots/notes/leeinitialnotespage6.jpg) | ![Notes 7](media/screenshots/notes/leeinitialnotespage7.jpg) | ![Notes 8](media/screenshots/notes/leeinitialnotespage8.jpg) |
| ![Notes 9](media/screenshots/notes/leeinitialnotespage9.jpg) | ![Notes 10](media/screenshots/notes/leeinitialnotespage10.jpg) | ![Notes 11](media/screenshots/notes/leeinitialnotespage11.jpg) | |

| | |
|---|---|
| ![Lee's sketch](media/screenshots/notes/leeinitialnotespage11.jpg) | ![Gaia's sketch](media/screenshots/notes/gaiawebsitesketch.jpeg) |
| *Lee's initial website sketch* | *Gaia's initial website sketch* |


## Notes for Markers

- The AI proxy server is already deployed and live. no local setup needed
- All custom data structures are in `src/PropertyAuction.DataStructures/`
- Unit tests are in `tests/PropertyAuction.Tests.Unit/`
- The report is in `docs/Report/`
- The video demonstration is in `media/demo/`
---


## Authors

- [@Gaia](https://www.github.com/gaiafiorillo) -Developer
- [@Lee](https://github.com/devlene) - Tester
- [@Elysia](https://github.com/OrchidRural) - Secretary
- [@Sell](https://github.com/MadameNana) - SCRUM 
- [@Khalid](https://github.com/khxlrd20) - Developer
