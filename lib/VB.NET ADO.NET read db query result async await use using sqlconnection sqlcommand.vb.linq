<Query Kind="VBProgram" />

Sub Main
	'GetData()
	GetDataAsync()
End Sub

Public Async Sub GetDataAsync
	Console.WriteLine("VB.NET ADO.NET read db query result use using sqlconnection sqlcommand")
	Using conn = New SqlConnection("Data Source=.;Initial Catalog=SampleDb;Integrated Security=True;")
		conn.Open()
		Using cmd = New SqlCommand("Select 1 union Select 2", conn)
			Using reader = Await cmd.ExecuteReaderAsync()
				While reader.Read()
					Console.WriteLine(reader(0))
				End While
			End Using
		End Using
	End Using
End Sub

Sub GetData
	Console.WriteLine("VB.NET ADO.NET read db query result use using sqlconnection sqlcommand")
	Using conn = New SqlConnection("Data Source=.;Initial Catalog=SampleDb;Integrated Security=True;")
		conn.Open()
		Using cmd = New SqlCommand("Select 1 union Select 2", conn)
			Using reader = cmd.ExecuteReader()
				While reader.Read()
					Console.WriteLine(reader(0))
				End While
			End Using
		End Using
	End Using
End Sub