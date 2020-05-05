## Battleship.Microservices (example application)
![dotnet core - build](https://github.com/visualsanity/Battleship.Microservices/workflows/dotnet%20core%20-%20Continuous%20Integration%20for%20Microservices%20Web%20Application/badge.svg)

The following Battleship game uses the dimensions of a 10 X 10 grid.  The game is plotted on a X and Y axis. Instead of coding the X axis as chars, I decided to use the Ascii Table as it contains a numerical representation of characters, such as 'A' starting at point 65 and 'Z' ending in 90.  Both char and int consume one byte in memory (0 to 255) so the only overhead is the user input string manipulation. The reason for working in integers only, was to work in a geometrical graph quadrant manor. This would make working with Line equations, such as the linear Intersection equation easier:

[https://en.wikipedia.org/wiki/Line-line_intersection](https://en.wikipedia.org/wiki/Line%E2%80%93line_intersection)

However, this is a simple console application where the X axis runs from segment 65 to segment 74. It's still presented to the gamer as A to J, and the Y axis starts at segment 1 and ends at segment 10.  The game randomly generates the coordinates for the ships, either aligning them horizontally or vertically along its corresponding axis.
![Grid Dimension](https://github.com/VisualSanity/Battleship.Microservices/blob/master/support/Grid.jpg)

The user then enters the string input e.g. "A1" or "c7", and this will get mapped to coordinates X and Y accordingly. The flow of the game, is as follows: 

![Flow chart](https://github.com/VisualSanity/Battleship.Microservices/blob/master/support/Game_FlowChart.jpg)

Console UI code is extracted into a ConsoleHelper class . This was so that I could unit test the UI layer. Most of the other classes are Singletons, as no IoC framework was available. The reason I implemented singletons rather than static classes was so that I could unit test the public methods and follow SOLID practices.

I try and write my code in C#, without to much "syntactic sugar" e.g. chained Linq queries, as it makes debugging unnecessarily more difficult during development. I try and write my code for the next developer in mind.   

## Running the game
 The game was written in C# (.Net Core 3.1). Fork or clone this repository with your favourite git client or just use the command line. Whatever your  preferred flavour of platform is, make sure you are updated to .Net Core 3.1 SDK.

**Visual Studio 2017 or newer**
If you are using Visual Studio 2017 on a Microsoft environment , open the solution file in the source directory, (BattleshipGame.sln) and run the build. The nuget packages (NUnit, Newtonsoft) should auto restore.

**Visual Studio Code**
If you are using Visual Studio Code on any environment (Linux, Mac or Microsoft), open the Battleship.code-workspace and from the Terminal Window inside Visual Studio Code run the following commands in order:
 1. dotnet restore
 2. dotnet build -c release 
 3. dotnet publish -c release
 4. cd Battleship.Game
 5. dotnet run

**Testing**
The game has been tested on the following three environments:
 1. Ubuntu 18.10
 2. Windows 10 Pro
 3. macOS 10.14: Mojave

NUnit was used to do the unit tests during development. You can run the unit tests within Visual Studio 2017 or if you prefer using Visual Studio Code download the .NET Core Test Explorer extension, if you don't have it already.

For **Automated Testing**, you can enter a json string, into the startup method.  This will simulate game play.  If the json string parse fails, it will default into manual game play.

To run the game simulator, make sure your Visual Studio Code terminal is set to bash or you use git bash terminal for Windows:

[https://code.visualstudio.com/docs/editor/integrated-terminal](https://code.visualstudio.com/docs/editor/integrated-terminal)

Run the following commands from the BattleshipGame Source directory (Make sure that you published the game, as mentioned above):

 1. cd ./Battleship.Game/bin/Release/netcoreapp2.2/publish
 2. dotnet ./Battleship.Game.dll "{'SimulationTimer': 50, 'X':['A','B','C','D','E','F','G','H','I','J'],'Y':[1,2,3,4,5,6,7,8,9,10]}"

## What have I learnt ?

The game won't scale well. For the moment, the max loop count is 100 (10 X 10), which is nothing. For example the ReDraw() method is suffice for this current implementation, without making things to complicated. Should the grid grow to 3000 for example (the Chinese alphabet, worst case scenario has 3000 characters), the loop will become highly inefficient, as it would need to loop a maximum of 9000000 times. A complete rethink would need to be implemented, such as a SortedDictionary or B-Tree structure.

However, this is only a Kata, so keeping it simple and to specification is the idea. 

## Support
If you have any questions or problems running the application, open a support ticket within the GitHub repository, so that I can assist.

**Enjoy!**
