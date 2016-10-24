using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Configuration;

namespace global
{
    public class Email
    {
        private MailMessage m;

        public Email()
        {
            m = new MailMessage();
            m.Sender = m.From = new MailAddress(global.config.emailFrom);
            m.IsBodyHtml = true;
        }

        public bool isHTML
        {
            set
            {
                m.IsBodyHtml = value;
            }
        }

        public string to
        {
            set
            {
                m.To.Add(value);
            }
        }

        public string from
        {
            set
            {
                m.Sender = m.From = new MailAddress(value);
            }
        }
        public string subject
        {
            set
            {
                m.Subject = value;
            }
        }

        public string body
        {
            get
            {
                return body;
            }
            set
            {
                m.Body = value;

            }
        }

        public void send()
        {
            SmtpClient sc = new SmtpClient();
            sc.Host = global.config.emailSMTPHost;
            sc.DeliveryMethod = SmtpDeliveryMethod.Network;
            sc.UseDefaultCredentials = false;
            sc.Port = 25;
            sc.Credentials = new System.Net.NetworkCredential(
                    global.config.emailSMTPUser,
                    global.config.emailSMTPPassword
            );

            sc.Send(m);
        }

    }


    public class EmailAgent
    {
        private ASPEMAILLib.MailSender m;

        public EmailAgent()
        {
            m = new ASPEMAILLib.MailSender();
            m.From = global.config.emailFrom;
            m.IsHTML = 1;
        }

        public int isHTML
        {
            set
            {
                m.IsHTML = value;
            }
        }

        public string to
        {
            set
            {
                m.AddAddress(value, Type.Missing);
            }
        }

        public string from
        {
            set
            {
                m.From = value;
            }
        }
        public string subject
        {
            set
            {
                m.Subject = value;
            }
        }

        public string body
        {
            get
            {
                return body;
            }
            set
            {
                m.Body = value;

            }
        }

        public void send()
        {
            m.Host = global.config.emailSMTPHost;
            m.Port = 25;
            m.Send(Type.Missing);
        }

    }
}
