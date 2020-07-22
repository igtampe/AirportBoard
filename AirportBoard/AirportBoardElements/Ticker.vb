Imports System.IO

Public Class Ticker

    '------------------------------------[Variables/Properties]------------------------------------

    Private CurrentPos As Integer
    Private ReadOnly TickerString As String

    '------------------------------------[Constructors]------------------------------------

    Public Sub New(TickerFile As String)

        'Make sure the file exists
        If Not File.Exists(TickerFile) Then
            GuruMeditationError.RenderGuruMeditationError("Could not load TickerFile", TickerFile, "Does not exist!", "")
        End If

        FileOpen(1, TickerFile, OpenMode.Input)
        TickerString = LineInput(1)
        FileClose(1)

        CurrentPos = 0
    End Sub

    '------------------------------------[Functions]------------------------------------

    ''' <summary>
    ''' Gets a ticker string of specified length<br></br>
    ''' Also increments currentpos by 1
    ''' </summary>
    Public Function GetTicker(Length As Integer) As String

        Dim ReturnTicker As String = TickerString.Substring(CurrentPos)
        While ReturnTicker.Length < Length
            ReturnTicker &= TickerString
        End While

        If ReturnTicker.Length > Length Then ReturnTicker = ReturnTicker.Remove(Length)

        CurrentPos += 1
        If CurrentPos = TickerString.Length Then CurrentPos = 0
        Return ReturnTicker
    End Function



End Class
