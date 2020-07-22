Public Class LandingPad
    Inherits AirportBoard.AirportBoard

    Public Sub New()
        MyBase.New()

    End Sub

    Public Overrides Sub Execute()

        'All the pre-checks

        MyBase.Execute()
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
                For Each Opt As LandingPageOption In AllActions
                    If pressedkey.KeyChar = Opt.Keychar Then
                        Run(PreActionAB)
                        Try
                            StartProcessInLine(Opt.Command)
                        Catch ex As Exception
                            GuruMeditationError("An error occurred while launching the process:", Opt.Command, ex.Message, "")
                        End Try
                        ReRender()
                        Return True
                    End If
                Next
        End Select

        Return False

        Return MyBase.ProcessKeyInput(pressedkey)
    End Function

End Class
