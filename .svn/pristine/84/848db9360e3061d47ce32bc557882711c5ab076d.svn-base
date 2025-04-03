using ManifestacionEnLinea.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ManifestacionEnLinea
{
    public partial class PaginaError : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string error = string.Empty;
            try
            {
                error = Request.QueryString["Error"];
                LabelError.Text = error;
            }
            catch (Exception ex)
            {
                LabelError.Text = "Error inesperado, vuelva intentarlo mas tarde.";
            }
           

        }

        protected void RegresarInicio_Click(object sender, EventArgs e)
        {
            Response.Redirect("index.aspx");
        }

        //protected void EnviarCorreoErrores_Click(object sender, EventArgs e)
        //{
        //    string mensajeError = MsjDescError.Text;
        //    D_EnvioCorreo CS = new D_EnvioCorreo();
        //    CS.EnvioCorreoPagError(mensajeError);
        //}
    }
}