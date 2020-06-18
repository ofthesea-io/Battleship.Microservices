
# Battleship.Microservices
![dotnet core build](https://github.com/visualsanity/Battleship.Microservices/workflows/dotnet%20core%20build/badge.svg) ![angular build](https://github.com/visualsanity/Battleship.Microservices/workflows/angular%20build/badge.svg) ![docker build](https://github.com/visualsanity/Battleship.Microservices/workflows/docker%20build/badge.svg)

The following application is a port from a console kata application that I develop on periodically to hone my skills. This application is merely for technology exploration and coding practise disipline training. 

**DISCLAIMER:** This game is currently in pre-alpha and is under constant development and tinkering.

The following technology is used within the application:

![Angular](https://github.com/VisualSanity/Battleship.Microservices/blob/master/support/angular.png) 
![DotNet Core](https://github.com/VisualSanity/Battleship.Microservices/blob/master/support/dotnetcore.png) 
![Sql Server](https://github.com/VisualSanity/Battleship.Microservices/blob/master/support/sqlserver.png) 
![Docker](https://github.com/VisualSanity/Battleship.Microservices/blob/master/support/docker.png) 
![RabbitMQ](https://github.com/VisualSanity/Battleship.Microservices/blob/master/support/rabbitmq.png) 


## About the Battleship Game
The following Battleship Game uses the dimensions of a 10 X 10 grid.  The game is plotted on a X and Y axis. Instead of coding the X axis as chars, I decided to use the ASCII Table as it contains a numerical representation of characters, such as 'A' starting at point 65 and 'Z' ending in 90.  Both char and int consume one byte in memory (0 to 255) so the only overhead is the user input string manipulation. The reason for working in integers only, is to work in a geometrical graph quadrant manor. This would make working with Line equations, such as the linear Intersection equation easier (Unity):

[https://en.wikipedia.org/wiki/Line-line_intersection](https://en.wikipedia.org/wiki/Line%E2%80%93line_intersection)

However, this is a port of the current Kata application where the X axis runs from segment 65 to segment 74. It's still presented to the gamer as A to J, and the Y axis starts at segment 1 and ends at segment 10.  The game randomly generates the coordinates for the ships, either aligning them horizontally or vertically along its corresponding axis.
![Grid Dimension](https://github.com/VisualSanity/Battleship.Microservices/blob/master/support/Grid.jpg)

The user then enters then clicks on a particular box e.g. "A1" or "c7", and this will get mapped to coordinates X and Y accordingly. The flow of the game, is as follows: 

![Flow chart](https://github.com/visualsanity/Battleship.Microservices/blob/master/support/Game_FlowChart.jpg)

With a domain driven approach (DDD), the applicaition is broken up into microservices i.e. Player, Game, Audit, Warehouse and  Statistics ASPNet Core 3.1 API web services and a Angular 9 front end. It uses RabbitMQ as the comminication bus. This allows for indivdual components to be updated in relative isolation, while leveraging Docker and Kubernetes.

The application follows SOLID practices, such as single source of responsibility, dependency injection, open/closed princliples to name a few. 

I try and write my code in C#, without to much "syntactic sugar" e.g. chained Linq queries, as it makes debugging unnecessarily more difficult during development. I try and write my code for the next developer in mind. 

#Gameplay

The game has two gameplay paths, a demo path and a player account path. The demo account path does not allow for extra features such as warehousing, statistics and audits.

![Login](https://github.com/visualsanity/Battleship.Microservices/blob/master/support/login.png)

 1. Navigation bar
 2. Demo login
 3. Registered player login
 
 Once you have logged in, you can start the game at a selected level. The lower the level, the more ship you will be able to target.
 
![gampeplay](https://github.com/visualsanity/Battleship.Microservices/blob/master/support/gameplay.png)

 1. Game level selection
 2. Ship hit during gameplay
 3. Ship missed during gameplay
 4. Gameplay messages
 5. Scoreboard
 6. Actions

When development is completed (alpha), your will be able to play the game remotely at http://visualsanity.io/battleship

## Running the game locally
 The game was written in C# (.Net Core 3.1). Fork or clone this repository with your favourite git client or just use the command line. Whatever your preferred flavour of platform is, make sure you are updated to .Net Core 3.1 SDK.

**Visual Studio 2019**
If you are using Visual Studio 2019 on a Microsoft environment , open the solution file in the source directory, (Battleship.Microservices.sln) and run the build. The nuget packages (NUnit, Newtonsoft... ext) should auto restore.

**Visual Studio Code**
Navigate to the Battleship.Microservices.Web application and open the Battleship.code-workspace. Run the ng serve commands in the terminal. You should now be able to navigate to http://localhost:4200

**Testing**
The game has been tested on the following three environments:
 1. Ubuntu 18.10
 2. Windows 10 Pro
 3. MacOS 10.14: Mojave

NUnit was used to do the unit tests during development. You can run the unit tests within Visual Studio 2019 or if you prefer using Visual Studio Code download the .NET Core Test Explorer extension, if you don't have it already.

To run the game simulator, make sure your Visual Studio Code terminal is set to bash or you use git bash terminal for Windows:

[https://code.visualstudio.com/docs/editor/integrated-terminal](https://code.visualstudio.com/docs/editor/integrated-terminal)


## What have I learnt ?

The game won't scale well. For the moment, the max loop count is 100 (10 X 10), which is nothing. For example the ReDraw() method is suffice for this current implementation, without making things to complicated. Should the grid grow to 3000 for example (the Chinese alphabet, worst case scenario has 3000 characters), the loop will become highly inefficient, as it would need to loop a maximum of 9000000 times. A complete rethink would need to be implemented, such as a SortedDictionary or B-Tree structure.

However, this is only a Kata, so keeping it simple and to specification is the idea. 

## Support
If you have any questions or problems running the application, open a support ticket within the GitHub repository, so that I can assist.

**Enjoy!**
