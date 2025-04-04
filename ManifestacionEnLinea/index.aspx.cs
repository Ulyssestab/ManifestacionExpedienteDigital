﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using ManifestacionEnLinea.Clases;
using ManifestacionEnLinea.DataModel;
using iText.Barcodes;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.Text;
using System.Web.Security;

namespace ManifestacionEnLinea
{
    public partial class index : System.Web.UI.Page
    {
        public static string ClaveCatastralOri { get; set; }
        Clase ws = new Clase();
        string CAPAP;
        string IPServidor = ConfigurationManager.AppSettings["IPServidor"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            TxtMunicipio.Attributes.Add("readonly", "readonly");
            if (!Page.IsPostBack)
            {
                ///*Comprobar conexion SQL*/
                //bool prueba = ws.Probarconexion();
                //if (prueba == false)
                //{
                //    Mantenimiento.Visible = true;
                //    form1.Visible = false;
                //}
                //else
                //{
                //    Mantenimiento.Visible = false;
                //    form1.Visible = true;
                //}
            }

        }

        protected void BuscarInfo_Click(object sender, EventArgs e)
        {
            string Municipio = TxtMunicipio.Text;
            string Localidad = TxtLocalidad.Text;
            string Sector = TxtSector.Text;
            string Manzana = TxtManzana.Text;
            string Predio = TxtPredio.Text;
            string Condominio = TxtCondominio.Text;
            string ClaveCatastralOriginal = Municipio + Localidad + Sector + Manzana + Predio + Condominio;
            string ClaveCatastralEstandar = string.Empty;

            if (ClaveCatastralOriginal.Length != 17)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "warningsalert('Verificar la clave catastral ingresada');", true);
            }
            else
            {
                /*Verificar que la clave Exista en SIS_PC_CLAVE_CATASTRAL*/
                ManifestacionEnLinea.DataModel.SIS_PC_CLAVE_CATASTRAL dblib = new SIS_PC_CLAVE_CATASTRAL();
                dblib = ws.Predio_Clave_Catastral(ClaveCatastralOriginal);

                ManifestacionEnLinea.DataModel.SIS_PC_SUPERFICIES2 dblibSup = new SIS_PC_SUPERFICIES2();
                dblibSup = ws.InfoSuperficie(ClaveCatastralOriginal);

                /*Si existe*/
                if (dblib != null || dblibSup != null )
                {

                    ClaveCatastralEstandar = dblib.CVE_CAT_EST;
                    if(ClaveCatastralEstandar == null)
                    {
                        ClaveCatastralEstandar = dblibSup.CVE_CAT_EST;
                    }

                    /*Validar que no tiene abstencion */
                    ManifestacionEnLinea.DataModel.SIS_TRACAT_ABSTENCIONES dbAbstencion = new SIS_TRACAT_ABSTENCIONES();
                    dbAbstencion = ws.ValidarAbstencion(ClaveCatastralOriginal);
                    if (dbAbstencion != null)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "warningsalert('No se puede ingresar Trámite de esta Clave');", true);
                    }
                    else
                    {
                        /*Validar si no hay un tramite activo */
                        ManifestacionEnLinea.DataModel.Tramite DBTram = new Tramite();
                        DBTram = ws.ValidarTramite(ClaveCatastralOriginal);
                        if (DBTram != null)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "warningsalert('Trámite involucrado en proceso');", true);
                        }
                        else
                        {
                            ValidarCartografia(Municipio, ClaveCatastralOriginal, ClaveCatastralEstandar);
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "warningsalert('La clave ingresada no existe.');", true);
                }
            }
        }

        protected void ServerCveValidar_Click(object sender, EventArgs e)
        {
            string MunCve = TxtMunicipio.Text;
            string LocCve = TxtLocalidad.Text;
            string SecCve = TxtSector.Text;
            string ManCve = TxtManzana.Text;
            string PreCve = TxtPredio.Text;
            string ConCve = TxtCondominio.Text;
            string CveCompleta = MunCve + LocCve + SecCve + ManCve + PreCve + ConCve;

            if (CveCompleta.Length != 17)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "warningsalert('La clave ingresada es incorrecta.');", true);
            }
            else
            {
                /*Validar si Existe*/
                ManifestacionEnLinea.DataModel.SIS_PC_CLAVE_CATASTRAL dblib = new SIS_PC_CLAVE_CATASTRAL();
                dblib = ws.Predio_Clave_Catastral(CveCompleta);

                /*Si existe*/
                if (dblib != null)
                {
                    /*Validar que no tiene abstencion */
                    ManifestacionEnLinea.DataModel.SIS_TRACAT_ABSTENCIONES dbAbstencion = new SIS_TRACAT_ABSTENCIONES();
                    dbAbstencion = ws.ValidarAbstencion(CveCompleta);
                    if (dbAbstencion != null)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "warningsalert('No se puede ingresar Trámite de esta Clave');", true);
                    }
                    else
                    {
                        var plaintextBytes = Encoding.UTF8.GetBytes(CveCompleta);
                        var encryptedCode = Convert.ToBase64String(MachineKey.Protect(plaintextBytes, "value"));
                        var encryptedValue = HttpUtility.UrlEncode(encryptedCode);
                        string url = "FormularioPago.aspx?value="+ encryptedValue;
                        string script = "window.open('" + url + "');";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "openWindow", script, true);
                        //Response.Redirect("FormularioPago.aspx?value=" + encryptedValue);

                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "warningsalert('La clave ingresada no existe.');", true);
                }
            }
        }

        protected void ValidarCartografia(string Municipio, string ClaveCatastralOriginal, string ClaveCatastralEstandar)
        {
            /*Encriptar la clave catastral*/
            var normalTextByte = Encoding.UTF8.GetBytes(ClaveCatastralOriginal);
            var encryptedCode = Convert.ToBase64String(MachineKey.Protect(normalTextByte, "value"));
            var encryptedValue = HttpUtility.UrlEncode(encryptedCode);
            string[] arregloCoordenada = new string[4];

            /*Variables Coordenada*/
            var Superficie = "";
            var Construcciones = new ArrayList();
            string[] CoordenadasUTMSup;
            
            string[] fullCoordenadas = new string[4];
            var CoordenadasUTMSinEspacios = new ArrayList();
            string LongitudUTM = string.Empty;
            string LatitudUTM = string.Empty;
            string newsup = string.Empty;
            string sinEspacios = string.Empty;
            string newsup2 = string.Empty;
            string newsup3 = string.Empty;

            /*REVISAR SI ESTA EN CENTROIDES*/
            ManifestacionEnLinea.DataModel.SIS_PC_CENTROIDES dblibCent = new SIS_PC_CENTROIDES();
            dblibCent = ws.CoordenadaCentro(ClaveCatastralEstandar);
            /*Si encontramos centroide*/
            if(dblibCent != null)
            {
                LongitudUTM = dblibCent.CENT_PRED_X.ToString();
                LatitudUTM = dblibCent.CENT_PRED_Y.ToString();

            }
            else
            {
                SqlConnection cnnCveP = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP.Open();
                SqlCommand cmdP = new SqlCommand("select * from GDB010" + Municipio + ".sde.P where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP);
                using (SqlDataReader drP = cmdP.ExecuteReader())
                {
                    while (drP.Read())
                    {
                        Superficie = Convert.ToString(drP["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string [] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP.Close();

                SqlConnection cnnCveP_01 = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP_01.Open();
                SqlCommand cmdP_01 = new SqlCommand("select * from GDB010" + Municipio + ".sde.P_01 where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP_01);
                using (SqlDataReader drP_01 = cmdP_01.ExecuteReader())
                {
                    while (drP_01.Read())
                    {
                        Superficie = Convert.ToString(drP_01["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP_01.Close();

                SqlConnection cnnCveP_02 = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP_02.Open();
                SqlCommand cmdP_02 = new SqlCommand("select * from GDB010" + Municipio + ".sde.P_02 where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP_02);
                using (SqlDataReader drP_02 = cmdP_02.ExecuteReader())
                {
                    while (drP_02.Read())
                    {
                        Superficie = Convert.ToString(drP_02["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP_02.Close();

                SqlConnection cnnCveP_03 = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP_03.Open();
                SqlCommand cmdP_03 = new SqlCommand("select * from GDB010" + Municipio + ".sde.P_02 where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP_03);
                using (SqlDataReader drP_03 = cmdP_03.ExecuteReader())
                {
                    while (drP_03.Read())
                    {
                        Superficie = Convert.ToString(drP_03["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP_03.Close();

                SqlConnection cnnCveP_04 = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP_04.Open();
                SqlCommand cmdP_04 = new SqlCommand("select * from GDB010" + Municipio + ".sde.P_04 where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP_04);
                using (SqlDataReader drP_04 = cmdP_04.ExecuteReader())
                {
                    while (drP_04.Read())
                    {
                        Superficie = Convert.ToString(drP_04["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP_04.Close();

                SqlConnection cnnCveP_05 = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP_05.Open();
                SqlCommand cmdP_05 = new SqlCommand("select * from GDB010" + Municipio + ".sde.P_05 where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP_05);
                using (SqlDataReader drP_05 = cmdP_05.ExecuteReader())
                {
                    while (drP_05.Read())
                    {
                        Superficie = Convert.ToString(drP_05["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP_05.Close();

                SqlConnection cnnCveP00 = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP00.Open();
                SqlCommand cmdP00 = new SqlCommand("select * from GDB010" + Municipio + ".sde.P00 where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP00);
                using (SqlDataReader drP00 = cmdP00.ExecuteReader())
                {
                    while (drP00.Read())
                    {
                        Superficie = Convert.ToString(drP00["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP00.Close();

                /*P01*/
                SqlConnection cnnCveP01 = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP01.Open();
                SqlCommand cmdP01 = new SqlCommand("select * from GDB010" + Municipio + ".sde.P01 where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP01);
                using (SqlDataReader drP01 = cmdP01.ExecuteReader())
                {
                    while (drP01.Read())
                    {
                        Superficie = Convert.ToString(drP01["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP01.Close();

                /*P02*/
                SqlConnection cnnCveP02 = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP02.Open();
                SqlCommand cmdP02 = new SqlCommand("select * from GDB010" + Municipio + ".sde.P02 where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP02);
                using (SqlDataReader drP02 = cmdP02.ExecuteReader())
                {
                    while (drP02.Read())
                    {
                        Superficie = Convert.ToString(drP02["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP02.Close();

                /*P03*/
                SqlConnection cnnCveP03 = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP03.Open();
                SqlCommand cmdP03 = new SqlCommand("select * from GDB010" + Municipio + ".sde.P03 where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP03);
                using (SqlDataReader drP03 = cmdP03.ExecuteReader())
                {
                    while (drP03.Read())
                    {
                        Superficie = Convert.ToString(drP03["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP03.Close();

                /*P04*/
                SqlConnection cnnCveP04 = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP04.Open();
                SqlCommand cmdP04 = new SqlCommand("select * from GDB010" + Municipio + ".sde.P04 where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP04);
                using (SqlDataReader drP04 = cmdP04.ExecuteReader())
                {
                    while (drP04.Read())
                    {
                        Superficie = Convert.ToString(drP04["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP04.Close();

                /*P05*/
                SqlConnection cnnCveP05 = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP05.Open();
                SqlCommand cmdP05 = new SqlCommand("select * from GDB010" + Municipio + ".sde.P05 where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP05);
                using (SqlDataReader drP05 = cmdP05.ExecuteReader())
                {
                    while (drP05.Read())
                    {
                        Superficie = Convert.ToString(drP05["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP05.Close();

                /*P06*/
                SqlConnection cnnCveP06 = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP06.Open();
                SqlCommand cmdP06 = new SqlCommand("select * from GDB010" + Municipio + ".sde.P06 where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP06);
                using (SqlDataReader drP06 = cmdP06.ExecuteReader())
                {
                    while (drP06.Read())
                    {
                        Superficie = Convert.ToString(drP06["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP06.Close();

                /*P07*/
                SqlConnection cnnCveP07 = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP07.Open();
                SqlCommand cmdP07 = new SqlCommand("select * from GDB010" + Municipio + ".sde.P07 where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP07);
                using (SqlDataReader drP07 = cmdP07.ExecuteReader())
                {
                    while (drP07.Read())
                    {
                        Superficie = Convert.ToString(drP07["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP07.Close();

                /*P08*/
                SqlConnection cnnCveP08 = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP08.Open();
                SqlCommand cmdP08 = new SqlCommand("select * from GDB010" + Municipio + ".sde.P08 where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP08);
                using (SqlDataReader drP08 = cmdP08.ExecuteReader())
                {
                    while (drP08.Read())
                    {
                        Superficie = Convert.ToString(drP08["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP08.Close();

                SqlConnection cnnCveP09 = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP09.Open();
                SqlCommand cmdP09 = new SqlCommand("select * from GDB010" + Municipio + ".sde.P09 where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP09);
                using (SqlDataReader drP09 = cmdP09.ExecuteReader())
                {
                    while (drP09.Read())
                    {
                        Superficie = Convert.ToString(drP09["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP09.Close();

                /*P10*/
                SqlConnection cnnCveP10 = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP10.Open();
                SqlCommand cmdP10 = new SqlCommand("select * from GDB010" + Municipio + ".sde.P10 where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP10);
                using (SqlDataReader drP10 = cmdP10.ExecuteReader())
                {
                    while (drP10.Read())
                    {
                        Superficie = Convert.ToString(drP10["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP10.Close();

                /*P11*/
                SqlConnection cnnCveP11 = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP11.Open();
                SqlCommand cmdP11 = new SqlCommand("select * from GDB010" + Municipio + ".sde.P11 where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP11);
                using (SqlDataReader drP11 = cmdP11.ExecuteReader())
                {
                    while (drP11.Read())
                    {
                        Superficie = Convert.ToString(drP11["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP11.Close();

                /*P12*/
                SqlConnection cnnCveP12 = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP12.Open();
                SqlCommand cmdP12 = new SqlCommand("select * from GDB010" + Municipio + ".sde.P12 where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP12);
                using (SqlDataReader drP12 = cmdP12.ExecuteReader())
                {
                    while (drP12.Read())
                    {
                        Superficie = Convert.ToString(drP12["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP12.Close();

                /*P13*/
                SqlConnection cnnCveP13 = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP13.Open();
                SqlCommand cmdP13 = new SqlCommand("select * from GDB010" + Municipio + ".sde.P13 where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP13);
                using (SqlDataReader drP13 = cmdP13.ExecuteReader())
                {
                    while (drP13.Read())
                    {
                        Superficie = Convert.ToString(drP13["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP13.Close();

                /*P14*/
                SqlConnection cnnCveP14 = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP14.Open();
                SqlCommand cmdP14 = new SqlCommand("select * from GDB010" + Municipio + ".sde.P14 where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP14);
                using (SqlDataReader drP14 = cmdP14.ExecuteReader())
                {
                    while (drP14.Read())
                    {
                        Superficie = Convert.ToString(drP14["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP14.Close();

                /*p15*/
                SqlConnection cnnCveP15 = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP15.Open();
                SqlCommand cmdP15 = new SqlCommand("select * from GDB010" + Municipio + ".sde.P15 where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP15);
                using (SqlDataReader drP15 = cmdP15.ExecuteReader())
                {
                    while (drP15.Read())
                    {
                        Superficie = Convert.ToString(drP15["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP15.Close();

                /*P16*/
                SqlConnection cnnCveP16 = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP16.Open();
                SqlCommand cmdP16 = new SqlCommand("select * from GDB010" + Municipio + ".sde.P16 where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP16);
                using (SqlDataReader drP16 = cmdP16.ExecuteReader())
                {
                    while (drP16.Read())
                    {
                        Superficie = Convert.ToString(drP16["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP16.Close();

                /*P17*/
                SqlConnection cnnCveP17 = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP17.Open();
                SqlCommand cmdP17 = new SqlCommand("select * from GDB010" + Municipio + ".sde.P17 where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP17);
                using (SqlDataReader drP17 = cmdP17.ExecuteReader())
                {
                    while (drP17.Read())
                    {
                        Superficie = Convert.ToString(drP17["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP17.Close();

                /*P18*/
                SqlConnection cnnCveP18 = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP18.Open();
                SqlCommand cmdP18 = new SqlCommand("select * from GDB010" + Municipio + ".sde.P18 where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP18);
                using (SqlDataReader drP18 = cmdP18.ExecuteReader())
                {
                    while (drP18.Read())
                    {
                        Superficie = Convert.ToString(drP18["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP18.Close();

                /*P19*/
                SqlConnection cnnCveP19 = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP19.Open();
                SqlCommand cmdP19 = new SqlCommand("select * from GDB010" + Municipio + ".sde.P19 where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP19);
                using (SqlDataReader drP19 = cmdP19.ExecuteReader())
                {
                    while (drP19.Read())
                    {
                        Superficie = Convert.ToString(drP19["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP19.Close();

                /*P20*/
                SqlConnection cnnCveP20 = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP20.Open();
                SqlCommand cmdP20 = new SqlCommand("select * from GDB010" + Municipio + ".sde.P20 where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP20);
                using (SqlDataReader drP20 = cmdP20.ExecuteReader())
                {
                    while (drP20.Read())
                    {
                        Superficie = Convert.ToString(drP20["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP20.Close();

                /*P_VW*/
                SqlConnection cnnCveP_VW = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP_VW.Open();
                SqlCommand cmdP_VW = new SqlCommand("select * from GDB010" + Municipio + ".sde.P_VW where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP_VW);
                using (SqlDataReader drP_VW = cmdP_VW.ExecuteReader())
                {
                    while (drP_VW.Read())
                    {
                        Superficie = Convert.ToString(drP_VW["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP_VW.Close();

                /*P00_VW*/
                SqlConnection cnnCveP00_VW = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP00_VW.Open();
                SqlCommand cmdP00_VW = new SqlCommand("select * from GDB010" + Municipio + ".sde.P00_VW where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP00_VW);
                using (SqlDataReader drP00_VW = cmdP00_VW.ExecuteReader())
                {
                    while (drP00_VW.Read())
                    {
                        Superficie = Convert.ToString(drP00_VW["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP00_VW.Close();

                /*P01_VW*/
                SqlConnection cnnCveP01_VW = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP01_VW.Open();
                SqlCommand cmdP01_VW = new SqlCommand("select * from GDB010" + Municipio + ".sde.P01_VW where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP01_VW);
                using (SqlDataReader drP01_VW = cmdP01_VW.ExecuteReader())
                {
                    while (drP01_VW.Read())
                    {
                        Superficie = Convert.ToString(drP01_VW["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP01_VW.Close();

                /*P02_VW*/
                SqlConnection cnnCveP02_VW = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP02_VW.Open();
                SqlCommand cmdP02_VW = new SqlCommand("select * from GDB010" + Municipio + ".sde.P02_VW where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP02_VW);
                using (SqlDataReader drP02_VW = cmdP02_VW.ExecuteReader())
                {
                    while (drP02_VW.Read())
                    {
                        Superficie = Convert.ToString(drP02_VW["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP02_VW.Close();

                /*P03_VW*/
                SqlConnection cnnCveP03_VW = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP03_VW.Open();
                SqlCommand cmdP03_VW = new SqlCommand("select * from GDB010" + Municipio + ".sde.P03_VW where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP03_VW);
                using (SqlDataReader drP03_VW = cmdP03_VW.ExecuteReader())
                {
                    while (drP03_VW.Read())
                    {
                        Superficie = Convert.ToString(drP03_VW["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP03_VW.Close();

                /*P04_VW*/
                SqlConnection cnnCveP04_VW = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP04_VW.Open();
                SqlCommand cmdP04_VW = new SqlCommand("select * from GDB010" + Municipio + ".sde.P04_VW where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP04_VW);
                using (SqlDataReader drP04_VW = cmdP04_VW.ExecuteReader())
                {
                    while (drP04_VW.Read())
                    {
                        Superficie = Convert.ToString(drP04_VW["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP04_VW.Close();

                /*P05_VW*/
                SqlConnection cnnCveP05_VW = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP05_VW.Open();
                SqlCommand cmdP05_VW = new SqlCommand("select * from GDB010" + Municipio + ".sde.P05_VW where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP05_VW);
                using (SqlDataReader drP05_VW = cmdP05_VW.ExecuteReader())
                {
                    while (drP05_VW.Read())
                    {
                        Superficie = Convert.ToString(drP05_VW["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP05_VW.Close();

                /*P06*/
                SqlConnection cnnCveP06_VW = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP06_VW.Open();
                SqlCommand cmdP06_VW = new SqlCommand("select * from GDB010" + Municipio + ".sde.P06_VW where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP06_VW);
                using (SqlDataReader drP06_VW = cmdP06_VW.ExecuteReader())
                {
                    while (drP06_VW.Read())
                    {
                        Superficie = Convert.ToString(drP06_VW["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP06_VW.Close();

                /*P07_VW*/
                SqlConnection cnnCveP07_VW = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP07_VW.Open();
                SqlCommand cmdP07_VW = new SqlCommand("select * from GDB010" + Municipio + ".sde.P07_VW where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP07_VW);
                using (SqlDataReader drP07_VW = cmdP07_VW.ExecuteReader())
                {
                    while (drP07_VW.Read())
                    {
                        Superficie = Convert.ToString(drP07_VW["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP07_VW.Close();

                /*P08*/
                SqlConnection cnnCveP08_VW = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP08_VW.Open();
                SqlCommand cmdP08_VW = new SqlCommand("select * from GDB010" + Municipio + ".sde.P08_VW where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP08_VW);
                using (SqlDataReader drP08_VW = cmdP08_VW.ExecuteReader())
                {
                    while (drP08_VW.Read())
                    {
                        Superficie = Convert.ToString(drP08_VW["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP08_VW.Close();

                /*P09_VW*/
                SqlConnection cnnCveP09_VW = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP09_VW.Open();
                SqlCommand cmdP09_VW = new SqlCommand("select * from GDB010" + Municipio + ".sde.P09_VW where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP09_VW);
                using (SqlDataReader drP09_VW = cmdP09_VW.ExecuteReader())
                {
                    while (drP09_VW.Read())
                    {
                        Superficie = Convert.ToString(drP09_VW["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP09_VW.Close();

                /*P10*/
                SqlConnection cnnCveP10_VW = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP10_VW.Open();
                SqlCommand cmdP10_VW = new SqlCommand("select * from GDB010" + Municipio + ".sde.P10_VW where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP10_VW);
                using (SqlDataReader drP10_VW = cmdP10_VW.ExecuteReader())
                {
                    while (drP10_VW.Read())
                    {
                        Superficie = Convert.ToString(drP10_VW["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP10_VW.Close();

                /*P11_VW*/
                SqlConnection cnnCveP11_VW = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP11_VW.Open();
                SqlCommand cmdP11_VW = new SqlCommand("select * from GDB010" + Municipio + ".sde.P11_VW where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP11_VW);
                using (SqlDataReader drP11_VW = cmdP11_VW.ExecuteReader())
                {
                    while (drP11_VW.Read())
                    {
                        Superficie = Convert.ToString(drP11_VW["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP11_VW.Close();

                /*P12_VW*/
                SqlConnection cnnCveP12_VW = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP12_VW.Open();
                SqlCommand cmdP12_VW = new SqlCommand("select * from GDB010" + Municipio + ".sde.P12_VW where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP12_VW);
                using (SqlDataReader drP12_VW = cmdP12_VW.ExecuteReader())
                {
                    while (drP12_VW.Read())
                    {
                        Superficie = Convert.ToString(drP12_VW["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP12_VW.Close();

                /*P13_VW*/
                SqlConnection cnnCveP13_VW = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP13_VW.Open();
                SqlCommand cmdP13_VW = new SqlCommand("select * from GDB010" + Municipio + ".sde.P13_VW where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP13_VW);
                using (SqlDataReader drP13_VW = cmdP13_VW.ExecuteReader())
                {
                    while (drP13_VW.Read())
                    {
                        Superficie = Convert.ToString(drP13_VW["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP13_VW.Close();

                /*P14_VW*/
                SqlConnection cnnCveP14_VW = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP14_VW.Open();
                SqlCommand cmdP14_VW = new SqlCommand("select * from GDB010" + Municipio + ".sde.P14_VW where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP14_VW);
                using (SqlDataReader drP14_VW = cmdP14_VW.ExecuteReader())
                {
                    while (drP14_VW.Read())
                    {
                        Superficie = Convert.ToString(drP14_VW["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP14_VW.Close();

                /*p15_VW*/
                SqlConnection cnnCveP15_VW = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP15_VW.Open();
                SqlCommand cmdP15_VW = new SqlCommand("select * from GDB010" + Municipio + ".sde.P15_VW where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP15_VW);
                using (SqlDataReader drP15_VW = cmdP15_VW.ExecuteReader())
                {
                    while (drP15_VW.Read())
                    {
                        Superficie = Convert.ToString(drP15_VW["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP15_VW.Close();

                /*P16_VW*/
                SqlConnection cnnCveP16_VW = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP16_VW.Open();
                SqlCommand cmdP16_VW = new SqlCommand("select * from GDB010" + Municipio + ".sde.P16_VW where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP16_VW);
                using (SqlDataReader drP16_VW = cmdP16_VW.ExecuteReader())
                {
                    while (drP16_VW.Read())
                    {
                        Superficie = Convert.ToString(drP16_VW["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP16_VW.Close();

                /*P17_VW*/
                SqlConnection cnnCveP17_VW = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP17_VW.Open();
                SqlCommand cmdP17_VW = new SqlCommand("select * from GDB010" + Municipio + ".sde.P17_VW where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP17_VW);
                using (SqlDataReader drP17_VW = cmdP17_VW.ExecuteReader())
                {
                    while (drP17_VW.Read())
                    {
                        Superficie = Convert.ToString(drP17_VW["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP17_VW.Close();

                /*P18*/
                SqlConnection cnnCveP18_VW = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP18_VW.Open();
                SqlCommand cmdP18_VW = new SqlCommand("select * from GDB010" + Municipio + ".sde.P18_VW where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP18_VW);
                using (SqlDataReader drP18_VW = cmdP18_VW.ExecuteReader())
                {
                    while (drP18_VW.Read())
                    {
                        Superficie = Convert.ToString(drP18_VW["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP18_VW.Close();

                /*P19_VW*/
                SqlConnection cnnCveP19_VW = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP19_VW.Open();
                SqlCommand cmdP19_VW = new SqlCommand("select * from GDB010" + Municipio + ".sde.P19_VW where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP19_VW);
                using (SqlDataReader drP19_VW = cmdP19_VW.ExecuteReader())
                {
                    while (drP19_VW.Read())
                    {
                        Superficie = Convert.ToString(drP19_VW["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP19_VW.Close();

                /*P20_VW*/
                SqlConnection cnnCveP20_VW = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnCveP20_VW.Open();
                SqlCommand cmdP20_VW = new SqlCommand("select * from GDB010" + Municipio + ".sde.P20_VW where CVE_CAT_ORI='" + ClaveCatastralOriginal + "'", cnnCveP20_VW);
                using (SqlDataReader drP20_VW = cmdP20_VW.ExecuteReader())
                {
                    while (drP20_VW.Read())
                    {
                        Superficie = Convert.ToString(drP20_VW["SHAPE"]);
                        newsup = Superficie.Replace("POLYGON", "");
                        sinEspacios = newsup.Trim();
                        newsup2 = sinEspacios.Replace("(", "");
                        newsup3 = newsup2.Replace(")", "");
                        CoordenadasUTMSup = newsup3.Split(' ');
                        int tam = CoordenadasUTMSup.Length;
                        string[] CoordenadasSeparadas = new string[tam];
                        for (int x = 0; x < CoordenadasUTMSup.Length; x++)
                        {

                            CoordenadasSeparadas[x] = CoordenadasUTMSup[x].Replace(",", "");
                        }
                        LongitudUTM = CoordenadasSeparadas[0];
                        LatitudUTM = CoordenadasSeparadas[1];

                    }
                }
                cnnCveP20_VW.Close();

            }

            if(LongitudUTM == "" && LatitudUTM == "")
            {
                Session["ClaveCatastral"] = ClaveCatastralOriginal;
                //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "warningUbicacion('Para ubicar el predio.');", true);
                Response.Redirect("UbicarPredio.aspx");
            }
            else
            {
                Response.Redirect("InformacionPredio.aspx?value=" + encryptedValue + "&latitud=" + LatitudUTM + "&longitud=" + LongitudUTM);
            }
           
        }

        public void CrearBoletaUbicacion(string ClaveCatastralOriginal)
        {
            string strFolder = @"\\"+IPServidor +"\\irc\\Archivos\\ManifestacionCatastralEnLinea";
            string path = strFolder + "\\" + ClaveCatastralOriginal;
            System.IO.Directory.CreateDirectory(path);

            string FilePath = path + "\\BoletaUbicacion.pdf";

            PdfWriter writer = new PdfWriter("\\\\"+IPServidor+"\\irc\\Archivos\\ManifestacionCatastralEnLinea\\" + ClaveCatastralOriginal + "\\BoletaUbicacion.pdf"); //Produccion
            PdfDocument pdfDoc = new PdfDocument(writer);
            Document document = new Document(pdfDoc);
            //BarcodeQRCode qrCode = new BarcodeQRCode("http://eservicios2.aguascalientes.gob.mx/seguot/seguimientomovil/Catastro.aspx" + "?folio=" + FolioControl.ToString());
            //PdfFormXObject barcodeObject = qrCode.CreateFormXObject(ColorConstants.BLACK, pdfDoc);
            //iText.Layout.Element.Image barcodeImage = new iText.Layout.Element.Image(barcodeObject).SetWidth(100f).SetHeight(100f);
            iText.Layout.Element.Table tabla = new iText.Layout.Element.Table(1, false);
            Cell cell11 = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(12)
                .Add(new Paragraph("SECRETARÍA GENERAL DE GOBIERNO\n INSTITUTO REGISTRAL Y CATASTRAL \n DIRECCIÓN GENERAL DE CATASTRO"));
            Cell cell21 = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.LEFT)
                .Add(new Paragraph("CLAVE CATASTRAL: " + ClaveCatastralOriginal));
            Cell cell31 = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.LEFT)
                .Add(new Paragraph("TIPO DE TRÁMITE \n" + "MANIFESTACIÓN Y/O AVALÚO"));
            Cell cell41 = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.LEFT)
                .Add(new Paragraph("OBSERVACIÓNES:" + "Dirigirse al Instituto Catastral en Av. Convención 1914 #102 Segundo Piso, Col. del Trabajo, Aguascalientes,Ags. para ubicar su predio. \n Presentar este documento y la Documentación para el trámite: \n Copia de Identificación oficial del propietario o poder con dos testigos e identificaciones de los mismos, recibo predial, documento que acredite propiedad y formato de solicitud "));
            //Cell cell51 = new Cell(1, 1)
            //    .SetTextAlignment(TextAlignment.LEFT)
            //    .Add(new Paragraph("FECHA DE RECEPCIÓN DE TRÁMITE: " + altaregistrotabla));
            //Cell cell61 = new Cell(1, 1)
            //    .SetTextAlignment(TextAlignment.LEFT)
            //    .Add(new Paragraph("NOMBRE SOLICITANTE: " + NombreSolicitante));
            //Text bluetext = new Text("http://eservicios2.aguascalientes.gob.mx/seguot/seguimientomovil/Catastro.aspx")
            //    .SetFontColor(ColorConstants.BLUE);
            //Cell cell71 = new Cell(1, 1)
            //    .SetTextAlignment(TextAlignment.LEFT)
            //    .Add(new Paragraph("Instrucciones para revisar el trámite: \n 1. Entre a la página (Dar click en la Liga)"))
            //    .Add(new Paragraph(bluetext))
            //    .Add(new Paragraph("2.Capture el número de folio y presione el botón Consulta, se mostrará un recuardo con el estatus de su tramite"));
            tabla.AddCell(cell11);
            tabla.AddCell(cell21);
            tabla.AddCell(cell31);
            tabla.AddCell(cell41);
            //tabla.AddCell(cell51);
            //tabla.AddCell(cell61);
            //tabla.AddCell(cell71);
            document.Add(tabla);
            //document.Add(new Paragraph().Add(barcodeImage));
            document.Close();

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=BoletaUbicacion.pdf");
            Response.TransmitFile(FilePath);
            Response.End();
        }

        protected void DescargaBolUbi_Click(object sender, EventArgs e)
        {
            string Municipio = TxtMunicipio.Text;
            string Localidad = TxtLocalidad.Text;
            string Sector = TxtSector.Text;
            string Manzana = TxtManzana.Text;
            string Predio = TxtPredio.Text;
            string Condominio = TxtCondominio.Text;
            string ClaveCatastralOriginal = Municipio + Localidad + Sector + Manzana + Predio + Condominio;

            CrearBoletaUbicacion(ClaveCatastralOriginal);
        }
    }
}