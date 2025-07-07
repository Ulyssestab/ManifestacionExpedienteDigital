using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManifestacionEnLinea.Clases
{
    public class SolicitudMCAV
    {
        public string STATUSREGISTROTABLA { get; set; }
        public DateTime? ALTAREGISTROTABLA { get; set; }
        public DateTime? BAJAREGISTROTABLA { get; set; }
        public string USUARIOALTA { get; set; }
        public string USUARIOBAJA { get; set; }
        public string FOLIO_TRAMITE { get; set; }
        public string TIPO_TRAMITE { get; set; }
        public string CVE_CAT_EST { get; set; }
        public string CVE_CAT_ORI { get; set; }
        public string CVE_PREDIAL { get; set; }
        public string OBSERVACIONES { get; set; }
        public string TRAMITADOR { get; set; }
        public string SOLICITANTE { get; set; }
        public string PROPIETARIO { get; set; }
        public string UBICACION { get; set; }
        public string NOTIFICACION { get; set; }
        public bool NOTIFICACION_RECHAZO { get; set; }
        public bool? AVALUO { get; set; }
        public string FOLIO_PAGO_AVALUO { get; set; }
        public bool? IGUAL_CARTOGRAFIA { get; set; }
        public string CORREOELECTRONICO { get; set; }
        public int? CONTROLFOLIO { get; set; }
        public string PROPIETARIO_CURP { get; set; }
        public string PROPIETARIO_RFC { get; set; }
        public string PROPIETARIO_TELEFONO { get; set; }
        public string PROPIETARIO_DOMICILIO_CALLE { get; set; }
        public string PROPIETARIO_DOMICILIO_NO_EXT { get; set; }
        public string PROPIETARIO_DOMICILIO_NO_INT { get; set; }
        public string PROPIETARIO_DOMICILIO_COLONIA { get; set; }
        public string PROPIETARIO_DOMICILIO_CP { get; set; }
        public string PROPIETARIO_DOMICILIO_LOCALIDAD { get; set; }
        public string PROPIETARIO_DOMICILIO_MUNICIPIO { get; set; }
        public string INMUEBLE_DOMICILIO_CALLE { get; set; }
        public string INMUEBLE_DOMICILIO_NO_EXT { get; set; }
        public string INMUEBLE_DOMICILIO_NO_INT { get; set; }
        public string INMUEBLE_DOMICILIO_COLONIA { get; set; }
        public string INMUEBLE_DOMICILIO_CP { get; set; }
        public string INMUEBLE_DOMICILIO_LOCALIDAD { get; set; }
        public string INMUEBLE_DOMICILIO_MUNICIPIO { get; set; }
        public string LOTE { get; set; }
        public string MANZANA { get; set; }
        public string INDIVISO { get; set; }
        public string FOLIOREAL { get; set; }
        public decimal? CORDENADAX { get; set; }
        public decimal? CORDENADAY { get; set; }
        public string INMUEBLE_TIPO_PREDIO { get; set; }
        public decimal? INMUEBLE_SUPERFICIE_M2 { get; set; }
        public decimal? INMUEBLE_CONSTRUCCION_CONCRETO_M2 { get; set; }
        public decimal? INMUEBLE_CONSTRUCCION_TEJABAN_M2 { get; set; }
        public decimal? INMUEBLE_CONSTRUCCION_TOTAL_M2 { get; set; }
        public string INMUEBLE_AVANCE_OBRA { get; set; }
        public string INMUEBLE_ESTADO_CONSERVACION { get; set; }
        public int? INMUEBLE_ANTIGUEDAD_ANIOS { get; set; }
        public string INMUEBLE_TIPO_USO { get; set; }
        public string INMUEBLE_TIPO_INDUSTRIAL { get; set; }
        public string INMUEBLE_COMERCIAL_Y_SERVICIO { get; set; }
        public string ESTADO_DE_CONSERVACION { get; set; }

    }
}