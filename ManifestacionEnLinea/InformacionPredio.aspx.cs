using ManifestacionEnLinea.Clases;
using ManifestacionEnLinea.DataModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Text;
using System.Web.Security;
using System.Data;
using System.Web.Script.Serialization;
using System.Globalization;
using System.Threading;

namespace ManifestacionEnLinea
{
    public partial class InformacionPredio : System.Web.UI.Page
    {
        string cve, tipopredio, regimen, std;
        decimal superficie, latitudbd, longitudbd;
        CultureInfo culture = new CultureInfo("es-MX");

        protected void botonreturnP_Click(object sender, EventArgs e)
        {
            Response.Redirect("index.aspx");
        }

        protected void BotonEnviar_Click(object sender, EventArgs e)
        {
            string clavecatastral = ClaveCatastralOculta.Value;
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "PagarAvaluoEnLinea('"+ clavecatastral + "');", true);
        }

        Clase ws = new Clase();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString.ToString()))
            {
                /*Desencriptar clave catastral*/
                var decryptedtextBytes = Convert.FromBase64String(Request.QueryString["value"].ToString());
                var output = MachineKey.Unprotect(decryptedtextBytes, "value");
                var decryptedText = Encoding.UTF8.GetString(output);
                ClaveCatastralOculta.Value = decryptedText;

                string LatitudP = Request.QueryString["latitud"];
                string LongitudP = Request.QueryString["longitud"];
                if (!string.IsNullOrEmpty(LatitudP) || !string.IsNullOrEmpty(LongitudP))
                {
                   

                    GetInformacion(decryptedText);
                }
                else
                {
                    hiddenLatitud.Value = LatitudP;
                    hiddenLongitud.Value = LongitudP;
                    CoordenadaX.Text = LongitudP;
                    CoordenadaY.Text = LatitudP;
                    GetInformacion(decryptedText);
                }
            }
            
        }
        public void GetInformacion(string CuentaCatastral )
        {
            //CultureInfo cul = new CultureInfo("es-MX");
            if (CuentaCatastral != null)
            {
                

                string Municipio = CuentaCatastral.Substring(0, 2);
                SqlConnection conInfoSup = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                conInfoSup.Open();
                SqlCommand cmdInfoSup = new SqlCommand("select top 1 * from GDB010" + Municipio + ".sde.SIS_PC_SUPERFICIES2 where CVE_CAT_ORI='" + CuentaCatastral + "' and STATUSREGISTROTABLA='ACTIVO' order by OBJECTID desc", conInfoSup);
                using (SqlDataReader readerInfoSup = cmdInfoSup.ExecuteReader())
                {
                    while (readerInfoSup.Read())
                    {
                        //std = readerInfoSup.GetString(10);//clave catastral estandar
                        if (!readerInfoSup.IsDBNull(10))
                        {
                            std = readerInfoSup.GetString(10);//clave catastral estandar
                        }
                        else
                        {
                            std = "";
                        }
                        cve = readerInfoSup.GetString(11);//Clave catastral Original
                        tipopredio = readerInfoSup.GetString(17);//tipo de predio Urbano, Rustico, Transicicion
                        regimen = readerInfoSup.GetString(14);//tipo de regimen Privado o Condominio

                        if (!readerInfoSup.IsDBNull(23))
                        {
                            superficie = readerInfoSup.GetDecimal(23);//Superficie Privativa
                        }
                        else
                        {
                            superficie = 0;
                        }

                    }

                    CVE_TABLE.Text = cve;
                    TIPO_TABLE.Text = tipopredio;
                    REGIMEN_TABLE.Text = regimen;
                    hiddenRegimen.Value = regimen;
                    SUPPRIV_TABLE.Text = superficie.ToString("N",culture) +" m2";
                }
                conInfoSup.Close();



                //SqlConnection conInfoConst = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                //conInfoConst.Open();
                //SqlCommand cmdInfoConst = new SqlCommand("select DESCRIPCION_TIPO, DESCRIPCION_ESTADO,ANIO,DESCRIPCION_AVANCE,AREA   from GDB010" + Municipio + ".sde.SIS_PC_CONSTRUCCIONES2 where CVE_CAT_ORI='" + CuentaCatastral + "' and STATUSREGISTROTABLA='ACTIVO' order by OBJECTID desc", conInfoConst);
                //SqlDataReader reader = cmdInfoConst.ExecuteReader();
                //if (reader.HasRows)
                //{

                //    GridConst.DataSource = reader;
                //    GridConst.DataBind();
                //    GridViewRow HeaderRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                //    TableCell HeaderCell = new TableCell();
                //    HeaderCell.ColumnSpan = 5;
                //    HeaderCell.Text = "INFORMACIÓN DE CONSTRUCIÓN";
                //    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                //    GridConst.HeaderStyle.Font.Bold = true;
                //    HeaderRow.Cells.Add(HeaderCell);
                //    //GridConst.Columns[14].HeaderText = "TIPO DE CONSTRUCCIÓN";
                //    GridConst.Controls[0].Controls.AddAt(0, HeaderRow);
                //}
                //else
                //{
                //    tableSC.Attributes.Add("style", "display:block");
                //}
                //conInfoConst.Close();
                string connectionString = ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString();
                using (SqlConnection conInfoConst = new SqlConnection(connectionString))
                {
                    conInfoConst.Open();
                    string query = "SELECT DESCRIPCION_TIPO, DESCRIPCION_ESTADO, ANIO, DESCRIPCION_AVANCE, AREA " +
                                   "FROM GDB010" + Municipio + ".sde.SIS_PC_CONSTRUCCIONES2 " +
                                   "WHERE CVE_CAT_ORI=@CuentaCatastral AND STATUSREGISTROTABLA='ACTIVO' " +
                                   "ORDER BY OBJECTID DESC";

                    using (SqlCommand cmdInfoConst = new SqlCommand(query, conInfoConst))
                    {
                        cmdInfoConst.Parameters.AddWithValue("@CuentaCatastral", CuentaCatastral);
                        using (SqlDataReader reader = cmdInfoConst.ExecuteReader())
                        {
                            List<Construccion> construcciones = new List<Construccion>();

                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    Construccion construccion = new Construccion
                                    {
                                        DESCRIPCION_TIPO = reader["DESCRIPCION_TIPO"].ToString(),
                                        DESCRIPCION_ESTADO = reader["DESCRIPCION_ESTADO"].ToString(),
                                        ANIO = Convert.ToInt32(reader["ANIO"]),
                                        DESCRIPCION_AVANCE = reader["DESCRIPCION_AVANCE"].ToString(),
                                        AREA = Convert.ToDecimal(reader["AREA"]).ToString("N", culture)
                                    };
                                    construcciones.Add(construccion);
                                } 
                            }
                            else
                            {
                                Construccion sinConstrucciones = new Construccion
                                {
                                    DESCRIPCION_TIPO = "SIN CONSTRUCCIONES",
                                    DESCRIPCION_ESTADO = string.Empty,
                                    ANIO = null,
                                    DESCRIPCION_AVANCE = string.Empty,
                                    AREA = string.Empty
                                };
                                construcciones.Add(sinConstrucciones);
                            }

                           
                            GridConst.DataSource = construcciones;
                            GridConst.DataBind();
                        }
                    }
                }


                SqlConnection conInfoCoordenada = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                conInfoCoordenada.Open();
                SqlCommand cmdInfoCoordenada = new SqlCommand("select top 1 * from GDB010" + Municipio + ".sde.SIS_PC_CENTROIDES where CVE_CAT_EST='" + std + "' and STATUSREGISTROTABLA='ACTIVO' order by OBJECTID desc", conInfoCoordenada);
                using (SqlDataReader readerInfoCoordenada = cmdInfoCoordenada.ExecuteReader())
                {
                    if (readerInfoCoordenada.HasRows)
                    {
                        while (readerInfoCoordenada.Read())
                        {
                            latitudbd = readerInfoCoordenada.GetDecimal(15);//Centroide X
                            longitudbd = readerInfoCoordenada.GetDecimal(16);//Centroide Y
                        }
                        hiddenLatitud.Value = latitudbd.ToString();
                        CoordenadaX.Text = latitudbd.ToString("0.##");
                        hiddenLongitud.Value = longitudbd.ToString();
                        CoordenadaY.Text = longitudbd.ToString("0.##");
                    }
                    else
                    {
                        string LatitudP = Request.QueryString["latitud"].ToString();
                        string LongitudP = Request.QueryString["longitud"].ToString();
                        hiddenLatitud.Value = LatitudP;
                        CoordenadaX.Text = LatitudP.Substring(0, 10);
                        hiddenLongitud.Value = LongitudP;
                        CoordenadaY.Text = LongitudP.Substring(0, 11);

                    }

                }
                conInfoCoordenada.Close();

                string VAR_ResultCoordinate = string.Empty;
                string[] coordenadas = new string[0];
                int VAR_CantidadCoordendas = 0;
                List<string> listadeCoordenadas = new List<string>();
                string conexionGDBTL = System.Configuration.ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ConnectionString;
                using (SqlConnection connectionP = new SqlConnection(conexionGDBTL))
                {
                    connectionP.Open();

                    using (SqlCommand command = new SqlCommand("ObtenerPoligonoP", connectionP))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Agregar parámetros al procedimiento almacenado
                        command.Parameters.AddWithValue("@CVE_CAT_ORI", CuentaCatastral);

                        // Ejecutar el procedimiento almacenado
                        using (SqlDataReader readerP = command.ExecuteReader())
                        {
                            while (readerP.Read())
                            {
                                // Acceder a los campos del resultado utilizando el índice o el nombre del campo
                                object geomValue = readerP.GetValue(0);
                                VAR_ResultCoordinate = geomValue.ToString();
                                coordenadas = VAR_ResultCoordinate.Split(',');
                                VAR_CantidadCoordendas = coordenadas.Length;

                            }

                        }
                    }
                    connectionP.Close();
                }
                for (int x = 0; x < VAR_CantidadCoordendas; x++)
                {
                    if (coordenadas[x].Contains("POLYGON"))
                    {
                        string VAR_SinEspacio1 = coordenadas[x].Trim();
                        string VAR_SinPolygon = VAR_SinEspacio1.Replace("POLYGON ((", "");
                        listadeCoordenadas.Add(VAR_SinPolygon);
                    }
                    else
                    {
                        if (coordenadas[x].Contains("))"))
                        {
                            string VAR_SinEspacio2 = coordenadas[x].Trim();
                            string VAR_Sinparenectecis = VAR_SinEspacio2.Replace(")", "");
                            listadeCoordenadas.Add(VAR_Sinparenectecis);
                        }
                        else
                        {
                            string VAR_SinEspacio3 = coordenadas[x].Trim();
                            //punto = VAR_SinEspacio.Split(' ');
                            listadeCoordenadas.Add(VAR_SinEspacio3);
                        }

                    }
                }
                JavaScriptSerializer js = new JavaScriptSerializer();


                string json = js.Serialize(new { var1 = listadeCoordenadas });

                //Enviar la lista al cliente a través de una respuesta de JavaScript.
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "lista", "var lista = " + json + ";", true);

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "warningsalert('Error inesperado, comunicarse con el administrador');", true);
            }


        }
        protected void btnUploadArchivos_Click(object sender, EventArgs e)
        {
            //Boolean checkCartografia = igualCartografia.Checked;
            Boolean checkAvaluo = CheckboxAvaluo.Checked;
            Boolean checkManifestacion = CheckboxManifestacion.Checked;
            Boolean CheckManifestacionAvaluo = CheckboxManiAvaluo.Checked;
            string cvecatastraloriginal = ClaveCatastralOculta.Value;
            string tramiteSel;
            var normalTextByte = Encoding.UTF8.GetBytes(cvecatastraloriginal);
            var encryptedCode = Convert.ToBase64String(MachineKey.Protect(normalTextByte,"value"));

            var encryptedValue = HttpUtility.UrlEncode(encryptedCode);
            int carto;
            //if (checkCartografia == true)
            //{
            //     carto = 1;
            //}
            //else
            //{
            //    carto = 0;
            //}

            string LatitudP = Request.QueryString["latitud"];
            string LongitudP = Request.QueryString["longitud"];

            if (checkAvaluo == true)
            {
                tramiteSel = "AVALÚO CATASTRAL";
                Response.Redirect("SubirArchivos.aspx?value="+ encryptedValue + "&tramite="+tramiteSel + "&latitud=" + LatitudP + "&longitud="+ LongitudP);
            }
            if (checkManifestacion == true)
            {
                tramiteSel = "MANIFESTACIÓN DE CONSTRUCCIÓN";
                Response.Redirect("SubirArchivos.aspx?value="+ encryptedValue + "&tramite="+ tramiteSel + "&latitud=" + LatitudP + "&longitud=" + LongitudP);
            }
            if (CheckManifestacionAvaluo == true)
            {
                tramiteSel = "MANIFESTACIÓN CON AVALÚO";
                Response.Redirect("SubirArchivos.aspx?value=" + encryptedValue + "&tramite="+ tramiteSel + "&latitud=" + LatitudP + "&longitud=" + LongitudP);
            }
        }
    }

   
}
