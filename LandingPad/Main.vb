Imports System.IO
Imports Igtampe.BasicGraphics
Imports Igtampe.BasicRender.Draw
Imports Igtampe.BasicRender.RenderUtils
Imports AirportBoard.ErrorWindow
Imports AirportBoard

''' <summary>Holds the main processes of AirportBoard</summary>
Module Main

    '----------------------------------------------------[Variables]----------------------------------------------------
    ''' <summary>Holds the current page</summary>
    Private currentpage As String()

    ''' <summary>Holds current line so that it is possible to continue execution</summary>
    Private CurrentLine As Integer

    ''' <summary>Holds the tick.ab page in memory</summary>
    Private TickAB As String()
    Private Tickable As Boolean = False

    ''' <summary>Holds the main.ab page in memory</summary>
    Private MainAB As String()

    ''' <summary>Holds the pre-action AB in memory</summary>
    Private PreActionAB As String()

    ''' <summary>Holds all available actions in memory</summary>
    Private AllActions As ArrayList

    ''' <summary>Holds the ticker</summary>
    Private BoardTicker As Ticker

    Private ConsoleEnabled As Boolean = True

    Private AllExtensions As IABParser()

    '----------------------------------------------------[Initialization]----------------------------------------------------

    Public Sub init()
        AllExtensions = {New LandingPadElementParser, New LandingPadElementParser}
    End Sub

    ''' <summary>Starts the AirportBoard</summary>
    Public Sub Main()

        'Set color
        Color(ConsoleColor.Black, ConsoleColor.Green)

        'Set window size, clear, and set the title
        Console.SetWindowSize(80, 24)
        Console.SetBufferSize(80, 24)
        Console.Clear()
        Console.Title = "LandingPad [Version 1.0] (Based on AirportBoard [Version 2.0])"

        'LandingPad is a little smaller, as the SSH window we have is just one line smaller
        'I didn't realize that when I made AirportBoard. Oopsie.

        'We also need a lot more files
        If Not File.Exists("Tick.ab") Or Not File.Exists("Main.ab") Or Not File.Exists("Options.txt") Or Not File.Exists("PreAction.ab") Or Not File.Exists("About.ab") Then
            Dim Window As ErrorWindow = New ErrorWindow("Missing Critical Files! Cannot continue")
            Window.Execute()
            Return
        Else
            Tickable = True
            TickAB = GetFileContents("Tick.ab")
            MainAB = GetFileContents("Main.ab")
            PreActionAB = GetFileContents("PreAction.AB")

            Dim TempOptions As String() = GetFileContents("Options.txt")
            AllActions = New ArrayList(TempOptions.Count)
            Try
                For Each Line As String In TempOptions
                    AllActions.Add(New LandingPageOption(Line.Split("~")(0), Line.Split("~")(1)))
                Next
            Catch ex As Exception
                GuruMeditationError("An error occurred while processing your options", ex.Message, "", "")
            End Try

        End If

        If Not File.Exists("ConsolePass.txt") Then
            ConsoleEnabled = False
            Dim Window As ErrorWindow = New ErrorWindow("ConsolePass.txt wasn't found, Console disabled")
            Window.Execute()
        End If

        If File.Exists("init.ab") Then
            Run("init.ab")
        End If

        Dim Args() As String = Environment.GetCommandLineArgs()
        If Args.Count = 2 Then
            Console.Title &= " " & Args(1)

            If Args(1).ToUpper.EndsWith(".AB") Then Run(Args(1)) 'Run an AB file for a preview
            If Args(1).ToUpper.EndsWith(".DF") Then
                'Draw a file as a preview
                Dim Drawfile As Graphic = New BasicGraphicFromFile(Args(1))
                Drawfile.Draw(0, 0)
            End If

            Exit Sub
        End If

        'This call is now made by INIT, so if anyone wants to run the screentest, they can.
        'ScreenTest()
        MainMenu()
    End Sub

    '----------------------------------------------------[Special Pages]----------------------------------------------------

    Private Sub ScreenTest()

        'Test CenterText
        SetPos(0, 1)
        CenterText("SYSTEM TEST")

        'Test Box
        Box(ConsoleColor.Gray, 70, 3, 5, 3)

        'Test Draw
        SetPos(6, 4)
        BasicGraphic.DrawColorString("0123456789ABCDEF")

        'Test Sprite
        Sprite(" Hello!", ConsoleColor.Gray, ConsoleColor.Black)

        'Test DrawFromFile

        Dim DootDF As Graphic = New BasicGraphicFromResource(My.Resources.Doot)
        DootDF.Draw(0, 8)

        'Test HiColorDraw
        SetPos(23, 22)
        HiColorGraphic.HiColorDraw("010-011-012-112-120-121-122-222-230-231-232-330-340-341-342-440-450-451-452-550-560-561-562-660-670-671-672-770-780-781-782-880-800-801-802-000")

        'Sleep to make sure we can see this for at least a moment
        SetPos(0, 23)
        Sleep(1000)
    End Sub

    ''' <summary>Renders About.df and a few suplemental cositas</summary>
    Public Sub AboutPage()
        Dim oldcurrentpage As String() = currentpage
        Dim oldcurrentline As Integer = CurrentLine
        Run("About.ab")
        currentpage = oldcurrentpage
        CurrentLine = oldcurrentline

        SetPos(0, 16)
        CenterText("LandingPad V 1.0")

        SetPos(23, 18)
        HiColorGraphic.HiColorDraw("010-011-012-112-120-121-122-222-230-231-232-330-340-341-342-440-450-451-452-550-560-561-562-660-670-671-672-770-780-781-782-880-800-801-802-000")
        SetPos(23, 19)
        HiColorGraphic.HiColorDraw("090-091-092-992-120-9A1-9A2-AA2-AB0-AB1-AB2-BB0-BC0-BC1-BC2-CC0-CD0-CD1-CD2-DD0-DE0-DE1-DE2-EE0-EF0-EF1-EF2-FF0-F70-F71-F72-770-700-701-702-000")

        SetPos(0, 21)
        CenterText("Based on Airportboard 2.0")

        SetPos(0, 22)
        CenterText("A program by Igtampe, 2020")

        Pause()
    End Sub

    ''' <summary>Challenges the user with a password and then gives them access to the console if they pass</summary>
    Public Sub ConsoleAccess()
        'Extra security check
        If Not ConsoleEnabled Then Return

        Color(ConsoleColor.Black, ConsoleColor.White)
        Console.Clear()
        Echo("Please enter your remote access code", True)
        Echo(":")

        Dim pressedkey As ConsoleKeyInfo
        Dim pinattempt As String = ""

        'wow look at me doing a do while instead of a while do que lindo
        Do
            pressedkey = Console.ReadKey(True)
            If Char.IsLetterOrDigit(pressedkey.KeyChar) Then
                'add it
                pinattempt &= pressedkey.KeyChar
                Echo("*")
            ElseIf pressedkey.Key = ConsoleKey.Backspace And Not pinattempt.Length = 0 Then
                SetPos(Console.CursorLeft - 1, Console.CursorTop)
                Echo(" ")
                SetPos(Console.CursorLeft - 1, Console.CursorTop)
                pinattempt = pinattempt.Remove(pinattempt.Length - 1)
            End If
        Loop While Not pressedkey.Key = ConsoleKey.Enter

        Dim Pin As String = GetFileContents("ConsolePass.txt")(0)

        If Pin = pinattempt Then
            Pin = ""
            Console.Clear()
            Color(ConsoleColor.Black, ConsoleColor.Gray)
            StartProcessInLine("CMD")
        End If

        Pin = ""

    End Sub

    '----------------------------------------------------[Main Operations]----------------------------------------------------

    ''' <summary>Handle     s the main operations</summary>
    Private Sub MainMenu()

        'Clear the console from the last page (which should be the screen test)
        Console.Clear()

        'If there's no files, make sure we tell the user there's no files!
        If Not File.Exists("Page0.AB") Then
            Dim Window As ErrorWindow = New ErrorWindow("No pages found!")
            Window.Execute()
            Exit Sub
        End If

        Dim CurrentPage As Integer = 0

        Run(MainAB)

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
    Public Sub Run(File As String, Optional maxline As Integer = -1)
        'Get page contents and run the page contents
        currentpage = GetFileContents(File)
        Run(currentpage, maxline)
    End Sub

    ''' <summary>Run an array of ABScript Commands</summary>
    Private Sub Run(PageContents() As String, Optional maxline As Integer = -1)

        Dim CurrentCommand() As String
        Dim Temp As String
        CurrentLine = -1

        'Handles empty pages
        If IsNothing(PageContents) Then Exit Sub

        For Each Line As String In PageContents
            CurrentLine += 1
            Dim Parsed As Boolean = False

            Parsed = Parse(Line)

            'If it's not parsed, try with the extensions
            If Not Parsed Then
                For Each Parser As IABParser In AllExtensions
                    Parsed = Parser.Parse(Line)
                    If Parsed Then Exit For
                Next
            End If

            'If it's still not parsed, then it's unparsable
            If Not Parsed Then GuruMeditationError("Could not interpret line " & CurrentLine, Line.Split(" ")(0), "", "")

            'This is used by re-render
            If Not maxline = -1 Then If CurrentLine = maxline Then Return

        Next

    End Sub

    ''' <summary>Processes keyInput for ABSleep, and returns true if ABSleep needs to quit (IE because it processed a key)</summary>
    Public Function ProcessKeyInput() As Boolean

        Dim pressedkey = Console.ReadKey(True)
        Select Case pressedkey.Key
            Case ConsoleKey.Escape
                'Escape, skip ABSleep
                Return True
            Case ConsoleKey.A
                'show about
                AboutPage()
                ReRender()
                Return True
            Case ConsoleKey.D
                'exit
                Run(PreActionAB)
                CenterText("G O O D B Y E")
                Sleep(2000) 'This sleep is ok because we don't need to call absleep to tick stuff now.
                Environment.Exit(0)
            Case ConsoleKey.Oem3
                'tilde, activate console
                If ConsoleEnabled Then
                    ConsoleAccess()
                    ReRender()
                    Return True
                End If
            Case Else
                For Each Opt As LandingPageOption In AllActions
                    If pressedkey.KeyChar = Opt.Keychar Then
                        Run(PreActionAB)
                        Try
                            StartProcessInLine(Opt.Command)
                        Catch ex As Exception
                            GuruMeditationError("An error occurred while launching the process:", Opt.Command, ex.Message, "")
                        End Try
                        ReRender()
                        Return True
                    End If
                Next
        End Select

        Return False

    End Function

    ''' <summary>Sleep call for AirportBoard. Keeps any elements that need to tick ticking</summary>
    ''' <param name="time"></param>
    Public Sub ABSleep(time As Integer)

        Dim Steppy As Integer = 250

        If time < Steppy Or Not Tickable Then
            Sleep(time)
            Return
        End If

        For X = 0 To time Step Steppy

            If Console.KeyAvailable Then
                If ProcessKeyInput() Then Return
            End If

            Run(TickAB)
            Sleep(Math.Min(time - X, Steppy))
        Next

    End Sub

    ''' <summary>Starts the specified process in line with this console</summary>
    Public Sub StartProcessInLine(Command As String)
        'Create a process and create its startinfo which needs to specify that we're not using shell execute it
        Dim PSI As Process = New Process With {.StartInfo = New ProcessStartInfo(Command) With {.UseShellExecute = False}}

        'Start the process and wait for it to close
        PSI.Start()
        PSI.WaitForExit()
    End Sub

    ''' <summary>Rerenders the current page after something was done that would make it have to rerender</summary>
    Public Sub ReRender()
        'Rerender 
        Run(MainAB)
        Run(currentpage, CurrentLine)
    End Sub

    '----------------------------------------------------[Rendering Smaller Items]----------------------------------------------------

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

    '----------------------------------------------------[Error Handling]----------------------------------------------------

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

    '----------------------------------------------------[Core Parser]----------------------------------------------------

    ''' <summary>Parses critical funcitons, along with empty lines and comments</summary>
    Private Function Parse(Line As String) As Boolean
        Dim CurrentCommand() As String
        Dim Temp As String


        'ToUpper it
        Dim UpperLine As String = Line.ToUpper

        If String.IsNullOrWhiteSpace(Line) Then
            'do nothing
            Return True

        ElseIf UpperLine.StartsWith("'") Then
            'Comment, do nothing
            Return True

        ElseIf UpperLine.StartsWith("DRAW") Then
            'Draw from file (DRAW FILE LEFT TOP)
            CurrentCommand = Line.Split(" ")
            Dim Drawfile As Graphic = New BasicGraphicFromFile(CurrentCommand(1))
            Drawfile.Draw(CurrentCommand(2), CurrentCommand(3))
            Return True

        ElseIf UpperLine.StartsWith("CLEAR") Then
            'Clear the screen (CLEAR)
            Console.Clear()
            Return True

        ElseIf UpperLine.StartsWith("COLOR") Then
            'Set Screenwriter color (COLOR 0F)
            Temp = UpperLine.Replace("COLOR ", "") 'Temp holds the color string (0F)
            Color(GraphicUtils.ColorCharToConsoleColor(Temp(0)), GraphicUtils.ColorCharToConsoleColor(Temp(1)))
            Return True

        ElseIf UpperLine.StartsWith("TEXT") Then
            'Draw text (TEXT~the text~0F~LEFT~TOP
            CurrentCommand = Line.Split("~")
            Temp = CurrentCommand(2) 'Temp holds a color string (0F)
            Sprite(CurrentCommand(1), GraphicUtils.ColorCharToConsoleColor(Temp(0)), GraphicUtils.ColorCharToConsoleColor(Temp(1)), CurrentCommand(3), CurrentCommand(4))
            Return True

        ElseIf UpperLine.StartsWith("RUN") Then
            'Run another ABScript file (RUN Page0.AB)
            Temp = UpperLine.Replace("RUN ", "")
            Dim oldcurrentpage As String() = currentpage
            Dim oldcurrentline As Integer = CurrentLine
            Run(Temp)
            currentpage = oldcurrentpage
            CurrentLine = oldcurrentline
            Return True

        ElseIf UpperLine.StartsWith("SLEEP") Then
            'Wait for a specified number of milliseconds (SLEEP 100)
            Temp = UpperLine.Replace("SLEEP ", "")
            ABSleep(Temp)
            Return True

        ElseIf UpperLine.StartsWith("PAUSE") Then
            'Wait for user to hit a key to continue
            Pause()
            Return True

        ElseIf UpperLine.StartsWith("BOX") Then
            'Draws a box (BOX F LENGTH HEIGHT LEFT TOP)
            CurrentCommand = Line.Split(" ")
            Box(GraphicUtils.ColorCharToConsoleColor(CurrentCommand(1).ToString), CurrentCommand(2), CurrentCommand(3), CurrentCommand(4), CurrentCommand(5))
            Return True

        ElseIf UpperLine.StartsWith("CLOCK") Then
            'Draws a clock at the specified position (CLOCK 0F LEFT TOP)
            CurrentCommand = Line.Split(" ")
            Temp = CurrentCommand(1) 'Temp holds a colorstring
            Clock(GraphicUtils.ColorCharToConsoleColor(Temp(0).ToString), GraphicUtils.ColorCharToConsoleColor(Temp(1).ToString), CurrentCommand(2), CurrentCommand(3))
            Return True

        ElseIf UpperLine.StartsWith("DATE") Then
            'Draws a date at the specified position (DATE 0F LEFT TOP)
            CurrentCommand = Line.Split(" ")
            Temp = CurrentCommand(1) 'Temp Holds a colorstring
            DateRender(GraphicUtils.ColorCharToConsoleColor(Temp(0).ToString), GraphicUtils.ColorCharToConsoleColor(Temp(1).ToString), CurrentCommand(2), CurrentCommand(3))
            Return True

        ElseIf UpperLine.StartsWith("CENTERTEXT") Then
            'Centers text on screen (Centertext~text~row)
            CurrentCommand = Line.Split("~")
            SetPos(0, CurrentCommand(2))
            CenterText(CurrentCommand(1))
            Return True

        ElseIf UpperLine.StartsWith("INITTICKER") Then
            'Initialize the ticker
            CurrentCommand = Line.Split(" ")
            BoardTicker = New Ticker(CurrentCommand(1))
            Return True

        ElseIf UpperLine.StartsWith("TICKER") Then
            'Draws the initialized ticker with specified colors, at specified position, with specified lenght
            '(TICKER Colorstring Length leftpos toppos)
            CurrentCommand = Line.Split(" ")
            Temp = CurrentCommand(1) 'Temp Holds a colorstring
            RenderTicker(GraphicUtils.ColorCharToConsoleColor(Temp(0).ToString), GraphicUtils.ColorCharToConsoleColor(Temp(1).ToString), CurrentCommand(2), CurrentCommand(3), CurrentCommand(4))
            Return True

        ElseIf UpperLine.StartsWith("SCREENTEST") Then
            ScreenTest()
            Console.Clear()
            Return True
        End If

        Return False

    End Function


End Module
