Imports System.IO
Imports AirportBoard
Imports Igtampe.BasicRender.Draw
Imports Igtampe.BasicRender.RenderUtils
Imports Igtampe.BasicGraphics

''' <summary>LandingPad Extends AirportBoard, and while using it to display ABScript files, Also allows users to launch programs on the console</summary>
Public Class LandingPad
    Inherits AirportBoard.AirportBoard

    '----------------------------------------------------[Variables]----------------------------------------------------
    ''' <summary>Holds the main.ab page in memory</summary>
    Private MainAB As String()

    ''' <summary>Holds the pre-action AB in memory</summary>
    Private PreActionAB As String()

    ''' <summary>Holds all available actions in memory</summary>
    Private AllActions As ArrayList

    Private ConsoleEnabled As Boolean = True

    Public Sub New()
        MyBase.New()
    End Sub

    Public Overrides Sub Execute(Args() As String)

        'All the pre-checks
        If Not File.Exists("Tick.ab") Or Not File.Exists("Main.ab") Or Not File.Exists("Options.txt") Or Not File.Exists("PreAction.ab") Or Not File.Exists("About.ab") Then
            Dim Window As ErrorWindow = New ErrorWindow("Missing Critical Files! Cannot continue")
            Window.Execute()
            Return
        Else
            'Tick can be loaded during AirportBoard's initialization
            MainAB = GetFileContents("Main.ab")
            PreActionAB = GetFileContents("PreAction.AB")

            Dim TempOptions As String() = GetFileContents("Options.txt")
            AllActions = New ArrayList(TempOptions.Count)
            Try
                For Each Line As String In TempOptions
                    AllActions.Add(New LandingPadOption(Line.Split("~")(0), Line.Split("~")(1)))
                Next
            Catch ex As Exception
                GuruMeditationError.RenderGuruMeditationError("An error occurred while processing your options", ex.Message, "", "")
            End Try

        End If

        If Not File.Exists("ConsolePass.txt") Then
            ConsoleEnabled = False
            Dim Window As ErrorWindow = New ErrorWindow("ConsolePass.txt wasn't found, Console disabled")
            Window.Execute()
        End If

        MyBase.Execute(Args)
    End Sub

    Public Overrides Sub PreFirstPage()
        Run(MainAB)
        MyBase.PreFirstPage()
    End Sub

    Public Overrides Function ProcessKeyInput(pressedkey As ConsoleKeyInfo) As Boolean
        'OK Check the cosita

        Select Case pressedkey.Key
            Case ConsoleKey.Escape
                'Escape, skip ABSleep
                Return True
            Case ConsoleKey.A
                'show about
                AboutPage()
                ReRender()
                Return True
            Case ConsoleKey.D
                'exit
                Run(PreActionAB)
                CenterText("G O O D B Y E")
                Sleep(2000) 'This sleep is ok because we don't need to call absleep to tick stuff now.
                Environment.Exit(0)
            Case ConsoleKey.Oem3
                'tilde, activate console
                If ConsoleEnabled Then
                    ConsoleAccess()
                    ReRender()
                    Return True
                End If
            Case Else
                For Each Opt As LandingPadOption In AllActions
                    If pressedkey.KeyChar = Opt.Keychar Then
                        Run(PreActionAB)
                        Try
                            StartProcessInLine(Opt.Command)
                        Catch ex As Exception
                            GuruMeditationError.RenderGuruMeditationError("An error occurred while launching the process:", Opt.Command, ex.Message, "")
                        End Try
                        ReRender()
                        Return True
                    End If
                Next
        End Select

        Return False

        Return MyBase.ProcessKeyInput(pressedkey)
    End Function

    ''' <summary>Renders About.df and a few suplemental cositas</summary>
    Public Sub AboutPage()
        Dim oldcurrentpage As String() = CurrentPageContents
        Dim oldcurrentline As Integer = CurrentLine
        Run("About.ab")
        CurrentPageContents = oldcurrentpage
        CurrentLine = oldcurrentline

        SetPos(0, 16)
        CenterText("LandingPad V 1.0")

        SetPos(23, 18)
        HiColorGraphic.HiColorDraw("010-011-012-112-120-121-122-222-230-231-232-330-340-341-342-440-450-451-452-550-560-561-562-660-670-671-672-770-780-781-782-880-800-801-802-000")
        SetPos(23, 19)
        HiColorGraphic.HiColorDraw("090-091-092-992-120-9A1-9A2-AA2-AB0-AB1-AB2-BB0-BC0-BC1-BC2-CC0-CD0-CD1-CD2-DD0-DE0-DE1-DE2-EE0-EF0-EF1-EF2-FF0-F70-F71-F72-770-700-701-702-000")

        SetPos(0, 21)
        CenterText("Based on AirportBoard 3.0")

        SetPos(0, 22)
        CenterText("A program by Igtampe, 2020")

        Pause()
    End Sub

    ''' <summary>Challenges the user with a password and then gives them access to the console if they pass</summary>
    Public Sub ConsoleAccess()
        'Extra security check
        If Not ConsoleEnabled Then Return

        Color(ConsoleColor.Black, ConsoleColor.White)
        Console.Clear()
        Echo("Please enter your remote access code", True)
        Echo(":")

        Dim pressedkey As ConsoleKeyInfo
        Dim pinattempt As String = ""

        'wow look at me doing a do while instead of a while do que lindo
        Do
            pressedkey = Console.ReadKey(True)
            If Char.IsLetterOrDigit(pressedkey.KeyChar) Then
                'add it
                pinattempt &= pressedkey.KeyChar
                Echo("*")
            ElseIf pressedkey.Key = ConsoleKey.Backspace And Not pinattempt.Length = 0 Then
                SetPos(Console.CursorLeft - 1, Console.CursorTop)
                Echo(" ")
                SetPos(Console.CursorLeft - 1, Console.CursorTop)
                pinattempt = pinattempt.Remove(pinattempt.Length - 1)
            End If
        Loop While Not pressedkey.Key = ConsoleKey.Enter

        Dim Pin As String = GetFileContents("ConsolePass.txt")(0)

        If Pin = pinattempt Then
            Console.Clear()
            Color(ConsoleColor.Black, ConsoleColor.Gray)
            StartProcessInLine("CMD")
        End If

    End Sub

    ''' <summary>Starts the specified process in line with this console</summary>
    Public Shared Sub StartProcessInLine(Command As String)
        'Create a process and create its startinfo which needs to specify that we're not using shell execute it
        Dim PSI As Process = New Process With {.StartInfo = New ProcessStartInfo(Command) With {.UseShellExecute = False}}

        'Start the process and wait for it to close
        PSI.Start()
        PSI.WaitForExit()
    End Sub

    ''' <summary>Rerenders the current page after something was done that would make it have to rerender</summary>
    Public Sub ReRender()
        'Rerender 
        Run(MainAB)
        Run(CurrentPageContents, CurrentLine)
    End Sub


End Class
