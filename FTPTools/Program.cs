using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTPTools
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (global.config.exist())
                {
                    global.config config = new global.config();
                    sharedOP.WriteToLog("Inicio de FTPTools:" + global.config.descripcion, "Tarea iniciada con exito");
                    ftp oFtp = new ftp(global.config.sftp, global.config.user, global.config.pass, global.config.proxy_user, global.config.proxy_pass, global.config.useproxy);

                    Console.WriteLine("/**********FTPTOOLS*************/");
                    Console.WriteLine("Fichero:" + global.config.fichero);
                    Console.WriteLine("Ftp:" + global.config.sftp);

                    if (global.config.action == "download")
                    {
                        Console.WriteLine("Accion:" + global.config.action);
                        oFtp.download(global.config.fichero, global.config.rutLocal + "/" + global.config.fichero);
                        sharedOP.WriteToLog("Download", "Descarga de " + global.config.fichero + " realizada correctamente");
                    }
                    if (global.config.action == "upload")
                    {
                        Console.WriteLine("Accion:" + global.config.action);
                        oFtp.upload(global.config.fichero, global.config.rutLocal + "/" + global.config.fichero);
                        sharedOP.WriteToLog("Upload", "Subida de " + global.config.fichero + " realizada correctamente");
                    }
                }
                else
                {
                    global.Errores.add(new Exception("No se encuentra el fichero de configuración"));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("/**********ERRORES*************/");
                Console.WriteLine("Error: " + ex.Message.ToString());
                Console.WriteLine("StackTrace: " + ex.StackTrace);
                global.Errores.add(ex);
            }

            if (global.Errores.getNuErrores() >= 1)
            {
                global.Errores.writeLog();
                //global.Errores.sendEmail();
                //Envio por ASPMail que va bastante mejor que la caca de microsoft
                if (global.config.emailTipo == "EmailAgent")
                {
                    global.Errores.sendEmailAgent();
                }
                else if (global.config.emailTipo == "SmtpFramework")
                {
                    global.Errores.sendEmail();
                }
                else
                {
                    global.Errores.sendEmail();
                }

            }

        }
    }
}
