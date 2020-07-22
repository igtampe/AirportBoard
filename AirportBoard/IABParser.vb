Public Interface IABParser

    ''' <summary>Parses a line of an ABScript file</summary>
    ''' <param name="Line">A line of an ABScript file</param>
    ''' <returns>True if it could parse it, false otherwise</returns>
    Function Parse(Line As String) As Boolean

End Interface
