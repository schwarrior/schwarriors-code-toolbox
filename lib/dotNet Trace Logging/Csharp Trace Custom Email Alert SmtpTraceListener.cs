using System;
using System.Diagnostics;
using System.Net.Mail;
using System.Web.Mail;
using MailMessage = System.Net.Mail.MailMessage;
using MailPriority = System.Net.Mail.MailPriority;

namespace CustomTraceListeners.CustomListeners
{
    public class SmtpTraceListener : TraceListener
    {
        string _smtpServer;
        string _from;
        string _to;
        string _message;
        string _subject;

        //Normal mail priority by default
        MailPriority _priority = MailPriority.Normal;

        public MailPriority Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        public string Server
        {
            get { return _smtpServer; }
            set { _smtpServer = value; }
        }

        public string From
        {
            get { return _from; }
            set { _from = value; }
        }

        public string To
        {
            get { return _to; }
            set { _to = value; }
        }

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }

        public SmtpTraceListener()
        {

        }

        public SmtpTraceListener(string ListenerName)
        //: base(ListenerName)
        {

        }

        public SmtpTraceListener(string ListenerName,
                string Server, string From)
        //: base(ListenerName)
        {
            _smtpServer = Server;
            _from = From;
        }

        public SmtpTraceListener(string Server, string From)
            : this("SMTPTraceListener", Server, From)
        {

        }

        public override void Write(string message)
        {
            try
            {
                SmtpMail.SmtpServer = _smtpServer;
                MailMessage mailMessage = new MailMessage(_from, _to, _subject, message);
                var client = new SmtpClient(_smtpServer);
                client.Send(mailMessage);
                this.Flush();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Write(string Server, string From,
           string To, string Subject, string Message)
        {
            this._smtpServer = Server;
            this._from = From;
            this._to = To;
            this._subject = Subject;
            this._message = Message;
            try
            {
                this.Write(Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void WriteLine(string Message)
        {
            try
            {
                this.Write(Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }//SMTPTraceListener
}//namespace