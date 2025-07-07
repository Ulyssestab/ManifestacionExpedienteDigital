//using COPIASENLINEA.DataModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace COPIASENLINEA
{
    //public partial class Motor : System.Web.UI.Page
    //{
    //    protected void Page_Load(object sender, EventArgs e)
    //    {
    //        string UrlRetorno = ConfigurationManager.AppSettings["UrlRetorno"];
    //        string conexionGDB = System.Configuration.ConfigurationManager.ConnectionStrings["WFTRAMITESConnectionString"].ConnectionString;
    //        WFTramitesDataContext ContextoGDB = new WFTramitesDataContext(conexionGDB);
    //        try
    //        {
    //            string pc = Request.QueryString["pc"];
    //            //SIS_TRACAT_SOLICITUD_COPIA resultado2;
    //            if (string.IsNullOrWhiteSpace(pc))
    //            {
    //                Response.Redirect(ConfigurationManager.AppSettings["MotorPagos"]);
    //                return;
    //            }



    //            var resultado = (from record in ContextoGDB.PAGOSCOPIAS
    //                             where record.PASEACAJA.Equals(pc)
    //                             select record).FirstOrDefault();

    //            //if(resultado != null)
    //            //{
    //            //    resultado2 = (from record in ContextoGDB.SIS_TRACAT_SOLICITUD_COPIA
    //            //                      where record.CVE_CAT_ORI.Equals(resultado.CVE_CAT_ORI)
    //            //                      select record).FirstOrDefault();
    //            //}
    //            //else
    //            //{
    //            //    Response.Redirect("PagarCopias.aspx");
    //            //    return;
    //            //}


    //            string url = ConfigurationManager.AppSettings["Nuevo_MotorPagos"];
    //            Response.Clear();
    //            //StringBuilder sb = new StringBuilder();
    //            var sb = new System.Text.StringBuilder();
    //            sb.Append("<html>");
    //            sb.AppendFormat("<body onload='document.forms[0].submit()'>");
    //            sb.AppendFormat("<form action='{0}' method='post'>", url);
    //            sb.AppendFormat("<input type='hidden' name='importe' value='{0}'>", resultado.TOTALPAGO);
    //            sb.AppendFormat("<input type='hidden' name='nombreContribuyente' value='{0}'>", resultado.SOLICITANTE);
    //            sb.AppendFormat("<input type='hidden' name='impuesto' value='{0}'>", resultado.TRAMITE);
    //            sb.AppendFormat("<input type='hidden' name='referencia' value='{0}'>", pc);
    //            sb.AppendFormat("<input type='hidden' name='correo' value='{0}'>", resultado.CORREO_ELECTRONICO);
    //            sb.AppendFormat("<input type='hidden' name='dispositivo' value='{0}'>", "3");                // "1" IOS "2" ANDROID "3" WEB O PONER LA URL DEL SITIO DE DONDE SE HACE LA PETICION
    //            sb.AppendFormat("<input type='hidden' name='urlRetorno' value='{0}'>", UrlRetorno + resultado.CVE_CAT_ORI);
    //            sb.Append("</form>");
    //            sb.Append("</body>");
    //            sb.Append("</html>");
    //            Response.Write(sb.ToString());
    //            Response.End();
    //            //Session.RemoveAll();
    //            //Session.Clear();
    //        }
    //        catch (Exception ex)
    //        {
    //            //log.Error("Error en el Page Load de Motor.aspx", ex);
    //        }
    //    }
    //}
}