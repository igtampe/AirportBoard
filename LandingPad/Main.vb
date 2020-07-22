Imports Igtampe.BasicRender.RenderUtils
Imports AirportBoard

''' <summary>Holds the main processes of LandingPad</summary>
Module Main

    ''' <summary>Creates and starts the LandingPad</summary>
    Public Sub Main()

        'Set color
        Color(ConsoleColor.Black, ConsoleColor.Green)

        'Set window size, clear, and set the title
        Console.SetWindowSize(80, 24)
        Console.SetBufferSize(80, 24)
        Console.Clear()
        Console.Title = "LandingPad [Version 2.0] (Based on AirportBoard [Version 3.0])"

        'LandingPad is a little smaller, as the SSH window we have is just one line smaller
        'I didn't realize that when I made AirportBoard. Oopsie.

        Dim Pad As LandingPad = New LandingPad()

        Pad.AllParsers.Add(New CoreParser(Pad))
        Pad.AllParsers.Add(New AirportElementParser(Pad)) 'Maybe disable this cosa later.
        Pad.AllParsers.Add(New LandingPadElementParser())

        Pad.Execute(Environment.GetCommandLineArgs())
    End Sub


End Module
