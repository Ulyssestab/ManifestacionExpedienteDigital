using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManifestacionEnLinea.Clases
{
    public class DocumentoExpedienteDigital
    {
        public int IdArchivo { get; set; }
        public int IdDocumento { get; set; }
        public string Documento { get; set; } = string.Empty;
        public string Folio { get; set; } = string.Empty;
        public string URLDocumento { get; set; } = string.Empty;
        public string expedicion { get; set; }
        public string vigencia { get; set; }
        public bool validado { get; set; }
        public string metadato { get; set; } = string.Empty;
        public bool conFolio { get; set; }
        public string estatus { get; set; } = string.Empty;
        public string dependencia { get; set; } = string.Empty;
        public string validador { get; set; } = string.Empty;
    }
}