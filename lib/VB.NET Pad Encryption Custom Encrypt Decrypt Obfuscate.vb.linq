<Query Kind="VBProgram">
  <Namespace>Microsoft.VisualBasic</Namespace>
</Query>

Sub Main
	Dim padEncrypt = new PadEncryption()
	Dim key = "                                                                         "
	Dim sourceString = "Super Secret Stuff, Man!"
	Dim encryptedString = padEncrypt.Transform(true,key,sourceString)
	Dim decryptedString = padEncrypt.Transform(false,key,encryptedString)
	string.Format("Source String: ""{0}"". Encrypted String: ""{1}"". Check: ""{2}"". Key: ""{3}""", _
		sourceString, encryptedString, decryptedString, key).Dump()
End Sub

' Define other methods and classes here
Public Class PadEncryption

    Private Const firstPrintableASCIICode As Integer = 32
    Private Const lastPrintableASCIICode As Integer = 126

    Private charOffset As Integer = lastPrintableASCIICode - firstPrintableASCIICode + 1

    Private __charMap As SortedDictionary(Of Integer, Char) = Nothing

    Private ReadOnly Property charMap() As SortedDictionary(Of Integer, Char)
        Get
            If __charMap Is Nothing Then

                __charMap = New SortedDictionary(Of Integer, Char)

                For charCode As Integer = firstPrintableASCIICode To lastPrintableASCIICode
                    'Console.WriteLine("{0}: {1}", charCode, Chr(charCode))
                    __charMap.Add(charCode, Chr(charCode))
                    'Console.WriteLine("{0}: {1}", charCode + charOffset, Chr(charCode))
                    __charMap.Add(charCode + charOffset, Chr(charCode))
                    'Console.WriteLine("{0}: {1}", charCode + charOffset + charOffset, Chr(charCode))
                    __charMap.Add(charCode + charOffset + charOffset, Chr(charCode))
                    'Console.WriteLine("{0}: {1}", charCode - charOffset, Chr(charCode))
                    __charMap.Add(charCode - charOffset, Chr(charCode))
                    'Console.WriteLine("{0}: {1}", charCode - charOffset - charOffset, Chr(charCode))
                    __charMap.Add(charCode - charOffset - charOffset, Chr(charCode))
                Next

            End If
            Return __charMap
        End Get
    End Property

    Public Function Transform(ByVal Encrypt As Boolean, ByVal key As String, ByVal source As String) As String _

        Dim destination As String = String.Empty

        Dim ascii As Encoding = Encoding.ASCII

        'Make sure the key is the same size as the DecryptionBits
        While key.Length < source.Length
            key += key
        End While
        key = key.Substring(0, source.Length)

        'encrypt each source character using the key 
        For charPosition As Integer = 0 To key.Length - 1

            Dim keyCharCode As Integer = Asc(key(charPosition))
            Dim sourceCharCode As Integer = Asc(source(charPosition))

            If keyCharCode < firstPrintableASCIICode Or keyCharCode > lastPrintableASCIICode Then
                Throw New System.ApplicationException("Key string has characters outside printable ASCII range")
            End If
            If sourceCharCode < firstPrintableASCIICode Or sourceCharCode > lastPrintableASCIICode Then
                Throw New System.ApplicationException("Source string has characters outside printable ASCII range")
            End If

            Dim destinationCharPostion As Integer
            If Encrypt Then
                destinationCharPostion = sourceCharCode + keyCharCode
            Else
                destinationCharPostion = sourceCharCode - keyCharCode
            End If

            destination += charMap(destinationCharPostion)
        Next

        Return destination
    End Function

End Class