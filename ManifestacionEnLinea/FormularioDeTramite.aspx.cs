
using System;
using System.Web.UI;
using ManifestacionEnLinea.Clases;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using System.Linq;

namespace ManifestacionEnLinea


{
    public partial class FormularioDeTramite : System.Web.UI.Page
    {
        Clase ws = new Clase();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["TipoPersona"] != null && Session["ClavePersona"] != null && Session["idtramite"] != null && Session["ClaveCatrastral"] != null)
                {
                    uint tipoPersona = (uint)Session["TipoPersona"];
                    string clavePersona = Session["ClavePersona"].ToString();
                    uint idtramite = (uint)Session["idtramite"];

                    ExpedienteDigital expedienteDigital = new ExpedienteDigital();

                    try
                    {
                        expedienteDigital.UrlWebServiceExpedienteIdTramite(tipoPersona, clavePersona, idtramite.ToString());
                        LlenarFormulario(expedienteDigital);
                        tabla(expedienteDigital);
                    }
                    catch (Exception ex)
                    {
                        lblMensajeError.Text = $"Ocurrió un error al obtener la información: {ex.Message}";
                    }
                }
                else
                {
                    lblMensajeError.Text = "No se encontraron datos de sesión.";
                }
            }

        }


        protected void Redirigir_Click(object sender, EventArgs e)
        {

            Enviar();
            //EviarBD();

        }
            private void Enviar()
            {
                string ClaveCatrastal = Session["ClaveCatrastral"].ToString();
                string TipoTramite = Session["TipoDeTramite"].ToString();

                if (TipoTramite == "01" || TipoTramite == "03")
                {
                    Response.Redirect("Motor.aspx");
                }
                else if (TipoTramite == "02")
                {
                    Response.Redirect("DescargaBoleta.aspx");
                }
            }

        private void EviarBD()
        {
            SolicitudMCAV nuevaSolicitud = new SolicitudMCAV
            {
                STATUSREGISTROTABLA = "ACTIVO",
                ALTAREGISTROTABLA = DateTime.Now,
                USUARIOALTA = "UsuarioExterno",
                FOLIO_TRAMITE = "FT123456",
                TIPO_TRAMITE = "MANIFESTACIÓN DE CONSTRUCCIÓN",
                CVE_CAT_EST = " ",
                CVE_CAT_ORI = TxtClave.Text,
                CVE_PREDIAL = "",
                OBSERVACIONES = "",
                TRAMITADOR = "",
                SOLICITANTE = "",
                PROPIETARIO = TxtNombre.Text,
                CORREOELECTRONICO = TxtCorreo.Text,
                PROPIETARIO_CURP = TxtCURP.Text,
                PROPIETARIO_RFC = TxtRFC.Text,
                PROPIETARIO_TELEFONO = TxtTelefono.Text,
                PROPIETARIO_DOMICILIO_CALLE = TxtDomicilioPropietario.Text,
                PROPIETARIO_DOMICILIO_NO_EXT = TxtNoExtPropietario.Text,
                PROPIETARIO_DOMICILIO_NO_INT = TxtNoIntPropietario.Text,
                PROPIETARIO_DOMICILIO_COLONIA = TxtCalle.Text,
                PROPIETARIO_DOMICILIO_CP = TxtCP.Text,
                PROPIETARIO_DOMICILIO_LOCALIDAD = TxtLocalidad.Text,
                PROPIETARIO_DOMICILIO_MUNICIPIO = TxtMunicipio.Text,
                INMUEBLE_DOMICILIO_CALLE = TxtDomInmueble.Text,
                INMUEBLE_DOMICILIO_COLONIA = TxtColInmueble.Text,
                INMUEBLE_DOMICILIO_NO_EXT = TxtNoExtInmueble.Text,
                INMUEBLE_DOMICILIO_NO_INT = TxtNoIntInmueble.Text,
                INMUEBLE_DOMICILIO_CP = TxtCPInmueble.Text,
                INMUEBLE_DOMICILIO_LOCALIDAD = TxtLocalidadInmueble.Text,
                INMUEBLE_DOMICILIO_MUNICIPIO = TxtMunicipioInmueble.Text,
                LOTE = TxtLote.Text,
                MANZANA = TxtManzana.Text,
                INDIVISO = TxtIndiviso.Text,
                FOLIOREAL = TextFolio.Text,
                CORDENADAX = string.IsNullOrWhiteSpace(TextCordenadasx.Text)
                ? (decimal?)null
                : Convert.ToDecimal(TextCordenadasx.Text),
     
                CORDENADAY = string.IsNullOrWhiteSpace(TextCordenadasy.Text)
                ? (decimal?)null
                : Convert.ToDecimal(TextCordenadasy.Text),
                INMUEBLE_TIPO_PREDIO = string.IsNullOrWhiteSpace(HiddenTipo.Value)
                ? null
                : HiddenAvanceObra.Value,
                INMUEBLE_SUPERFICIE_M2 = string.IsNullOrWhiteSpace(Textsuperficie_predio.Text)
                ? (decimal?)null
                : Convert.ToDecimal(Textsuperficie_predio.Text),

                INMUEBLE_CONSTRUCCION_CONCRETO_M2 = string.IsNullOrWhiteSpace(TextConcreto.Text)
                ? (decimal?)null
                : Convert.ToDecimal(TextConcreto.Text),

                INMUEBLE_CONSTRUCCION_TEJABAN_M2 = string.IsNullOrWhiteSpace(TextTejaban.Text)
                ? (decimal?)null
                : Convert.ToDecimal(TextTejaban.Text),
                INMUEBLE_CONSTRUCCION_TOTAL_M2 = string.IsNullOrWhiteSpace(TextTotal.Text)
                ? (decimal?)null
                : Convert.ToDecimal(TextTotal.Text),
                INMUEBLE_AVANCE_OBRA = string.IsNullOrWhiteSpace(HiddenAvanceObra.Value)
                ? null
                : HiddenAvanceObra.Value,

                INMUEBLE_ANTIGUEDAD_ANIOS = string.IsNullOrWhiteSpace(Textantiguedad.Text)
                ? (int?)null
                : Convert.ToInt32(Textantiguedad.Text),
                INMUEBLE_TIPO_USO = string.IsNullOrWhiteSpace(HiddenTipoUso.Value)
                ? null
                : HiddenTipoUso.Value,

                 INMUEBLE_TIPO_INDUSTRIAL = string.IsNullOrWhiteSpace(HiddenTipoIndustrial.Value)
                ? null
                : HiddenTipoIndustrial.Value,

                 INMUEBLE_COMERCIAL_Y_SERVICIO = string.IsNullOrWhiteSpace(HiddenComercialServicio.Value)
                ? null
                : HiddenComercialServicio.Value,

                 INMUEBLE_ESTADO_CONSERVACION = string.IsNullOrWhiteSpace(Hiddenestado_conserv.Value)
                ? null
                : HiddenComercialServicio.Value

            };
        }



        private void LlenarFormulario(ExpedienteDigital expediente)
        {
            // Datos del propietario
            if (TxtCURP != null) TxtCURP.Text = expediente.d_CURP;
            if (TxtRFC != null) TxtRFC.Text = expediente.d_RFC;
            if (TxtNombre != null) TxtNombre.Text = expediente.nombre;
            if (TxtDomicilioPropietario != null) TxtDomicilioPropietario.Text = expediente.Colonia;
            if (TxtCP != null) TxtCP.Text = expediente.CP;
            if (TxtCalle != null) TxtCalle.Text = expediente.Calle;
            if (TxtNoExtPropietario != null) TxtNoExtPropietario.Text = expediente.NoExt;
            if (TxtNoIntPropietario != null) TxtNoIntPropietario.Text = expediente.NoInt;
            if (TxtTelefono != null) TxtTelefono.Text = expediente.TelCelular;
            if (TxtCorreo != null) TxtCorreo.Text = expediente.Correo;
            if (TxtLocalidad != null) TxtLocalidad.Text = expediente.Localidad;
            if (TxtMunicipio != null) TxtMunicipio.Text = expediente.Municipio;

            // Obtener clave catastral de sesión
            string claveCatastralOriginal = Session["ClaveCatrastral"]?.ToString() ?? "";

            // Mostrar la clave catastral en el campo TxtClave
            if (TxtClave != null) TxtClave.Text = claveCatastralOriginal;

            if (!string.IsNullOrEmpty(claveCatastralOriginal))
            {
                Clase objClase = new Clase();

                try
                {
                    // Obtener ubicación del inmueble
                    DataModel.SIS_PC_UBICACION ubicacion = objClase.ObtenerUbicacionInmueble(claveCatastralOriginal);
                    if (ubicacion != null)
                    {
                        if (TxtDomInmueble != null) TxtDomInmueble.Text = ubicacion.NOMBRE_COMPLETO_VIALIDAD;
                        if (TxtColInmueble != null) TxtColInmueble.Text = ubicacion.NOMBRE_ASENTAMIENTO;
                        if (TxtNoExtInmueble != null) TxtNoExtInmueble.Text = ubicacion.NUMERO_EXTERIOR;
                        if (TxtNoIntInmueble != null) TxtNoIntInmueble.Text = ubicacion.NUMERO_INTERIOR;
                        if (TxtCPInmueble != null) TxtCPInmueble.Text = ubicacion.CP;
                        if (TxtLocalidadInmueble != null) TxtLocalidadInmueble.Text = ubicacion.NOM_LOCALIDAD;
                        if (TxtMunicipioInmueble != null) TxtMunicipioInmueble.Text = ubicacion.NOM_MUNICIPIO;
                        if (TxtLote != null) TxtLote.Text = ubicacion.LOTE;
                        if (TxtManzana != null) TxtManzana.Text = ubicacion.MANZANA;
                    }

                    // Obtener coordenadas
                    DataModel.SIS_PC_CENTROIDES cordenadas = objClase.CoordenadaCentro(claveCatastralOriginal);

                    if (cordenadas != null)
                    {
                        if (TextCordenadasx != null) TextCordenadasx.Text = cordenadas.CENT_PRED_X?.ToString() ?? "";
                        if (TextCordenadasy != null) TextCordenadasy.Text = cordenadas.CENT_PRED_Y?.ToString() ?? "";
                    }
                    else
                    {
                        lblMensajeError.Text += "No se encontró información en la tabla CENTROIDES para la clave catastral: " + claveCatastralOriginal + "<br />";
                    }
                }
                catch (Exception ex)
                {
                    lblMensajeError.Text += "Ocurrió un error al consultar la información del inmueble: " + ex.Message + "<br />";
                }
            }
        }

        private void tabla(ExpedienteDigital expediente)
        {
            string ClaveCatastral = Session["ClaveCatrastral"].ToString(); 
            string TipoTramite = Session["TipoDeTramite"].ToString();
            string CarpetaLocalMani = @"C:\Archivos\ManifestacionCatastralEnLinea";
            string CarpetaClaveMani = Path.Combine(CarpetaLocalMani, ClaveCatastral);
            string CarpetaLocalAvaluo = @"C:\Archivos\AvaluoCatastralEnLinea";
            string CarpetaClaveAvaluo = Path.Combine(CarpetaLocalAvaluo, ClaveCatastral);
            string URL = expediente.Documentos[0].URLDocumento;

            // Descargar documento según tipo de trámite
            if (TipoTramite == "01")
            {
                string CarpetaTramite = "AvaluoCatastralEnLinea";
                if (!Directory.Exists(CarpetaClaveAvaluo))
                {
                    Directory.CreateDirectory(CarpetaClaveAvaluo);
                }
                ws.DescargarDocumento(URL, ClaveCatastral, "INE.pdf", CarpetaTramite);
            }
            else if (TipoTramite == "02" || TipoTramite == "03")
            {
                string CarpetaTramite = "ManifestacionCatastralEnLinea";
                if (!Directory.Exists(CarpetaClaveMani))
                {
                    Directory.CreateDirectory(CarpetaClaveMani);
                }
                ws.DescargarDocumento(URL, ClaveCatastral, "INE.pdf", CarpetaTramite);
            }

          
            string[] requisitos = new string[] { "INE.pdf"};

            string carpetaBase = (TipoTramite == "01") ? CarpetaClaveAvaluo : CarpetaClaveMani;
            string[] rutas = requisitos.Select(r => Path.Combine(carpetaBase, r)).ToArray();

            // Crear la tabla
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Estatus"); 
            dt.Columns.Add("Documento");
            dt.Columns.Add("Ver");

            for (int i = 0; i < requisitos.Length; i++)
            {
                DataRow row = dt.NewRow();
                row["Id"] = i + 1;
                row["Documento"] = requisitos[i];

                if (File.Exists(rutas[i]))
                {
                    row["Estatus"] = "Cargado";
                }
                else
                {
                    row["Estatus"] = "Pendiente";
                }

                row["Ver"] = rutas[i]; 

                dt.Rows.Add(row);
            }

            GridRevision.DataSource = dt;
            GridRevision.DataBind();
        }

        protected void VerDocumentacion_Click(object sender, EventArgs e)
        {

        }

        protected void GridRevision_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
        }



    }
}


