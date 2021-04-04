Module Module1

    'VB.NET Random No Repeat Hashset

    'Possibly a better way to do this:
    'Generate long random decimal values (very unlikely to repeat)
    'Sort them ascending and assign integers based on row number

    Sub Main()
        Console.WriteLine("Non-Repeating Random Lab")

        Dim uniqueValues = GetUniqueRandoms(10, 1, 20)

        uniqueValues.ToList().ForEach(Sub(uniqueValue)
                                          Console.WriteLine(uniqueValue)
                                      End Sub)

        Console.WriteLine("Press Enter to Exit")
        Console.WriteLine()
        Console.ReadLine()
    End Sub

    Function GetUniqueRandoms(NumberOfItems As Integer, Optional MinValue As Integer = 0, Optional MaxValue As Integer = Integer.MaxValue - 1) As IEnumerable(Of Integer)
        Dim values As New HashSet(Of Integer)
        Dim random As New Random()
        While (True)
            Dim nextValue = random.Next(MinValue, MaxValue + 1)
            values.Add(nextValue)
            If (values.Count >= NumberOfItems OrElse values.Count >= (MaxValue - MinValue)) Then
                Return values
            End If
        End While
    End Function

End Module
