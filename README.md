# AirportBoard and LandingPad
Both of these programs are very very similar, so we put them in the same repo. Though we probably shouldn't've but oh well, now its time to deal with the consequences of our actions.

Airportboard was originally made as an example of the BasicRender VB.NET module. BasicRender is now a C# Class Library, visible [here](https://github.com/igtampe/BasicRender). More information about the classes are available at the mentiond repo. We've managed to port this app from the VB.NET Module to the C# library.

AirportBoard is based on the strategies used on the Publicus Network Billboard batch program made as part of the Network Services package I made back in 2015, and I upgraded in 2017. Publicus is included in the source of this project, along with 2 configurations (one from 2015, and one from 2017). In addition, [here](http://igtdos.blogspot.com/2014/06/network-services-version-20.html) is a post from the Igtampe DOS Blog which mentions it, and has a link to the network services package with Publicus 1.0. 

Publicus was inspired by the overnight teletext service Nite-Owl which was used by WFLD Channel 32 in 1982. A recording of the system in action is viewable [here](https://youtu.be/Bgs0kbxo68w)

## AirportBoard
![VolucrisTitleCard](https://raw.githubusercontent.com/igtampe/AirportBoard/master/Images/VolucrisNew.png)<br>
A VB.NET Console public information board renderer. <br>
**An example configuration is provided in the release**

### ABScript
AirportBoard uses its own scripting language called ABScript stored in .ab files. It sequentially loads page0.ab, page1.ab... etc until there is no more pages left, then loops. Along with this there should be a Header.ab and Footer.ab which may be called upon to render headers and footers at a moment's notice. 

ABScript files can be written with [ABWriter](https://github.com/igtampe/ABWriter), an IDE-like I made for this purpose. 

Each AB File can call upon a few select commands including the following:

#### `DRAW [File] [Leftpos] [TopPos]`<br>
Draw will draw from a proprietary .DF File. See the BasicGraphic section under [BasicRender's Readme](https://github.com/igtampe/BasicRender) for more information.
DF Files can be created using [Henja3](https://github.com/igtampe/Henja3), a graphical editor that runs in the console to preview exactly how it would look.

#### `COLOR [Background][Foreground]`<br>
Color changes the color of all text typed afterwards. Use a Hexadecimal number to specify background and foreground colors. See the BasicGraphic section under [BasicRender's Readme](https://github.com/igtampe/BasicRender) for more information. There is no space between Background and Foreground. It's phrased exactly like the color command on the windows commandline. (IE: Color 0F)
     
#### `TEXT~[TEXT]~[COLOR]~[LEFTPOS]~[TOPPOS]`<br>
Text is the only command separated with tildes. This is to allow spaces in the text you write. Color is specified with two hexadecimal numbers. Use LeftPos and TopPos to specify where the text should start.

#### `CenterText~[Text]~[Line]`<br>
I lied. There's one more separated with tildes. CenterText will render a centered piece of text at the specified line. Note that the last set background color (using COLOR) will be the color of the text and the preceeding space.
     
#### `BOX [color] [Length] [Height] [LeftPos] [TopPos]`<br>
BOX Will render a box of the specified color (as a hexadecimal number), length, and width at the specified upper left corner coordinate.
     
#### `CLEAR`<br>
It clears the screen. The background color is the last specified background color by using COLOR.

#### `RUN [File]`<br>
Runs another AB File. Useful if you need to redo something like the Footer or Header.
     
#### `CLOCK [COLOR] [Leftpos] [Toppos]`<br>
Displays the time in the specified color (as two hexadecimal numbers without a space between them) at the specified position.
     
#### `DATE [COLOR] [Leftpos] [Toppos]`<br>
Same as Clock but instead of time it's the date.

#### `WeatherWindow [WeatherWindow File] [Columns] [Rows] [Leftmost] [Topmost]`<br>
![WeatherWindow](https://raw.githubusercontent.com/igtampe/AirportBoard/master/Images/WeatherWindowActual.png)<br>
_Page 4 of the included AirportBoard configuration, which shows a WeatherWindow (`WeatherWindow CommonDestinations.WW 2 3 10 4`)_<br><br>

WeatherWindow uses a WeatherWindow file (.ww) to display a table (With specified columns and rows) of weather information at the specified position. A weather Window file is structured with this information:
     
          [First Line of Text]~[Second Line of Text]~[Weather Icon]
          [First Line of Text]~[Second Line of Text]~[Weather Icon]

Here's a portion of CommonWeather.DF to illustrate this:
     
          Jimoto: 70F    ~Raining          ~TinyWeatherIcons\rain.df
          Hepe Intl: 85F ~Sunny            ~TinyWeatherIcons\sun.df
          
#### `NewsWindow [NewsPage File]`<br>
NewsWindow generates a window of news based on a newspage file (.np). NewsPage files are rather easy to make. Here's an example:
     
          [Header]
          [The News in one line]
     
          [Header for another piece]
          [The other news in one line]
          
Here's a portion of ReliantNews.np to illustrate:
     
          Wisp Air merges with Avaris
          The Wisp Aviation company has merged with Avaris mere days after it opened its doors.
          
          Hepe Intl. Begins Construction
          Hepe Intl. finally begins construction with a few more components added by Hepe and Chopo last minute. The airport is currently at about 50% completion.
 
Note that Airportboard doesn't format the text to avoid text being split at line changes, so keep that in mind when writing blurbs.
     
#### `FlightWindow [FlightWindowFile] [IsDepartures]`<br>
![DepartureWindow](https://raw.githubusercontent.com/igtampe/AirportBoard/master/Images/DepartureWindow.png)<br>
_Page 8 of the included AirportBoard configuration, which shows a FlightWindow in Departure mode (`FlightWindow Departures.FW TRUE`)_<br><br>

FlightWindow generates a table of departures and arrivals based on a FlightWindow File (.fw). IsDepartures specifies wether theFlightWindow is a departure board or an arrivals board. Each Flightwindow file is formatted as such:
     
          --(TERMINAL LETTER OR NUMBER)
          ~Airline Color~Airline~Flight Number~Gate~Time~Status~Destination (or Origin)
     
Airline color is specified with a hexadecimal number. Status is determined with an integer, with the following definitions:
          
          0: ON TIME
          1: BOARDING
          2: DElAYED
          3: CANCELED
          4: DEPARTED or ARRIVED
     
Here's an example of one terminal from Arrivals.fw:
     
          --A
          ~A~ISLAND HOP~7 ~A1~10 30 AM~0~ALPHA ISLANDS               
          ~A~ISLAND HOP~8 ~A4~11 30 AM~0~BETA ISLANDS                
          ~A~ISLAND HOP~9 ~A4~10 00 AM~0~DELTA ISLANDS               

Note that you can also comment stuff out by adding a semicolon (;) at the start of a line in .FW Files and with an appostraphe (') on .AB Files. You can execute an .AB File by opening it with AirportBoard (Dragging it into AirportBoard's icon), and preview .df files with the same procedure (note that Airportboard will close as soon as its done rendering it).

## LandingPad
![LandingPad Cookie Logo](https://raw.githubusercontent.com/igtampe/AirportBoard/master/Images/LandingPad.png)<br>
A VB.NET Menu-type thing which uses AirportBoard as a base.<br>
**An Example configuration is available on our release**

LandingPad is a small program designed to be used, as its name implies, a landing pad which can be used to access other programs. It's a rewrite of an old batch program used as the main-menu for my SSH Server. Graphics operations have now been improved, thanks to the fact that they run natively on VB.NET thanks to BasicRender, instead of calling another program to handle everything. <br><br>

In addition, the ticker now gets prepared instantly, rather than the long workaround I made in batch. Granted, the workaround was perhaps a tad more interesting.<br><br>

### Getting Started with Landing Pad
LandingPad uses the same Page0, Page1, Page2 system that Airportboard uses, and has all features from Airportboard available. I'd recommend using ABWriter to write your AB Files.

**Unlike AirportBoard, which has dimensions 80x25, LandingPad has dimensions 80x24 due to the limitations of my SSH Handler, FreeSSHd**
(Maybe I should add an ABScript command to change dimensions. Hmmmmmm)
Also unlike Airportboard, LandingPad requires more files to start. These include the following:

#### Tick.AB
The board needs to be tickable, so we need Tick.ab. Without being tickable, the board wouldn't be able to read user inputs. Plus, it'd be nice to include a ticker of sorts in a main-menu that goes through different pages.

#### Main.ab
Main.ab is run before running Page0, and run every time you return to LandingPad after executing a program. It's recommended that Main.AB hold information for your user on which keys to press to access which programs. 
In addition, Main.AB should tell users that hitting `A` will launch the about page, and hitting `D` will disconnect them from the terminal.

#### Options.txt
Options.txt should hold which keychar a user should press to run a certain command on the host machine. The format for each line is as follows:
```
1~WinVer
```

#### PreAction.ab
Pre-action is run before an action/command is to be executed, and when disconnecting from the terminal. It should say something like "Stand By". Note that when disconnecting, LandingPad will draw Centered text saying `G O O D B Y E` on whatever row the cursor happens to be on.

#### About.AB
![Default About Page](https://raw.githubusercontent.com/igtampe/AirportBoard/master/Images/LandingPad%20About.png)<br>
_Default About Page included in the release_<br><br>
About.AB is run by the about page. It should really only use lines 0 through 15. Line 16 will display (in centered text) `LandingPad V (version)`, Lines 18 and 19 display the old HiColor rainbow, Line 21 displays `based on AirportBoard 2.0`, and line 22 displys `A Program by Igtampe, 2020` becuase I had to stick my name on their somewhere.

#### ConsolePass.txt (Optional)
ConsolePass.txt holds the password used to access the actual console (CMD). Without it, the board won't allow users to access the console. It's stored in Plaintext.

### Recommendations when writing pages.
LandingPad will re-execute the current page from the start once it returns after running a command (Mostly so that it can re-render itself). It's recommended that if you have an element that has multiple animations, that you split them into different pages. Otherwise these animations will restart from the beginning when a user returns to LandingPad.
**For this reason, although they are included for compatibility, it is not recommended to use WeatherWindow, NewsWindow, and FlightWindow**

----

I hope you all can enjoy and find some use in this mess I made. <br>
-IGT

