Imports System.IO
Imports Igtampe.BasicGraphics
Imports Igtampe.BasicRender.RenderUtils

Public Class AirportBoard

    Public AllParsers As List(Of IABParser)

    ''' <summary>Currentline *should* probably be a variable set only in Run, but it makes it harder to access it for the surrounding try-catch so that users can debug their own ABScript files. 
    ''' So we need to keep this here. </summary>
    Public CurrentLine As Integer
    Public CurrentPage As Integer

    Public TickAB As String()
    Public Tickable As Boolean = False



    Public BoardTicker As Ticker

    Public Sub New()
        AllParsers = New List(Of IABParser)
    End Sub

    Public Overridable Sub Execute(Args() As String)

        If Not File.Exists("Tick.ab") Then
            Dim Window As ErrorWindow = New ErrorWindow("Could not find Tick.ab! Board marked as not tickable")
            Window.Execute()
        Else
            Tickable = True
            TickAB = GetFileContents("Tick.ab")
        End If

        If File.Exists("init.ab") Then
            Run("init.ab")
        End If

        If Args.Count = 2 Then
            Console.Title &= " " & Args(1)

            If Args(1).ToUpper.EndsWith(".AB") Then Run(Args(1)) 'Run an AB file for a preview
            If Args(1).ToUpper.EndsWith(".DF") Then
                'Draw a file as a preview
                Dim Drawfile As Graphic = New BasicGraphicFromFile(Args(1))
                Drawfile.Draw(0, 0)
            End If

            Exit Sub
        End If

        'Clear the console from the last page (which should be the screen test)
        Console.Clear()

        'If there's no files, make sure we tell the user there's no files!
        If Not File.Exists("Page0.AB") Then
            Dim Window As ErrorWindow = New ErrorWindow("No pages found!")
            Window.Execute()
            Exit Sub
        End If

        CurrentPage = 0

        Do
            Try
                'Run the page
                Run("Page" & CurrentPage & ".ab")
            Catch ex As Exception
                'in case there has been an error
                GuruMeditationError.LoadErrorPage(ex, CurrentPage, CurrentLine)
            End Try

            CurrentPage += 1
            If Not File.Exists("Page" & CurrentPage & ".AB") Then CurrentPage = 0
            'If the next page doesn't exist, we've already reached the end, so loop back to the first page
        Loop

    End Sub

    ''' <summary>Returns the contents of a file as an array</summary>
    Public Function GetFileContents(File As String) As String()
        FileOpen(1, File, OpenMode.Input)
        Dim PageContents() As String
        Dim I As Integer = 0
        While Not EOF(1)
            ReDim Preserve PageContents(I)
            PageContents(I) = LineInput(1)
            I += 1
        End While
        FileClose(1)
        Return PageContents
    End Function

    ''' <summary>Run an ABScript file</summary>
    Public Sub Run(File As String)
        'Get page contents and run the page contents
        Run(GetFileContents(File))
    End Sub

    ''' <summary>Run an array of ABScript Commands</summary>
    Private Sub Run(PageContents() As String, Optional maxline As Integer = -1)

        CurrentLine = -1

        'Handles empty pages
        If IsNothing(PageContents) Then Exit Sub

        For Each Line As String In PageContents
            CurrentLine += 1
            Dim Parsed As Boolean

            'Try to parse it
            For Each Parser As IABParser In AllParsers
                Parsed = Parser.Parse(Line)
                If Parsed Then Exit For
            Next

            'If it's still not parsed, then it's unparsable
            If Not Parsed Then GuruMeditationError.Render("Could not interpret line " & CurrentLine, Line.Split(" ")(0), "", "")

            'This is used by re-render
            If Not maxline = -1 Then If CurrentLine = maxline Then Return

        Next


    End Sub

    ''' <summary>Sleep call for AirportBoard. Keeps any elements that need to tick ticking</summary>
    ''' <param name="time"></param>
    Public Sub ABSleep(time As Integer)

        Dim Steppy As Integer = 250

        If time < Steppy Or Not Tickable Then
            Sleep(time)
            Return
        End If

        For X = 0 To time Step Steppy

            If Console.KeyAvailable Then
                If ProcessKeyInput(Console.ReadKey(True)) Then Return
            End If

            Run(TickAB)
            Sleep(Math.Min(time - X, Steppy))
        Next

    End Sub

    Public Overridable Function ProcessKeyInput(pressedkey As ConsoleKeyInfo) As Boolean
        Return pressedkey.Key = ConsoleKey.Escape
    End Function

End Class
