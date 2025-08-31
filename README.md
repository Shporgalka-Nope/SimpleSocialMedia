# Connection Point - Microbloging platform
![ASP.NET Core](https://img.shields.io/badge/-ASP.NET_Core-512BD4?logo=dotnet) 
[![Main branch tests](https://github.com/Shporgalka-Nope/SimpleSocialMedia/actions/workflows/dotnet-test.yml/badge.svg)](https://github.com/Shporgalka-Nope/SimpleSocialMedia/actions/workflows/dotnet-test.yml)
[![Coverage Status](https://coveralls.io/repos/github/Shporgalka-Nope/SimpleSocialMedia/badge.svg)](https://coveralls.io/github/Shporgalka-Nope/SimpleSocialMedia)

[![GitHub Release](https://img.shields.io/github/v/release/Shporgalka-Nope/SimpleSocialMedia?sort=date&display_name=release&style=flat&label=Download%20latest%20release)](https://github.com/Shporgalka-Nope/SimpleSocialMedia/releases/download/Release/Release.build.zip)

Backend - ASP.Net Core (MVC), Identity Core, EF Core

Frontend - Razor view, HTML, CSS, JavaScript
## Current status: Planning migration onto a new model
In order to modernise the project, learn new tech and improve old knowledge, I've made a decision to move project from full ASP.Net Core to React + Web API model.
New tech stack will be as follows:
Backend: 
- ASP.Net Core Web API
- EF Core
- *Database is to be discussed*
Frontend:
- React
- Tailwind
- Eslint
I will also change my approach to development of new features and transition.
From now on, changes will be made in their own branches and merged into master after (self) code review and all tests passing.
Feature development also changes - backend should be developed in TDD'ish way, meaning writing tests before the actual code.
### Migration status
- [ ] Registration and login
- [ ] Profile customization (Bio, profile picture)
- [ ] Creating and deleting posts
- [ ] Settings (Profile visibility, changing profile picture and bio)
- [ ] Search for other users, taking their visibility settings into account
## Functionality
The following features are currently implemented:
  - Registration and login
  - Profile customization (Bio, profile picture)
  - Creating and deleting posts
  - Settings (Profile visibility, changing profile picture and bio)
  - Search for other users, taking their visibility settings into account

## How to Deploy?
  1. Download the latest stable version of the project.
  2. Extract the archive.
  3. From the project folder, run the following command in the console: ```dotnet ProfileProject.dll```
  4. Navigate to the address shown in the line that says ```info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5249``` (In this case, ```http://localhost:5249```).

## Known Issues
  1. Database connection error when starting the server ([Issue #2](https://github.com/Shporgalka-Nope/SimpleSocialMedia/issues/2)) - When attempting to start the server, an "error - 50" exception will be thrown. The issue has already been discussed in Issue #2. A brief solution is as follows:
     Run the commands `sqllocaldb stop mssqllocaldb`, `sqllocaldb delete mssqllocaldb`, and `sqllocaldb create mssqllocaldb` in the console and try again. The server should then start successfully.
