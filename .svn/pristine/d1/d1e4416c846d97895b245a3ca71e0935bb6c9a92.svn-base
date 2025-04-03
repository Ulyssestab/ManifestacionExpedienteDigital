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
    public partial class Pago : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString.ToString()))
            {
                /*Desencriptar Referencia*/
                var decryptedtextBytes = Convert.FromBase64String(Request.QueryString["referencia"].ToString());
                var output = MachineKey.Unprotect(decryptedtextBytes, "referencia");
                var decryptedText = Encoding.UTF8.GetString(output);

                referencia.Value = decryptedText;

                /*Desencriptar Nombre*/
                var decryptedtextNameBytes = Convert.FromBase64String(Request.QueryString["contribuyente"].ToString());
                var Nameoutput = MachineKey.Unprotect(decryptedtextNameBytes, "contribuyente");
                var NamedecryptedText = Encoding.UTF8.GetString(Nameoutput);
                NombrePersona.Value = NamedecryptedText;

                /*Desencriptar CuentaCatastral*/
                var decryptedCuentaBytes = Convert.FromBase64String(Request.QueryString["cuentacatastral"].ToString());
                var CuentaOutput = MachineKey.Unprotect(decryptedCuentaBytes, "cuentacatastral");
                var cuentadecrptedText = Encoding.UTF8.GetString(CuentaOutput);
                CuentaCatastralPago.Value = cuentadecrptedText;

                /*Desencriptar Precio*/
                var decryptedPrecioBytes = Convert.FromBase64String(Request.QueryString["precio"].ToString());
                var PrecioOutput = MachineKey.Unprotect(decryptedPrecioBytes, "precio");
                var preciodecrptedText = Encoding.UTF8.GetString(PrecioOutput);

                HF_Precio.Value = preciodecrptedText;

            }

            string script = "llamarServicio();";

            ScriptManager.RegisterStartupScript(this, typeof(Page), "carga", script, true);
        }
    }
}