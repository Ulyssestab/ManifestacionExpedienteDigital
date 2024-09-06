using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManifestacionEnLinea.Clases
{
    public class ResponseData
    {
        public string Clave { get; set; }
        public int Cantidad { get; set; }
        public double Valor { get; set; }
        public double ValorTotal { get; set; }
        public string Descripcion { get; set; }
        public string Error { get; set; }
        public int NumError { get; set; }
    }
}