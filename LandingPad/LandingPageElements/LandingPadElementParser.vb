Public Class LandingPadElementParser
    Implements IABParser

    Public Function Parse(Line As String) As Boolean Implements IABParser.Parse

        Dim Upperline As String = Line.ToUpper
        Dim CurrentCommand() As String

        'This will eventually hold the real world weather page but for now no.
        Return False


    End Function
End Class
