using System;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Specialized ;
using System.Data;

namespace CustomTraceListeners
{

	//Required DB Objects

	/* 
	create database TraceStore
	go

	use TraceStore
	go

	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Traces]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	drop table [dbo].[Traces]
	GO

	CREATE TABLE [dbo].[Traces] (
		[TraceDateTime] [datetime] NULL ,
		[TraceCategory] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
		[TraceDescription] [varchar] (1024) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
		[StackTrace] [varchar] (2048) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
		[DetailedErrorDescription] [varchar] (2048) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
	) ON [PRIMARY]
	GO


	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddTrace]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[AddTrace]
	go

	create procedure AddTrace

	@r_dtTraceDateTime	datetime,
	@r_vcTraceCategory	varchar(50),
	@r_vcTraceDescription	varchar(1024),
	@r_vcStackTrace		varchar(2048),
	@r_vcDetailedErrorDescription	varchar(2048)

	as
	begin
		insert
			Traces 	(TraceDateTime, TraceCategory, TraceDescription, StackTrace, DetailedErrorDescription)
		values
				(@r_dtTraceDateTime, @r_vcTraceCategory, @r_vcTraceDescription, @r_vcStackTrace, @r_vcDetailedErrorDescription)
		return @@error
	end
	go
	*/
	

	// Project .config sample

	/*
	<configuration>
		<appSettings>
			<add key="ConnectionString" value="Data Source=localhost;uid=sa;pwd=;Initial Catalog=TraceStore" />
			<add key="MaximumRequests" value="2" />
		</appSettings>

	<system.diagnostics>
	<trace autoflush="true" indentsize="2">
	<listeners>
		<add	name="DBTL"
				type="CustomTraceListeners.DatabaseTraceListener,CustomTraceListeners"/>
	</listeners>
	</trace>
	</system.diagnostics>
	</configuration>
	*/

	public class DatabaseTraceListener : TraceListener
	{
		private const string COLUMN_SEPARATOR = "|" ;
		private string m_strConnectionString ;
		private int m_iMaximumRequests ;
		private StringCollection m_objCollection ;

		public DatabaseTraceListener()
		{
			InitializeListener() ;
		}

		public DatabaseTraceListener(string r_strListenerName) : base(r_strListenerName)
		{
			InitializeListener() ;
		}
		
		private void InitializeListener()
		{
			m_strConnectionString = ConfigurationSettings.AppSettings["ConnectionString"] ;
			m_iMaximumRequests = Convert.ToInt32(ConfigurationSettings.AppSettings["MaximumRequests"]) ;
			m_objCollection = new StringCollection() ;
		}

		private void SaveErrors()
		{
			SqlConnection objConnection = new SqlConnection(m_strConnectionString) ;
			SqlCommand objCommand = new SqlCommand() ;
			try
			{
				objCommand.Connection = objConnection ;
				objCommand.CommandText = "AddTrace" ;
				objCommand.CommandType = CommandType.StoredProcedure ;
				objConnection.Open() ;

				foreach (string m_strError in m_objCollection)
				{
					CreateParameters(objCommand, m_strError) ;
					objCommand.ExecuteNonQuery() ;
				}
				m_objCollection.Clear() ;
			}
			catch (Exception e)
			{

			}
			finally
			{
				if (objConnection != null)
				{
					if (objConnection.State == ConnectionState.Open)
						objConnection.Close() ;
				}
				objConnection = null ;
				objCommand = null ;
			}
		}

		private void AddToCollection(	string r_strTraceDateTime,
			string r_strTraceCategory,
			string r_strTraceDescription,
			string r_strStackTrace,
			string r_strDetailedErrorDescription)
		{
			string strError =	r_strTraceDateTime + COLUMN_SEPARATOR +
				r_strTraceCategory + COLUMN_SEPARATOR +
				r_strTraceDescription + COLUMN_SEPARATOR +
				r_strStackTrace + COLUMN_SEPARATOR +
				r_strDetailedErrorDescription ;
			m_objCollection.Add(strError) ;
			if (m_objCollection.Count == m_iMaximumRequests)
			{
				SaveErrors() ;
			}
		}

		private void CreateParameters(SqlCommand r_objCommand, string r_strError)
		{
			if ( (r_objCommand != null) && (! r_strError.Equals("")) )
			{
				string[] strColumns ;
				SqlParameterCollection objParameters = r_objCommand.Parameters ;

				strColumns = r_strError.Split(COLUMN_SEPARATOR.ToCharArray()) ;
				objParameters.Clear() ;

				objParameters.Add(new SqlParameter(	"@r_dtTraceDateTime",
					SqlDbType.DateTime,
					8) ) ;
				objParameters.Add(new SqlParameter(	"@r_vcTraceCategory",
					SqlDbType.VarChar,
					50) ) ;
				objParameters.Add(new SqlParameter(	"@r_vcTraceDescription",
					SqlDbType.VarChar,
					1024) ) ;
				objParameters.Add(new SqlParameter(	"@r_vcStackTrace",
					SqlDbType.VarChar,
					2048) ) ;
				objParameters.Add(new SqlParameter(	"@r_vcDetailedErrorDescription",
					SqlDbType.VarChar,
					2048) ) ;
				
				int iCount = strColumns.GetLength(0) ;
				for (int i = 0; i < iCount; i++)
				{
					objParameters[i].IsNullable	= true ;
					objParameters[i].Direction = ParameterDirection.Input ;
					objParameters[i].Value = strColumns.GetValue(i).ToString().Trim() ;
				}
			}
		}

		
		public override void Write(string message)
		{
			StackTrace objTrace = new StackTrace(true) ;
			AddToCollection(DateTime.Now.ToString() , "", message, objTrace.ToString(), "" ) ;
		}

		public override void Write(object o)
		{
			StackTrace objTrace = new StackTrace(true) ;
			AddToCollection(DateTime.Now.ToString(), "", o.ToString() , objTrace.ToString(), "" ) ;
		}

		public override void Write(string message, string category)
		{
			StackTrace objTrace = new StackTrace(true) ;
			AddToCollection(DateTime.Now.ToString() , category , message, objTrace.ToString(), "" ) ;
		}

		public override void Write(object o, string category)
		{
			StackTrace objTrace = new StackTrace(true) ;
			AddToCollection(DateTime.Now.ToString() , category, o.ToString(), objTrace.ToString(), "" ) ;
		}


		public override void WriteLine(string message)
		{
			Write(message + "\n") ;			
		}

		public override void WriteLine(object o)
		{
			Write(o.ToString() + "\n") ;
		}

		public override void WriteLine(string message, string category)
		{
			Write((message + "\n"), category) ;
		}

		public override void WriteLine(object o, string category)
		{
			Write( (o.ToString() + "\n"), category) ;
		}


		public override void Fail(string message)
		{
			StackTrace objTrace = new StackTrace(true) ;
			AddToCollection(DateTime.Now.ToString(), "Fail", message, objTrace.ToString(), "") ;
		}

		public override void Fail(string message, string detailMessage)
		{
			StackTrace objTrace = new StackTrace(true) ;
			AddToCollection(DateTime.Now.ToString(), "Fail", message, objTrace.ToString(), detailMessage) ;
		}

		public override void Close()
		{
			SaveErrors() ;
		}

		public override void Flush()
		{
			SaveErrors() ;
		}
				
	}
}
