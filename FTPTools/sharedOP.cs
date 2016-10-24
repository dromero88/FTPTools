using System;
using System.IO;
using System.Diagnostics;
class sharedOP
{
    public string quitarComas(string cadena)
    {
        return cadena.Replace("'", "");
    }

    public string toSqlCmnd(string Obj)
    {
        if (Obj == "" || Obj == null)
        {
            return "null";
        }
        else
        {
            try
            {
                return "'" + Obj.Replace("'", "''") + "'";
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }
    }

    public string toSqlCmnd(int Obj)
    {
        if (Obj == 0)
        {
            return "null";
        }
        else
        {
            return Obj.ToString();
        }
    }

    public string toSqlCmnd(bool Obj)
    {
        if (Obj == null)
        {
            return "null";
        }
        else
        {
            if (Obj) { return "1"; } else { return "0"; }
        }
    }

    public void getFtpFile(string url, string user, string password, string file, string download_path)
    {
        ftp ftpClient = new ftp(url, user, password);
        ftpClient.download(file, download_path + "\\" + global.config.fichero);

    }

    public static void WriteToLog(string titulo, string msg)
    {
        /* if (!System.IO.Directory.Exists(global.config.rutLog))
        {
                    System.IO.Directory.CreateDirectory(global.config.rutLog);
        }*/

        FileStream fs = new FileStream(global.config.rutLog, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        StreamWriter s = new StreamWriter(fs);
        s.Close();
        fs.Close();

        FileStream fs1 = new FileStream(global.config.rutLog, FileMode.Append, FileAccess.Write);
        StreamWriter s1 = new StreamWriter(fs1);

        s1.Write(titulo + (char)13 + (char)10);
        s1.Write(msg + (char)13 + (char)10);
        s1.Write(DateTime.Now.ToString() + (char)13 + (char)10);
        s1.Write("================================================" + (char)13 + (char)10);
        s1.Close();
        fs1.Close();

        EventLog objEventLog = new EventLog();
        objEventLog.Source = System.IO.Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        objEventLog.WriteEntry(msg, EventLogEntryType.Information);

    }

}