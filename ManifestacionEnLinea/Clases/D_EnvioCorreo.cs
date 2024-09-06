using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;

namespace ManifestacionEnLinea.Clases
{
    public class D_EnvioCorreo
    {
		MailMessage correo = new MailMessage();
		SmtpClient servidorCorreo = new SmtpClient();
        string ServidorCorreo = System.Configuration.ConfigurationManager.AppSettings["ServidorCorreo"];
        string IPServidor = ConfigurationManager.AppSettings["IPServidor"].ToString();
        public D_EnvioCorreo()
		{
            //string contrasena;
            //contrasena = Obtenercontrasena();
            servidorCorreo.Host = ServidorCorreo;
			servidorCorreo.Port = 25;
			servidorCorreo.Credentials = new NetworkCredential("notificacion@aguascalientes.gob.mx", "N0+iF1c@2023");
			servidorCorreo.EnableSsl = false;

		}

        public string EnvioCorreoManiyAvaluo(string correoElectronico, string cuentacatastral)
        {
            string curFile = @"\\"+IPServidor+"\\irc\\Archivos\\ManifestacionCatastralEnLinea\\" + cuentacatastral + "\\BoletaParaDescargar.pdf";

            Attachment data = new Attachment(curFile, MediaTypeNames.Application.Octet);

            string vRes = "1";

            string body = "<!DOCTYPE HTML>"
                        + "<HTML><HEAD> "
                        + "<META content='text/html; charset=utf-8 http-equiv=Content-Type>'"
                        + "<META content=MSHTML 5.00.2920.0 name=GENERATOR>"
                        + "<STYLE></STYLE>"
                        + "</HEAD>"
                        + "<BODY bgColor=#ffffff>"
                        + "<DIV><FONT face=Verdana, Arial, Helvetica, sans-serif size=2>"
                        + "<br>Su trámite fue ingresado para su revisión, se anexa boleta con folio para que realize su seguimiento de estatus en la siguiente liga" + "<br><br>"
                        + "<br><br>Consulta de Folios SEGUOT - Instituto Catastral https://eservicios2.aguascalientes.gob.mx/seguot/seguimientomovil/Catastro.aspx"
                        + "<br><br><br><br>Gobierno del Estado<br>"
                        + "Av. de la Convenci&oacute;n Ote 102, Col. del Trabajo.<br>"
                        + "C.P. 20180, Aguascalientes, Ags.<br>"
                        + "Tel. 449 910 25 20<br>"
                        + "</font>"
                        + "</BODY></HTML>";

            try
            {
                correo.To.Clear();
                correo.CC.Clear();
                correo.Bcc.Clear();
                correo.From = new System.Net.Mail.MailAddress("notificacion@aguascalientes.gob.mx");
                correo.To.Add(correoElectronico);
                correo.Subject = "Solicitud de Manifestación y Avalúo Catastral, Instituto Catastral del Estado de Aguascalientes ";
                correo.Body = body;
                correo.IsBodyHtml = true;
                correo.Priority = System.Net.Mail.MailPriority.Normal;
                correo.Attachments.Add(data);
                servidorCorreo.Send(correo);
            }
            catch (Exception ex)
            {
                vRes = ex.Message;
            }
            return vRes;
        }

        public string EnvioCorreoAviso(string correoElectronico, string cuentacatastral)
        {
            string curFile = @"\\"+IPServidor+"\\irc\\Archivos\\ManifestacionCatastralEnLinea\\" + cuentacatastral + "\\BoletaParaDescargar.pdf";

            Attachment data = new Attachment(curFile, MediaTypeNames.Application.Octet);

            string vRes = "1";

            string body = "<!DOCTYPE HTML>"
                        + "<HTML><HEAD> "
                        + "<META content='text/html; charset=utf-8 http-equiv=Content-Type>'"
                        + "<META content=MSHTML 5.00.2920.0 name=GENERATOR>"
                        + "<STYLE></STYLE>"
                        + "</HEAD>"
                        + "<BODY bgColor=#ffffff>"
                        + "<DIV><FONT face=Verdana, Arial, Helvetica, sans-serif size=2>"
                        + "<br>Su trámite fue ingresado para su revisión, se anexa boleta con folio para que realize su seguimiento de estatus en la siguiente liga" + "<br><br>"
                        + "<br><br>Consulta de Folios SEGUOT - Instituto Catastral https://eservicios2.aguascalientes.gob.mx/seguot/seguimientomovil/Catastro.aspx"
                        + "<br><br><br><br>Gobierno del Estado<br>"
                        + "Av. de la Convenci&oacute;n Ote 102, Col. del Trabajo.<br>"
                        + "C.P. 20180, Aguascalientes, Ags.<br>"
                        + "Tel. 449 910 25 20<br>"
                        + "</font>"
                        + "</BODY></HTML>";

            try
            {
                correo.To.Clear();
                correo.CC.Clear();
                correo.Bcc.Clear();
                correo.From = new System.Net.Mail.MailAddress("notificacion@aguascalientes.gob.mx");
                correo.To.Add(correoElectronico);
                correo.Subject = "Solicitud de Manifestación y/o Avalúo Catastral, Instituto Catastral del Estado de Aguascalientes ";
                correo.Body = body;
                correo.IsBodyHtml = true;
                correo.Priority = System.Net.Mail.MailPriority.Normal;
                correo.Attachments.Add(data);
                servidorCorreo.Send(correo);
            }
            catch (Exception ex)
            {
                vRes = ex.Message;
            }
            return vRes;
        }

        public string EnvioCorreoAvaluo(string correoElectronico, string cuentacatastral)
        {
            string curFile = @"\\"+IPServidor+"\\irc\\Archivos\\AvaluoCatastralEnLinea\\" + cuentacatastral + "\\BoletaParaDescargar.pdf";

            Attachment data = new Attachment(curFile, MediaTypeNames.Application.Octet);

            string vRes = "1";

            string body = "<!DOCTYPE HTML>"
                        + "<HTML><HEAD> "
                        + "<META content='text/html; charset=utf-8 http-equiv=Content-Type>'"
                        + "<META content=MSHTML 5.00.2920.0 name=GENERATOR>"
                        + "<STYLE></STYLE>"
                        + "</HEAD>"
                        + "<BODY bgColor=#ffffff>"
                        + "<DIV><FONT face=Verdana, Arial, Helvetica, sans-serif size=2>"
                        + "<br>Su trámite fue ingresado para su revisión, se anexa boleta con folio para que realize su seguimiento de estatus en la siguiente liga" + "<br><br>"
                        + "<br><br>Consulta de Folios SEGUOT - Instituto Catastral https://eservicios2.aguascalientes.gob.mx/seguot/seguimientomovil/Catastro.aspx"
                        + "<br><br><br><br>Gobierno del Estado<br>"
                        + "Av. de la Convenci&oacute;n Ote 102, Col. del Trabajo.<br>"
                        + "C.P. 20180, Aguascalientes, Ags.<br>"
                        + "Tel. 449 910 25 20<br>"
                        + "</font>"
                        + "</BODY></HTML>";

            try
            {
                correo.To.Clear();
                correo.CC.Clear();
                correo.Bcc.Clear();
                correo.From = new System.Net.Mail.MailAddress("notificacion@aguascalientes.gob.mx");
                correo.To.Add(correoElectronico);
                correo.Subject = "Solicitud de Manifestación y/o Avalúo Catastral, Instituto Catastral del Estado de Aguascalientes ";
                correo.Body = body;
                correo.IsBodyHtml = true;
                correo.Priority = System.Net.Mail.MailPriority.Normal;
                correo.Attachments.Add(data);
                servidorCorreo.Send(correo);
            }
            catch (Exception ex)
            {
                vRes = ex.Message;
            }
            return vRes;
        }

        public string EnvioCorreoPagError(string mensaje)
        {
            string vRes = "1";
            string body = "<!DOCTYPE HTML>"
                       + "<HTML><HEAD> "
                       + "<META content='text/html; charset=utf-8 http-equiv=Content-Type>'"
                       + "<META content=MSHTML 5.00.2920.0 name=GENERATOR>"
                       + "<STYLE></STYLE>"
                       + "</HEAD>"
                       + "<BODY bgColor=#ffffff>"
                       + "<DIV><FONT face=Verdana, Arial, Helvetica, sans-serif size=2>"
                       + "<br>"+ mensaje + "<br><br>"
                       + "<br><br>Consulta de Folios SEGUOT - Instituto Catastral https://eservicios2.aguascalientes.gob.mx/seguot/seguimientomovil/Catastro.aspx"
                       + "<br><br><br><br>Gobierno del Estado<br>"
                       + "Av. de la Convenci&oacute;n Ote 102, Col. del Trabajo.<br>"
                       + "C.P. 20180, Aguascalientes, Ags.<br>"
                       + "Tel. 449 910 25 20<br>"
                       + "</font>"
                       + "</BODY></HTML>";

            try
            {
                correo.To.Clear();
                correo.CC.Clear();
                correo.Bcc.Clear();
                correo.From = new System.Net.Mail.MailAddress("notificacion@aguascalientes.gob.mx");
                correo.To.Add("cesar.tabares@aguascalientes.gob.mx");
                correo.Subject = "Reporte de error Manifestación en Linea";
                correo.Body = body;
                correo.IsBodyHtml = true;
                correo.Priority = System.Net.Mail.MailPriority.Normal;
                servidorCorreo.Send(correo);
            }
            catch (Exception ex)
            {
                vRes = ex.Message;
            }
            return vRes;
        }

        public string Obtenercontrasena()
        {
            string contra = string.Empty;
            string nombreBD = "WFTRAMITES";
            string conexionGDBTL = System.Configuration.ConfigurationManager.ConnectionStrings[nombreBD + "ConnectionString"].ConnectionString;
            SqlConnection cnnPassEmail;
            cnnPassEmail = new SqlConnection(conexionGDBTL);
            cnnPassEmail.Open();
            String sqlCon = @"SELECT top 1 * FROM [" + nombreBD + "].[dbo].[PassEmail] order by Id desc";
            SqlCommand cmdCon = new SqlCommand(sqlCon, cnnPassEmail);
            SqlDataReader readerCon = cmdCon.ExecuteReader();
            while (readerCon.Read())
            {
                contra = readerCon.GetString(2);

            }
            cnnPassEmail.Close();
            return contra;

        }
    }
}