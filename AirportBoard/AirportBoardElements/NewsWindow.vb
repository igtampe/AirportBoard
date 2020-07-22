Imports System.IO
Imports Igtampe.BasicRender.Draw
Imports Igtampe.BasicRender.RenderUtils
''' <summary>Holds and renders a NewsWindow</summary>
Public Class NewsWindow

    Private Structure NewsItem
        Public ReadOnly Header As String
        Public ReadOnly Lines As ArrayList

        Public Sub New(Header As String)
            Me.Header = Header
            Lines = New ArrayList
        End Sub
    End Structure

    Private ReadOnly AllNews As ArrayList


    Public Sub New(Filename As String)

        If Not File.Exists(Filename) Then
            Sprite("[ ERROR ]", ConsoleColor.Red, ConsoleColor.Black)
            Sprite(" File " & Filename & " was not found.", ConsoleColor.Black, ConsoleColor.Red)
            Exit Sub
        End If

        AllNews = New ArrayList

        '4 to 23
        'like 20 lines
        FileOpen(1, Filename, OpenMode.Input)

        While Not EOF(1)

            'Read the header
            Dim CurrentNewsItem As NewsItem = New NewsItem(LineInput(1))

            'Read all the lines (as they're all in one string)
            Dim AllLines As String = LineInput(1)

            'Add them to the lines in 79 character chunks
            While AllLines.Length > 79
                CurrentNewsItem.Lines.Add(AllLines.Substring(0, 79))
                AllLines = AllLines.Remove(0, 79)
            End While

            'Add allLines at the end of this
            CurrentNewsItem.Lines.Add(AllLines)

            'Add the news Item
            AllNews.Add(CurrentNewsItem)

            If EOF(1) Then Exit While

            'Read the empty line
            LineInput(1)

        End While
        FileClose(1)

    End Sub

    ''' <summary>Renders a NewsWindow</summary>
    Public Sub Render(ByRef Board As AirportBoard)
        Box(ConsoleColor.DarkRed, 80, 20, 0, 4)

        Dim currentLine As Integer = 4

        For Each Item As NewsItem In AllNews

            If Item.Lines.Count + currentLine > 23 Then
                'We're out of space, wait, then redraw the box that holds news
                currentLine = 4
                Board.ABSleep(30000)

                Box(ConsoleColor.DarkRed, 80, 20, 0, 4)
            End If

            'Draw the header
            Row(ConsoleColor.Gray, 80, 0, currentLine)
            Color(ConsoleColor.Gray, ConsoleColor.Black)

            SetPos(0, currentLine)
            CenterText(Item.Header)

            Color(ConsoleColor.DarkRed, ConsoleColor.White)
            currentLine += 1

            'Draw each line
            For Each Line As String In Item.Lines
                SetPos(0, currentLine)
                Echo(Line)
                currentLine += 1
            Next
            currentLine += 1
        Next

        'Last sleep to make sure people can read the last items
        Board.ABSleep(30000)

    End Sub

End Class
