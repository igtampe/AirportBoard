Imports Igtampe.BasicRender.RenderUtils

''' <summary>Holds the main processes of AirportBoard</summary>
Module Main

    ''' <summary>Creates and starts the AirportBoard</summary>
    Public Sub Main()

        'Set color
        Color(ConsoleColor.Black, ConsoleColor.Green)

        'Set window size, clear, and set the title
        Console.SetWindowSize(80, 25)
        Console.SetBufferSize(80, 25)
        Console.Clear()
        Console.Title = "AirportBoard [Version 3.0]"

        Dim Mainboard As AirportBoard = New AirportBoard()

        Mainboard.AllParsers.Add(New CoreParser(Mainboard))
        Mainboard.AllParsers.Add(New AirportElementParser(Mainboard))

        Try
            Mainboard.Execute(Environment.GetCommandLineArgs())
        Catch ex As Exception
            LoadErrorPage(ex, Mainboard.CurrentPage, Mainboard.CurrentLine)
        End Try

    End Sub

End Module
