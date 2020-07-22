Imports Igtampe.BasicWindows
Imports Igtampe.BasicWindows.WindowElements


''' <summary>A very simple </summary>
Public Class ErrorWindow
    Inherits Window

    Public Sub New(Text As String)
        MyBase.New(False, True, ConsoleColor.Gray, ConsoleColor.Red, ConsoleColor.White, "Error", 46, 8)
        'First lets add each element.

        AllElements.Add(New Icon(Me, Icon.IconType.ERROR, 1, 2))
        'now lets calculate a label

        'Split the text with spaces
        Dim Words As String() = Text.Split(" ")

        Dim CurrentWord As Integer = 0
        Dim Lines As List(Of String) = New List(Of String)
        Dim LongestLine As Integer = 0

        While Lines.Count < 3

            Dim Line As String = ""
            While (Line.Length + Words(CurrentWord).Length) < 40

                'If we have a next word, And the word's length is less than 
                Line &= Words(CurrentWord) & " "
                CurrentWord += 1

                If CurrentWord > Words.Length - 1 Then Exit While

            End While

            LongestLine = Math.Max(LongestLine, Line.Length)

            'The line Is as long as its going to be.
            Lines.Add(Line)

            If CurrentWord > Words.Length - 1 Then Exit While

        End While


        AllElements.Add(New Label(Me, String.Join(Environment.NewLine, Lines.ToArray()), ConsoleColor.Gray, ConsoleColor.Black, 5, 2))

        Dim OK As CloseButton = New CloseButton(Me, "[     O K     ]", ConsoleColor.DarkGray, ConsoleColor.White, ConsoleColor.DarkBlue, Length - "[     O K     ] ".Length, Height - 2)

        AllElements.Add(OK)
        HighlightedElement = OK
        OK.Highlighted = True

    End Sub

End Class
