''' <summary>Holds the parsers for WeatherWindow, NewsWindow, and FlightWindow </summary>
Public Class AirportElementParser
    Implements IABParser

    '------------------------------------[Variables/Properties]------------------------------------

    ''' <summary>MainBoard that will be used to ABSleep</summary>
    Private MainBoard As AirportBoard

    '------------------------------------[Constructor]------------------------------------

    ''' <summary>Creates a new Airport Element Parser</summary>
    ''' <param name="MainBoard">Mainboard that will be used to ABSleep</param>
    Public Sub New(ByRef MainBoard As AirportBoard)
        Me.MainBoard = MainBoard
    End Sub

    '------------------------------------[Functions]------------------------------------

    Public Function Parse(Line As String) As Boolean Implements IABParser.Parse

        Dim Upperline As String = Line.ToUpper
        Dim CurrentCommand() As String

        If Upperline.StartsWith("WEATHERWINDOW") Then
            'Draws a WeatherWindow using a WeatherWindow File (WeatherWindow Filename Length Height leftpos Toppos)
            CurrentCommand = Line.Split(" ")
            Dim WW As WeatherWindow = New WeatherWindow(CurrentCommand(1), CurrentCommand(2), CurrentCommand(3), CurrentCommand(4), CurrentCommand(5))
            WW.Render(MainBoard)
            Return True

        ElseIf Upperline.StartsWith("NEWSWINDOW") Then
            'Draws a NewsWindow using a NewsWindow File (NEWSWIDNDOW File)
            CurrentCommand = Line.Split(" ")
            Dim NW As NewsWindow = New NewsWindow(CurrentCommand(1))
            NW.Render(MainBoard)
            Return True

        ElseIf Upperline.StartsWith("FLIGHTWINDOW") Then
            'Draws a FlightWindow using a Flightwindow file (FlightWindow, DepartureMode)
            CurrentCommand = Line.Split(" ")
            Dim FW As FlightWindow = New FlightWindow(CurrentCommand(1), CurrentCommand(2))
            FW.Render(MainBoard)
            Return True
        End If

        Return False

    End Function
End Class
