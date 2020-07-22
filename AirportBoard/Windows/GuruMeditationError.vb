Imports Igtampe.BasicRender.Draw
Imports Igtampe.BasicRender.RenderUtils

''' <summary>Holds some error handling functions</summary>
Public Module GuruMeditationError

    ''' <summary>Shows a Guru Meditation Error after saving the error's stacktrace to disk</summary>
    Public Sub LoadErrorPage(ex As Exception, PageIndex As Integer, LineIndex As Integer)

        Try
            FileOpen(1, "ABErrors.txt", OpenMode.Append)
            PrintLine(1, "=[AB ERROR]======================================================")
            PrintLine(1, ex.Message)
            PrintLine(1, ex.StackTrace)
            PrintLine(1, "=================================================================")
            FileClose(1)
        Catch ex2 As Exception
            RenderGuruMeditationError("There was an error saving the error", ex2.Message.Split(vbNewLine)(0), ex.Source, "I don't know how this could happen")
        End Try

        RenderGuruMeditationError("Error at Page " & PageIndex & " Line " & LineIndex, ex.Message.Split(vbNewLine)(0), ex.Source, "The error was written to ABErrors.txt")

    End Sub

    ''' <summary>Shows a guru meditation error</summary>
    Public Sub RenderGuruMeditationError(Line1 As String, Line2 As String, Line3 As String, line4 As String)
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
