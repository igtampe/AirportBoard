
Imports System.IO

Public Class BasicRender

    'Configuration of BasicRender:
    Public Const WindowForegroundColor As ConsoleColor = ConsoleColor.DarkBlue
    Public Const WindowBackgroundColor As ConsoleColor = ConsoleColor.Gray
    Public Const WindowClearCOlor As ConsoleColor = ConsoleColor.Black


    ''' <summary>
    ''' Renders a colored block of whatever color you choose
    ''' </summary>
    ''' <param name="Blockcolor">
    ''' Color of the row of blocks
    ''' </param>
    ''' <param name="Leftpos">
    ''' Position of the sprite leftwise (Starts at 0, must include TOPPOS)
    ''' </param>
    ''' <param name="TopPos">
    ''' Position of the sprite topwise (Starts at 0, must include LEFTPOS)
    ''' </param>
    Public Shared Sub Block(ByVal BlockColor As ConsoleColor, Optional ByVal leftpos As Integer = -1, Optional ByVal toppos As Integer = -1)
        If Not leftpos = -1 And Not toppos = -1 Then
            Console.SetCursorPosition(leftpos, toppos)
        End If

        Dim OLDBC As ConsoleColor = Console.BackgroundColor
        Dim OLDFC As ConsoleColor = Console.ForegroundColor
        Console.BackgroundColor = BlockColor
        Console.ForegroundColor = BlockColor
        Console.Write(" ")
        Console.BackgroundColor = OLDBC
        Console.ForegroundColor = OLDFC
    End Sub

    ''' <summary>
    ''' Renders a colored sprite of whatever color you choose
    ''' </summary>
    ''' <param name="Sprite">
    ''' Character of the Sprite
    ''' </param>
    ''' <param name="Background">
    ''' Background color of the sprite
    ''' </param>
    ''' <param name="Foreground">
    ''' Foreground color of the sprite
    ''' </param>
    ''' <param name="Leftpos">
    ''' Position of the sprite leftwise (Starts at 0, must include TOPPOS)
    ''' </param>
    ''' <param name="TopPos">
    ''' Position of the sprite topwise (Starts at 0, must include LEFTPOS)
    ''' </param>
    Public Shared Sub Sprite(ByVal sprite As String, ByVal Background As ConsoleColor, ByVal Foreground As ConsoleColor, Optional ByVal leftpos As Integer = -1, Optional ByVal toppos As Integer = -1)
        If Not leftpos = -1 And Not toppos = -1 Then
            Console.SetCursorPosition(leftpos, toppos)
        End If

        Dim OLDBC As ConsoleColor = Console.BackgroundColor
        Dim OLDFC As ConsoleColor = Console.ForegroundColor
        Console.BackgroundColor = Background
        Console.ForegroundColor = Foreground
        Console.Write(sprite)
        Console.BackgroundColor = OLDBC
        Console.ForegroundColor = OLDFC

    End Sub

    ''' <summary>
    ''' Renders a box on the screen
    ''' </summary>
    ''' <param name="color">Color of the Box</param>
    ''' <param name="length">Length dimension of the box</param>
    ''' <param name="height">Height dimension of the box</param>
    ''' <param name="leftpos">Leftmost position of the box</param>
    ''' <param name="toppos">Top most coordinate of the box</param>
    Public Shared Sub Box(ByVal color As ConsoleColor, ByVal length As Integer, ByVal height As Integer, ByVal leftpos As Integer, ByVal toppos As Integer)


        Dim HeightCounter As Integer

        For HeightCounter = 0 To height - 1

            Row(color, length, leftpos, toppos + HeightCounter)

        Next






    End Sub

    ''' <summary>
    ''' Renders a colored row of blocks of whatever color you choose
    ''' </summary>
    ''' <param name="Length">
    ''' Length of the row
    ''' </param>
    ''' <param name="Rowcolor">
    ''' Color of the row of blocks
    ''' </param>
    ''' <param name="Leftpos">
    ''' Position of the sprite leftwise (Starts at 0, must include TOPPOS)
    ''' </param>
    ''' <param name="TopPos">
    ''' Position of the sprite topwise (Starts at 0, must include LEFTPOS)
    ''' </param>
    Public Shared Sub Row(ByVal RowColor As ConsoleColor, ByVal Length As Integer, Optional ByVal leftpos As Integer = -1, Optional ByVal toppos As Integer = -1)


        If Not leftpos = -1 And Not toppos = -1 Then
            Console.SetCursorPosition(leftpos, toppos)
        End If


        Dim OLDBC As ConsoleColor = Console.BackgroundColor
        Dim OLDFC As ConsoleColor = Console.ForegroundColor
        Console.BackgroundColor = RowColor
        Console.ForegroundColor = RowColor

        For x = 1 To Length
            Console.Write(" ")
        Next

        Console.BackgroundColor = OLDBC
        Console.ForegroundColor = OLDFC

    End Sub

    ''' <summary>
    ''' Clears a line
    ''' </summary>
    ''' <param name="Line">
    ''' Line that you're clearing (Line 0 is the top line)
    ''' </param>
    ''' <param name="BackgroundColor">
    ''' In the event that the background isn't black, you can set it to be whatever color you want.
    ''' </param>
    Public Shared Sub ClearLine(ByVal Line As Integer, Optional ByVal Backgroundcolor As ConsoleColor = ConsoleColor.Black)



        Dim OLDBC As ConsoleColor = Console.BackgroundColor
        Dim OLDFC As ConsoleColor = Console.ForegroundColor
        Console.BackgroundColor = Backgroundcolor
        Console.ForegroundColor = Backgroundcolor

        Console.SetCursorPosition(0, Line)

        For x = 1 To 79
            Console.Write(" ")
        Next

        Console.BackgroundColor = OLDBC
        Console.ForegroundColor = OLDFC


    End Sub

    ''' <summary>
    ''' Centers Text
    ''' </summary>
    ''' <param name="Text">Text you wish to be centered</param>
    Public Shared Sub CenterText(Text As String)
        Dim Spacing As Integer = (79 - Text.Count) / 2
        Dim OLDFC As ConsoleColor = Console.ForegroundColor
        Console.ForegroundColor = Console.BackgroundColor


        For x = 0 To Spacing
            Console.Write(" ")
        Next

        Console.ForegroundColor = OLDFC
        Console.Write(Text)




    End Sub

    ''' <summary>
    ''' Render a window on screen
    ''' </summary>
    ''' <param name="Title">Title of the Window</param>
    ''' <param name="length">Length dimension of the window</param>
    ''' <param name="height">Height dimension of the window</param>
    ''' <param name="leftpos">Leftmost position (Centered by default)</param>
    ''' <param name="toppos">Topmost position (Centered by default)</param>
    Public Shared Sub Window(ByVal Title As String, ByVal length As Integer, ByVal height As Integer, Optional ByVal shadow As Boolean = False, Optional ByVal animated As Boolean = False, Optional ByVal leftpos As Integer = -1, Optional ByVal toppos As Integer = -1)

        If toppos = -1 Then
            toppos = ((24) - height) / 2
        End If

        If leftpos = -1 Then
            leftpos = ((79) - length) / 2
        End If


        Title = " " & Title & " "

        Do
            If Title.Count >= length Then Exit Do
            Title = "═" & Title
            If Title.Count >= length Then Exit Do
            Title = Title & "═"
        Loop
        Dim bottomborder As String = "═"

        Do
            If bottomborder.Count = Title.Count Then Exit Do
            bottomborder = bottomborder & "═"
        Loop


        If shadow = True Then
            Box(ConsoleColor.Black, length, height - 1, leftpos + 2, toppos + 2)
        End If

        If animated = True Then
            Dim SmallLength As Integer = 1
            Dim SmallHeight As Integer = 1

            For SmallLength = 1 To length Step CInt(length / height)
                If Not SmallHeight = height Then
                    SmallHeight = SmallHeight + 1
                End If
                Box(WindowBackgroundColor, SmallLength, SmallHeight, leftpos, toppos)
                Sleep(5)
            Next

        End If

        Box(WindowBackgroundColor, length, height, leftpos, toppos)
        Sprite(Title, WindowForegroundColor, ConsoleColor.White, leftpos, toppos)
        Sprite(bottomborder, WindowBackgroundColor, ConsoleColor.White, leftpos, toppos + height - 1)



    End Sub

    ''' <summary>
    ''' Clears a window
    ''' </summary>
    ''' <param name="length">Length dimension of the window</param>
    ''' <param name="height">Height dimension of the window</param>
    ''' <param name="leftpos">Leftmost position (Centered by default)</param>
    ''' <param name="toppos">Topmost position (Centered by default)</param>
    Public Shared Sub RemoveWindow(ByVal length As Integer, ByVal height As Integer, Optional ByVal animated As Boolean = False, Optional ByVal leftpos As Integer = -1, Optional ByVal toppos As Integer = -1)

        If toppos = -1 Then
            toppos = ((24) - height) / 2
        End If

        If leftpos = -1 Then
            leftpos = ((79) - length) / 2
        End If


        If animated = True Then
            Dim SmallLength As Integer = 1
            Dim SmallHeight As Integer = 1

            For SmallLength = 1 To length + 2 Step CInt(length / height)
                If Not SmallHeight = height + 1 Then
                    SmallHeight = SmallHeight + 1
                End If
                Box(WindowClearCOlor, SmallLength, SmallHeight, leftpos, toppos)
                Sleep(5)
            Next

        End If


        Box(WindowClearCOlor, length + 2, height + 1, leftpos, toppos)

    End Sub



    ''' <summary>
    ''' Renders and handles an ITOS Dialog Box
    ''' </summary>
    ''' <param name="text">Text of the dialog box</param>
    ''' <param name="Icon">Icon: 1 = Information, 2 = Exclamation, 3 = critical, 4 = question </param>
    ''' <param name="Options">Options: 1= OK, 2= OK, Cancel, 3=Yes, No</param>
    ''' <param name="shadow">Does the window need a shaddow?</param>
    ''' <param name="animated">Is this window animated?</param>
    ''' <param name="leftpos">Left position of the box</param>
    ''' <param name="toppos">right position of the box</param>
    ''' <returns></returns>
    Public Shared Function DialogBox(ByVal text As String, ByVal Icon As Integer, ByVal Options As Integer, Optional ByVal shadow As Boolean = False, Optional ByVal animated As Boolean = False, Optional ByVal leftpos As Integer = -1, Optional ByVal toppos As Integer = -1) As Integer

        Dim length As Integer
        Dim height As Integer
        Dim ButtonCenterAll As Boolean
        Dim title As String
rerenderdialogbox:

        Select Case Icon
            Case 1
                length = 39
                height = 10
                ButtonCenterAll = False
                title = "Information"

            Case 2
                length = 39
                height = 10
                ButtonCenterAll = False
                title = "Warning"

            Case 3
                length = 29
                height = 7
                ButtonCenterAll = True
                title = "Critical"

            Case 5
                length = 29
                height = 7
                ButtonCenterAll = True
                title = "Notif"

            Case 4
                length = 39
                height = 10
                ButtonCenterAll = False
                title = "Question"
            Case Else
                DialogBox = -1
                Exit Function
        End Select

        If toppos = -1 Then
            toppos = ((24) - height) / 2
        End If

        If leftpos = -1 Then
            leftpos = ((79) - length) / 2
        End If


        Window(title, length, height, shadow, animated, leftpos, toppos)

        Select Case Icon
            Case 1
                Box(ConsoleColor.DarkBlue, 5, 6, leftpos + 1, toppos + 2)
                Block(ConsoleColor.Gray, leftpos + 3, toppos + 3)
                Block(ConsoleColor.Gray, leftpos + 3, toppos + 5)
                Block(ConsoleColor.Gray, leftpos + 3, toppos + 6)

                Color(ConsoleColor.Gray, ConsoleColor.Black)

                SetPos(leftpos + 8, toppos + 2)
                Console.WriteLine("Info: ")

                Dim textline1 As String
                Dim textline2 As String
                Dim textline3 As String


                SetPos(leftpos + 8, toppos + 4)
                If text.Count > 30 Then
                    textline1 = text.Remove(30)
                    textline2 = text.Remove(0, 30)
                Else
                    textline1 = text
                    textline2 = ""
                End If
                Console.Write(textline1)

                SetPos(leftpos + 8, toppos + 5)
                If textline2.Count > 30 Then
                    textline2 = textline2.Remove(29)
                    textline3 = text.Remove(0, 60)
                Else
                    textline3 = ""
                End If
                Console.Write(textline2)
                SetPos(leftpos + 8, toppos + 6)
                If textline3.Count > 30 Then
                    textline3 = textline3.Remove(29)
                End If
                Console.Write(textline3)


            Case 2
                Box(ConsoleColor.Yellow, 5, 6, leftpos + 1, toppos + 2)
                Block(ConsoleColor.Black, leftpos + 3, toppos + 3)
                Block(ConsoleColor.Black, leftpos + 3, toppos + 4)
                Block(ConsoleColor.Black, leftpos + 3, toppos + 6)

                Color(ConsoleColor.Gray, ConsoleColor.Black)

                SetPos(leftpos + 8, toppos + 2)
                Console.WriteLine("Warning: ")

                Dim textline1 As String
                Dim textline2 As String
                Dim textline3 As String


                SetPos(leftpos + 8, toppos + 4)
                If text.Count > 30 Then
                    textline1 = text.Remove(30)
                    textline2 = text.Remove(0, 30)
                Else
                    textline1 = text
                    textline2 = ""
                End If
                Console.Write(textline1)

                SetPos(leftpos + 8, toppos + 5)
                If textline2.Count > 30 Then
                    textline2 = textline2.Remove(29)
                    textline3 = text.Remove(0, 60)
                Else
                    textline3 = ""
                End If
                Console.Write(textline2)
                SetPos(leftpos + 8, toppos + 6)
                If textline3.Count > 30 Then
                    textline3 = textline3.Remove(29)
                End If
                Console.Write(textline3)

            Case 3
                Block(ConsoleColor.DarkRed, leftpos + 1, toppos + 2)
                Block(ConsoleColor.DarkRed, leftpos + 3, toppos + 2)
                Block(ConsoleColor.DarkRed, leftpos + 2, toppos + 3)
                Block(ConsoleColor.DarkRed, leftpos + 1, toppos + 4)
                Block(ConsoleColor.DarkRed, leftpos + 3, toppos + 4)

                Color(ConsoleColor.Gray, ConsoleColor.Black)



                Dim textline1 As String
                Dim textline2 As String

                SetPos(leftpos + 6, toppos + 2)
                If text.Count > 23 Then
                    textline1 = text.Remove(23)
                    textline2 = text.Remove(0, 23)
                Else
                    textline1 = text
                    textline2 = ""
                End If
                Console.Write(textline1)

                SetPos(leftpos + 6, toppos + 3)
                If textline2.Count > 30 Then
                    textline2 = textline2.Remove(29)
                End If

                Console.Write(textline2)

            Case 5
                Block(ConsoleColor.Black, leftpos + 2, toppos + 2)
                Block(ConsoleColor.Black, leftpos + 2, toppos + 4)

                Color(ConsoleColor.Gray, ConsoleColor.Black)



                Dim textline1 As String
                Dim textline2 As String

                SetPos(leftpos + 6, toppos + 2)
                If text.Count > 22 Then
                    textline1 = text.Remove(22)
                    textline2 = text.Remove(0, 22)
                Else
                    textline1 = text
                    textline2 = ""
                End If
                Console.Write(textline1)

                SetPos(leftpos + 6, toppos + 3)
                If textline2.Count > 30 Then
                    textline2 = textline2.Remove(29)
                End If

                Console.Write(textline2)


            Case 4
                Box(ConsoleColor.DarkBlue, 5, 6, leftpos + 1, toppos + 2)
                Row(ConsoleColor.Gray, 3, leftpos + 2, toppos + 3)
                Block(ConsoleColor.Gray, leftpos + 4, toppos + 4)
                Row(ConsoleColor.Gray, 2, leftpos + 3, toppos + 5)
                Block(ConsoleColor.Gray, leftpos + 3, toppos + 6)

                Color(ConsoleColor.Gray, ConsoleColor.Black)

                SetPos(leftpos + 8, toppos + 2)
                Console.WriteLine("Question: ")

                Dim textline1 As String
                Dim textline2 As String
                Dim textline3 As String


                SetPos(leftpos + 8, toppos + 4)
                If text.Count > 30 Then
                    textline1 = text.Remove(30)
                    textline2 = text.Remove(0, 30)
                Else
                    textline1 = text
                    textline2 = ""
                End If
                Console.Write(textline1)

                SetPos(leftpos + 8, toppos + 5)
                If textline2.Count > 30 Then
                    textline2 = textline2.Remove(29)
                    textline3 = text.Remove(0, 60)
                Else
                    textline3 = ""
                End If
                Console.Write(textline2)
                SetPos(leftpos + 8, toppos + 6)
                If textline3.Count > 30 Then
                    textline3 = textline3.Remove(29)
                End If
                Console.Write(textline3)

        End Select

        Select Case Icon
            Case 3
                Sprite("[ O K ]", ConsoleColor.DarkBlue, ConsoleColor.White, leftpos + 11, toppos + 5)
                Do
                    Dim PressedKey As String = (Console.ReadKey(True)).Key.ToString
                    If PressedKey = "Enter" Then Exit Do

                Loop
                RemoveWindow(29, 7, True)
            Case 5
                Sprite("[ O K ]", ConsoleColor.DarkBlue, ConsoleColor.White, leftpos + 11, toppos + 5)
                Do
                    Dim PressedKey As String = (Console.ReadKey(True)).Key.ToString
                    If PressedKey = "Enter" Then Exit Do

                Loop
                RemoveWindow(29, 7, True)

            Case Else
                Select Case Options
                    Case 1
                        Sprite("[   OK   ]", ConsoleColor.DarkBlue, ConsoleColor.White, leftpos + 8, toppos + 8)
                        Pause()
                        RemoveWindow(length, height, True)
                    Case 2
                        'OK and Cancel
                        Dim Currentposition As Integer = 1
                        Do
                            If Currentposition = 1 Then
                                Sprite("[   OK   ]", ConsoleColor.DarkBlue, ConsoleColor.White, leftpos + 8, toppos + 8)
                                Sprite("[ CANCEL ]", WindowBackgroundColor, ConsoleColor.Black, leftpos + 25, toppos + 8)

                            ElseIf Currentposition = 2 Then
                                Sprite("[   OK   ]", WindowBackgroundColor, ConsoleColor.Black, leftpos + 8, toppos + 8)
                                Sprite("[ CANCEL ]", ConsoleColor.DarkBlue, ConsoleColor.White, leftpos + 25, toppos + 8)

                            End If
                            Dim PressedKey As String = (Console.ReadKey(True)).Key.ToString
                            If PressedKey = "Enter" Then Exit Do

                            If PressedKey = "RightArrow" Then Currentposition = 2
                            If PressedKey = "LeftArrow" Then Currentposition = 1
                        Loop
                        DialogBox = Currentposition
                        RemoveWindow(39, 10, True)
                    Case 3
                        Dim Currentposition As Integer = 1
                        Do
                            If Currentposition = 1 Then
                                Sprite("[   YES   ]", ConsoleColor.DarkBlue, ConsoleColor.White, leftpos + 8, toppos + 8)
                                Sprite("[   N O   ]", WindowBackgroundColor, ConsoleColor.Black, leftpos + 24, toppos + 8)

                            ElseIf Currentposition = 2 Then
                                Sprite("[   YES   ]", ConsoleColor.Gray, ConsoleColor.Black, leftpos + 8, toppos + 8)
                                Sprite("[   N O   ]", WindowBackgroundColor, ConsoleColor.White, leftpos + 24, toppos + 8)

                            End If
                            Dim PressedKey As String = (Console.ReadKey(True)).Key.ToString
                            If PressedKey = "Enter" Then Exit Do

                            If PressedKey = "RightArrow" Then Currentposition = 2
                            If PressedKey = "LeftArrow" Then Currentposition = 1
                        Loop
                        DialogBox = Currentposition
                        RemoveWindow(39, 10, True)



                    Case Else
                        DialogBox = -1
                        Exit Function
                End Select
        End Select

    End Function

    ''' <summary>
    ''' Sleep for a specified amount of miliseconds
    ''' </summary>
    Public Shared Sub Sleep(time As Integer)
        Threading.Thread.Sleep(time)
    End Sub

    ''' <summary>
    ''' Sets the position of the cursor to whatever you want.
    ''' </summary>
    Public Shared Sub SetPos(left As Integer, top As Integer)
        Console.SetCursorPosition(left, top)
    End Sub

    ''' <summary>
    ''' Press a key to continue
    ''' </summary>
    Public Shared Sub Pause()
        Console.ReadKey(True)
    End Sub

    ''' <summary>
    ''' Change the color
    ''' </summary>
    ''' <param name="background"></param>
    ''' <param name="Foreground"></param>
    Public Shared Sub Color(background As ConsoleColor, Foreground As ConsoleColor)
        Console.BackgroundColor = background
        Console.ForegroundColor = Foreground
    End Sub

    ''' <summary>
    ''' Converts coordinates to be on a centered window
    ''' </summary>
    ''' <param name="Dimension">Pertinent dimension of the window</param>
    ''' <param name="Reference">Dimension with reference to the window</param>
    ''' <param name="resulttype">0: Width or 1: Height</param>
    ''' <returns></returns>
    Public Shared Function OnWindow(ByVal Dimension As Integer, ByVal Reference As Integer, resulttype As Integer) As Integer
        Select Case resulttype
            Case 1
                OnWindow = CInt(((24 + 25) - Dimension) / 2) + Reference
            Case 0
                OnWindow = CInt(((79 + 80) - Dimension) / 2) + Reference
            Case Else
                OnWindow = 0
        End Select
    End Function

    Public Shared Function GetTopPos(ByVal Dimension As Integer) As Integer
        GetTopPos = ((24) - Dimension) / 2
    End Function

    Public Shared Function GetLeftPos(ByVal Dimension As Integer) As Integer
        GetLeftPos = CInt((79) - Dimension) / 2
    End Function
    ''' <summary>
    ''' Draw allows you to use a string of characters to "Draw" on screen.
    ''' 
    '''0 = Black       8 = Gray
    '''1 = Blue        9 = Light Blue
    '''2 = Green       A = Light Green
    '''3 = Aqua        B = Light Aqua
    '''4 = Red         C = Light Red
    '''5 = Purple      D = Light Purple
    '''6 = Yellow      E = Light Yellow
    '''7 = White       F = Bright White
    ''' 
    ''' </summary>
    ''' <param name="ColorString"></param>
    Public Shared Sub Draw(ColorString As String)
        For X = 0 To ColorString.Count - 1
            Select Case ColorString(X).ToString.ToUpper
                Case "0"
                    Block(ConsoleColor.Black)
                Case "1"
                    Block(ConsoleColor.DarkBlue)
                Case "2"
                    Block(ConsoleColor.DarkGreen)
                Case "3"
                    Block(ConsoleColor.DarkCyan)
                Case "4"
                    Block(ConsoleColor.DarkRed)
                Case "5"
                    Block(ConsoleColor.DarkMagenta)
                Case "6"
                    Block(ConsoleColor.DarkYellow)
                Case "7"
                    Block(ConsoleColor.Gray)
                Case "8"
                    Block(ConsoleColor.DarkGray)
                Case "9"
                    Block(ConsoleColor.Blue)
                Case "A"
                    Block(ConsoleColor.Green)
                Case "B"
                    Block(ConsoleColor.Cyan)
                Case "C"
                    Block(ConsoleColor.Red)
                Case "D"
                    Block(ConsoleColor.Magenta)
                Case "E"
                    Block(ConsoleColor.Yellow)
                Case "F"
                    Block(ConsoleColor.White)
                Case Else
                    Block(Console.BackgroundColor)
            End Select
        Next
    End Sub

    Public Shared Sub HiColorDraw(HiColorString As String)
        Dim HiColorRow() As String = HiColorString.Split("-")

        'An example would be 0F1-0F2-1F3

        Dim CurrentPixel As String
        Dim CurrentPixelForeground As ConsoleColor
        Dim CurrentPixelBackground As ConsoleColor
        Dim CurrentPixelShade As Integer

        For X = 0 To HiColorRow.Count - 1
            CurrentPixel = HiColorRow(X)
            CurrentPixelBackground = StringToColor(CurrentPixel(0))
            CurrentPixelForeground = StringToColor(CurrentPixel(1))
            CurrentPixelShade = CInt(CurrentPixel(2).ToString)

            Select Case CurrentPixelShade
                Case 0
                    Sprite("░", CurrentPixelBackground, CurrentPixelForeground)
                Case 1
                    Sprite("▒", CurrentPixelBackground, CurrentPixelForeground)
                Case 2
                    Sprite("▓", CurrentPixelBackground, CurrentPixelForeground)
            End Select


        Next


    End Sub

    Public Shared Sub HiColorDrawFromFile(Filename As String, leftpos As Integer, toppos As Integer)

        If Not File.Exists(Filename) Then
            Sprite("[ ERROR ]", ConsoleColor.Red, ConsoleColor.Black)
            Sprite(" File " & Filename & " was not found.", ConsoleColor.Black, ConsoleColor.Red)
        Else
            FileOpen(1, Filename, OpenMode.Input)

            Dim Graphic() As String
            Dim I As Integer

            I = 0

            While Not EOF(1)
                ReDim Preserve Graphic(I)
                Graphic(I) = LineInput(1)
                I = I + 1
            End While
            FileClose(1)

            For X = 0 To Graphic.Count - 1
                SetPos(leftpos, toppos + X)
                HiColorDraw(Graphic(X))
            Next



        End If



    End Sub

    Public Shared Sub DrawFromFile(Filename As String, leftpos As Integer, toppos As Integer)

        If Not File.Exists(Filename) Then
            Sprite("[ ERROR ]", ConsoleColor.Red, ConsoleColor.Black)
            Sprite(" File " & Filename & " was not found.", ConsoleColor.Black, ConsoleColor.Red)
        Else
            FileOpen(1, Filename, OpenMode.Input)

            Dim Graphic() As String
            Dim I As Integer

            I = 0

            While Not EOF(1)
                ReDim Preserve Graphic(I)
                Graphic(I) = LineInput(1)
                I = I + 1
            End While
            FileClose(1)

            For X = 0 To Graphic.Count - 1
                SetPos(leftpos, toppos + X)
                Draw(Graphic(X))
            Next



        End If



    End Sub
    ''' <summary>
    ''' Displays a message
    ''' </summary>
    ''' <param name="Message">The message you wish to display, dumb dumb</param>
    ''' <param name="LineBreak">Wether or not to put a linebreak</param>
    Public Shared Sub Echo(Message As String, Optional LineBreak As Boolean = False)
        If Message = "." Then
            Echo("", True)
            Exit Sub
        End If

        If LineBreak Then
            Console.WriteLine(Message)
        Else
            Console.Write(Message)
        End If

    End Sub

    ''' <summary>
    ''' Write text to a window
    ''' </summary>
    ''' <param name="Text">The text to be put on the window</param>
    ''' <param name="Length">Length of the window</param>
    ''' <param name="height">Height of the window</param>
    ''' <param name="leftpos">Leftpos in reference to the window</param>
    ''' <param name="toppos">Toppos in reference to the window</param>
    Public Shared Sub WriteToWindow(Text As String, Length As Integer, height As Integer, leftpos As Integer, toppos As Integer)
        Sprite(Text, WindowBackgroundColor, ConsoleColor.Black, GetLeftPos(Length) + leftpos, GetTopPos(height) + toppos)
    End Sub

    Public Shared Function StringToColor(Doot As String) As ConsoleColor
        Select Case Doot.ToUpper
            Case "0"
                Return ConsoleColor.Black
            Case "1"
                Return ConsoleColor.DarkBlue
            Case "2"
                Return ConsoleColor.DarkGreen
            Case "3"
                Return ConsoleColor.DarkCyan
            Case "4"
                Return ConsoleColor.DarkRed
            Case "5"
                Return ConsoleColor.DarkMagenta
            Case "6"
                Return ConsoleColor.DarkYellow
            Case "7"
                Return ConsoleColor.Gray
            Case "8"
                Return ConsoleColor.DarkGray
            Case "9"
                Return ConsoleColor.Blue
            Case "A"
                Return ConsoleColor.Green
            Case "B"
                Return ConsoleColor.Cyan
            Case "C"
                Return ConsoleColor.Red
            Case "D"
                Return ConsoleColor.Magenta
            Case "E"
                Return ConsoleColor.Yellow
            Case "F"
                Return ConsoleColor.White
            Case Else
                Return Console.BackgroundColor
        End Select
    End Function
End Class
