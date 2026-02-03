# Online Voting System (ONLINEVS_Project)

A complete **web-based Online Voting System** developed using **ASP.NET Core MVC**, **Entity Framework Core**, and **SQL Server**.  
This project enables secure voter registration, admin-managed elections and candidates, real-time vote casting, and detailed election results.

## Features

### Admin Panel
- Secure login using CNIC, Email, and Password
- Create, Edit, Delete Elections (with Title, Description, Start/End Dates)
- Add, Edit, Delete Candidates (with CNIC, Name, Party)
- Dashboard with real-time stats:
  - Total Voters
  - Total Elections
  - Total Candidates
  - Total Votes Cast
- View all voters, votes, and grouped results (party-wise)

### Voter Module
- Registration with validation (CNIC unique, Email unique, Age ≥ 18)
- Login and personalized dashboard
- View active/ongoing elections
- Cast vote (single vote per election, double-voting prevention)
- See election results after end date

### Voting & Results
- Timestamped vote recording
- Results grouped by Party with total votes and candidate breakdown
- Winner/Loser/ongoing status badges
- (Optional) PDF export for results

### Security & Technical Features
- Session-based authentication (HttpContext.Session)
- Data validation using DataAnnotations (Required, StringLength, EmailAddress, Range)
- Asynchronous database operations (async/await with EF Core)
- Responsive UI using Bootstrap
- Entity Framework Code-First approach with migrations

## Tech Stack

- **Framework**: ASP.NET Core MVC (.NET 6/7/8)
- **ORM/Database**: Entity Framework Core + SQL Server
- **Frontend**: Razor Views + Bootstrap 5
- **Authentication**: Session-based (no ASP.NET Identity in this version)
- **Other**: SMTP for email confirmation (configurable in code)

## Project Folder Structure

ONLINEVS_Project/
├── Controllers/                # All business logic (Admin, Voter, Election, etc.)
├── Models/                     # Data entities (Admin, Voter, Election, Candidate, Voting)
├── Views/                      # Razor UI pages (Login, Dashboard, Create, Results)
│   ├── Admin/
│   ├── Voter/
│   ├── Election/
│   ├── Voting/
│   └── Shared/
├── wwwroot/                    # Static files (css, js, images)
├── Migrations/                 # EF Core database migrations
├── Program.cs                  # App startup & configuration
├── ONLINEVS_Project.csproj     # Project file
├── ONLINEVS_Project.sln        # Solution file
├── appsettings.json            # Configuration (DB connection string)
└── README.md                   # This file

## About the Developer

- **Name**: Sahil  
- **Location**: Karachi, Pakistan  
- **GitHub**: [@sahilkumar-05](https://github.com/sahilkumar-05)
- **Purpose**: University project / Final year viva demo  
- **Contact**: www.linkedin.com/in/sahil-kumar-39b684305

Built with ❤️ using ASP.NET Core  
Last updated: February 2026
```
