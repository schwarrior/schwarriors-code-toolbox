Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Collections.Specialized

' Required DB Objects


' create database TraceStore
' go

' use TraceStore
' go

' if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Traces]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
' drop table [dbo].[Traces]
' GO

' CREATE TABLE [dbo].[Traces] (
'     [TraceDateTime] [datetime] NULL ,
'     [TraceCategory] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
'     [TraceDescription] [varchar] (1024) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
'     [StackTrace] [varchar] (2048) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
'     [DetailedErrorDescription] [varchar] (2048) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
' ) ON [PRIMARY]
' GO


' if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddTrace]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
' drop procedure [dbo].[AddTrace]
' go

' create procedure AddTrace

' @r_dtTraceDateTime	datetime,
' @r_vcTraceCategory	varchar(50),
' @r_vcTraceDescription	varchar(1024),
' @r_vcStackTrace		varchar(2048),
' @r_vcDetailedErrorDescription	varchar(2048)

' as
' begin
'     insert
'         Traces 	(TraceDateTime, TraceCategory, TraceDescription, StackTrace, DetailedErrorDescription)
'     values
'             (@r_dtTraceDateTime, @r_vcTraceCategory, @r_vcTraceDescription, @r_vcStackTrace, @r_vcDetailedErrorDescription)
'     return @@error
' end
' go


' Project .config sample

' <configuration>
'     <appSettings>
'         <add key="ConnectionString" value="Data Source=localhost;uid=sa;pwd=;Initial Catalog=TraceStore" />
'         <add key="MaximumRequests" value="2" />
'     </appSettings>

' <system.diagnostics>
' <trace autoflush="true" indentsize="2">
' <listeners>
'     <add	name="DBTL"
'             type="CustomTraceListeners.DatabaseTraceListener,CustomTraceListeners"/>
' </listeners>
' </trace>
' </system.diagnostics>
' </configuration>

Public Class DatabaseTraceListener
    Inherits TraceListener

    Private Const COLUMN_SEPARATOR As String = "|"
    Private m_strConnectionString As String
    Private m_iMaximumRequests As Integer
    Private m_objCollection As StringCollection

    Public Sub New()
        InitializeListener()
    End Sub

    Public Sub New(ByVal r_strListenerName As String)
        MyBase.New(r_strListenerName)
        InitializeListener()
    End Sub

    Private Sub InitializeListener()
        m_strConnectionString = ConfigurationSettings.AppSettings("ConnectionString")
        m_iMaximumRequests = Convert.ToInt32(ConfigurationSettings.AppSettings("MaximumRequests"))
        m_objCollection = New StringCollection()
    End Sub

    Private Sub SaveErrors()
        Dim objConnection As SqlConnection = New SqlConnection(m_strConnectionString)
        Dim objCommand As SqlCommand = New SqlCommand()

        Try
            objCommand.Connection = objConnection
            objCommand.CommandText = "AddTrace"
            objCommand.CommandType = CommandType.StoredProcedure
            objConnection.Open()

            For Each m_strError As String In m_objCollection
                CreateParameters(objCommand, m_strError)
                objCommand.ExecuteNonQuery()
            Next

            m_objCollection.Clear()
        Catch e As Exception
        Finally

            If objConnection IsNot Nothing Then
                If objConnection.State = ConnectionState.Open Then objConnection.Close()
            End If

            objConnection = Nothing
            objCommand = Nothing
        End Try
    End Sub

    Private Sub AddToCollection(ByVal r_strTraceDateTime As String, ByVal r_strTraceCategory As String, ByVal r_strTraceDescription As String, ByVal r_strStackTrace As String, ByVal r_strDetailedErrorDescription As String)
        Dim strError As String = r_strTraceDateTime & COLUMN_SEPARATOR & r_strTraceCategory & COLUMN_SEPARATOR & r_strTraceDescription & COLUMN_SEPARATOR & r_strStackTrace & COLUMN_SEPARATOR & r_strDetailedErrorDescription
        m_objCollection.Add(strError)

        If m_objCollection.Count = m_iMaximumRequests Then
            SaveErrors()
        End If
    End Sub

    Private Sub CreateParameters(ByVal r_objCommand As SqlCommand, ByVal r_strError As String)
        If (r_objCommand IsNot Nothing) AndAlso (Not r_strError.Equals("")) Then
            Dim strColumns As String()
            Dim objParameters As SqlParameterCollection = r_objCommand.Parameters
            strColumns = r_strError.Split(COLUMN_SEPARATOR.ToCharArray())
            objParameters.Clear()
            objParameters.Add(New SqlParameter("@r_dtTraceDateTime", SqlDbType.DateTime, 8))
            objParameters.Add(New SqlParameter("@r_vcTraceCategory", SqlDbType.VarChar, 50))
            objParameters.Add(New SqlParameter("@r_vcTraceDescription", SqlDbType.VarChar, 1024))
            objParameters.Add(New SqlParameter("@r_vcStackTrace", SqlDbType.VarChar, 2048))
            objParameters.Add(New SqlParameter("@r_vcDetailedErrorDescription", SqlDbType.VarChar, 2048))
            Dim iCount As Integer = strColumns.GetLength(0)

            For i As Integer = 0 To iCount - 1
                objParameters(i).IsNullable = True
                objParameters(i).Direction = ParameterDirection.Input
                objParameters(i).Value = strColumns.GetValue(i).ToString().Trim()
            Next
        End If
    End Sub

    Public Overrides Sub Write(ByVal message As String)
        Dim objTrace As StackTrace = New StackTrace(True)
        AddToCollection(DateTime.Now.ToString(), "", message, objTrace.ToString(), "")
    End Sub

    Public Overrides Sub Write(ByVal o As Object)
        Dim objTrace As StackTrace = New StackTrace(True)
        AddToCollection(DateTime.Now.ToString(), "", o.ToString(), objTrace.ToString(), "")
    End Sub

    Public Overrides Sub Write(ByVal message As String, ByVal category As String)
        Dim objTrace As StackTrace = New StackTrace(True)
        AddToCollection(DateTime.Now.ToString(), category, message, objTrace.ToString(), "")
    End Sub

    Public Overrides Sub Write(ByVal o As Object, ByVal category As String)
        Dim objTrace As StackTrace = New StackTrace(True)
        AddToCollection(DateTime.Now.ToString(), category, o.ToString(), objTrace.ToString(), "")
    End Sub

    Public Overrides Sub WriteLine(ByVal message As String)
        Write(message & vbLf)
    End Sub

    Public Overrides Sub WriteLine(ByVal o As Object)
        Write(o.ToString() & vbLf)
    End Sub

    Public Overrides Sub WriteLine(ByVal message As String, ByVal category As String)
        Write((message & vbLf), category)
    End Sub

    Public Overrides Sub WriteLine(ByVal o As Object, ByVal category As String)
        Write((o.ToString() & vbLf), category)
    End Sub

    Public Overrides Sub Fail(ByVal message As String)
        Dim objTrace As StackTrace = New StackTrace(True)
        AddToCollection(DateTime.Now.ToString(), "Fail", message, objTrace.ToString(), "")
    End Sub

    Public Overrides Sub Fail(ByVal message As String, ByVal detailMessage As String)
        Dim objTrace As StackTrace = New StackTrace(True)
        AddToCollection(DateTime.Now.ToString(), "Fail", message, objTrace.ToString(), detailMessage)
    End Sub

    Public Overrides Sub Close()
        SaveErrors()
    End Sub

    Public Overrides Sub Flush()
        SaveErrors()
    End Sub
End Class
