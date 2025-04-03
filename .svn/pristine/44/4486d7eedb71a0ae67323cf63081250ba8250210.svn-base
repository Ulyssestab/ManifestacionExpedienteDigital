using ManifestacionEnLinea.Clases;
using ManifestacionEnLinea.DataModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace ManifestacionEnLinea
{
    public partial class FormularioPago : System.Web.UI.Page
    {
        Clase ws = new Clase();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                //if (Request.QueryString.ToString().Length != 23)
                //{
                //    var bytes = Convert.FromBase64String(Request.QueryString["value"].ToString());
                //    var output = MachineKey.Unprotect(bytes, "value");
                //    var txtb = Encoding.UTF8.GetString(output);
                //    ClaveOriginal.Text = txtb;
                //}
                //else
                //{
                //    ClaveOriginal.Text = Request.QueryString["value"].ToString();
                //}

                RegisterAsyncTask(new PageAsyncTask(GetWebServiceResponseAsync));
            
            // Haz algo con el resultado
            }
            else
            {

            }
        }

        protected void botonEnviar_Click(object sender, EventArgs e)
        {
            string ClaveCatastral = ClaveOriginal.Text;
            string NombreConstribuyente = nombreContribuyente.Text;
            string Precio = Txt_Total.Text;
            /*Validar que la Clave Catastral Existe*/
            SIS_PC_CLAVE_CATASTRAL ValidarCve = ws.Predio_Clave_Catastral(ClaveCatastral);

            if(ClaveCatastral != "" )
            {
                if(NombreConstribuyente != "")
                {
                    if(ValidarCve != null)
                    {
                        ManifestacionEnLinea.DataModel.FOLIOPAGOAVALUO DBPago = new DataModel.FOLIOPAGOAVALUO();
                        DBPago = ws.BuscarPagoActivo(ClaveCatastral);
                        string Municipio = ClaveCatastral.Substring(0, 2);

                        string ReferenciaPagoNuevo = string.Empty;
                        if (DBPago != null)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "eliminarPagoActivo();", true);
                        }
                        else
                        {
                            ManifestacionEnLinea.DataModel.FOLIOPAGOAVALUO DBReferencia = new DataModel.FOLIOPAGOAVALUO();
                            DBReferencia = ws.CrearReferenciaPagoAvaluo(ClaveCatastral);
                            ReferenciaPagoNuevo = ws.CrearReferenciaPagoAvaluoNuevo(DBReferencia.REFERENCIAFOLIO);


                            if (DBReferencia != null)
                            {
                                /*Encriptar Referencia*/
                                var plaintextBytes = Encoding.UTF8.GetBytes(ReferenciaPagoNuevo);
                                var encryptedCode = Convert.ToBase64String(MachineKey.Protect(plaintextBytes, "referencia"));
                                var encryptedValue = HttpUtility.UrlEncode(encryptedCode);

                                /*Encriptar Nombre*/
                                var NametextBytes = Encoding.UTF8.GetBytes(NombreConstribuyente);
                                var encryptedNameCode = Convert.ToBase64String(MachineKey.Protect(NametextBytes, "contribuyente"));
                                var encryptedNameValue = HttpUtility.UrlEncode(encryptedNameCode);

                                /*Encriptar cuenta catastral*/
                                var cuentatextBytes = Encoding.UTF8.GetBytes(ClaveCatastral);
                                var encryptedCuentaCode = Convert.ToBase64String(MachineKey.Protect(cuentatextBytes, "cuentacatastral"));
                                var encryptedCuentaValue = HttpUtility.UrlEncode(encryptedCuentaCode);

                                /*Encriptar Precio*/
                                var Preciotextbytes = Encoding.UTF8.GetBytes(ClaveCatastral);
                                var EncryptedPrecioCode = Convert.ToBase64String(MachineKey.Protect(Preciotextbytes, "precio"));
                                var encryptedPrecioValue = HttpUtility.UrlEncode(EncryptedPrecioCode);

                                ws.InsertarPago(ClaveCatastral, ReferenciaPagoNuevo, NombreConstribuyente);
                                Response.Redirect("Pago.aspx?referencia=" + encryptedValue + "&contribuyente=" + encryptedNameValue + "&cuentacatastral=" + encryptedCuentaValue + "&precio=" + encryptedPrecioValue);

                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "warningsalert('Error al generar referencia de pago.');", true);
                            }

                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "warningsalert('No se encontró la clave catastral ingresada, revisar la longitud y los dígitos o Comunicarse con Catastro.');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "warningsalert('Ingresar nombre.');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "warningsalert('Ingresar una clave catastral.');", true);
            }
            
        }

        protected void BotonResetPago_Click(object sender, EventArgs e)
        {

            string cvecatastro = ClaveOriginal.Text;
            ws.actualizarPago(cvecatastro);
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "sussessalert('Da clic nuevamente en Realizar Pago.');", true);
        }

        protected void BotonRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("index.aspx");
        }

        private async Task GetWebServiceResponseAsync()
        {
            string url = "https://epagos.aguascalientes.gob.mx/SEFI/ServiciosSIIF/ServiceConcepto.asmx/GetDataByConcepto?concepto=43060703";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string response = await client.GetStringAsync(url);
                    ResponseData data = JsonConvert.DeserializeObject<ResponseData>(response);
                    double valor = data.Valor;
                    Txt_Total.Text = valor.ToString("F2", new CultureInfo("es-MX"));

                }
                catch (HttpRequestException e)
                {
                    // Manejar el error
                    
                }
            }
        }
    }
}