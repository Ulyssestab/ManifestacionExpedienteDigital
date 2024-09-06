using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using iText.Barcodes;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using ManifestacionEnLinea.Clases;
using ManifestacionEnLinea.ControlFolios; //Produccion
/*using ManifestacionEnLinea.ControlDeFolioDesa;*/ //Desarrollo Pruebas
using System.Text;
using System.Web.Security;
using ManifestacionEnLinea.DataModel;
using System.Xml.Linq;
using System.Web.UI.HtmlControls;

namespace ManifestacionEnLinea
{
    public partial class SubirArchivos : System.Web.UI.Page
    {
        string ESTANDAR = "";
        string FOLIO_REAL = "";
        string PREDIAL = "";
        string FOLIO = "";

        string MCMUNI;
        string Anio;
        string FORMATOMANI = "";
        string CuentaGlobal;
        string Cartografia;
        int conCarto;
        string NombreSol;
        static DateTime fechaAlta = System.DateTime.Now;
        string FormatoFecha = "yyyy-MM-dd HH:mm:ss:fff";
        static int year = fechaAlta.Year;
        static int Mes = fechaAlta.Month;
        static int dia = fechaAlta.Day;
        string correoelec;

        string rutacompleta = string.Empty;
        String resp;
        string ConvertAnio = Convert.ToString(year);

        /*Variables Mani y Avaluo*/

        string MAMUNI;
        string MAFOLIONUEVO, MAANIO, FOLIOMANIAVALUO, CONVNUM;

        /*Variables Avaluo*/
        string ESTANDARAVALUO = "";
        string FOLIO_REALAVALUO = "";
        string PREDIALAVALUO = "";
        string FOLIOAVALUO = "";
        string ACMUNI; //Municipio del avaluo
        string AnioAvaluo;
        string FORMATOAVALUO = "";
        string formatoFechaBoleta = dia.ToString() + "/" + Mes.ToString() + "/" + year.ToString();
        string NombreSolAvaluo;
        Clase ObjetosPago = new Clase();
        Avaluo wa = new Avaluo();
        RevisionPago rp = new RevisionPago();
        string IPServidor = ConfigurationManager.AppSettings["IPServidor"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            /*Decodificar Cuenta Catastral*/
            var decryptedText = string.Empty;
            string Tramite = string.Empty;
            string Folio = string.Empty;

            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString.ToString()))
                {
                    /*descencriptar clave catastral*/
                    var bytes = Convert.FromBase64String(Request.QueryString["value"].ToString());
                    var output = MachineKey.Unprotect(bytes, "value");
                    decryptedText = Encoding.UTF8.GetString(output);
                    CuentaGlobal = decryptedText;

                    /*Obtener el tramite seleccionado*/
                    Tramite = Request.QueryString["tramite"];
                    /*Buscar pago de avaluo en caso de avaluo*/
                    if(Tramite == "AVALÚO CATASTRAL" || Tramite == "MANIFESTACIÓN CON AVALÚO")
                    {
                        FOLIOPAGOAVALUO ResultadoPago = ObjetosPago.BuscarPagoActivo(CuentaGlobal);

                        if (ResultadoPago != null)
                        {
                            Folio = ResultadoPago.REFERENCIAFOLIO;
                        }
                        else
                        {
                            Folio = "";
                        }
                    }
                    else
                    {
                        // Obtener una referencia al div con ID "miDiv"
                        HtmlGenericControl miDiv = (HtmlGenericControl)FindControl("divFolioPago");

                        // Ocultar el div
                        miDiv.Visible = false;
                        
                    }
                    
                    //Cartografia = Request.QueryString["carto"];
                    //conCarto = Int32.Parse(Cartografia);
                }
                else
                {
                    string DescError = "Usuario no inicio en index.";
                    Response.Redirect("PaginaError.aspx?Error=" + DescError);
                }

                cuentasubFiles.Text = CuentaGlobal;
                TxtTramite.Text = Tramite;
                TxtFolioPago.Text = Folio;
            }
            else
            {
                var bytes = Convert.FromBase64String(Request.QueryString["value"].ToString());
                var output = MachineKey.Unprotect(bytes, "value");
                decryptedText = Encoding.UTF8.GetString(output);
                CuentaGlobal = decryptedText;
                cuentasubFiles.Text = CuentaGlobal;
                Tramite = Request.QueryString["tramite"];
                TxtTramite.Text = Tramite;
                //Cartografia = Request.QueryString["carto"];
                //conCarto = Int32.Parse(Cartografia);
            }
            
        }

        /*Insertar Manifestacion*/
        protected void InsertarInfoMani()
        {
            string FormatoFecha = "yyyy-MM-dd HH:mm:ss:fff";
            string Municipio = CuentaGlobal.Substring(0, 2);
            string N_VIALIDAD, NUM_EXT, NUM_INT, N_ASENTAMIENTO, NCP, N_LOCALIDAD, N_MUNICIPIO, N_ENTIDAD, FechaFinal;
            string NOMBRE_COMPLETO_UBI = "";
            string MCANIO, FOLIOMANI, CONVNUM, MCFOLIONUEVO;

            /*Obtener el Numero del FOLIO SIC*/
            SqlConnection cnnFolioSIC = new SqlConnection(ConfigurationManager.ConnectionStrings["WFTRAMITESConnectionString"].ToString());
            cnnFolioSIC.Open();
            SqlCommand cmdFolioSIC = new SqlCommand("select top 1 FOLIO_TRAMITE from WFTRAMITES.dbo.SIS_TRACAT_MC where FOLIO_TRAMITE LIKE '%MC0" + Municipio + "%' order by OBJECTID desc", cnnFolioSIC);
            using (SqlDataReader readerFolioSIC = cmdFolioSIC.ExecuteReader())
            {
                if (readerFolioSIC.HasRows)
                {
                    while (readerFolioSIC.Read())
                    {
                        FOLIO = readerFolioSIC.GetString(0);//FOLIO TRAMITE
                    }
                    //MessageBox.Show("Ultimo Folio de la manifestacion" + FOLIO);

                    MCMUNI = FOLIO.Substring(0, 5);//Prefijo Manifestación
                                                   //MessageBox.Show("Prefijo Mani" + MCMUNI);
                    Anio = FOLIO.Substring(5, 4); //Año de la ultima Manifestacion
                    if (Anio != year.ToString())
                    {
                        //  MessageBox.Show("La ultima manifestacion fue del año pasado");

                        int numFolio = 1;

                        MCFOLIONUEVO = MCMUNI + ConvertAnio + "00000" + numFolio.ToString();
                        FORMATOMANI = MCFOLIONUEVO;
                        //MessageBox.Show("Resultadofinal: " + MCFOLIONUEVO);
                        /*Obtener la Clave Estandar, Cuenta predial, Folio real*/
                        
                    }
                    else
                    {
                        MCANIO = FOLIO.Substring(0, 9);
                        FOLIOMANI = FOLIO.Substring(9);

                        int numeroFolio = Int32.Parse(FOLIOMANI);
                        int numInc = numeroFolio + 1;
                        CONVNUM = Convert.ToString(numInc);
                        if (numInc < 10)
                        {
                            FORMATOMANI = MCANIO + "00000" + CONVNUM;
                        }
                        if (numInc >= 10 && numInc < 100)
                        {
                            FORMATOMANI = MCANIO + "0000" + CONVNUM;
                        }
                        if (numInc >= 100 && numInc < 1000)
                        {
                            FORMATOMANI = MCANIO + "000" + CONVNUM;
                        }
                        if (numInc >= 1000 && numInc < 10000)
                        {
                            FORMATOMANI = MCANIO + "00" + CONVNUM;
                        }
                        if (numInc >= 10000 && numInc < 100000)
                        {
                            FORMATOMANI = MCANIO + "0" + CONVNUM;
                        }
                        if (numInc >= 100000 && numInc < 999999)
                        {
                            FORMATOMANI = MCANIO + CONVNUM;
                        }
                        //MessageBox.Show("Resultadofinal: " + FORMATOMANI);
                        
                    }
                }
                else
                {
                    
                    string DescError = "Error al obtener el numero de FOLIO SIC";
                    Response.Redirect("PaginaError.aspx?Error="+ DescError);

                }

                cnnFolioSIC.Close();
            }

            /*Obtener la Clave Estandar, Cuenta predial, Folio real*/

            SqlConnection cnnInfo = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
            cnnInfo.Open();
            SqlCommand cmdInfo = new SqlCommand("select top 1 CVE_CAT_EST,CVE_PREDIAL,FOLIO_REAL from GDB010" + Municipio + ".SDE.SIS_PC_CLAVE_CATASTRAL where STATUSREGISTROTABLA='ACTIVO' AND CVE_CAT_ORI='" + CuentaGlobal + "' order by OBJECTID desc", cnnInfo);
            using (SqlDataReader readerInfo = cmdInfo.ExecuteReader())
            {
                int i = 0;
                if (readerInfo.HasRows)
                {
                    while (readerInfo.Read())
                    {
                        if (!readerInfo.IsDBNull(0))
                        {
                            ESTANDAR = readerInfo.GetString(0);//Clave Estandar
                        }
                        else
                        {
                            ESTANDAR = "";
                        }
                        if (!readerInfo.IsDBNull(1))
                        {
                            PREDIAL = readerInfo.GetString(1);//Predial
                        }
                        else
                        {
                            PREDIAL = "";
                        }
                        if (!readerInfo.IsDBNull(2))
                        {
                            FOLIO_REAL = readerInfo.GetString(2);//Folio Real
                        }
                        else
                        {
                            FOLIO_REAL = "";
                        }
                    }
                }
                else
                {
                    string DescError = "Error al obtener clave estandar, folio real y clave predial.";
                    Response.Redirect("PaginaError.aspx?Error="+ DescError);

                }

                cnnInfo.Close();
            }

            /*Consulta para sacar la ubicacion */
            try
            {
                SqlConnection cnnUbi = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnUbi.Open();
                SqlCommand cmdUbi = new SqlCommand("select top 1 NOMBRE_COMPLETO_VIALIDAD,NUMERO_EXTERIOR,NUMERO_INTERIOR,NOMBRE_COMPLETO_ASENTAMIENTO,CP,NOM_LOCALIDAD,NOM_MUNICIPIO,NOM_ENTIDAD from GDB010" + Municipio + ".sde.SIS_PC_UBICACION WHERE CVE_CAT_ORI='" + CuentaGlobal + "' and STATUSREGISTROTABLA='ACTIVO'", cnnUbi);
                using (SqlDataReader readerUbi = cmdUbi.ExecuteReader())
                {
                    while (readerUbi.Read())
                    {
                        if (!readerUbi.IsDBNull(0))
                        {
                            N_VIALIDAD = readerUbi.GetString(0);
                        }
                        else
                        {
                            N_VIALIDAD = "";
                        }
                        if (!readerUbi.IsDBNull(1))
                        {
                            NUM_EXT = readerUbi.GetString(1);
                        }
                        else
                        {
                            NUM_EXT = "";
                        }
                        if (!readerUbi.IsDBNull(2))
                        {
                            NUM_INT = readerUbi.GetString(2);
                        }
                        else
                        {
                            NUM_INT = "";
                        }
                        if (!readerUbi.IsDBNull(3))
                        {
                            N_ASENTAMIENTO = readerUbi.GetString(3);
                        }
                        else
                        {
                            N_ASENTAMIENTO = "";
                        }
                        if (!readerUbi.IsDBNull(4))
                        {
                            NCP = readerUbi.GetString(4);
                        }
                        else
                        {
                            NCP = "";
                        }
                        if (!readerUbi.IsDBNull(5))
                        {
                            N_LOCALIDAD = readerUbi.GetString(5);
                        }
                        else
                        {
                            N_LOCALIDAD = "";
                        }
                        if (!readerUbi.IsDBNull(6))
                        {
                            N_MUNICIPIO = readerUbi.GetString(6);
                        }
                        else
                        {
                            N_MUNICIPIO = "";
                        }
                        if (!readerUbi.IsDBNull(7))
                        {
                            N_ENTIDAD = readerUbi.GetString(7);
                        }
                        else
                        {
                            N_ENTIDAD = "";
                        }

                        NOMBRE_COMPLETO_UBI = N_VIALIDAD + " EXT. " + NUM_EXT + " INT. " + NUM_INT + " " + N_ASENTAMIENTO + " C.P. " + NCP + ", " + N_LOCALIDAD + ", " + N_MUNICIPIO + ", " + N_ENTIDAD;

                    }

                    //MessageBox.Show("Direccion: " + NOMBRE_COMPLETO_UBI);
                }
                cnnUbi.Close();
            }
            catch (Exception exUbi)
            {
                string DescError = "Error al obtener la Ubicación " + exUbi.ToString();
                Response.Redirect("PaginaError.aspx?Error=" + DescError);
            }

            NombreSol = inputSolicitanteUpload.Text;
            correoelec = CorreoElectronico.Text;
            /*Insert into SIS_TRACAT_MC*/
            try
            {

                SqlConnection cnnInsertMC = new SqlConnection(ConfigurationManager.ConnectionStrings["WFTRAMITESConnectionString"].ToString());
                cnnInsertMC.Open();
                string QueryInsertMC = "insert into WFTRAMITES.dbo.SIS_TRACAT_MC(STATUSREGISTROTABLA, ALTAREGISTROTABLA, BAJAREGISTROTABLA, USUARIOALTA, USUARIOBAJA, FOLIO_TRAMITE, TIPO_TRAMITE, CVE_CAT_EST, CVE_CAT_ORI, CVE_PREDIAL, OBSERVACIONES, TRAMITADOR, EN_USO, IMAGEN, SOLICITANTE, PROPIETARIO, UBICACION, NOTIFICACION, NOTIFICACION_RECHAZO, AVALUO, FOLIO_PAGO_AVALUO, IGUAL_CARTOGRAFIA,CORREOELECTRONICO,CONTROLFOLIO) VALUES \n" +
                     "('ACTIVO','" + fechaAlta.ToString(FormatoFecha) + "',NULL,'UsuarioExterno',NULL,'" + FORMATOMANI + "','MANIFESTACIÓN DE CONSTRUCCIÓN','" + ESTANDAR + "','" + CuentaGlobal + "','" + PREDIAL + "','','',0,NULL,'" + NombreSol + "',NULL,'" + NOMBRE_COMPLETO_UBI + "',NULL,0,0,NULL,0,'"+correoelec+"',NULL)";
                SqlCommand cmdMC = new SqlCommand(QueryInsertMC, cnnInsertMC);
                cmdMC.ExecuteNonQuery();
                cnnInsertMC.Close();
            }
            catch (Exception ErrorMC)
            {
                string DescError = "Error al insertar en SIS_TRACAT_MC " + ErrorMC.ToString();
                
                Response.Redirect("PaginaError.aspx?Error="+ DescError);

            }

            /*Insert into Tramites*/
            try
            {

                SqlConnection cnnInsertTramite = new SqlConnection(ConfigurationManager.ConnectionStrings["WFTRAMITESConnectionString"].ToString());
                cnnInsertTramite.Open();
                string QueryInsertTramite = "insert into WFTRAMITES.dbo.Tramite(ClaveCatastralOriginal, ClaveCatastralEstandar, FechaInicial, FechaFinal, FK_CatEstatus, Pagado, Condonado, FK_Cat_Notaria, Prioridad, Observaciones, FK_Cat_TipodeProcesoTramite, FK_NombreUsuario, Region, Manzana, Lote, Departamento, FK_Cat_TipodePredio, NumeroCuenta, Delegacion, NumeroTramite, FK_Cat_Solicitante, NombrePersonalSolicitante, ObservacionAclaracion, Procede, Entidad, Tipo, Año, Consecutivo, Derivada, TipoBusqueda, Bloqueado, Reserva, Rechazado, NumeroClave, NumeroCuentas, Excentopago, FK_Cat_TipoClaveCatastralGenerada, FK_Cat_TipoInspeccion, PagarReingreso, NumeroTramiteSIC, NumeroTramiteValuacion, ProntoPago, Sector, Localidad, Predio, Edificio, Unidad, Zona, FK_Cat_Municipio, RequiereCartografia, ObjectId_Cartografico, Condominio, OtroEstado, Folio, Corett, FK_Cat_Dependencia, Habilitado, SuperficieConstruccion, SuperficieTerreno, usuarioSolicitante, Observaciones_Rechazo_Direccion, FK_CatCoordinacion, USUARIOCANCELA) VALUES \n" +
                     "('" + CuentaGlobal + "','" + ESTANDAR + "','" + fechaAlta.ToString(FormatoFecha) + "',NULL,2,0,0,0,'Alta',NULL,2,'UsuarioExterno',NULL,NULL,NULL,NULL,1,NULL,0,'" + FORMATOMANI + "',1,'" + NombreSol + "',NULL,NULL,NULL,'MANIFESTACIÓN DE CONSTRUCCIÓN',NULL,NULL,NULL,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,1,0,NULL,NULL,NULL,0,NULL,NULL,1,NULL,NULL,'UsuarioExterno',NULL,1,NULL)";
                SqlCommand cmdTramite = new SqlCommand(QueryInsertTramite, cnnInsertTramite);
                cmdTramite.ExecuteNonQuery();
                cnnInsertTramite.Close();
            }
            catch (Exception ErrorTramite)
            {
                string DescError = "Error al insertar en Tramite " + ErrorTramite.ToString();
                
                Response.Redirect("PaginaError.aspx?Error="+ DescError);

            }

            /*Insert into Seguimiento Tramite*/
            try
            {

                SqlConnection cnnInsertSeguimiento = new SqlConnection(ConfigurationManager.ConnectionStrings["WFTRAMITESConnectionString"].ToString());
                cnnInsertSeguimiento.Open();
                string QueryInsertSeguimiento = "insert into WFTRAMITES.dbo.SeguimientoTramite (FK_NumeroTramite,FK_Cat_TipoProcesoTramite,FK_Cat_Coordinacion,FK_Cat_EstatusTramite,Tarea,TipoFlujo,Observaciones,Duracion,Orden,	FK_Cat_OpcionesSistema) VALUES \n" +
                        "('" + FORMATOMANI + "',2,1,5,'SOLICITUD DE MANIFESTACIÓN ELECTRÓNICA','Normal',NULL,20,1,1202)," +
                        "('" + FORMATOMANI + "',2,1,2,'ACTUALIZACIÓN CATASTRAL DE MANIFESTACIÓN','Normal',NULL,20,2,1211)," +
                        "('" + FORMATOMANI + "',2,1,7,'FIRMA ELECTRÓNICA TRAMITADOR MANIFESTACIÓN','Normal',NULL,20,3,1213)," +
                        "('" + FORMATOMANI + "',2,1,7,'FIRMA ELECTRONICA DIRECCION','Normal',NULL,20,4,184)";
                SqlCommand cmdSeguimiento = new SqlCommand(QueryInsertSeguimiento, cnnInsertSeguimiento);
                cmdSeguimiento.ExecuteNonQuery();
                cnnInsertSeguimiento.Close();

            }
            catch (Exception ErrorSeguimiento)
            {
                string DescError = "Error al insertar en Seguimiento Tramite " + ErrorSeguimiento.ToString();
                Response.Redirect("PaginaError.aspx?Error="+ DescError);
            }
        }/*Fin Insertar Mani*/

        private void InsertarInfoAvaluo()
        {

            string FormatoFecha = "yyyy-MM-dd HH:mm:ss:fff";
            string Municipio = CuentaGlobal.Substring(0, 2);
            string N_VIALIDADAVALUO, NUM_EXTAVALUO, NUM_INTAVALUO, N_ASENTAMIENTOAVALUO, NCPAVALUO, N_LOCALIDADAVALUO, N_MUNICIPIOAVALUO, N_ENTIDADAVALUO, FechaFinal;
            string NOMBRE_COMPLETO_UBIAVALUO = "";
            string ACANIO, FOLIOSICAVALUO, CONVNUM, ACFOLIONUEVO;
            string CfdioReferencia = TxtFolioPago.Text;

            /*Obtener el Numero del FOLIO SIC*/
            SqlConnection cnnFolioSICAvaluo = new SqlConnection(ConfigurationManager.ConnectionStrings["WFTRAMITESConnectionString"].ToString());
            cnnFolioSICAvaluo.Open();
            SqlCommand cmdFolioSICAvaluo = new SqlCommand("select top 1 FOLIO_TRAMITE from WFTRAMITES.dbo.SIS_TRACAT_AC where FOLIO_TRAMITE LIKE '%AC0" + Municipio + "%' order by OBJECTID desc", cnnFolioSICAvaluo);
            using (SqlDataReader readerFolioSIC = cmdFolioSICAvaluo.ExecuteReader())
            {
                if (readerFolioSIC.HasRows)
                {
                    while (readerFolioSIC.Read())
                    {
                        FOLIOAVALUO = readerFolioSIC.GetString(0);//FOLIO TRAMITE
                    }
                    //MessageBox.Show("Ultimo Folio de el Avaluo" + FOLIO);

                    ACMUNI = FOLIOAVALUO.Substring(0, 5);//Prefijo Avaluo
                                                         //MessageBox.Show("Prefijo Mani" + MCMUNI);
                    AnioAvaluo = FOLIOAVALUO.Substring(5, 4); //Año de la ultima Avaluo
                    if (AnioAvaluo != year.ToString())
                    {
                        //  MessageBox.Show("La ultima Avaluo fue del año pasado");

                        int numFolio = 1;

                        ACFOLIONUEVO = ACMUNI + ConvertAnio + "00000" + numFolio.ToString();
                        FORMATOAVALUO = ACFOLIONUEVO;
                        //MessageBox.Show("Resultadofinal: " + ACFOLIONUEVO);
                        /*Obtener la Clave Estandar, Cuenta predial, Folio real*/
                        
                    }
                    else
                    {
                        ACANIO = FOLIOAVALUO.Substring(0, 9);
                        FOLIOSICAVALUO = FOLIOAVALUO.Substring(9);

                        int numeroFolioAvaluo = Int32.Parse(FOLIOSICAVALUO);
                        int numIncAvaluo = numeroFolioAvaluo + 1;
                        CONVNUM = Convert.ToString(numIncAvaluo);
                        if (numIncAvaluo < 10)
                        {
                            FORMATOAVALUO = ACANIO + "00000" + CONVNUM;
                        }
                        if (numIncAvaluo >= 10 && numIncAvaluo < 100)
                        {
                            FORMATOAVALUO = ACANIO + "0000" + CONVNUM;
                        }
                        if (numIncAvaluo >= 100 && numIncAvaluo < 1000)
                        {
                            FORMATOAVALUO = ACANIO + "000" + CONVNUM;
                        }
                        if (numIncAvaluo >= 1000 && numIncAvaluo < 10000)
                        {
                            FORMATOAVALUO = ACANIO + "00" + CONVNUM;
                        }
                        if (numIncAvaluo >= 10000 && numIncAvaluo < 100000)
                        {
                            FORMATOAVALUO = ACANIO + "0" + CONVNUM;
                        }
                        if (numIncAvaluo >= 100000 && numIncAvaluo < 999999)
                        {
                            FORMATOAVALUO = ACANIO + CONVNUM;
                        }
                        //MessageBox.Show("Resultadofinal: " + FORMATOMANI);
                        
                    }
                }
                else
                {
                    string DescError = "Error al obtener el Folio SIC.";
                    Response.Redirect("PaginaError.aspx?Error="+ DescError);
                }

                cnnFolioSICAvaluo.Close();
            }


            /*Obtener la Clave Estandar, Cuenta predial, Folio real*/
            SqlConnection cnnInfoAvaluo = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
            cnnInfoAvaluo.Open();
            SqlCommand cmdInfoAvaluo = new SqlCommand("select top 1 CVE_CAT_EST,CVE_PREDIAL,FOLIO_REAL from GDB010" + Municipio + ".SDE.SIS_PC_CLAVE_CATASTRAL where STATUSREGISTROTABLA='ACTIVO' AND CVE_CAT_ORI='" + CuentaGlobal + "' order by OBJECTID desc", cnnInfoAvaluo);
            using (SqlDataReader readerInfoAvaluo = cmdInfoAvaluo.ExecuteReader())
            {
                int i = 0;
                if (readerInfoAvaluo.HasRows)
                {
                    while (readerInfoAvaluo.Read())
                    {
                        if (!readerInfoAvaluo.IsDBNull(0))
                        {
                            ESTANDARAVALUO = readerInfoAvaluo.GetString(0);//Clave Estandar
                        }
                        else
                        {
                            ESTANDARAVALUO = "";
                        }
                        if (!readerInfoAvaluo.IsDBNull(1))
                        {
                            PREDIALAVALUO = readerInfoAvaluo.GetString(1);//Predial
                        }
                        else
                        {
                            PREDIALAVALUO = "";
                        }
                        if (!readerInfoAvaluo.IsDBNull(2))
                        {
                            FOLIO_REALAVALUO = readerInfoAvaluo.GetString(2);//Folio Real
                        }
                        else
                        {
                            FOLIO_REALAVALUO = "";
                        }
                    }
                }
                else
                {
                    string DescError = "Error al obtener clave estandar, predial, folio real.";
                    Response.Redirect("PaginaError.aspx?Error="+ DescError);
                }

                cnnInfoAvaluo.Close();
            }
            /*Consulta para sacar la ubicacion */
            try
            {
                SqlConnection cnnUbiAvaluo = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnUbiAvaluo.Open();
                SqlCommand cmdUbiAvaluo = new SqlCommand("select top 1 NOMBRE_COMPLETO_VIALIDAD,NUMERO_EXTERIOR,NUMERO_INTERIOR,NOMBRE_COMPLETO_ASENTAMIENTO,CP,NOM_LOCALIDAD,NOM_MUNICIPIO,NOM_ENTIDAD from GDB010" + Municipio + ".sde.SIS_PC_UBICACION WHERE CVE_CAT_ORI='" + CuentaGlobal + "' and STATUSREGISTROTABLA='ACTIVO'", cnnUbiAvaluo);
                using (SqlDataReader readerUbiAvaluo = cmdUbiAvaluo.ExecuteReader())
                {
                    while (readerUbiAvaluo.Read())
                    {
                        if (!readerUbiAvaluo.IsDBNull(0))
                        {
                            N_VIALIDADAVALUO = readerUbiAvaluo.GetString(0);
                        }
                        else
                        {
                            N_VIALIDADAVALUO = "";
                        }
                        if (!readerUbiAvaluo.IsDBNull(1))
                        {
                            NUM_EXTAVALUO = readerUbiAvaluo.GetString(1);
                        }
                        else
                        {
                            NUM_EXTAVALUO = "";
                        }
                        if (!readerUbiAvaluo.IsDBNull(2))
                        {
                            NUM_INTAVALUO = readerUbiAvaluo.GetString(2);
                        }
                        else
                        {
                            NUM_INTAVALUO = "";
                        }
                        if (!readerUbiAvaluo.IsDBNull(3))
                        {
                            N_ASENTAMIENTOAVALUO = readerUbiAvaluo.GetString(3);
                        }
                        else
                        {
                            N_ASENTAMIENTOAVALUO = "";
                        }
                        if (!readerUbiAvaluo.IsDBNull(4))
                        {
                            NCPAVALUO = readerUbiAvaluo.GetString(4);
                        }
                        else
                        {
                            NCPAVALUO = "";
                        }
                        if (!readerUbiAvaluo.IsDBNull(5))
                        {
                            N_LOCALIDADAVALUO = readerUbiAvaluo.GetString(5);
                        }
                        else
                        {
                            N_LOCALIDADAVALUO = "";
                        }
                        if (!readerUbiAvaluo.IsDBNull(6))
                        {
                            N_MUNICIPIOAVALUO = readerUbiAvaluo.GetString(6);
                        }
                        else
                        {
                            N_MUNICIPIOAVALUO = "";
                        }
                        if (!readerUbiAvaluo.IsDBNull(7))
                        {
                            N_ENTIDADAVALUO = readerUbiAvaluo.GetString(7);
                        }
                        else
                        {
                            N_ENTIDADAVALUO = "";
                        }

                        NOMBRE_COMPLETO_UBIAVALUO = N_VIALIDADAVALUO + " EXT. " + NUM_EXTAVALUO + " INT. " + NUM_INTAVALUO + " " + N_ASENTAMIENTOAVALUO + " C.P. " + NCPAVALUO + ", " + N_LOCALIDADAVALUO + ", " + N_MUNICIPIOAVALUO + ", " + N_ENTIDADAVALUO;

                    }

                    //MessageBox.Show("Direccion: " + NOMBRE_COMPLETO_UBI);
                }
                cnnUbiAvaluo.Close();
            }
            catch (Exception exUbiAvaluo)
            {
                string DescError = "Error al buscar la ubicación, " + exUbiAvaluo.ToString();
                Response.Redirect("PaginaError.aspx?Error="+ DescError);
            }

            NombreSolAvaluo = inputSolicitanteUpload.Text;
            correoelec = CorreoElectronico.Text;
            /*Insert into SIS_TRACAT_AC*/
            try
            {

                SqlConnection cnnInsertAC = new SqlConnection(ConfigurationManager.ConnectionStrings["WFTRAMITESConnectionString"].ToString());
                cnnInsertAC.Open();
                string QueryInsertAC = "insert into WFTRAMITES.dbo.SIS_TRACAT_AC(STATUSREGISTROTABLA, ALTAREGISTROTABLA, BAJAREGISTROTABLA, USUARIOALTA, USUARIOBAJA, TIPO_TRAMITE,FOLIO_TRAMITE, CVE_CAT_EST, CVE_CAT_ORI, OBSERVACIONES, TRAMITADOR, SOLICITANTE, UBICACION, FOLIO_MC, FOLIO_PAGO_AVALUO, NO_NOTARIA, NO_ESCRITURA, NATURALEZA_DEL_ACTO,BANDERA_EXTERNO,BANDERA_ACEPTADO,CORREOELECTRONICO,CONTROLFOLIO) VALUES \n" +
                     "('ACTIVO','" + fechaAlta.ToString(FormatoFecha) + "',NULL,'UsuarioExterno',NULL,'SOLICITUD DE AVALÚO CATASTRAL','" + FORMATOAVALUO + "','" + ESTANDARAVALUO + "','" + CuentaGlobal + "',NULL,'','" + NombreSolAvaluo + "','" + NOMBRE_COMPLETO_UBIAVALUO + "',NULL,'"+CfdioReferencia+"',NULL,NULL,NULL,NULL,NULL,'" + correoelec +"',NULL)";
                SqlCommand cmdAC = new SqlCommand(QueryInsertAC, cnnInsertAC);
                cmdAC.ExecuteNonQuery();
                cnnInsertAC.Close();
            }
            catch (Exception ErrorAC)
            {
                string DescError = "Error al insertar en AC, " + ErrorAC.ToString();
                Response.Redirect("PaginaError.aspx?Error="+ DescError);
            }

            /*Insert into Tramites*/
            try
            {

                SqlConnection cnnInsertTramiteAvaluo = new SqlConnection(ConfigurationManager.ConnectionStrings["WFTRAMITESConnectionString"].ToString());
                cnnInsertTramiteAvaluo.Open();
                string QueryInsertTramiteAvaluo = "insert into WFTRAMITES.dbo.Tramite(ClaveCatastralOriginal, ClaveCatastralEstandar, FechaInicial, FechaFinal, FK_CatEstatus, Pagado, Condonado, FK_Cat_Notaria, Prioridad, Observaciones, FK_Cat_TipodeProcesoTramite, FK_NombreUsuario, Region, Manzana, Lote, Departamento, FK_Cat_TipodePredio, NumeroCuenta, Delegacion, NumeroTramite, FK_Cat_Solicitante, NombrePersonalSolicitante, ObservacionAclaracion, Procede, Entidad, Tipo, Año, Consecutivo, Derivada, TipoBusqueda, Bloqueado, Reserva, Rechazado, NumeroClave, NumeroCuentas, Excentopago, FK_Cat_TipoClaveCatastralGenerada, FK_Cat_TipoInspeccion, PagarReingreso, NumeroTramiteSIC, NumeroTramiteValuacion, ProntoPago, Sector, Localidad, Predio, Edificio, Unidad, Zona, FK_Cat_Municipio, RequiereCartografia, ObjectId_Cartografico, Condominio, OtroEstado, Folio, Corett, FK_Cat_Dependencia, Habilitado, SuperficieConstruccion, SuperficieTerreno, usuarioSolicitante, Observaciones_Rechazo_Direccion, FK_CatCoordinacion, USUARIOCANCELA) VALUES \n" +
                     "('" + CuentaGlobal + "','" + ESTANDARAVALUO + "','" + fechaAlta.ToString(FormatoFecha) + "',NULL,2,0,0,0,'Alta',NULL,3001,'UsuarioExterno',NULL,NULL,NULL,NULL,1,NULL,0,'" + FORMATOAVALUO + "',1,'" + NombreSolAvaluo + "',NULL,NULL,NULL,'SOLICITUD DE AVALÚO CATASTRAL',NULL,NULL,NULL,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,1,0,NULL,NULL,NULL,0,NULL,NULL,1,NULL,NULL,'UsuarioExterno',NULL,1,NULL)";
                SqlCommand cmdTramiteAvaluo = new SqlCommand(QueryInsertTramiteAvaluo, cnnInsertTramiteAvaluo);
                cmdTramiteAvaluo.ExecuteNonQuery();
                cnnInsertTramiteAvaluo.Close();
            }
            catch (Exception ErrorTr)
            {
                string DescError = "Error al insertar en AC, " + ErrorTr.ToString();
                Response.Redirect("PaginaError.aspx?Error="+ DescError);

            }

            /*Insert into Seguimiento Tramite*/
            try
            {

                SqlConnection cnnInsertSeguimientoAvaluo = new SqlConnection(ConfigurationManager.ConnectionStrings["WFTRAMITESConnectionString"].ToString());
                cnnInsertSeguimientoAvaluo.Open();
                string QueryInsertSeguimientoAvaluo = "insert into WFTRAMITES.dbo.SeguimientoTramite (FK_NumeroTramite,FK_Cat_TipoProcesoTramite,FK_Cat_Coordinacion,FK_Cat_EstatusTramite,Tarea,TipoFlujo,Observaciones,Duracion,Orden,	FK_Cat_OpcionesSistema) VALUES \n" +
                        "('" + FORMATOAVALUO + "',3001,1,5,'SOLICITUD DE AVALÚO CATASTRAL','Normal',NULL,20,1,104)," +
                        "('" + FORMATOAVALUO + "',3001,1,2,'FIRMA ELECTRONICA TRAMITADOR AVALUO','Normal',NULL,20,2,1200)," +
                        "('" + FORMATOAVALUO + "',3001,1,7,'FIRMA ELECTRONICA DIRECCION','Normal',NULL,20,3,184)";
                SqlCommand cmdSeguimientoAvaluo = new SqlCommand(QueryInsertSeguimientoAvaluo, cnnInsertSeguimientoAvaluo);
                cmdSeguimientoAvaluo.ExecuteNonQuery();
                cnnInsertSeguimientoAvaluo.Close();

            }
            catch (Exception ErrorSg)
            {
                string DescError= "Error al insertar en Segumiento, " + ErrorSg.ToString();
                Response.Redirect("PaginaError.aspx?Error=" + DescError);

            }



        } /*Fin Insertar Avaluo*/

        protected void InsertarInfoManiyAvaluo()
        {
            string Municipio = CuentaGlobal.Substring(0, 2);//Sacar el Municipio de el Predio
            string N_VIALIDAD, NUM_EXT, NUM_INT, N_ASENTAMIENTO, NCP, N_LOCALIDAD, N_MUNICIPIO, N_ENTIDAD;
            string NOMBRE_COMPLETO_UBI = "";
            string anioactual = fechaAlta.Year.ToString();
            
            /*Obtener el Numero del FOLIO SIC*/
            SqlConnection cnnFolioSICManiAvaluo = new SqlConnection(ConfigurationManager.ConnectionStrings["WFTRAMITESConnectionString"].ToString());
            cnnFolioSICManiAvaluo.Open();
            SqlCommand cmdFolioSICManiAvaluo = new SqlCommand("select top 1 FOLIO_TRAMITE from WFTRAMITES.dbo.SIS_TRACAT_MC_AV where FOLIO_TRAMITE LIKE '%MA0" + Municipio + "%' order by OBJECTID desc", cnnFolioSICManiAvaluo);
            using (SqlDataReader readerFolioSICManiAvaluo = cmdFolioSICManiAvaluo.ExecuteReader())
            {
                if (readerFolioSICManiAvaluo.HasRows)
                {
                    while (readerFolioSICManiAvaluo.Read())
                    {
                        FOLIO = readerFolioSICManiAvaluo.GetString(0);//FOLIO TRAMITE
                    }
                    //MessageBox.Show("Ultimo Folio de la manifestacion" + FOLIO);

                    MAMUNI = FOLIO.Substring(0, 5);//Prefijo Manifestación
                                                   //MessageBox.Show("Prefijo Mani" + MCMUNI);
                    Anio = FOLIO.Substring(5, 4); //Año de la ultima Manifestacion
                    if (Anio != year.ToString())
                    {
                        //  MessageBox.Show("La ultima manifestacion fue del año pasado");

                        int numFolio = 1;

                        MAFOLIONUEVO = MAMUNI + ConvertAnio + "00000" + numFolio.ToString();
                        FORMATOMANI = MAFOLIONUEVO;
                        //MessageBox.Show("Resultadofinal: " + MAFOLIONUEVO);
                        /*Obtener la Clave Estandar, Cuenta predial, Folio real*/
                        
                    }
                    else
                    {
                        MAANIO = FOLIO.Substring(0, 9);
                        FOLIOMANIAVALUO = FOLIO.Substring(9);

                        int numeroFolio = Int32.Parse(FOLIOMANIAVALUO);
                        int numInc = numeroFolio + 1;
                        CONVNUM = Convert.ToString(numInc);
                        if (numInc < 10)
                        {
                            FORMATOMANI = MAANIO + "00000" + CONVNUM;
                        }
                        if (numInc >= 10 && numInc < 100)
                        {
                            FORMATOMANI = MAANIO + "0000" + CONVNUM;
                        }
                        if (numInc >= 100 && numInc < 1000)
                        {
                            FORMATOMANI = MAANIO + "000" + CONVNUM;
                        }
                        if (numInc >= 1000 && numInc < 10000)
                        {
                            FORMATOMANI = MAANIO + "00" + CONVNUM;
                        }
                        if (numInc >= 10000 && numInc < 100000)
                        {
                            FORMATOMANI = MAANIO + "0" + CONVNUM;
                        }
                        if (numInc >= 100000 && numInc < 999999)
                        {
                            FORMATOMANI = MAANIO + CONVNUM;
                        }
                        //MessageBox.Show("Resultadofinal: " + FORMATOMANI);
                       
                    }
                }
                else
                {
                    FORMATOMANI = "MA0" + Municipio + anioactual + "000001";
                }

                cnnFolioSICManiAvaluo.Close();
            }

            /*Obtener la Clave Estandar, Cuenta predial, Folio real*/

            SqlConnection cnnInfoManiAvaluo = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
            cnnInfoManiAvaluo.Open();
            SqlCommand cmdInfoManiAvaluo = new SqlCommand("select top 1 CVE_CAT_EST,CVE_PREDIAL,FOLIO_REAL from GDB010" + Municipio + ".SDE.SIS_PC_CLAVE_CATASTRAL where STATUSREGISTROTABLA='ACTIVO' AND CVE_CAT_ORI='" + CuentaGlobal + "' order by OBJECTID desc", cnnInfoManiAvaluo);
            using (SqlDataReader readerInfoManiAvaluo = cmdInfoManiAvaluo.ExecuteReader())
            {
                int i = 0;
                if (readerInfoManiAvaluo.HasRows)
                {
                    while (readerInfoManiAvaluo.Read())
                    {
                        if (!readerInfoManiAvaluo.IsDBNull(0))
                        {
                            ESTANDAR = readerInfoManiAvaluo.GetString(0);//Clave Estandar
                        }
                        else
                        {
                            ESTANDAR = "";
                        }
                        if (!readerInfoManiAvaluo.IsDBNull(1))
                        {
                            PREDIAL = readerInfoManiAvaluo.GetString(1);//Predial
                        }
                        else
                        {
                            PREDIAL = "";
                        }
                        if (!readerInfoManiAvaluo.IsDBNull(2))
                        {
                            FOLIO_REAL = readerInfoManiAvaluo.GetString(2);//Folio Real
                        }
                        else
                        {
                            FOLIO_REAL = "";
                        }
                    }
                }
                else
                {
                    string DescError = "Error al obtener clave estandar";
                    Response.Redirect("PaginaError.aspx?Error=" + DescError);
                }

                cnnInfoManiAvaluo.Close();
            }

            try
            {
                SqlConnection cnnUbiManiAvaluo = new SqlConnection(ConfigurationManager.ConnectionStrings["GDB010" + Municipio + "ConnectionString"].ToString());
                cnnUbiManiAvaluo.Open();
                SqlCommand cmdUbiManiAvaluo = new SqlCommand("select top 1 NOMBRE_COMPLETO_VIALIDAD,NUMERO_EXTERIOR,NUMERO_INTERIOR,NOMBRE_COMPLETO_ASENTAMIENTO,CP,NOM_LOCALIDAD,NOM_MUNICIPIO,NOM_ENTIDAD from GDB010" + Municipio + ".sde.SIS_PC_UBICACION WHERE CVE_CAT_ORI='" + CuentaGlobal + "' and STATUSREGISTROTABLA='ACTIVO'", cnnUbiManiAvaluo);
                using (SqlDataReader readerUbi = cmdUbiManiAvaluo.ExecuteReader())
                {
                    while (readerUbi.Read())
                    {
                        if (!readerUbi.IsDBNull(0))
                        {
                            N_VIALIDAD = readerUbi.GetString(0);
                        }
                        else
                        {
                            N_VIALIDAD = "";
                        }
                        if (!readerUbi.IsDBNull(1))
                        {
                            NUM_EXT = readerUbi.GetString(1);
                        }
                        else
                        {
                            NUM_EXT = "";
                        }
                        if (!readerUbi.IsDBNull(2))
                        {
                            NUM_INT = readerUbi.GetString(2);
                        }
                        else
                        {
                            NUM_INT = "";
                        }
                        if (!readerUbi.IsDBNull(3))
                        {
                            N_ASENTAMIENTO = readerUbi.GetString(3);
                        }
                        else
                        {
                            N_ASENTAMIENTO = "";
                        }
                        if (!readerUbi.IsDBNull(4))
                        {
                            NCP = readerUbi.GetString(4);
                        }
                        else
                        {
                            NCP = "";
                        }
                        if (!readerUbi.IsDBNull(5))
                        {
                            N_LOCALIDAD = readerUbi.GetString(5);
                        }
                        else
                        {
                            N_LOCALIDAD = "";
                        }
                        if (!readerUbi.IsDBNull(6))
                        {
                            N_MUNICIPIO = readerUbi.GetString(6);
                        }
                        else
                        {
                            N_MUNICIPIO = "";
                        }
                        if (!readerUbi.IsDBNull(7))
                        {
                            N_ENTIDAD = readerUbi.GetString(7);
                        }
                        else
                        {
                            N_ENTIDAD = "";
                        }

                        NOMBRE_COMPLETO_UBI = N_VIALIDAD + " EXT. " + NUM_EXT + " INT. " + NUM_INT + " " + N_ASENTAMIENTO + " C.P. " + NCP + ", " + N_LOCALIDAD + ", " + N_MUNICIPIO + ", " + N_ENTIDAD;

                    }
                }
                cnnUbiManiAvaluo.Close();
            }
            catch (Exception exUbi)
            {
                string DescError = "Error al obtener la ubicacion";
                Response.Redirect("PaginaError.aspx?Error="+ DescError);
            }

            NombreSol = inputSolicitanteUpload.Text;
            correoelec = CorreoElectronico.Text;
            /*Insert into SIS_TRACAT_MC_AV*/
            try
            {

                SqlConnection cnnInsertMCManiAvaluo = new SqlConnection(ConfigurationManager.ConnectionStrings["WFTRAMITESConnectionString"].ToString());
                cnnInsertMCManiAvaluo.Open();
                string QueryInsertMCManiAvaluo = "insert into WFTRAMITES.dbo.SIS_TRACAT_MC_AV(STATUSREGISTROTABLA, ALTAREGISTROTABLA, BAJAREGISTROTABLA, USUARIOALTA, USUARIOBAJA, FOLIO_TRAMITE, TIPO_TRAMITE, CVE_CAT_EST, CVE_CAT_ORI, CVE_PREDIAL, OBSERVACIONES, TRAMITADOR, EN_USO, IMAGEN, SOLICITANTE, PROPIETARIO, UBICACION, NOTIFICACION, NOTIFICACION_RECHAZO, AVALUO, FOLIO_PAGO_AVALUO, IGUAL_CARTOGRAFIA,CORREOELECTRONICO,CONTROLFOLIO) VALUES \n" +
                     "('ACTIVO','" + fechaAlta.ToString(FormatoFecha) + "',NULL,'UsuarioExterno',NULL,'" + FORMATOMANI + "','MANIFESTACIÓN DE CONSTRUCCIÓN','" + ESTANDAR + "','" + CuentaGlobal + "','" + PREDIAL + "','','',0,NULL,'" + NombreSol + "',NULL,'" + NOMBRE_COMPLETO_UBI + "',NULL,0,1,NULL,0,'"+ correoelec +"',NULL)";
                SqlCommand cmdMCManiAvaluo = new SqlCommand(QueryInsertMCManiAvaluo, cnnInsertMCManiAvaluo);
                cmdMCManiAvaluo.ExecuteNonQuery();
                cnnInsertMCManiAvaluo.Close();
            }
            catch (Exception ErrorMC)
            {
                string DescError = "Error al insertar en Base de Datos MC_AV";
                Response.Redirect("PaginaError.aspx?Error="+ DescError);
            }

            /*Insert into Tramites*/
            try
            {

                SqlConnection cnnInsertTramiteManiAvaluo = new SqlConnection(ConfigurationManager.ConnectionStrings["WFTRAMITESConnectionString"].ToString());
                cnnInsertTramiteManiAvaluo.Open();
                string QueryInsertTramiteManiAvaluo = "insert into WFTRAMITES.dbo.Tramite(ClaveCatastralOriginal, ClaveCatastralEstandar, FechaInicial, FechaFinal, FK_CatEstatus, Pagado, Condonado, FK_Cat_Notaria, Prioridad, Observaciones, FK_Cat_TipodeProcesoTramite, FK_NombreUsuario, Region, Manzana, Lote, Departamento, FK_Cat_TipodePredio, NumeroCuenta, Delegacion, NumeroTramite, FK_Cat_Solicitante, NombrePersonalSolicitante, ObservacionAclaracion, Procede, Entidad, Tipo, Año, Consecutivo, Derivada, TipoBusqueda, Bloqueado, Reserva, Rechazado, NumeroClave, NumeroCuentas, Excentopago, FK_Cat_TipoClaveCatastralGenerada, FK_Cat_TipoInspeccion, PagarReingreso, NumeroTramiteSIC, NumeroTramiteValuacion, ProntoPago, Sector, Localidad, Predio, Edificio, Unidad, Zona, FK_Cat_Municipio, RequiereCartografia, ObjectId_Cartografico, Condominio, OtroEstado, Folio, Corett, FK_Cat_Dependencia, Habilitado, SuperficieConstruccion, SuperficieTerreno, usuarioSolicitante, Observaciones_Rechazo_Direccion, FK_CatCoordinacion, USUARIOCANCELA) VALUES \n" +
                     "('" + CuentaGlobal + "','" + ESTANDAR + "','" + fechaAlta.ToString(FormatoFecha) + "',NULL,2,0,0,0,'Alta',NULL,2,'UsuarioExterno',NULL,NULL,NULL,NULL,1,NULL,0,'" + FORMATOMANI + "',1,'" + NombreSol + "',NULL,NULL,NULL,'MANIFESTACIÓN DE CONSTRUCCIÓN',NULL,NULL,NULL,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,1,0,NULL,NULL,NULL,0,NULL,NULL,1,NULL,NULL,'UsuarioExterno',NULL,1,NULL)";
                SqlCommand cmdTramite = new SqlCommand(QueryInsertTramiteManiAvaluo, cnnInsertTramiteManiAvaluo);
                cmdTramite.ExecuteNonQuery();
                cnnInsertTramiteManiAvaluo.Close();
            }
            catch (Exception Error)
            {
                string DescError = "Error al insertar en Base de Datos Tramite";
                Response.Redirect("PaginaError.aspx?Error="+ DescError);

            }

            /*Insert into Seguimiento Tramite*/
            try
            {

                SqlConnection cnnInsertSeguimientoManiAvaluo = new SqlConnection(ConfigurationManager.ConnectionStrings["WFTRAMITESConnectionString"].ToString());
                cnnInsertSeguimientoManiAvaluo.Open();
                string QueryInsertSeguimientoManiAvaluo = "insert into WFTRAMITES.dbo.SeguimientoTramite (FK_NumeroTramite,FK_Cat_TipoProcesoTramite,FK_Cat_Coordinacion,FK_Cat_EstatusTramite,Tarea,TipoFlujo,Observaciones,Duracion,Orden,	FK_Cat_OpcionesSistema) VALUES \n" +
                        "('" + FORMATOMANI + "',2,1,5,'SOLICITUD DE MANIFESTACIÓN ELECTRÓNICA','Normal',NULL,20,1,1202)," +
                        "('" + FORMATOMANI + "',2,1,2,'ACTUALIZACIÓN CATASTRAL DE MANIFESTACIÓN','Normal',NULL,20,2,1211)," +
                        "('" + FORMATOMANI + "',2,1,7,'FIRMA ELECTRÓNICA TRAMITADOR MANIFESTACIÓN','Normal',NULL,20,3,1213)," +
                        "('" + FORMATOMANI + "',2,1,7,'FIRMA ELECTRONICA DIRECCION','Normal',NULL,20,4,184)";
                SqlCommand cmdSeguimiento = new SqlCommand(QueryInsertSeguimientoManiAvaluo, cnnInsertSeguimientoManiAvaluo);
                cmdSeguimiento.ExecuteNonQuery();
                cnnInsertSeguimientoManiAvaluo.Close();

            }
            catch (Exception Error)
            {
                string DescError = "Error al insertar en Base de Datos seguimiento";
                Response.Redirect("PaginaError.aspx?Error="+ DescError);
            }

        }

        protected void ValidarCampos()
        {

            string solicitante = inputSolicitanteUpload.Text;/*TextBox Nombre*/
            string correoEmail = CorreoElectronico.Text; /*TextBox CorreoElectronico*/
            Boolean Archivo = FileUploadControl.HasFile; /*InputField Archivos*/
            string Tramite = TxtTramite.Text; /*TextBox Tramite*/
            string FolioPago = TxtFolioPago.Text; /*TextBox FolioPago*/
            int declaracion = 0;
            string estatus = string.Empty;
            string estatus2 = string.Empty;
            string estatussinesp = string.Empty;
            /*No se ingreso Nombre, Archivos ni Correo */
            if (solicitante.Length == 0 && Archivo == false && correoEmail.Length == 0)
            {

                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "warningsalert('Faltó Ingresar Nombre de Solicitante, Correo Electronico y seleccionar documentos digitalizados.');", true);
            }
            else
            {
                /*Se ingreso solo nombre */
                if (solicitante.Length > 0 && Archivo == false && correoEmail.Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "warningsalert('Faltó Ingresar Correo Electronico y seleccionar documentos digitalizados.');", true);
                }
                else
                {
                    /*se ingreso solo correo*/
                    if (solicitante.Length == 0 && Archivo == false && correoEmail.Length > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "warningsalert('Faltó Ingresar Nombre de Solicitante y seleccionar documentos digitalizados.');", true);
                    }
                    else
                    {
                        /*Se ingreso archivo de identidad, */
                        if (solicitante.Length == 0 && Archivo == true && correoEmail.Length == 0)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "warningsalert('Faltó Ingresar Nombre de Solicitante y Correo Electronico');", true);
                        }
                        else
                        {
                            /*Se ingreso Nombre de Solicitante y Correo Electronico*/
                            if (solicitante.Length > 0 && Archivo == false && correoEmail.Length > 0)
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "warningsalert('Faltó seleccionar documentos digitalizados.');", true);
                            }
                            else
                            {
                                /*Se ingreso Nombre de solicitante y Documentos Digitalizados*/
                                if (solicitante.Length > 0 && Archivo == true && correoEmail.Length == 0)
                                {
                                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "warningsalert('Faltó Ingresar Correo Electronico.');", true);
                                }
                                else
                                {
                                    /*Se ingreso Documentos Digitalizados y Correo Electronico*/
                                    if (solicitante.Length == 0 && Archivo == true && correoEmail.Length == 0)
                                    {
                                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "warningsalert('Faltó Ingresar Nombre de Solicitante.');", true);
                                    }
                                    else
                                    {
                                        /*Se llenaron todos los campos*/
                                        if (solicitante.Length > 0 && Archivo == true && correoEmail.Length > 0)
                                        {

                                            if (Tramite == "MANIFESTACIÓN DE CONSTRUCCIÓN")
                                            {

                                                InsertarInfoMani();
                                                GuardarArchivoManifestacion();
                                                ObtenerFolioControlManifestacion(correoEmail);

                                            }
                                            if (Tramite == "AVALÚO CATASTRAL")
                                            {
                                                InsertarInfoAvaluo();
                                                GuardarArchivoAvaluo();
                                                ObtenerFolioControlAvaluo(correoEmail);
                                                ///*Validar el Pago*/
                                                //if (FolioPago.Contains("AE"))
                                                //{
                                                //    string resultadoconsultaac = string.Empty;
                                                //    string resultadoconsultamc_av = string.Empty;


                                                //    resultadoconsultaac = rp.ConsultaPagoTramiteAC(FolioPago);
                                                //    resultadoconsultamc_av = rp.ConsultaPagoTramiteMC_AV(FolioPago);

                                                //    //verificar que el folio de pago no este en algun otro tramite
                                                //    if(resultadoconsultaac == "bloquear" || resultadoconsultamc_av == "bloquear")
                                                //    {
                                                //        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "warningsalert('Pago utilizado verificar cfdi');", true);
                                                //    }
                                                //    else
                                                //    {
                                                //        DeclaracionPagos.SDeclaracionSoapClient refere = new DeclaracionPagos.SDeclaracionSoapClient();
                                                //        var other = refere.validaDeclaracionReferenciaRPP(FolioPago);
                                                //        var mytype = other.GetType();
                                                //        estatus = other.Element("InfoVehiculo").Element("estatus").Value;
                                                //        estatussinesp = estatus.Trim();
                                                //        MessageBox.Show(estatussinesp);
                                                //        if (estatussinesp == "APLICADO")
                                                //        {
                                                //            wa.ActualizarFolioPago("APLICADO", FolioPago);
                                                //            InsertarInfoAvaluo();
                                                //            GuardarArchivoAvaluo();
                                                //            ObtenerFolioControlAvaluo(correoEmail);

                                                //        }
                                                //        else
                                                //        {
                                                //            if (estatussinesp == "FOLIO UTILIZADO")
                                                //            {
                                                //                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "warningsalert('Pago utilizado verificar folio');", true);
                                                //            }
                                                //            else
                                                //            {
                                                //                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "warningsalert('Pendiente de recibir el pago en finanzas. Nota: El pago puede tardar hasta 2 dias habiles en reflejarse.');", true);
                                                //            }
                                                //        }
                                                //    }                               
                                                //}
                                                //else
                                                //{
                                                //    if (FolioPago == "" || FolioPago.Length != 16)
                                                //    {
                                                //        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "warningsalert('Ingresar CFDI');", true);
                                                //    }
                                                //    else
                                                //    {
                                                //        string resultadoconsultaac = string.Empty;
                                                //        string resultadoconsultamc_av = string.Empty;


                                                //        resultadoconsultaac = rp.ConsultaPagoTramiteAC(FolioPago);
                                                //        resultadoconsultamc_av = rp.ConsultaPagoTramiteMC_AV(FolioPago);

                                                //        if(resultadoconsultaac == "bloquear" || resultadoconsultamc_av == "bloquear")
                                                //        {
                                                //            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "warningsalert('Pago utilizado verificar CFDI');", true);
                                                //        }
                                                //        else
                                                //        {
                                                //            PagosCaja.WSResguardosSoapClient SD = new PagosCaja.WSResguardosSoapClient();
                                                //            var res = SD.getInformacionRecibo(FolioPago);
                                                //            int indice = res.IndexOf("Estatus");
                                                //            estatus = res.Substring(40, 8);
                                                //            estatus2 = res.Substring(40, 15);

                                                //            if (estatus == "APLICADO")
                                                //            {
                                                //                wa.ActualizarFolioPago("APLICADO", FolioPago);
                                                //                InsertarInfoAvaluo();
                                                //                GuardarArchivoAvaluo();
                                                //                ObtenerFolioControlAvaluo(correoEmail);
                                                //            }
                                                //            else
                                                //            {
                                                //                if (estatus2 == "FOLIO UTILIZADO")
                                                //                {
                                                //                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "warningsalert('Pago utilizado verificar CFDI');", true);
                                                //                }
                                                //                else
                                                //                {
                                                //                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "warningsalert('Pendiente de recibir el pago, Nota: El pago puede tardar hasta 2 dias habiles en reflejarse.');", true);
                                                //                }
                                                //            }
                                                //        }
                                                //    }
                                                //}


                                            }
                                            if (Tramite == "MANIFESTACIÓN CON AVALÚO")
                                            {
                                                InsertarInfoManiyAvaluo();
                                                GuardarArchivosManiAvaluo();
                                                ObtenerFolioControlManiAvaluo(correoEmail);
                                                //    /*Validar el Pago*/
                                                //    if (FolioPago.Contains("AE"))
                                                //    {
                                                //        string resultadoconsultaac = string.Empty;
                                                //        string resultadoconsultamc_av = string.Empty;

                                                //        resultadoconsultaac = rp.ConsultaPagoTramiteAC(FolioPago);
                                                //        resultadoconsultamc_av = rp.ConsultaPagoTramiteMC_AV(FolioPago);

                                                //        if (resultadoconsultaac == "bloquear" || resultadoconsultamc_av == "bloquear")
                                                //        {
                                                //            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "warningsalert('Pago utilizado verificar folio');", true);
                                                //        }
                                                //        else
                                                //        {
                                                //            DeclaracionPagos.SDeclaracionSoapClient refere = new DeclaracionPagos.SDeclaracionSoapClient();
                                                //            var other = refere.validaDeclaracionReferenciaRPP(FolioPago);
                                                //            var mytype = other.GetType();
                                                //            estatus = other.Element("InfoVehiculo").Element("estatus").Value;
                                                //            estatussinesp = estatus.Trim();

                                                //            if (estatussinesp == "APLICADO")
                                                //            {
                                                //                InsertarInfoManiyAvaluo();
                                                //                GuardarArchivosManiAvaluo();
                                                //                ObtenerFolioControlManiAvaluo(correoEmail);
                                                //            }
                                                //            else
                                                //            {
                                                //                if (estatussinesp == "EJERCIDO")
                                                //                {
                                                //                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "warningsalert('Pago utilizado verificar cfdi');", true);
                                                //                }
                                                //                else
                                                //                {
                                                //                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "warningsalert('Pendiente de recibir el pago en finanzas. Nota: El pago puede tardar hasta 2 dias habiles en reflejarse.');", true);
                                                //                }
                                                //            }
                                                //        }



                                                //    }
                                                //    else
                                                //    {
                                                //        if (FolioPago == "" || FolioPago.Length!=16)
                                                //        {
                                                //            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "warningsalert('Ingresar CFDI');", true);
                                                //        }
                                                //        else
                                                //        {

                                                //            string resultadoconsultaac = string.Empty;
                                                //            string resultadoconsultamc_av = string.Empty;


                                                //            resultadoconsultaac = rp.ConsultaPagoTramiteAC(FolioPago);
                                                //            resultadoconsultamc_av = rp.ConsultaPagoTramiteMC_AV(FolioPago);

                                                //            if (resultadoconsultaac == "bloquear" || resultadoconsultamc_av == "bloquear")
                                                //            {
                                                //                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "warningsalert('Pago utilizado verificar cfdi');", true);
                                                //            }
                                                //            else
                                                //            {
                                                //                //declaracion = Convert.ToInt32(FolioPago);
                                                //                PagosCaja.WSResguardosSoapClient SD = new PagosCaja.WSResguardosSoapClient();
                                                //                var res = SD.getInformacionRecibo(FolioPago);
                                                //                int indice = res.IndexOf("Estatus");
                                                //                estatus = res.Substring(40, 8);
                                                //                estatus2 = res.Substring(40, 15);

                                                //                if (estatus == "APLICADO")
                                                //                {
                                                //                    InsertarInfoManiyAvaluo();
                                                //                    GuardarArchivosManiAvaluo();
                                                //                    ObtenerFolioControlManiAvaluo(correoEmail);
                                                //                }
                                                //                else
                                                //                {
                                                //                    if (estatus2 == "FOLIO UTILIZADO")
                                                //                    {
                                                //                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "warningsalert('Pago utilizado verificar CFDI');", true);
                                                //                    }
                                                //                    else
                                                //                    {
                                                //                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "warningsalert('Pendiente de recibir el pago, Nota: El pago puede tardar hasta 2 dias habiles en reflejarse.');", true);
                                                //                    }
                                                //                }
                                                //            }

                                                //        }
                                                //    }
                                                //}
                                            }
                                            else
                                            {
                                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "warningsalert('LLenar campos faltantes.');", true);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        protected void RegresarButton_Click(object sender, EventArgs e)
        {
            string clavecatastral = cuentasubFiles.Text;
            var plaintextBytes = Encoding.UTF8.GetBytes(clavecatastral);
            var encryptedValue = Convert.ToBase64String(MachineKey.Protect(plaintextBytes, "value"));
            var encryptedCode = HttpUtility.UrlEncode(encryptedValue);

            Response.Redirect("InformacionPredio.aspx?value="+ encryptedCode);
        }

        protected void GuardarArchivoManifestacion()
        {
            
            string strFolder;
            strFolder = @"\\"+ IPServidor + "\\irc\\Archivos\\ManifestacionCatastralEnLinea";

            string path = strFolder + "\\" + CuentaGlobal;

            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
                foreach (HttpPostedFile htfiles in FileUploadControl.PostedFiles)
                {
                    string strFileName = Path.GetFileName(htfiles.FileName);
                    htfiles.SaveAs(path + "\\" + strFileName);

                }
                ListofuploadedFiles.Visible = true;
                ListofuploadedFiles.Text = FileUploadControl.PostedFiles.Count.ToString() + " Files Uploaded Successfully";
            }
            else
            {
                foreach (HttpPostedFile htfiles in FileUploadControl.PostedFiles)
                {
                    string strFileName = Path.GetFileName(htfiles.FileName);
                    htfiles.SaveAs(path + "\\" + strFileName);

                }
                ListofuploadedFiles.Visible = true;
                ListofuploadedFiles.Text = FileUploadControl.PostedFiles.Count.ToString() + " Files Uploaded Successfully";
            }
        }

        private void GuardarArchivoAvaluo()
        {
            
            string strFolder;
            strFolder = @"\\"+ IPServidor + "\\irc\\Archivos\\AvaluoCatastralEnLinea";

            string path = strFolder + "\\" + CuentaGlobal;

            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
                foreach (HttpPostedFile htfiles in FileUploadControl.PostedFiles)
                {
                    string strFileName = Path.GetFileName(htfiles.FileName);
                    htfiles.SaveAs(path + "\\" + strFileName);

                }
                ListofuploadedFiles.Visible = true;
                ListofuploadedFiles.Text = FileUploadControl.PostedFiles.Count.ToString() + " Files Uploaded Successfully";
            }
            else
            {
                foreach (HttpPostedFile htfiles in FileUploadControl.PostedFiles)
                {
                    string strFileName = Path.GetFileName(htfiles.FileName);
                    htfiles.SaveAs(path + "\\" + strFileName);

                }
                ListofuploadedFiles.Visible = true;
                ListofuploadedFiles.Text = FileUploadControl.PostedFiles.Count.ToString() + " Files Uploaded Successfully";
            }

        }

        protected void GuardarArchivosManiAvaluo()
        {
            string strFolder;
            strFolder = @"\\"+IPServidor+"\\irc\\Archivos\\ManifestacionCatastralEnLinea";

            string path = strFolder + "\\" + CuentaGlobal;

            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
                foreach (HttpPostedFile htfiles in FileUploadControl.PostedFiles)
                {
                    string strFileName = Path.GetFileName(htfiles.FileName);
                    htfiles.SaveAs(path + "\\" + strFileName);

                }
                ListofuploadedFiles.Visible = true;
                ListofuploadedFiles.Text = FileUploadControl.PostedFiles.Count.ToString() + " Files Uploaded Successfully";
            }
            else
            {
                foreach (HttpPostedFile htfiles in FileUploadControl.PostedFiles)
                {
                    string strFileName = Path.GetFileName(htfiles.FileName);
                    htfiles.SaveAs(path + "\\" + strFileName);

                }
                ListofuploadedFiles.Visible = true;
                ListofuploadedFiles.Text = FileUploadControl.PostedFiles.Count.ToString() + " Files Uploaded Successfully";
            }

        }

        protected void ObtenerFolioControlManifestacion(string EmailCat)
        {
           
            D_EnvioCorreo wp = new D_EnvioCorreo();

            if (conCarto == 1)
            {
                resp = "Si";
            }
            else
            {
                resp = "No";
            }

            string formatoFechaBoleta = dia.ToString() + "/" + Mes.ToString() + "/" + year.ToString();

            /*Obtener un Control de Folio para descargar*/
            CatastroControlFoliosSoapClient cf = new CatastroControlFoliosSoapClient(); //Produccion
            ControlFolios.FolioSolicitud f = new ControlFolios.FolioSolicitud(); //Produccion
            //CatastroControlFoliosSoapClient cf = new CatastroControlFoliosSoapClient();  //Desarrollo Prueba
            //ControlDeFolioDesa.FolioSolicitud f = new ControlDeFolioDesa.FolioSolicitud();  //Desarrollo Prueba
            //ControldeFolios.FolioResp r= new ControldeFolios.FolioResp();


            f.folio_SIC = FORMATOMANI;
            f.mani_con_avaluo = false;
            f.nombreSolicitante = NombreSol;
            f.observacion = "Manifestacion en Linea " + FORMATOMANI;

            var resultado = cf.AgregarFolio(f, "c@Age19#");

            int FolioControl = resultado.id_folio;
            string Contraseña = resultado.clave;
            string Respuesta = resultado.res;
            string mensaje = resultado.msg;

            if (Respuesta == "error")
            {
                string DescError = "Control de Folios, No responde";
                Response.Redirect("PaginaError.aspx?Error="+ DescError);

            }
            else
            {
                PdfWriter writer = new PdfWriter("\\\\"+IPServidor+"\\irc\\Archivos\\ManifestacionCatastralEnLinea\\" + CuentaGlobal + "\\BoletaparaDescargar.pdf"); 

                PdfDocument pdfDoc = new PdfDocument(writer);
                Document document = new Document(pdfDoc);
                BarcodeQRCode qrCode = new BarcodeQRCode("https://eservicios2.aguascalientes.gob.mx/seguot/seguimientomovil/Catastro.aspx" + "?folio=" + FolioControl.ToString());
                PdfFormXObject barcodeObject = qrCode.CreateFormXObject(ColorConstants.BLACK, pdfDoc);
                iText.Layout.Element.Image barcodeImage = new iText.Layout.Element.Image(barcodeObject).SetWidth(100f).SetHeight(100f);
                iText.Layout.Element.Table tabla = new iText.Layout.Element.Table(1, false);
                Cell cell11 = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(12)
                    .Add(new Paragraph("INSTITUTO REGISTRAL Y CATASTRAL \n DIRECCION GENERAL DE CATASTRO"));
                Cell cell21 = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .Add(new Paragraph("NO. DE FOLIO: " + FolioControl + "\n CONTRASEÑA: " + Contraseña));
                Cell cell31 = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .Add(new Paragraph("TIPO DE TRÁMITE \n" + "Manifestación Catastral"));
                Cell cell41 = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .Add(new Paragraph("OBSERVACIÓNES:" + "Cuenta Catastral: " + CuentaGlobal + " " + FORMATOMANI + " " + "Igual a cartografía:" + resp));
                Cell cell51 = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .Add(new Paragraph("FECHA DE RECEPCIÓN DE TRÁMITE: " + formatoFechaBoleta));
                Cell cell61 = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .Add(new Paragraph("NOMBRE SOLICITANTE: " + NombreSol));
                Text bluetext = new Text("http://eservicios2.aguascalientes.gob.mx/seguot/seguimientomovil/Catastro.aspx")
                    .SetFontColor(ColorConstants.BLUE);
                Cell cell71 = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .Add(new Paragraph("Instrucciones para descargar el trámite: \n 1. Entre a la página (Dar click en la Liga)"))
                    .Add(new Paragraph(bluetext))
                    .Add(new Paragraph("2.Capture el número de folio y presione el botón Consulta, se mostrará una pequeña imagen de color rojo con el texto pdf en blanco. \n 3.De click sobre la imagen de color rojo. \n 4.Ingrese la contraseña y presione el botón Aceptar."));
                tabla.AddCell(cell11);
                tabla.AddCell(cell21);
                tabla.AddCell(cell31);
                tabla.AddCell(cell41);
                tabla.AddCell(cell51);
                tabla.AddCell(cell61);
                tabla.AddCell(cell71);
                document.Add(tabla);
                document.Add(new Paragraph().Add(barcodeImage));
                document.Close();

                wp.EnvioCorreoAviso(EmailCat, CuentaGlobal);
                /*Encriptar Correo*/
                var EmailByte = Encoding.UTF8.GetBytes(EmailCat);
                var encryptedEmail = Convert.ToBase64String(MachineKey.Protect(EmailByte, "value"));
                var encryptedValue = HttpUtility.UrlEncode(encryptedEmail);
                Response.Redirect("DescargaBoleta.aspx?value=" + encryptedValue);

            }
        }

        /*Control de Folio */
        private void ObtenerFolioControlAvaluo(string EmailCat)
        {
            
            D_EnvioCorreo wp = new D_EnvioCorreo();
            
            string formatoFechaBoleta = dia.ToString() + "/" + Mes.ToString() + "/" + year.ToString(); //Formatear fecha para ingresarla ala boleta
            /*Obtener un Control de Folio para descargar*/

            CatastroControlFoliosSoapClient cfAvaluo = new CatastroControlFoliosSoapClient(); //Produccion
            ControlFolios.FolioSolicitud fAvaluo = new ControlFolios.FolioSolicitud();  //Produccion
            //CatastroControlFoliosSoapClient cfAvaluo = new CatastroControlFoliosSoapClient(); //Desarrollo Prueba
            //ControlDeFolioDesa.FolioSolicitud fAvaluo = new ControlDeFolioDesa.FolioSolicitud(); //Desarrollo Prueba
                                                                                                 ////ControldeFolios.FolioResp r= new ControldeFolios.FolioResp();

            fAvaluo.folio_SIC = FORMATOAVALUO;
            fAvaluo.nombreSolicitante = NombreSolAvaluo;
            fAvaluo.observacion = "Avalúo Catastral, " + FORMATOAVALUO;

            var resultadoAvaluo = cfAvaluo.AgregarFolioAvaluoCatastral(fAvaluo, "c@Age19#");

            int FolioControlAvaluo = resultadoAvaluo.id_folio;
            string ContraseñaAvaluo = resultadoAvaluo.clave;
            string RespuestaAvaluo = resultadoAvaluo.res;
            string mensajeAvaluo = resultadoAvaluo.msg;
            


            if (RespuestaAvaluo == "error")
            {
                string DescError = "Error en Control de Folios";
                Response.Redirect("PaginaError.aspx?Error="+DescError);
            }
            else
            {
                wa.ActualizarSIS_AC(FolioControlAvaluo, FORMATOAVALUO);
                PdfWriter writerAvaluo = new PdfWriter("\\\\"+IPServidor+"\\irc\\Archivos\\AvaluoCatastralEnLinea\\" + CuentaGlobal + "\\BoletaparaDescargar.pdf"); //Produccion

                PdfDocument pdfDocAvaluo = new PdfDocument(writerAvaluo);
                Document documentAvaluo = new Document(pdfDocAvaluo);
                BarcodeQRCode qrCodeAvaluo = new BarcodeQRCode("https://eservicios2.aguascalientes.gob.mx/seguot/seguimientomovil/Catastro.aspx" + "?folio=" + FolioControlAvaluo.ToString());
                PdfFormXObject barcodeObjectAvaluo = qrCodeAvaluo.CreateFormXObject(ColorConstants.BLACK, pdfDocAvaluo);
                iText.Layout.Element.Image barcodeImageAvaluo = new iText.Layout.Element.Image(barcodeObjectAvaluo).SetWidth(100f).SetHeight(100f);
                iText.Layout.Element.Table tablaAvaluo = new iText.Layout.Element.Table(1, false);
                Cell cell11Avaluo = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(12)
                    .Add(new Paragraph("INSTITUTO REGISTRAL Y CATASTRAL \n DIRECCION GENERAL DE CATASTRO"));
                Cell cell21Avaluo = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .Add(new Paragraph("NO. DE FOLIO: " + FolioControlAvaluo + "\n CONTRASEÑA: " + ContraseñaAvaluo));
                Cell cell31Avaluo = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .Add(new Paragraph("TIPO DE TRÁMITE \n" + "Avalúo Catastral"));
                Cell cell41Avaluo = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .Add(new Paragraph("OBSERVACIÓNES:" + "Cuenta Catastral: " + CuentaGlobal + " " + FORMATOAVALUO + " "));
                Cell cell51Avaluo = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .Add(new Paragraph("FECHA DE RECEPCIÓN DE TRÁMITE: " + formatoFechaBoleta));
                Cell cell61Avaluo = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .Add(new Paragraph("NOMBRE SOLICITANTE: " + NombreSolAvaluo));
                Text bluetextAvaluo = new Text("http://eservicios2.aguascalientes.gob.mx/seguot/seguimientomovil/Catastro.aspx")
                    .SetFontColor(ColorConstants.BLUE);
                Cell cell71Avaluo = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.LEFT)
                .Add(new Paragraph("Instrucciones para descargar el trámite: \n 1. Entre a la página (Dar click en la Liga)"))
                .Add(new Paragraph(bluetextAvaluo))
                .Add(new Paragraph("2.Capture el número de folio y presione el botón Consulta, se mostrará una pequeña imagen de color rojo con el texto pdf en blanco. \n 3.De click sobre la imagen de color rojo. \n 4.Ingrese la contraseña y presione el botón Aceptar."));
                tablaAvaluo.AddCell(cell11Avaluo);
                tablaAvaluo.AddCell(cell21Avaluo);
                tablaAvaluo.AddCell(cell31Avaluo);
                tablaAvaluo.AddCell(cell41Avaluo);
                tablaAvaluo.AddCell(cell51Avaluo);
                tablaAvaluo.AddCell(cell61Avaluo);
                tablaAvaluo.AddCell(cell71Avaluo);
                documentAvaluo.Add(tablaAvaluo);
                documentAvaluo.Add(new Paragraph().Add(barcodeImageAvaluo));
                documentAvaluo.Close();

                wp.EnvioCorreoAvaluo(EmailCat, CuentaGlobal);

                /*Encriptar Correo*/
                var EmailByte = Encoding.UTF8.GetBytes(EmailCat);
                var encryptedEmail = Convert.ToBase64String(MachineKey.Protect(EmailByte, "value"));
                var encryptedValue = HttpUtility.UrlEncode(encryptedEmail);
                Response.Redirect("DescargaBoleta.aspx?value="+ encryptedValue);
            }
        }

        protected void ObtenerFolioControlManiAvaluo(string EmailCat)
        {
            
            D_EnvioCorreo wp = new D_EnvioCorreo();
            /*Obtener un Control de Folio para descargar*/

            CatastroControlFoliosSoapClient cf = new CatastroControlFoliosSoapClient(); //Produccion
            ControlFolios.FolioSolicitud f = new ControlFolios.FolioSolicitud(); //Produccion
            //CatastroControlFoliosSoapClient cf = new CatastroControlFoliosSoapClient(); //Desarrollo Prueba
            //ControlDeFolioDesa.FolioSolicitud f = new ControlDeFolioDesa.FolioSolicitud(); //Desarrollo Prueba

            //ControldeFolios.FolioResp r= new ControldeFolios.FolioResp();

            f.folio_SIC = FORMATOMANI;
            f.mani_con_avaluo = true;
            f.nombreSolicitante = NombreSol;
            f.observacion = "Manifestacion En linea, " + FORMATOMANI;

            var resultado = cf.AgregarFolio(f, "c@Age19#");

            int FolioControl = resultado.id_folio;
            string Contraseña = resultado.clave;
            string Respuesta = resultado.res;
            string mensaje = resultado.msg;


            if (conCarto == 1)
            {
                resp = "Si";
            }
            else
            {
                resp = "No";
            }


            if (Respuesta == "error")
            {
                string DescError = "Error en el Control de Folios";
                Response.Redirect("PaginaError.aspx?value="+ DescError);
            }
            else
            {
                PdfWriter writerManiAvaluo = new PdfWriter("\\\\"+IPServidor+"\\irc\\Archivos\\ManifestacionCatastralEnLinea\\" + CuentaGlobal + "\\BoletaparaDescargar.pdf"); //Produccion


                PdfDocument pdfDocManiAvaluo = new PdfDocument(writerManiAvaluo);
                Document documentManiAvaluo = new Document(pdfDocManiAvaluo);
                BarcodeQRCode qrCodeManiAvaluo = new BarcodeQRCode("https://eservicios2.aguascalientes.gob.mx/seguot/seguimientomovil/Catastro.aspx" + "?folio=" + FolioControl.ToString());
                PdfFormXObject barcodeObjectManiAvaluo = qrCodeManiAvaluo.CreateFormXObject(ColorConstants.BLACK, pdfDocManiAvaluo);
                iText.Layout.Element.Image barcodeImageManiAvaluo = new iText.Layout.Element.Image(barcodeObjectManiAvaluo).SetWidth(100f).SetHeight(100f);
                iText.Layout.Element.Table tablaManiAvaluo = new iText.Layout.Element.Table(1, false);
                Cell cell11ManiAvaluo = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(12)
                    .Add(new Paragraph("INSTITUTO REGISTRAL Y CATASTRAL \n DIRECCION GENERAL DE CATASTRO"));
                Cell cell21ManiAvaluo = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .Add(new Paragraph("NO. DE FOLIO: " + FolioControl + "\n CONTRASEÑA: " + Contraseña));
                Cell cell31ManiAvaluo = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .Add(new Paragraph("TIPO DE TRÁMITE \n" + "Manifestación Catastral con Avalúo"));
                Cell cell41ManiAvaluo = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .Add(new Paragraph("OBSERVACIÓNES:" + "Cuenta Catastral: " + CuentaGlobal + " " + FORMATOMANI + " " + "Igual a cartografía:" + resp + ""));
                Cell cell51ManiAvaluo = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .Add(new Paragraph("FECHA DE RECEPCIÓN DE TRÁMITE: " + formatoFechaBoleta));
                Cell cell61ManiAvaluo = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .Add(new Paragraph("NOMBRE SOLICITANTE: " + NombreSol));
                Text bluetextManiAvaluo = new Text("http://eservicios2.aguascalientes.gob.mx/seguot/seguimientomovil/Catastro.aspx")
                    .SetFontColor(ColorConstants.BLUE);
                Cell cell71ManiAvaluo = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .Add(new Paragraph("Instrucciones para descargar el trámite: \n 1. Entre a la página (Dar click en la Liga)"))
                    .Add(new Paragraph(bluetextManiAvaluo))
                    .Add(new Paragraph("2.Capture el número de folio y presione el botón Consulta, se mostrará una pequeña imagen de color rojo con el texto pdf en blanco. \n 3.De click sobre la imagen de color rojo. \n 4.Ingrese la contraseña y presione el botón Aceptar."));
                tablaManiAvaluo.AddCell(cell11ManiAvaluo);
                tablaManiAvaluo.AddCell(cell21ManiAvaluo);
                tablaManiAvaluo.AddCell(cell31ManiAvaluo);
                tablaManiAvaluo.AddCell(cell41ManiAvaluo);
                tablaManiAvaluo.AddCell(cell51ManiAvaluo);
                tablaManiAvaluo.AddCell(cell61ManiAvaluo);
                tablaManiAvaluo.AddCell(cell71ManiAvaluo);
                documentManiAvaluo.Add(tablaManiAvaluo);
                documentManiAvaluo.Add(new Paragraph().Add(barcodeImageManiAvaluo));
                documentManiAvaluo.Close();
                wp.EnvioCorreoAviso(EmailCat, CuentaGlobal);
                /*Encriptar Correo*/
                var EmailByte = Encoding.UTF8.GetBytes(EmailCat);
                var encryptedEmail = Convert.ToBase64String(MachineKey.Protect(EmailByte, "value"));
                var encryptedValue = HttpUtility.UrlEncode(encryptedEmail);

                Response.Redirect("DescargaBoleta.aspx?value="+ encryptedValue);
            }
        }
        protected void UploadButton_Click(object sender, EventArgs e)
        {
            ValidarCampos();
        }
    }
}