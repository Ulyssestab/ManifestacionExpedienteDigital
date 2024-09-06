using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ManifestacionEnLinea
{
    public partial class DescargaBoleta : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!string.IsNullOrEmpty(Request.QueryString.ToString()))
            //{
            //    /*Desencirptar Correo*/
            //    var decryptedCorreoBytes = Convert.FromBase64String(Request.QueryString["value"].ToString());
            //    var outputCorreo = MachineKey.Unprotect(decryptedCorreoBytes, "value");
            //    var decryptedCorreo = Encoding.UTF8.GetString(outputCorreo);


            //    MensajeFinal.Text = "Acabamos de enviarle un mensaje a " + decryptedCorreo;
            //}
            MensajeFinal.Text = "Acabamos de enviarle un mensaje a catastro@aguascalientes.gob.mx";

        }

        protected void ButtonReturn_Click(object sender, EventArgs e)
        {

            Response.Redirect("index.aspx");
        }
    }
}