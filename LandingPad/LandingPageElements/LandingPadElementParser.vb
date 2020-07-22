Public Class LandingPadElementParser
    Implements IABParser

    Public Function Parse(Line As String) As Boolean Implements IABParser.Parse

        Dim Upperline As String = Line.ToUpper
        Dim CurrentCommand() As String

        If Upperline.StartsWith("RWW") Then
            'Draws a WeatherWindow using a WeatherWindow File (WeatherWindow Filename Length Height leftpos Toppos)
            CurrentCommand = Line.Split(" ")
            Dim WW As WeatherWindow = New WeatherWindow(CurrentCommand(1), CurrentCommand(2), CurrentCommand(3), CurrentCommand(4), CurrentCommand(5))
            WW.Render()

        ElseIf Upperline.StartsWith("RWP") Then
            'Draws a NewsWindow using a NewsWindow File (RWP Gridpoints(X,X) Top Color)
            CurrentCommand = Line.Split(" ")
            Dim RWP As WeatherPage = New WeatherPage(CurrentCommand(1), CurrentCommand(2), CurrentCommand(1)(0), CurrentCommand(1)(1))
            RWP.Render()

        ElseIf Upperline.StartsWith("FLIGHTWINDOW") Then
            'Draws a FlightWindow using a Flightwindow file (FlightWindow, DepartureMode)
            CurrentCommand = Line.Split(" ")
            Dim FW As FlightWindow = New FlightWindow(CurrentCommand(1), CurrentCommand(2))
            FW.Render()
        Else
            Return False
        End If


    End Function
End Class
