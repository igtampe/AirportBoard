Imports Igtampe.BasicRender.RenderUtils
Imports Igtampe.BasicGraphics

''' <summary>Holds the main processes of AirportBoard</summary>
Module Main

    ''' <summary>Starts the AirportBoard</summary>
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

        Dim Args() As String = Environment.GetCommandLineArgs()


        Mainboard.Execute(Args)

    End Sub

End Module
