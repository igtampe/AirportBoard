Imports System.IO
Imports Igtampe.BasicGraphics
Imports Igtampe.BasicRender.RenderUtils

Public Class AirportBoard

    '------------------------------------[Variables/Properties]------------------------------------

    ''' <summary>All the available parsers to this board</summary>
    Public AllParsers As List(Of IABParser)

    ''' <summary>Currentline *should* probably be a variable set only in Run, but it makes it harder to access it for the surrounding try-catch so that users can debug their own ABScript files. 
    ''' So we need to keep this here. </summary>
    Public CurrentLine As Integer

    ''' <summary>Current page number</summary>
    Public CurrentPage As Integer

    ''' <summary>Current Page Contents (Used to resume execution in the event of a break)</summary>
    Public CurrentPageContents As String()

    ''' <summary>Tick ABScript that's held in memory</summary>
    Public TickAB As String()

    ''' <summary>Indicates whether this board is tickable</summary>
    Public Tickable As Boolean = False

    ''' <summary>Optionally loadable ticker text</summary>
    Public BoardTicker As Ticker

    '------------------------------------[Constructors]------------------------------------

    Public Sub New()
        AllParsers = New List(Of IABParser)
    End Sub

    '------------------------------------[Functions]------------------------------------

    ''' <summary>Executes the board</summary>
    ''' <param name="Args">Args from the console. Used if you need to pass/render a preview AB or DF file</param>
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
        PreFirstPage()

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

    ''' <summary>Runs before executing Page0 for the first time</summary>
    Public Overridable Sub PreFirstPage()
        'nothing
        'This is used by LandingPad, and potentially by other programs.
    End Sub

    ''' <summary>Returns the contents of a file as an array</summary>
    Public Shared Function GetFileContents(File As String) As String()
        FileOpen(1, File, OpenMode.Input)
        Dim PageContents() As String = Nothing
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
        CurrentPageContents = GetFileContents(File)
        Run(CurrentPageContents)
    End Sub

    ''' <summary>Run an array of ABScript Commands</summary>
    Protected Sub Run(PageContents() As String, Optional maxline As Integer = -1)

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
            If Not Parsed Then GuruMeditationError.RenderGuruMeditationError("Could not interpret line " & CurrentLine, Line.Split(" ")(0), "", "")

            'This is used by re-render
            If Not maxline = -1 Then If CurrentLine = maxline Then Return

        Next


    End Sub

    ''' <summary>Sleep call for AirportBoard. Keeps any elements that need to tick ticking, and checks for pressed keys to process</summary>
    ''' <param name="time">Time in Milliseconds</param>
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

    ''' <summary>Processes Key Inputs</summary>
    ''' <param name="pressedkey">The key that was pressed</param>
    ''' <returns>Returns true if the board should stop sleeping</returns>
    Public Overridable Function ProcessKeyInput(pressedkey As ConsoleKeyInfo) As Boolean
        Return pressedkey.Key = ConsoleKey.Escape
    End Function

End Class
