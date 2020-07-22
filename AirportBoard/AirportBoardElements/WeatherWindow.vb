Imports System.IO
Imports Igtampe.BasicGraphics
Imports Igtampe.BasicRender.Draw
Imports Igtampe.BasicRender.RenderUtils

''' <summary>Displays Weather information in a table for up to 6 places at a time</summary>
Public Class WeatherWindow

    '------------------------------------[Structures]------------------------------------

    ''' <summary>Weather Item</summary>
    Private Structure WeatherWindowItem
        Public ReadOnly Line1 As String
        Public ReadOnly Line2 As String
        Public ReadOnly Icon As String

        Public Sub New(Line1 As String, Line2 As String, Icon As String)
            Me.Line1 = Line1
            Me.Line2 = Line2
            Me.Icon = Icon
        End Sub

    End Structure

    Private ReadOnly WeatherWindowItems As ArrayList
    Private ReadOnly Length As Integer
    Private ReadOnly Height As Integer
    Private ReadOnly Leftpos As Integer
    Private ReadOnly Toppos As Integer


    Public Sub New(Filename As String, length As Integer, height As Integer, leftpos As Integer, toppos As Integer)

        Me.Length = length
        Me.Height = height
        Me.Leftpos = leftpos
        Me.Toppos = toppos

        If Not File.Exists(Filename) Then
            Sprite("[ ERROR ]", ConsoleColor.Red, ConsoleColor.Black)
            Sprite(" File " & Filename & " was not found.", ConsoleColor.Black, ConsoleColor.Red)
            Exit Sub
        End If

        WeatherWindowItems = New ArrayList

        FileOpen(1, Filename, OpenMode.Input)
        While Not EOF(1)
            Dim LineInputSplit As String() = LineInput(1).Split("~")
            WeatherWindowItems.Add(New WeatherWindowItem(LineInputSplit(0), LineInputSplit(1), LineInputSplit(2)))
        End While
        FileClose(1)
    End Sub

    Public Sub Render(ByRef Board As AirportBoard)

        Dim CurrentRow As Integer
        Dim CurrentColumn As Integer

        Try
            Box(ConsoleColor.Gray, (Length) * 30, (Height) * 5 + (Height), Leftpos, Toppos)

        Catch
            SetPos(0, 0)
            Echo("Hubo un problemita." & vbNewLine & vbNewLine & "LENGTH: " & Length & vbNewLine & "HEIGHT: " & Height & vbNewLine & "CurrentColumn: " & CurrentColumn & vbNewLine & "CurrentRow: " & CurrentRow)
            Pause()
        End Try


        CurrentRow = 1
        CurrentColumn = 1

        For Each CurrentItem As WeatherWindowItem In WeatherWindowItems

            Dim Icon As Graphic = New BasicGraphicFromFile(CurrentItem.Icon)
            Icon.Draw(Leftpos + (30 * (CurrentColumn - 1)), Toppos + (6 * (CurrentRow - 1)))
            Sprite(CurrentItem.Line1, ConsoleColor.Gray, ConsoleColor.Black, Leftpos + (30 * (CurrentColumn - 1)) + 13, Toppos + (5 * (CurrentRow - 1)) + 1 + (1 * (CurrentRow - 1)))
            Sprite(CurrentItem.Line2, ConsoleColor.Gray, ConsoleColor.Black, Leftpos + (30 * (CurrentColumn - 1)) + 13, Toppos + (5 * (CurrentRow - 1)) + 2 + (1 * (CurrentRow - 1)))

            CurrentRow += 1

            If CurrentRow > Height Then
                CurrentRow = 1
                CurrentColumn += 1

                If CurrentColumn > Length Then
                    'we're out of space, wait, then reset it
                    Board.ABSleep(10000)
                    Box(ConsoleColor.Gray, (Length) * 30, (Height) * 5 + (Height), Leftpos, Toppos)
                    CurrentColumn = 1
                End If

            End If
        Next

        Board.ABSleep(10000)

    End Sub


End Class
