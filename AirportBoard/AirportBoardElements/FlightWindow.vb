Imports System.IO
Imports Igtampe.BasicRender.Draw
Imports Igtampe.BasicRender.RenderUtils
Imports Igtampe.BasicGraphics

''' <summary>Holds and renders a flightwindow</summary>
Public Class FlightWindow

    ''' <summary>Holds a flight</summary>
    Private Structure Flight
        Public ReadOnly AirlineColor As String
        Public ReadOnly Airline As String
        Public ReadOnly FlightNumber As String
        Public ReadOnly Gate As String
        Public ReadOnly Time As String
        Public ReadOnly Status As Integer
        Public ReadOnly Destination As String

        Public Sub New(AirlineColor As String, Airline As String, FlightNumber As String, Gate As String, Time As String, Status As String, Destination As String)
            Me.AirlineColor = AirlineColor
            Me.Airline = Airline
            Me.FlightNumber = FlightNumber
            Me.Gate = Gate
            Me.Time = Time
            Me.Status = Status
            Me.Destination = Destination
        End Sub

    End Structure

    ''' <summary>Holds a terminal with a collection of flights</summary>
    Private Structure Terminal
        Public ReadOnly TerminalLetter As Char
        Public ReadOnly Flights As ArrayList

        Public Sub New(TerminalLetter As Char)
            Me.TerminalLetter = TerminalLetter
            Flights = New ArrayList
        End Sub
    End Structure

    Private ReadOnly AllTerminals As ArrayList
    Private ReadOnly DepartureMode As Boolean

    Public Sub New(Filename As String, Departuremode As Boolean)
        Me.DepartureMode = Departuremode

        'handles FileNotFound
        If Not File.Exists(Filename) Then
            Sprite("[ ERROR ]", ConsoleColor.Red, ConsoleColor.Black)
            Sprite(" File " & Filename & " was not found.", ConsoleColor.Black, ConsoleColor.Red)
            Exit Sub
        End If

        'Load the FlightWindow file
        FileOpen(1, Filename, OpenMode.Input)

        Dim CurrentTerminal As Terminal
        AllTerminals = New ArrayList

        While Not EOF(1)
            Dim Temp As String = LineInput(1)
            If Temp.StartsWith("-") Then
                'new terminal
                CurrentTerminal = New Terminal(Temp.Remove(0, 2))
                AllTerminals.Add(CurrentTerminal)
            End If
            If Temp.StartsWith("~") Then
                Dim TempSplit As String() = Temp.Remove(0, 1).Split("~")
                Dim NewFlight As Flight = New Flight(TempSplit(0), TempSplit(1), TempSplit(2), TempSplit(3), TempSplit(4), TempSplit(5), TempSplit(6))
                CurrentTerminal.Flights.Add(NewFlight)
            End If
        End While

        FileClose(1)

    End Sub

    ''' <summary>Draws the FlightWindow table</summary>
    Private Sub DrawTable()
        Row(ConsoleColor.Black, 80, 0, 2)
        Box(ConsoleColor.Black, 78, 19, 1, 4)

        'Table header
        SetPos(1, 4)
        Color(ConsoleColor.White, ConsoleColor.Black)
        If DepartureMode Then
            Echo("--FLIGHT NUMBER--|-GATE-|---TIME---|--STATUS--|-DESTINATION-------------------".Replace("-", " "))
        Else
            Echo("--FLIGHT NUMBER--|-GATE-|---TIME---|--STATUS--|-ORIGIN------------------------".Replace("-", " "))
        End If

        'Draw the darkgray lines
        For Doot = 6 To 22 Step 2
            Row(ConsoleColor.DarkGray, 78, 1, Doot)
        Next
    End Sub


    ''' <summary>Renders the flightwindow</summary>
    Public Sub Render()

        If IsNothing(AllTerminals) Then Return

        Dim StatusColor As ConsoleColor
        Dim StatusString As String
        Dim backgroundcolor As ConsoleColor
        For Each Terminal As Terminal In AllTerminals

            'Draw the table
            DrawTable()

            'Draw header
            Color(ConsoleColor.Black, ConsoleColor.White)
            SetPos(0, 2)
            CenterText("Terminal " & Terminal.TerminalLetter)


            Dim linecounter As Integer = 5

            For Each Flight As Flight In Terminal.Flights

                'Handles when we run out of space
                If linecounter = 23 Then
                    ABSleep(20000)

                    'Reset the linecounter
                    linecounter = 5

                    'Redraw the table
                    DrawTable()
                End If

                'Line background color
                If linecounter Mod 2 = 0 Then backgroundcolor = ConsoleColor.DarkGray Else backgroundcolor = ConsoleColor.Black

                'Draw most things
                Sprite(Flight.Airline, backgroundcolor, GraphicUtils.ColorCharToConsoleColor(Flight.AirlineColor), 1, linecounter)
                Sprite(Flight.FlightNumber, backgroundcolor, GraphicUtils.ColorCharToConsoleColor(Flight.AirlineColor), 14, linecounter)
                Sprite(Flight.Gate, backgroundcolor, ConsoleColor.White, 21, linecounter)
                Sprite(Flight.Time, backgroundcolor, ConsoleColor.White, 27, linecounter)

                'Status tree
                Select Case Flight.Status
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
                        If DepartureMode Then StatusString = "DEPARTED" Else StatusString = "ARRIVED"
                        StatusColor = ConsoleColor.DarkBlue
                    Case Else
                        StatusString = "UNKNOWN"
                        StatusColor = ConsoleColor.Gray
                End Select

                'Draw the status string and destination
                Sprite(StatusString, backgroundcolor, StatusColor, 38, linecounter)
                Sprite(Flight.Destination, backgroundcolor, ConsoleColor.White, 49, linecounter)

                'Table boundaries
                Sprite("|", backgroundcolor, ConsoleColor.White, 18, linecounter)
                Sprite("|", backgroundcolor, ConsoleColor.White, 25, linecounter)
                Sprite("|", backgroundcolor, ConsoleColor.White, 36, linecounter)
                Sprite("|", backgroundcolor, ConsoleColor.White, 47, linecounter)

                linecounter += 1


            Next

            'Fill the last few empty entries
            While Not linecounter = 23
                If linecounter Mod 2 = 0 Then backgroundcolor = ConsoleColor.DarkGray Else backgroundcolor = ConsoleColor.Black
                Sprite("|", backgroundcolor, ConsoleColor.White, 18, linecounter)
                Sprite("|", backgroundcolor, ConsoleColor.White, 25, linecounter)
                Sprite("|", backgroundcolor, ConsoleColor.White, 36, linecounter)
                Sprite("|", backgroundcolor, ConsoleColor.White, 47, linecounter)
                linecounter += 1
            End While

            'Sleep then move on to the next terminal
            ABSleep(20000)
        Next



    End Sub


End Class
