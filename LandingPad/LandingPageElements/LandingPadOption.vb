''' <summary>Holds a LandingPad Option that holds a command tied to a key</summary>
Public Class LandingPadOption

    ''' <summary>Command of this LandingPadOption</summary>
    Public ReadOnly Command As String

    ''' <summary>KeyChar that will trigger the command of this option</summary>
    Public ReadOnly Keychar As Char

    Public Sub New(Keychar As String, Command As String)
        Me.Keychar = Keychar(0)
        Me.Command = Command
    End Sub

End Class
