
using System.Collections;
using System.IO;
using System;
using System.Diagnostics;
using System.Xml;



namespace global
{
    class config
    {
        private static string _descripcion;
        private static string _rutLog;
        private static string _rutLocal;
        private static string _fichero;
        private static string _action;
        private static string _ftp;
        private static string _user;
        private static string _pass;
        private static string _proxy_user;
        private static string _proxy_pass;
        private static bool _useproxy;
        private static string _emailTipo;
        private static string _emailFrom;
        private static string _emailTo;
        private static string _emailSMTPHost;
        private static string _emailSMTPUser;
        private static string _emailSMTPPassword;
        private static bool _debug;

        public static string descripcion
        { get { return _descripcion; } set { _descripcion = value; } }

        public static string rutLog
        { get { return _rutLog; } set { _rutLog = value; } }

        public static string rutLocal
        { get { return _rutLocal; } set { checkPath(value); _rutLocal = value; } }

        public static string fichero
        { get { return _fichero; } set { _fichero = value; } }

        public static string action
        { get { return _action; } set { _action = value; } }

        public static string sftp
        { get { return _ftp; } set { _ftp = value; } }

        public static string user
        { get { return _user; } set { _user = value; } }

        public static string pass
        { get { return _pass; } set { _pass = value; } }

        public static string proxy_user
        { get { return _proxy_user; } set { _proxy_user = value; } }

        public static string proxy_pass
        { get { return _proxy_pass; } set { _proxy_pass = value; } }

        public static bool useproxy
        { get { return _useproxy; } set { _useproxy = value; } }

        public static string emailTipo
        { get { return _emailTipo; } set { _emailTipo = value; } }

        public static string emailFrom
        { get { return _emailFrom; } set { _emailFrom = value; } }

        public static string emailTo
        { get { return _emailTo; } set { _emailTo = value; } }

        public static string emailSMTPHost
        { get { return _emailSMTPHost; } set { _emailSMTPHost = value; } }

        public static string emailSMTPUser
        { get { return _emailSMTPUser; } set { _emailSMTPUser = value; } }

        public static string emailSMTPPassword
        { get { return _emailSMTPPassword; } set { _emailSMTPPassword = value; } }

        public static bool debug
        { get { return _debug; } set { _debug = value; } }



        public config()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "appConfig.xml");

            XmlNodeList config = xDoc.GetElementsByTagName("config");

            foreach (XmlNode nodo in config)
            {
                descripcion = nodo.SelectSingleNode("descripcion").InnerText;
                rutLog = nodo.SelectSingleNode("ruta_log").InnerText;
                rutLocal = nodo.SelectSingleNode("ruta_local").InnerText;
                fichero = nodo.SelectSingleNode("fichero").InnerText;
                action = nodo.SelectSingleNode("action").InnerText;
                sftp = nodo.SelectSingleNode("ftp").InnerText;
                user = nodo.SelectSingleNode("user").InnerText;
                pass = nodo.SelectSingleNode("pass").InnerText;
                proxy_user = nodo.SelectSingleNode("proxy_user").InnerText;
                proxy_pass = nodo.SelectSingleNode("proxy_pass").InnerText;
                if (proxy_user.Trim().Length + proxy_pass.Trim().Length > 0)
                {useproxy = true;}
                emailTipo = nodo.SelectSingleNode("emailTipo").InnerText;
                emailFrom = nodo.SelectSingleNode("emailFrom").InnerText;
                emailTo = nodo.SelectSingleNode("emailTo").InnerText;
                emailSMTPHost = nodo.SelectSingleNode("emailSMTPHost").InnerText;
                emailSMTPUser = nodo.SelectSingleNode("emailSMTPUser").InnerText;
                emailSMTPPassword = nodo.SelectSingleNode("emailSMTPPassword").InnerText;
            }

        }


        private static void checkPath(string path)
        {
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }

        }

        public static bool exist()
        {
            return System.IO.File.Exists(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "appConfig.xml");
        }


    }

    class Errores
    {
        private static Hashtable _Errores = new Hashtable();

        private static Hashtable errores
        { get { return _Errores; } set { _Errores = value; } }

        public static void add(Exception e)
        {
            errores.Add(errores.Count + 1,e);
        }

        public static int getNuErrores()
        {
            return errores.Count;
        }

        public static void WriteToErrorLog(Exception e)
        {
            FileStream fs = new FileStream(global.config.rutLog, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamWriter s = new StreamWriter(fs);
            s.Close();
            fs.Close();

            FileStream fs1 = new FileStream(global.config.rutLog, FileMode.Append, FileAccess.Write);
            StreamWriter s1 = new StreamWriter(fs1);

            s1.Write("ERROR DE CONFIGURACIÓN" + (char)13 + (char)10);
            s1.Write("Error: " + e.Message.ToString() + (char)13 + (char)10);
            s1.Write("StackTrace: " + e.StackTrace + (char)13 + (char)10);
            s1.Write("Date/Time: " + DateTime.Now.ToString() + (char)13 + (char)10);
            s1.Write("================================================" + (char)13 + (char)10);
            s1.Close();
            fs1.Close();

            EventLog objEventLog = new EventLog();
            objEventLog.Source = System.IO.Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            objEventLog.WriteEntry("ERROR DE CONFIGURACIÓN " + ".Error: " + e.Message.ToString() + ".StackTrace: " + e.StackTrace, EventLogEntryType.Error);

        }
        
        public static void WriteToErrorEmailLog(Exception e, ref string body)
        {
            body += "Error: " + e.Message.ToString() + "</br>";
            body += "StackTrace: " + e.StackTrace + "</br>";
            body += "Date/Time: " + DateTime.Now.ToString() + "</br>";
            body += "================================================" + "</br>";
        }

        public static void writeLog()
        {
            foreach (Exception e in global.Errores.errores.Values)
            {
                WriteToErrorLog(e);
            }
        }

        public static void sendEmail()
        {
            Email mail = new Email();
            mail.to = global.config.emailTo;
            mail.subject = "FTPTools";
            string body = "<p><b>Han ocurrido errores en el FTPTools.</b></p>";
            foreach (Exception e in global.Errores.errores.Values)
            {
                WriteToErrorEmailLog(e, ref body);
            }
            mail.body = body;
            mail.send();
        }

        public static void sendEmailAgent()
        {
            EmailAgent mail = new EmailAgent();
            mail.to = global.config.emailTo;
            mail.subject = "FTPTools";
            string body = "<p><b>Han ocurrido errores en el FTPTools.</b></p>";
            foreach (Exception e in global.Errores.errores.Values)
            {
                WriteToErrorEmailLog(e, ref body);
            }
            mail.body = body;
            mail.send();
        }

    }


}
