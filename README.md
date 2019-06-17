# AirportBoard
A VB.NET Console public information board renderer

[An example configuration is provided on the root under AIRPORTBOARD.ZIP]

AirportBoard uses its own scripting language called ABScript stored in .ab files. It sequentially loads page0.ab, page1.ab... etc until there is no more pages left, then loops. Along with this there should be a Header.ab and Footer.ab which may be called upon to render headers and footers at a moments notice. Each AB File can call upon a few select commands including the following:

DRAW [File] [Leftpos] [TopPos]
     
     Draw will draw from a proprietary .DF File. DF Files are pretty simple. Here's Doot.DF which is drawn as part of the system test:
     
          123456789ABCDEF                                                 FEDCBA987654321
          123456789ABCDEF  F  FFF FFF FFF FFF FFF FFF FF  FFF  F  FFF FF  FEDCBA987654321
          123456789ABCDEF F F  F  F F F F F F F F  F  F F F F F F F F F F FEDCBA987654321
          123456789ABCDEF F F  F  F F F F F F F F  F  F F F F F F F F F F FEDCBA987654321
          123456789ABCDEF F F  F  FFF FFF F F FFF  F  F F F F F F FFF F F FEDCBA987654321
          123456789ABCDEF F F  F  FF  F   F F FF   F  FF  F F F F FF  F F FEDCBA987654321
          123456789ABCDEF FFF  F  FF  F   F F FF   F  F F F F FFF FF  F F FEDCBA987654321
          123456789ABCDEF F F  F  F F F   F F F F  F  F F F F F F F F F F FEDCBA987654321
          123456789ABCDEF F F FFF F F F   FFF F F  F  FF  FFF F F F F FF  FEDCBA987654321
          123456789ABCDEF                                                 FEDCBA987654321
          
          00000000011111111112222222222333333333344444444445555555555666666666667777777778
          12345678901234567890123456789012345678901234567890123456789012345678901234567890
     
     Each hexadecimal number is converted to a block of its color. See the Color command in the windows commandline to see which color 
     each number represents. Use LeftPos and TopPos to specify the upper left corner of where to draw the graphic.

COLOR [Background][Foreground]
     
     Color changes the color of all text typed afterwards. Use a Hexadecimal number to specify background and foreground colors. There 
     is no space
     
TEXT~TEXT~COLOR~LEFTPOS~TOPPOS

     Text is the only command separated with tildes. This is to allow spaces in the text you write. Color is specified with two
     hexadecimal numbers. Use LeftPos and TopPos to specify where the text should start.
     
CenterText~[Text]~[Line]
     
     I lied. There's one more separated with tildes. CenterText will render a centered piece of text at the specified line. Note that
     the last set background color (using COLOR) will be the color of the text and the preceeding space.
     
BOX [color] [Length] [Height] [Leftpos] [Toppos]
     
     BOX Will render a box of the specified color (as a hexadecimal number), length, and width at the specified upper left corner 
     coordinate.
     
CLEAR

     It clears the screen. The background color is the last specified background color by using COLOR.
     
RUN [File]

     Runs another AB File. Useful if you need to redo something like the Footer or Header.
     
CLOCK [COLOR] [Leftpos] [Toppos]

     Displays the time in the specified color (as two hexadecimal numbers without a space between them) at the specified position.
     
DATE [COLOR] [Leftpos] [Toppos]

     Same as Clock but instead of time it's the date.
     
WeatherWindow [WeatherWindow File] [Columns] [Rows] [Leftmost] [Topmost]

     WeatherWindow uses a WeatherWindow file (.ww) to display a table (With specified columns and rows) of weather information at the
     specified position. A weather Window file is structured with this information:
     
          [First Line of Text]~[Second Line of Text]~[Weather Icon]
          [First Line of Text]~[Second Line of Text]~[Weather Icon]
          
     Here's a portion of CommonWeather.DF to illustrate this:
     
          Jimoto: 70F    ~Raining          ~TinyWeatherIcons\rain.df
          Hepe Intl: 85F ~Sunny            ~TinyWeatherIcons\sun.df
          
NewsWindow [NewsPage File]

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
     
FlightWindow [FlightWindowFile] [IsDepartures]

     FlightWindow generates a table of departures and arrivals based on a FlightWindow File (.fw). IsDepartures specifies wether the
     FlightWindow is a departure board or an arrivals board. Each Flightwindow file is formatted as such:
     
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
          ~A~ISLAND HOP~21~A4~12 00 PM~0~DELTA ISLANDS               
          ~A~ISLAND HOP~10~A2~11 00 AM~0~GAMMA ISLANDS               
          ~B~SKYFLY    ~41~A5~11 30 AM~0~GAMMA ISLANDS               
          ~A~ISLAND HOP~12~A3~ 8 00 AM~4~UMSTEP                      
          ~A~ISLAND HOP~48~A3~10 30 AM~0~UMSTEP                      
          ~A~ISLAND HOP~11~A3~ 9 00 AM~0~ZULU ISLANDS                
          ~A~ISLAND HOP~17~A3~11 30 AM~0~ZULU ISLANDS                

Note that you can also comment stuff out by adding a semicolon (;) at the start of a line in .FW Files and with an appostraphe (') on .AB Files. You can execute an .AB File by opening it with AirportBoard (Dragging it into AirportBoard's icon), and preview .df files with the same procedure (note that Airportboard will close as soon as its done rendering it).

AirportBoard is powered by (And to a certain extent could be considered a tech demo of) the BasicRender.VB module which you can also grab from this project and import to your own console application. It provides a set of commands to help render graphics on a console window. Unused in this app is the Hi-Color draw mode which is a little more complicated. Using a string like "0F0-0F1-1F2" you can dither two colors (the two first hexadecimal numbers) with varying intensity (0 is low (░), 1 is medium (▒), and 2 is high (▓)). BasicRender also includes a HiColorDrawFromFile function, but it is not accessible via ABScirpt.

I hope you all can enjoy and find some use in this mess I made. 
-IGT
