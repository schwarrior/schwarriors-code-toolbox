using System;
using System.ComponentModel;
using System.Web;

namespace SampleProgram.Entities
{
    public class Error
    {
        //shows example of how to create default values
        //with dynamic data (from .net)
        //these defaults will not be in sql server schema

        public Error()
        {
            if (HttpContext.Current != null)
            {
                _url = HttpContext.Current.Request.Url.ToString();
                var userIdRaw = HttpContext.Current.Session["UserId"];
                var userIdParsed = default(int);
                if(userIdRaw != null && int.TryParse(userIdRaw.ToString(),out userIdParsed))
                    _userId = userIdParsed;
            }
            
        }

        public int Id { get; set; }

        private DateTime _errorDate = DateTime.Now;
        public DateTime ErrorDate
        {
            get { return _errorDate; }
            set { _errorDate = value; }
        }

        private int? _userId;
        public int? UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        private string _url;
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        public string ErrorToString { get; set; }

        //example of how to do default value with constant
        //will also transmit default to db schema
        private const string StatusDefaultValue = "MyDefault";
        private string _status = StatusDefaultValue;
        [DefaultValue(StatusDefaultValue)]
        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }
    }
}
