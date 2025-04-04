﻿using System;
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
            if (Session["CorreoElectronico"] != null || !string.IsNullOrEmpty(Session["CorreoElectronico"].ToString()))
            {
                /*Desencirptar Correo*/
                string CorreoElectronico = Session["CorreoElectronico"].ToString();

                MensajeFinal.Text = "Revise su correo electronico "+ CorreoElectronico + ", se le envio folio de seguimiento, si no es asi comunicarse al instituto. 449 910 2520  ";
            }
            else
            {
                MensajeFinal.Text = "Revise su correo electronico, se le envio folio de seguimiento, si no es asi comunicarse al instituto. 449 910 2520  ";
            }
            

        }

        protected void ButtonReturn_Click(object sender, EventArgs e)
        {

            Response.Redirect("index.aspx");
        }
    }
}