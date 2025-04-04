﻿using ManifestacionEnLinea.Clases;
using ManifestacionEnLinea.DataModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ManifestacionEnLinea
{
    public partial class UbicarPredio : System.Web.UI.Page
    {
        string cve, tipopredio, regimen, std;
        decimal superficie, latitudbd, longitudbd;
        CultureInfo culture = new CultureInfo("es-MX");
        Clase ws = new Clase();
        protected void Page_Load(object sender, EventArgs e)
        {
            Txt_ClaveCatastralUbi.Text = Session["ClaveCatastral"].ToString();
            string ClaveCatastral = Txt_ClaveCatastralUbi.Text;
            GetInformacion(ClaveCatastral);
        }

        protected void btn_SeleccionarTramite_Click(object sender, EventArgs e)
        {
            //Boolean checkCartografia = igualCartografia.Checked;
            Boolean checkAvaluo = CheckboxAvaluo.Checked;
            Boolean checkManifestacion = CheckboxManifestacion.Checked;
            Boolean CheckManifestacionAvaluo = CheckboxManiAvaluo.Checked;
            string cvecatastraloriginal = Session["ClaveCatastral"].ToString();
            string Latitud = Txt_CoordenadaX.Text;
            string Longitud = Txt_CoordenadaY.Text;

            string tramiteSel;
            var normalTextByte = Encoding.UTF8.GetBytes(cvecatastraloriginal);
            var encryptedCode = Convert.ToBase64String(MachineKey.Protect(normalTextByte, "value"));

            var encryptedValue = HttpUtility.UrlEncode(encryptedCode);
            int carto;

            string LatitudP = Request.QueryString["latitud"];
            string LongitudP = Request.QueryString["longitud"];

            COORDENADAS_MANIFESTACION_AVALUO Consulta = ws.BuscarCoordenada(cvecatastraloriginal);

            if (Consulta != null)
            {
                string CooordenadaUtmX = Hf_UTMX.Value;
                string CoordenadaUtmY = Hf_UTMY.Value;
                ws.ActualizarCoordenadas(cvecatastraloriginal, Latitud, Longitud, CooordenadaUtmX, CoordenadaUtmY);

            }
            else
            {
                string CooordenadaUtmX = Hf_UTMX.Value;
                string CoordenadaUtmY = Hf_UTMY.Value;
                ws.InsertarCoordenadas(cvecatastraloriginal, Latitud, Longitud, CooordenadaUtmX, CoordenadaUtmY);
            }

            if (checkAvaluo == true)
            {
                tramiteSel = "AVALÚO CATASTRAL";
                Session["TramiteSeleccionado"] = tramiteSel;
                Response.Redirect("SubirArchivos.aspx");
            }
            if (checkManifestacion == true)
            {
                tramiteSel = "MANIFESTACIÓN DE CONSTRUCCIÓN";
                Session["TramiteSeleccionado"] = tramiteSel;
                Response.Redirect("SubirArchivos.aspx");
            }
            if (CheckManifestacionAvaluo == true)
            {
                tramiteSel = "MANIFESTACIÓN CON AVALÚO";
                Session["TramiteSeleccionado"] = tramiteSel;
                Response.Redirect("SubirArchivos.aspx");
            }
        }

        protected void Btn_RegresarPagina_Click(object sender, EventArgs e)
        {
            Response.Redirect("index.aspx");
        }

        public void GetInformacion(string CuentaCatastral)
        {
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
                    SUPPRIV_TABLE.Text = superficie.ToString();
                }
                conInfoSup.Close();



                //    SqlConnection conInfoConst = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                //    conInfoConst.Open();
                //    SqlCommand cmdInfoConst = new SqlCommand("select DESCRIPCION_TIPO, DESCRIPCION_ESTADO,ANIO,DESCRIPCION_AVANCE,AREA   from GDB010" + Municipio + ".sde.SIS_PC_CONSTRUCCIONES2 where CVE_CAT_ORI='" + CuentaCatastral + "' and STATUSREGISTROTABLA='ACTIVO' order by OBJECTID desc", conInfoConst);
                //    SqlDataReader reader = cmdInfoConst.ExecuteReader();
                //    if (reader.HasRows)
                //    {

                //        GridConst.DataSource = reader;
                //        GridConst.DataBind();
                //        GridViewRow HeaderRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                //        TableCell HeaderCell = new TableCell();
                //        HeaderCell.ColumnSpan = 5;
                //        HeaderCell.Text = "INFORMACIÓN DE CONSTRUCIÓN";
                //        HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                //        GridConst.HeaderStyle.Font.Bold = true;
                //        HeaderRow.Cells.Add(HeaderCell);
                //        //GridConst.Columns[14].HeaderText = "TIPO DE CONSTRUCCIÓN";
                //        GridConst.Controls[0].Controls.AddAt(0, HeaderRow);
                //    }

                //    conInfoConst.Close();
                //}
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
            }
        }
    }
}