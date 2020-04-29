Imports System.IO

Module Main

    Public Structure AirportBoard
        Public PageType As Integer
        Public Arguement1 As String
        Public Arguement2 As String
    End Structure

    Public Structure NewsPage
        Public Header As String
        Public Lines() As String
        Public LineNumber As Integer
    End Structure

    Public Structure Flights
        Dim AirlineColor As String
        Dim Airline As String
        Dim FlightNumber As String
        Dim Gate As String
        Dim Time As String
        Dim Status As Integer
        Dim Destination As String
    End Structure

    Public Structure Terminals
        Dim TerminalLetter As Char
        Dim Flight() As Flights
    End Structure
    Public CurrentBoard() As AirportBoard
    Public ErrorCommand As String

    Sub Main()
        Color(ConsoleColor.Black, ConsoleColor.Green)
        Console.SetWindowSize(80, 25)
        Console.SetBufferSize(80, 25)
        Console.Clear()
        Console.Title = "AirportBoard 1.0"

        Dim Args() As String
        Args = Environment.GetCommandLineArgs()
        Console.Title = String.Join(" ", Args)
        If Args.Count = 2 Then
            If Args(1).ToUpper.EndsWith(".AB") Then
                Run(Args(1))
                Exit Sub
            ElseIf Args(1).ToUpper.EndsWith(".DF") Then
                DrawFromFile(Args(1), 0, 0)
                Exit Sub
            End If

        End If

        ScreenTest()
        MainMenu()
    End Sub

    Sub ScreenTest()
        SetPos(0, 1)
        CenterText("SYSTEM TEST")
        Box(ConsoleColor.Gray, 70, 3, 5, 3)
        SetPos(6, 4)
        Draw("0123456789ABCDEF")
        Sprite(" Hello!", ConsoleColor.Gray, ConsoleColor.Black)
        DrawFromFile("Doot.DF", 0, 8)
        SetPos(23, 22)
        HiColorDraw("010-011-012-112-120-121-122-222-230-231-232-330-340-341-342-440-450-451-452-550-560-561-562-660-670-671-672-770-780-781-782-880-800-801-802-000")
        SetPos(0, 23)
        Sleep(1000)
    End Sub

    Sub MainMenu()
        Console.Clear()
        If Not File.Exists("Page0.AB") Then
            DialogBox("No Pages Found", 3, 1, False, True)
            Exit Sub
        End If

        Dim X As Integer
        X = 0

        Do
            Try
                Run("Page" & X & ".ab")
            Catch ex As Exception
                LoadErrorPage(ex.ToString)
            End Try

            X = X + 1
            If Not File.Exists("Page" & X & ".AB") Then
                X = 0
            End If
        Loop



    End Sub

    Sub Run(File As String)
        FileOpen(1, File, OpenMode.Input)
        Dim PageContents() As String
        Dim I As Integer = 0
        While Not EOF(1)
            ReDim Preserve PageContents(I)
            PageContents(I) = LineInput(1)
            I = I + 1
        End While
        FileClose(1)

        Dim CurrentCommand() As String
        Dim Doot As String

        For I = 0 To PageContents.Count - 1
            ErrorCommand = "LINE " & I & ": " & PageContents(I)
            If PageContents(I).StartsWith("'") Then
                'do nothing
            ElseIf PageContents(I) = "" Then
                'do nothing
            ElseIf PageContents(I).ToUpper.StartsWith("DRAW") Then
                CurrentCommand = PageContents(I).Split(" ")
                DrawFromFile(CurrentCommand(1), CurrentCommand(2), CurrentCommand(3))
            ElseIf PageContents(I).ToUpper.StartsWith("CLEAR") Then
                Console.Clear()
            ElseIf PageContents(I).ToUpper.StartsWith("COLOR") Then
                Doot = PageContents(I).ToUpper.Replace("COLOR ", "")
                Color(StringToColor(Doot(0)), StringToColor(Doot(1)))
            ElseIf PageContents(I).ToUpper.StartsWith("TEXT") Then
                CurrentCommand = PageContents(I).Split("~")
                Doot = CurrentCommand(2)
                Sprite(CurrentCommand(1), StringToColor(Doot(0)), StringToColor(Doot(1)), CurrentCommand(3), CurrentCommand(4))
            ElseIf PageContents(I).ToUpper.StartsWith("RUN") Then
                Doot = PageContents(I).ToUpper.Replace("RUN ", "")
                Run(Doot)
            ElseIf PageContents(I).ToUpper.StartsWith("SLEEP") Then
                Doot = PageContents(I).ToUpper.Replace("SLEEP ", "")
                Sleep(Doot)
            ElseIf PageContents(I).ToUpper.StartsWith("PAUSE") Then
                Pause()
            ElseIf PageContents(I).ToUpper.StartsWith("BOX") Then
                CurrentCommand = PageContents(I).Split(" ")
                Box(StringToColor(CurrentCommand(1).ToString), CurrentCommand(2), CurrentCommand(3), CurrentCommand(4), CurrentCommand(5))
            ElseIf PageContents(I).ToUpper.StartsWith("CLOCK") Then
                CurrentCommand = PageContents(I).Split(" ")
                Doot = CurrentCommand(1)
                Clock(StringToColor(Doot(0).ToString), StringToColor(Doot(1).ToString), CurrentCommand(2), CurrentCommand(3))
            ElseIf PageContents(I).ToUpper.StartsWith("DATE") Then
                CurrentCommand = PageContents(I).Split(" ")
                Doot = CurrentCommand(1)
                DateRender(StringToColor(Doot(0).ToString), StringToColor(Doot(1).ToString), CurrentCommand(2), CurrentCommand(3))
            ElseIf PageContents(I).ToUpper.StartsWith("CENTERTEXT") Then
                'Centertext~text~row
                CurrentCommand = PageContents(I).Split("~")
                SetPos(0, CurrentCommand(2))
                CenterText(CurrentCommand(1))
            ElseIf PageContents(I).ToUpper.StartsWith("WEATHERWINDOW") Then
                'WeatherWindow Filename Length(in boxes) Height(in boxes) leftpos Toppos
                CurrentCommand = PageContents(I).Split(" ")
                WeatherWindow(CurrentCommand(1), CurrentCommand(2), CurrentCommand(3), CurrentCommand(4), CurrentCommand(5))
            ElseIf PageContents(I).ToUpper.StartsWith("NEWSWINDOW") Then
                CurrentCommand = PageContents(I).Split(" ")
                NewsWindow(CurrentCommand(1))
            ElseIf PageContents(I).ToUpper.StartsWith("FLIGHTWINDOW") Then
                CurrentCommand = PageContents(I).Split(" ")
                FlightWindow(CurrentCommand(1), CurrentCommand(2))
            End If

        Next


    End Sub

    Sub FlightWindow(Filename As String, Departuremode As Boolean)
        If Not File.Exists(Filename) Then
            Sprite("[ ERROR ]", ConsoleColor.Red, ConsoleColor.Black)
            Sprite(" File " & Filename & " was not found.", ConsoleColor.Black, ConsoleColor.Red)
            Exit Sub
        End If

        SetPos(0, 2)

        Row(ConsoleColor.Black, 80, 0, 2)
        Box(ConsoleColor.Black, 78, 19, 1, 4)
        SetPos(1, 4)
        Color(ConsoleColor.White, ConsoleColor.Black)
        If Departuremode Then
            Echo("--FLIGHT NUMBER--|-GATE-|---TIME---|--STATUS--|-DESTINATION-------------------".Replace("-", " "))
        Else
            Echo("--FLIGHT NUMBER--|-GATE-|---TIME---|--STATUS--|-ORIGIN------------------------".Replace("-", " "))
        End If
        Color(ConsoleColor.Black, ConsoleColor.White)


        Dim Doot As Integer
        For Doot = 6 To 22 Step 2
            Row(ConsoleColor.DarkGray, 78, 1, Doot)
        Next

        FileOpen(1, Filename, OpenMode.Input)
        Dim Terminal() As Terminals
        Dim Temp As String
        Dim TerminalCounter As Integer
        Dim FlightCounter As Integer
        TerminalCounter = -1
        FlightCounter = -1
        While Not EOF(1)
            Temp = LineInput(1)
            If Temp.StartsWith("-") Then
                'new terminal
                TerminalCounter = TerminalCounter + 1
                ReDim Preserve Terminal(TerminalCounter)
                Terminal(TerminalCounter).TerminalLetter = Temp.Remove(0, 2)
                FlightCounter = -1
            End If
            If Temp.StartsWith("~") Then
                FlightCounter = FlightCounter + 1
                ReDim Preserve Terminal(TerminalCounter).Flight(FlightCounter)
                Temp = Temp.Remove(0, 1)
                Terminal(TerminalCounter).Flight(FlightCounter).AirlineColor = Temp.Split("~")(0)
                Terminal(TerminalCounter).Flight(FlightCounter).Airline = Temp.Split("~")(1)
                Terminal(TerminalCounter).Flight(FlightCounter).FlightNumber = Temp.Split("~")(2)
                Terminal(TerminalCounter).Flight(FlightCounter).Gate = Temp.Split("~")(3)
                Terminal(TerminalCounter).Flight(FlightCounter).Time = Temp.Split("~")(4)
                Terminal(TerminalCounter).Flight(FlightCounter).Status = Temp.Split("~")(5)
                Terminal(TerminalCounter).Flight(FlightCounter).Destination = Temp.Split("~")(6)
            End If
        End While
        FileClose(1)


        Dim linecounter As Integer
        linecounter = 5
        Dim X As Integer
        Dim Y As Integer
        Dim StatusColor As ConsoleColor
        Dim StatusString As String
        Dim backgroundcolor As ConsoleColor
        For X = 0 To TerminalCounter
            SetPos(0, 2)
            CenterText("Terminal " & Terminal(X).TerminalLetter)
            For Y = 0 To Terminal(X).Flight.Count - 1

                If linecounter = 23 Then
                    Sleep(20000)
                    Run("footer.ab")
                    linecounter = 5
                    Box(ConsoleColor.Black, 78, 19, 1, 4)
                    SetPos(1, 4)
                    Color(ConsoleColor.White, ConsoleColor.Black)
                    If Departuremode Then
                        Echo("--FLIGHT NUMBER--|-GATE-|---TIME---|--STATUS--|-DESTINATION-------------------".Replace("-", " "))
                    Else
                        Echo("--FLIGHT NUMBER--|-GATE-|---TIME---|--STATUS--|-ORIGIN------------------------".Replace("-", " "))
                    End If
                    Color(ConsoleColor.Black, ConsoleColor.White)

                    For Doot = 6 To 22 Step 2
                        Row(ConsoleColor.DarkGray, 78, 1, Doot)
                    Next

                End If

                If linecounter = 6 Or linecounter = 8 Or linecounter = 10 Or linecounter = 12 Or linecounter = 14 Or linecounter = 16 Or linecounter = 18 Or linecounter = 20 Or linecounter = 22 Then
                    backgroundcolor = ConsoleColor.DarkGray
                Else
                    backgroundcolor = ConsoleColor.Black
                End If

                '      0         1         2         3         4         5         6         7        
                '      01234567890123456789012345678901234567890123456789012345678901234567890123456789
                ''Echo("  FLIGHT NUMBER  | GATE |   TIME   |  STATUS  |  DESTINATION     -------------")
                Sprite(Terminal(X).Flight(Y).Airline, backgroundcolor, StringToColor(Terminal(X).Flight(Y).AirlineColor), 1, linecounter)
                Sprite(Terminal(X).Flight(Y).FlightNumber, backgroundcolor, StringToColor(Terminal(X).Flight(Y).AirlineColor), 14, linecounter)
                Sprite(Terminal(X).Flight(Y).Gate, backgroundcolor, ConsoleColor.White, 21, linecounter)
                Sprite(Terminal(X).Flight(Y).Time, backgroundcolor, ConsoleColor.White, 27, linecounter)

                ''Echo("  FLIGHT NUMBER  | GATE |   TIME   |  STATUS  | DESTINATION      -------------")

                'Status tree
                Select Case Terminal(X).Flight(Y).Status
                    Case 0
                        StatusString = "ON  TIME"
                        StatusColor = ConsoleColor.Green
                    Case 1
                        StatusString = "BOARDING"
                        StatusColor = ConsoleColor.Cyan
                    Case 2
                        StatusString = "DELAYED"
                        StatusColor = ConsoleColor.Yellow
                    Case 3
                        StatusString = "CANCELED"
                        StatusColor = ConsoleColor.Red
                    Case 4
                        If Departuremode Then StatusString = "DEPARTED" Else StatusString = "ARRIVED"
                        StatusColor = ConsoleColor.DarkBlue
                    Case Else
                        StatusString = "UNKNOWN"
                        StatusColor = ConsoleColor.Gray
                End Select
                Sprite(StatusString, backgroundcolor, StatusColor, 38, linecounter)

                Sprite(Terminal(X).Flight(Y).Destination, backgroundcolor, ConsoleColor.White, 49, linecounter)

                Sprite("|", backgroundcolor, ConsoleColor.White, 18, linecounter)
                Sprite("|", backgroundcolor, ConsoleColor.White, 25, linecounter)
                Sprite("|", backgroundcolor, ConsoleColor.White, 36, linecounter)
                Sprite("|", backgroundcolor, ConsoleColor.White, 47, linecounter)

                linecounter = linecounter + 1


            Next


            While Not linecounter = 23
                If linecounter = 6 Or linecounter = 8 Or linecounter = 10 Or linecounter = 12 Or linecounter = 14 Or linecounter = 16 Or linecounter = 18 Or linecounter = 20 Or linecounter = 22 Then
                    backgroundcolor = ConsoleColor.DarkGray
                Else
                    backgroundcolor = ConsoleColor.Black
                End If
                Sprite("|", backgroundcolor, ConsoleColor.White, 18, linecounter)
                Sprite("|", backgroundcolor, ConsoleColor.White, 25, linecounter)
                Sprite("|", backgroundcolor, ConsoleColor.White, 36, linecounter)
                Sprite("|", backgroundcolor, ConsoleColor.White, 47, linecounter)
                linecounter = linecounter + 1
            End While
            Sleep(20000)
            Run("footer.ab")
            linecounter = 5
            Box(ConsoleColor.Black, 78, 19, 1, 4)
            SetPos(1, 4)
            Color(ConsoleColor.White, ConsoleColor.Black)
            If Departuremode Then
                Echo("--FLIGHT NUMBER--|-GATE-|---TIME---|--STATUS--|-DESTINATION-------------------".Replace("-", " "))
            Else
                Echo("--FLIGHT NUMBER--|-GATE-|---TIME---|--STATUS--|-ORIGIN------------------------".Replace("-", " "))
            End If
            Color(ConsoleColor.Black, ConsoleColor.White)

            For Doot = 6 To 22 Step 2
                Row(ConsoleColor.DarkGray, 78, 1, Doot)
            Next
        Next

    End Sub

    Sub NewsWindow(Filename As String)
        If Not File.Exists(Filename) Then
            Sprite("[ ERROR ]", ConsoleColor.Red, ConsoleColor.Black)
            Sprite(" File " & Filename & " was not found.", ConsoleColor.Black, ConsoleColor.Red)
            Exit Sub
        End If

        '4 to 23
        'like 20 lines
        Box(ConsoleColor.DarkRed, 80, 20, 0, 4)
        FileOpen(1, Filename, OpenMode.Input)
        Dim News() As NewsPage
        Dim Temp As String
        Dim NewsCounter As Integer
        Dim LineCounter As Integer
        NewsCounter = 0
        While Not EOF(1)
            ReDim Preserve News(NewsCounter)
            News(NewsCounter).Header = LineInput(1)
            LineCounter = 0
            ReDim Preserve News(NewsCounter).Lines(LineCounter)
            News(NewsCounter).Lines(LineCounter) = LineInput(1)

            While News(NewsCounter).Lines(LineCounter).Length > 79
                Temp = News(NewsCounter).Lines(LineCounter)
                News(NewsCounter).Lines(LineCounter) = News(NewsCounter).Lines(LineCounter).Remove(79)
                LineCounter = LineCounter + 1
                ReDim Preserve News(NewsCounter).Lines(LineCounter)
                News(NewsCounter).Lines(LineCounter) = Temp.Remove(0, 79)
            End While
            News(NewsCounter).LineNumber = LineCounter + 1
            If EOF(1) Then Exit While
            NewsCounter = NewsCounter + 1
            Temp = LineInput(1)

        End While
        FileClose(1)
        Dim currentLine As Integer
        Dim Y As Integer
        currentLine = 4

        For X = 0 To News.Count - 1

            If News(X).LineNumber + currentLine > 23 Then
                currentLine = 4
                Sleep(30000)
                Run("Footer.ab")
                Box(ConsoleColor.DarkRed, 80, 20, 0, 4)
            End If

            Row(ConsoleColor.Gray, 80, 0, currentLine)
            Color(ConsoleColor.Gray, ConsoleColor.Black)
            SetPos(0, currentLine)
            CenterText(News(X).Header)
            Color(ConsoleColor.DarkRed, ConsoleColor.White)
            currentLine = currentLine + 1
            For Y = 0 To News(X).Lines.Count - 1
                SetPos(0, currentLine)
                Echo(News(X).Lines(Y))
                currentLine = currentLine + 1
            Next
            currentLine = currentLine + 1
        Next
        Sleep(30000)


    End Sub

    Sub WeatherWindow(Filename As String, length As Integer, height As Integer, leftpos As Integer, toppos As Integer)
        If Not File.Exists(Filename) Then
            Sprite("[ ERROR ]", ConsoleColor.Red, ConsoleColor.Black)
            Sprite(" File " & Filename & " was not found.", ConsoleColor.Black, ConsoleColor.Red)
            Exit Sub
        End If

        Dim WeatherWindow() As String
        Dim I As Integer
        I = 0

        FileOpen(1, Filename, OpenMode.Input)
        While Not EOF(1)
            ReDim Preserve WeatherWindow(I)
            WeatherWindow(I) = LineInput(1)
            I = I + 1
        End While
        FileClose(1)

        Dim CurrentRow As Integer
        Dim CurrentColumn As Integer
        Dim DarkGray As Boolean
        DarkGray = False
        I = 0
        Try
            Box(ConsoleColor.Gray, (length) * 30, (height) * 5 + (height), leftpos, toppos)

        Catch
            SetPos(0, 0)
            Echo("Hubo un problemita." & vbNewLine & vbNewLine & "LENGTH: " & length & vbNewLine & "HEIGHT: " & height & vbNewLine & "CurrentColumn: " & CurrentColumn & vbNewLine & "CurrentRow: " & CurrentRow)
            Pause()
        End Try


        CurrentRow = 1
        CurrentColumn = 1
        Dim CurrentItem() As String

        DarkGray = False

        Do
            If I = WeatherWindow.Count Then Exit Do

            CurrentItem = WeatherWindow(I).Split("~")
            DrawFromFile(CurrentItem(2), leftpos + (30 * (CurrentColumn - 1)), toppos + (6 * (CurrentRow - 1)))
            Sprite(CurrentItem(0), ConsoleColor.Gray, ConsoleColor.Black, leftpos + (30 * (CurrentColumn - 1)) + 13, toppos + (5 * (CurrentRow - 1)) + 1 + (1 * (CurrentRow - 1)))
            Sprite(CurrentItem(1), ConsoleColor.Gray, ConsoleColor.Black, leftpos + (30 * (CurrentColumn - 1)) + 13, toppos + (5 * (CurrentRow - 1)) + 2 + (1 * (CurrentRow - 1)))

            DarkGray = True
            CurrentRow = CurrentRow + 1
            If CurrentRow > height Then
                CurrentRow = 1
                CurrentColumn = CurrentColumn + 1
                If CurrentColumn > length Then
                    Sleep(10000)
                    Box(ConsoleColor.Gray, (length) * 30, (height) * 5 + (height), leftpos, toppos)
                    CurrentColumn = 1
                End If
            End If
            I = I + 1


        Loop



        Sleep(10000)

    End Sub

    Sub Clock(backgroundcolor As ConsoleColor, foregroundcolor As ConsoleColor, leftpos As Integer, toppos As Integer)
        Sprite(DateTime.Now.ToShortTimeString, backgroundcolor, foregroundcolor, leftpos, toppos)
    End Sub

    Sub DateRender(backgroundcolor As ConsoleColor, foregroundcolor As ConsoleColor, leftpos As Integer, toppos As Integer)
        Sprite(DateTime.Now.ToShortDateString, backgroundcolor, foregroundcolor, leftpos, toppos)
    End Sub

    Sub LoadErrorPage(Doot As String)

        Sprite(vbNewLine & "An Error occurred while rendering this page:" & vbNewLine & vbNewLine & Doot & vbNewLine & vbNewLine & ErrorCommand & vbNewLine & vbNewLine & "Press a key to continue execution.", ConsoleColor.Black, ConsoleColor.Red)
        Pause()
    End Sub

End Module
