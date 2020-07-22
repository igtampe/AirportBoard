Imports Igtampe.BasicGraphics
Imports Igtampe.BasicRender.Draw
Imports Igtampe.BasicRender.RenderUtils

''' <summary>Parser that holds core commands for AirportBoard</summary>
Public Class CoreParser
    Implements IABParser

    '------------------------------------[Variables/Properties]------------------------------------

    Private ReadOnly MainBoard As AirportBoard

    '------------------------------------[Constructors]------------------------------------

    Public Sub New(ByRef MainBoard As AirportBoard)
        Me.MainBoard = MainBoard
    End Sub

    '------------------------------------[Functions]------------------------------------

    Public Function Parse(Line As String) As Boolean Implements IABParser.Parse
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

            Dim oldcurrentline As Integer = MainBoard.CurrentLine
            Dim oldcurrentpage As String() = MainBoard.CurrentPageContents
            MainBoard.Run(Temp)
            MainBoard.CurrentLine = oldcurrentline
            MainBoard.CurrentPageContents = oldcurrentpage
            Return True

        ElseIf UpperLine.StartsWith("SLEEP") Then
            'Wait for a specified number of milliseconds (SLEEP 100)
            Temp = UpperLine.Replace("SLEEP ", "")
            MainBoard.ABSleep(Temp)
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
            MainBoard.BoardTicker = New Ticker(CurrentCommand(1))
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

    ''' <summary>Tests the screen by running a few of the BasicRender commands</summary>
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

        'Test Draw
        Dim DootDF As Graphic = New BasicGraphicFromResource(My.Resources.Doot)
        DootDF.Draw(0, 8)

        'Test HiColorDraw
        SetPos(23, 22)
        HiColorGraphic.HiColorDraw("010-011-012-112-120-121-122-222-230-231-232-330-340-341-342-440-450-451-452-550-560-561-562-660-670-671-672-770-780-781-782-880-800-801-802-000")

        'Sleep to make sure we can see this for at least a moment
        SetPos(0, 23)
        Sleep(1000)
    End Sub

    ''' <summary>Renders the current time to screen</summary>
    Public Shared Sub Clock(backgroundcolor As ConsoleColor, foregroundcolor As ConsoleColor, leftpos As Integer, toppos As Integer)
        Sprite(DateTime.Now.ToShortTimeString, backgroundcolor, foregroundcolor, leftpos, toppos)
    End Sub

    ''' <summary>Renders the current date to screen</summary>
    Public Shared Sub DateRender(backgroundcolor As ConsoleColor, foregroundcolor As ConsoleColor, leftpos As Integer, toppos As Integer)
        Sprite(DateTime.Now.ToShortDateString, backgroundcolor, foregroundcolor, leftpos, toppos)
    End Sub

    ''' <summary>Renders the ticker</summary>
    Public Sub RenderTicker(BackgroundColor As ConsoleColor, ForegroundColor As ConsoleColor, Length As Integer, leftpos As Integer, toppos As Integer)
        If IsNothing(MainBoard.BoardTicker) Then
            GuruMeditationError.RenderGuruMeditationError("Cannot run ticker, ticker not initialized!", "", "", "")
        Else
            Sprite(MainBoard.BoardTicker.GetTicker(Length), BackgroundColor, ForegroundColor, leftpos, toppos)
        End If
    End Sub

End Class
