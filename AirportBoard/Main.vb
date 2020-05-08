Imports System.IO

''' <summary>Holds the main processes of AirportBoard</summary>
Module Main

    Public CurrentLine As Integer
    Public TickAB As String()
    Public Tickable As Boolean = False

    Public BoardTicker As Ticker

    ''' <summary>Starts the AirportBoard</summary>
    Public Sub Main()

        'Set color
        Color(ConsoleColor.Black, ConsoleColor.Green)

        'Set window size, clear, and set the title
        Console.SetWindowSize(80, 25)
        Console.SetBufferSize(80, 25)
        Console.Clear()
        Console.Title = "AirportBoard [Version 2.0]"

        If Not File.Exists("Tick.ab") Then
            DialogBox("Could not find Tick.ab! Board marked as not tickable", 2, 1, True, True)
        Else
            Tickable = True
            TickAB = GetFileContents("Tick.ab")
        End If

        If File.Exists("init.ab") Then
            Run("init.ab")
        End If

        Dim Args() As String = Environment.GetCommandLineArgs()
        If Args.Count = 2 Then
            Console.Title &= " " & Args(1)

            If Args(1).ToUpper.EndsWith(".AB") Then Run(Args(1)) 'Run an AB file for a preview
            If Args(1).ToUpper.EndsWith(".DF") Then DrawFromFile(Args(1), 0, 0) 'Draw a file as a preview

            Exit Sub
        End If

        'This call is now made by INIT, so if anyone wants to run the screentest, they can.
        'ScreenTest()
        MainMenu()
    End Sub

    ''' <summary>Tests screen to show all of what AirportBoard can do</summary>
    Private Sub ScreenTest()

        'Test CenterText
        SetPos(0, 1)
        CenterText("SYSTEM TEST")

        'Test Box
        Box(ConsoleColor.Gray, 70, 3, 5, 3)

        'Test Draw
        SetPos(6, 4)
        Draw("0123456789ABCDEF")

        'Test Sprite
        Sprite(" Hello!", ConsoleColor.Gray, ConsoleColor.Black)

        'Test DrawFromFile
        DrawFromFile("Doot.DF", 0, 8)

        'Test HiColorDraw
        SetPos(23, 22)
        HiColorDraw("010-011-012-112-120-121-122-222-230-231-232-330-340-341-342-440-450-451-452-550-560-561-562-660-670-671-672-770-780-781-782-880-800-801-802-000")

        'Sleep to make sure we can see this for at least a moment
        SetPos(0, 23)
        Sleep(1000)
    End Sub

    ''' <summary>Handles the main operations</summary>
    Private Sub MainMenu()

        'Clear the console from the last page (which should be the screen test)
        Console.Clear()

        'If there's no files, make sure we tell the user there's no files!
        If Not File.Exists("Page0.AB") Then
            DialogBox("No Pages Found", 3, 1, False, True)
            Exit Sub
        End If

        Dim CurrentPage As Integer = 0

        Do
            Try
                'Run the page
                Run("Page" & CurrentPage & ".ab")
            Catch ex As Exception
                'in case there has been an error
                LoadErrorPage(ex, CurrentPage)
            End Try

            CurrentPage += 1
            If Not File.Exists("Page" & CurrentPage & ".AB") Then CurrentPage = 0
            'If the next page doesn't exist, we've already reached the end, so loop back to the first page
        Loop

    End Sub

    ''' <summary>Returns the contents of a file as an array</summary>
    Public Function GetFileContents(File As String) As String()
        FileOpen(1, File, OpenMode.Input)
        Dim PageContents() As String
        Dim I As Integer = 0
        While Not EOF(1)
            ReDim Preserve PageContents(I)
            PageContents(I) = LineInput(1)
            I += 1
        End While
        FileClose(1)
        Return PageContents
    End Function

    ''' <summary>Run an ABScript file</summary>
    Public Sub Run(File As String)
        'Get page contents and run the page contents
        Run(GetFileContents(File))
    End Sub

    ''' <summary>Run an array of ABScript Commands</summary>
    Private Sub Run(PageContents() As String)

        Dim CurrentCommand() As String
        Dim Temp As String
        CurrentLine = -1

        'Handles empty pages
        If IsNothing(PageContents) Then Exit Sub

        For Each Line As String In PageContents
            CurrentLine += 1

            'ToUpper it
            Dim UpperLine As String = Line.ToUpper

            If String.IsNullOrWhiteSpace(Line) Then
                'do nothing

            ElseIf UpperLine.StartsWith("'") Then
                'Comment, do nothing

            ElseIf UpperLine.StartsWith("DRAW") Then
                'Draw from file (DRAW FILE LEFT TOP)
                CurrentCommand = Line.Split(" ")
                DrawFromFile(CurrentCommand(1), CurrentCommand(2), CurrentCommand(3))

            ElseIf UpperLine.StartsWith("CLEAR") Then
                'Clear the screen (CLEAR)
                Console.Clear()

            ElseIf UpperLine.StartsWith("COLOR") Then
                'Set Screenwriter color (COLOR 0F)
                Temp = UpperLine.Replace("COLOR ", "") 'Temp holds the color string (0F)
                Color(StringToColor(Temp(0)), StringToColor(Temp(1)))

            ElseIf UpperLine.StartsWith("TEXT") Then
                'Draw text (TEXT~the text~0F~LEFT~TOP
                CurrentCommand = Line.Split("~")
                Temp = CurrentCommand(2) 'Temp holds a color string (0F)
                Sprite(CurrentCommand(1), StringToColor(Temp(0)), StringToColor(Temp(1)), CurrentCommand(3), CurrentCommand(4))

            ElseIf UpperLine.StartsWith("RUN") Then
                'Run another ABScript file (RUN Page0.AB)
                Temp = UpperLine.Replace("RUN ", "")
                Run(Temp)

            ElseIf UpperLine.StartsWith("SLEEP") Then
                'Wait for a specified number of milliseconds (SLEEP 100)
                Temp = UpperLine.Replace("SLEEP ", "")
                ABSleep(Temp)

            ElseIf UpperLine.StartsWith("PAUSE") Then
                'Wait for user to hit a key to continue
                Pause()

            ElseIf UpperLine.StartsWith("BOX") Then
                'Draws a box (BOX F LENGTH HEIGHT LEFT TOP)
                CurrentCommand = Line.Split(" ")
                Box(StringToColor(CurrentCommand(1).ToString), CurrentCommand(2), CurrentCommand(3), CurrentCommand(4), CurrentCommand(5))

            ElseIf UpperLine.StartsWith("CLOCK") Then
                'Draws a clock at the specified position (CLOCK 0F LEFT TOP)
                CurrentCommand = Line.Split(" ")
                Temp = CurrentCommand(1) 'Temp holds a colorstring
                Clock(StringToColor(Temp(0).ToString), StringToColor(Temp(1).ToString), CurrentCommand(2), CurrentCommand(3))

            ElseIf UpperLine.StartsWith("DATE") Then
                'Draws a date at the specified position (DATE 0F LEFT TOP)
                CurrentCommand = Line.Split(" ")
                Temp = CurrentCommand(1) 'Temp Holds a colorstring
                DateRender(StringToColor(Temp(0).ToString), StringToColor(Temp(1).ToString), CurrentCommand(2), CurrentCommand(3))

            ElseIf UpperLine.StartsWith("CENTERTEXT") Then
                'Centers text on screen (Centertext~text~row)
                CurrentCommand = Line.Split("~")
                SetPos(0, CurrentCommand(2))
                CenterText(CurrentCommand(1))

            ElseIf UpperLine.StartsWith("WEATHERWINDOW") Then
                'Draws a WeatherWindow using a WeatherWindow File (WeatherWindow Filename Length Height leftpos Toppos)
                CurrentCommand = Line.Split(" ")
                Dim WW As WeatherWindow = New WeatherWindow(CurrentCommand(1), CurrentCommand(2), CurrentCommand(3), CurrentCommand(4), CurrentCommand(5))
                WW.Render()

            ElseIf UpperLine.StartsWith("NEWSWINDOW") Then
                'Draws a NewsWindow using a NewsWindow File (NEWSWIDNDOW File)
                CurrentCommand = Line.Split(" ")
                Dim NW As NewsWindow = New NewsWindow(CurrentCommand(1))
                NW.Render()

            ElseIf UpperLine.StartsWith("FLIGHTWINDOW") Then
                'Draws a FlightWindow using a Flightwindow file (FlightWindow, DepartureMode)
                CurrentCommand = Line.Split(" ")
                Dim FW As FlightWindow = New FlightWindow(CurrentCommand(1), CurrentCommand(2))
                FW.Render()

            ElseIf UpperLine.StartsWith("INITTICKER") Then
                'Initialize the ticker
                CurrentCommand = Line.Split(" ")
                BoardTicker = New Ticker(CurrentCommand(1))

            ElseIf UpperLine.StartsWith("TICKER") Then
                'Draws the initialized ticker with specified colors, at specified position, with specified lenght
                '(TICKER Colorstring Length leftpos toppos)
                CurrentCommand = Line.Split(" ")
                Temp = CurrentCommand(1) 'Temp Holds a colorstring
                RenderTicker(StringToColor(Temp(0).ToString), StringToColor(Temp(1).ToString), CurrentCommand(2), CurrentCommand(3), CurrentCommand(4))

            ElseIf UpperLine.StartsWith("SCREENTEST") Then
                ScreenTest()
                Console.Clear()

            Else
                'Oopsie this line is unparsable
                GuruMeditationError("Could not interpret line " & CurrentLine, UpperLine.Split(" ")(0), "", "")
            End If
        Next

    End Sub

    ''' <summary>Sleep call for AirportBoard. Keeps any elements that need to tick ticking</summary>
    ''' <param name="time"></param>
    Public Sub ABSleep(time As Integer)

        Dim Steppy As Integer = 250
        Dim pressedkey As ConsoleKeyInfo

        If time < Steppy Or Not Tickable Then
            Sleep(time)
            Return
        End If

        For X = 0 To time Step Steppy

            If Console.KeyAvailable Then
                pressedkey = Console.ReadKey(True)
                If pressedkey.Key = ConsoleKey.Escape Then Exit Sub
            End If

            Run(TickAB)
            Sleep(Math.Min(time - X, Steppy))
        Next

    End Sub

    ''' <summary>Renders the ticker</summary>
    Public Sub RenderTicker(BackgroundColor As ConsoleColor, ForegroundColor As ConsoleColor, Length As Integer, leftpos As Integer, toppos As Integer)
        If IsNothing(BoardTicker) Then
            GuruMeditationError("Cannot run ticker, ticker not initialized!", "", "", "")
        Else
            Sprite(BoardTicker.GetTicker(Length), BackgroundColor, ForegroundColor, leftpos, toppos)
        End If

    End Sub

    ''' <summary>Renders the current time to screen</summary>
    Public Sub Clock(backgroundcolor As ConsoleColor, foregroundcolor As ConsoleColor, leftpos As Integer, toppos As Integer)
        Sprite(DateTime.Now.ToShortTimeString, backgroundcolor, foregroundcolor, leftpos, toppos)
    End Sub

    ''' <summary>Renders the current date to screen</summary>
    Public Sub DateRender(backgroundcolor As ConsoleColor, foregroundcolor As ConsoleColor, leftpos As Integer, toppos As Integer)
        Sprite(DateTime.Now.ToShortDateString, backgroundcolor, foregroundcolor, leftpos, toppos)
    End Sub

    ''' <summary>Shows a Guru Meditation Error after saving the error's stacktrace to disk</summary>
    Private Sub LoadErrorPage(ex As Exception, PageIndex As Integer)

        Try
            FileOpen(1, "ABErrors.txt", OpenMode.Append)
            PrintLine(1, "=[AB ERROR]======================================================")
            PrintLine(1, ex.Message)
            PrintLine(1, ex.StackTrace)
            PrintLine(1, "=================================================================")
            FileClose(1)
        Catch ex2 As Exception
            GuruMeditationError("There was an error saving the error", ex2.Message.Split(vbNewLine)(0), ex.Source, "I don't know how this could happen")
        End Try

        GuruMeditationError("Error at Page " & PageIndex & " Line " & CurrentLine, ex.Message.Split(vbNewLine)(0), ex.Source, "The error was written to ABErrors.txt")

    End Sub

    ''' <summary>Shows a guru meditation error</summary>
    Public Sub GuruMeditationError(Line1 As String, Line2 As String, Line3 As String, line4 As String)
        '80x25
        Color(ConsoleColor.DarkGray, ConsoleColor.Red)
        Box(ConsoleColor.DarkGray, 50, 15, 15, 5)
        Box(ConsoleColor.Red, 50, 1, 15, 5)
        Color(ConsoleColor.Red, ConsoleColor.White)
        SetPos(0, 5)
        CenterText("Guru Meditation Error")

        Box(ConsoleColor.Gray, 48, 10, 16, 7)

        Sprite(Line1, ConsoleColor.Gray, ConsoleColor.Black, 17, 8)
        Sprite(Line2, ConsoleColor.Gray, ConsoleColor.Black, 17, 10)
        Sprite(Line3, ConsoleColor.Gray, ConsoleColor.Black, 17, 11)
        Sprite(line4, ConsoleColor.Gray, ConsoleColor.Black, 17, 13)

        SetPos(0, 18)
        Color(ConsoleColor.DarkGray, ConsoleColor.Black)
        CenterText("Press a key to continue execution")

        Pause()
    End Sub

End Module
