Public Class LandingPageOption

    Public ReadOnly Command As String
    Public ReadOnly Keychar As Char

    Public Sub New(Keychar As String, Command As String)
        Me.Keychar = Keychar(0)
        Me.Command = Command
    End Sub

End Class
