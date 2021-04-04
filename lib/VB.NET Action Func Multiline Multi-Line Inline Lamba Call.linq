<Query Kind="VBProgram" />

Sub Main
	
'Func<> returns the type specified as the final generic type parameter, such that Func<int> returns an int and Func<int, string> accepts an integer and returns a string. Examples:
'Dim getOne = New Func(Of Integer)(Function() 1)
'Dim convertIntToString = New Func(Of Integer, String)(Function(i) i.ToString())
'Dim printToScreen = New Action(Of String)(Function(s) Console.WriteLine(s))
' use them
'printToScreen(convertIntToString(getOne()))


Dim inlineActionSub = New Action(Sub()
                                   If 1 = 1 Then
                                       Console.WriteLine("Hello from action")
                                   Else
                                       Console.WriteLine("No")
                                   End If
                               End Sub)

Call inlineActionSub

'an action (sub) that accepts a string parameter
Dim inlineActionSubWParam = New Action(Of String)(Sub(inStr)
                                                    If 1 = 1 Then
                                                        Console.WriteLine("Hello from Action with param: " + inStr)
                                                    Else
                                                        Console.WriteLine("No")
                                                    End If
                                                End Sub)

Call inlineActionSubWParam("Hello Param")

'a func that returns string
Dim inlineFunc = New Func(Of String)(Function()
                                         If 1 = 1 Then
                                             Return "hello from func"
                                         Else
                                             Return "No"
                                         End If
                                     End Function)
															  
Dim funcResult = inlineFunc()
Console.WriteLine(funcResult)

'a func that returns string and accepts integer as param 
   Dim inlineFuncWParam = New Func(Of Integer, String)(Function(inInt)
                                              If 1 = 1 Then
                                                  Return "hello from func with param: " + inInt.ToString()
                                              Else
                                                  Return "No"
                                              End If
                                          End Function)
														  
Dim funcWParamResult = inlineFuncWParam(22)
Console.WriteLine(funcWParamResult)

End Sub

' Define other methods and classes here
