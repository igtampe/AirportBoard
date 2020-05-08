# LandingPad
A VB.NET Menu-type thing which uses AirportBoard as a base

An Example configuration is available on our release

**For more information on ABScript, BasicRender, and AirportBoard, see the master branch**

LandingPad is a small program designed to be used, as its name implies, a landing pad which can be used to access other programs. It's a rewrite of an old batch program used as the main-menu of my SSH Server. Graphics operations have now been improved, thanks to the fact that they run natively on VB.NET thanks to BasicRender, instead of calling another program to handle everything. <br><br>

In addition, the ticker now gets prepared instantly, rather than the long workaround I made in batch. Granted, the workaround was perhaps a tad more interesting.<br><br>

## Getting Started with Landing Pad
LandingPad uses the same Page0, Page1, Page2 system that Airportboard uses, and has all features from Airportboard available. I'd recommend using ABWriter to write your AB Files.

**Unlike AirportBoard, which has dimensions 80x25, LandingPad has dimensions 80x24 due to the limitations of my SSH Handler, FreeSSHd**
(Maybe I should add an ABScript command to change dimensions. Hmmmmmm)
Also unlike Airportboard, LandingPad requires more files to start. These include the following:

### Tick.AB
The board needs to be tickable, so we need Tick.ab. Without being tickable, the board wouldn't be able to read user inputs. Plus, it'd be nice to include a ticker of sorts in a main-menu that goes through different pages.

### Main.ab
Main.ab is run before running Page0, and run every time you return to LandingPad after executing a program. It's recommended that Main.AB hold information for your user on which keys to press to access which programs. 
In addition, Main.AB should tell users that hitting `A` will launch the about page, and hitting `D` will disconnect them from the terminal.

### Options.txt
Options.txt should hold which keychar a user should press to run a certain command on the host machine. The format for each line is as follows:
```
1~CMD
```

### PreAction.ab
Pre-action is run before an action/command is to be executed, and when disconnecting from the terminal. It should say something like "Stand By". Note that when disconnecting, LandingPad will draw Centered text saying `G O O D B Y E` on whatever row the cursor happens to be on.

### About.AB
About.AB is run by the about page. It should really only use lines 0 through 15. Line 16 will display (in centered text) `LandingPad V (version)`, Lines 18 and 19 display the HiColor rainbow, Line 21 displays `based on AirportBoard 2.0`, and line 22 displys `A Program by Igtampe, 2020` becuase I had to stick my name on their somewhere.

### ConsolePass.txt (Optional)
ConsolePass.txt holds the password used to access the actual console (CMD). Without it, the board won't allow users to access the console. It's stored in Plaintext.

## Recommendations when writing pages.
LandingPad will re-execute the current page from the start once it returns after running a command (Mostly so that it can re-render itself). It's recommended that if you have an element that has multiple animations, that you split them into different pages. Otherwise these animations will restart from the beginning when a user returns to LandingPad.
**For this reason, although they are included for compatibility, it is not recommended to use WeatherWindow, NewsWindow, and FlightWindow**
