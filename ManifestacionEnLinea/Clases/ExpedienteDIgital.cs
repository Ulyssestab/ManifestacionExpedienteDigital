using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Threading.Tasks; 

namespace ManifestacionEnLinea.Clases
{
    public class ExpedienteDigital
    {
        public int id_ciudadano { get; set; }
        public string d_CURP { get; set; } = string.Empty;
        public string d_RFC { get; set; } = string.Empty;
        public string nombreCompleto { get; set; } = string.Empty;
        public string nombre { get; set; } = string.Empty;
        public string primerApellido { get; set; } = string.Empty;
        public string segundoApellido { get; set; } = string.Empty;
        public string Representante { get; set; } = string.Empty;
        public int id_razonsocial { get; set; }
        public string RazonSocial { get; set; } = string.Empty;
        public string FechaNac { get; set; }
        public string Nacionalidad { get; set; } = string.Empty;
        public string Id_Sexo { get; set; } = string.Empty;
        public string Sexo { get; set; } = string.Empty;
        public string Id_LugarNacimiento { get; set; } = string.Empty;
        public string LugarNacimiento { get; set; } = string.Empty;
        public string Id_estado { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Id_Municipio { get; set; } = string.Empty;
        public string Municipio { get; set; } = string.Empty;
        public string id_localidad { get; set; } = string.Empty;
        public string Localidad { get; set; } = string.Empty;
        public string Colonia { get; set; } = string.Empty;
        public string CP { get; set; } = string.Empty;
        public string Calle { get; set; } = string.Empty;
        public string NoExt { get; set; } = string.Empty;
        public string NoInt { get; set; } = string.Empty;
        public string TelCasa { get; set; } = string.Empty;
        public string TelCelular { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string FotoPerfilURL { get; set; } = string.Empty;
        public string FotoBase64 { get; set; } = string.Empty;
        public bool identidad { get; set; }
        public int porcentaje { get; set; }

        public List<DocumentoExpedienteDigital> Documentos { get; set; } = new List<DocumentoExpedienteDigital>();
        internal void UrlWebServiceExpedienteIdTramite(uint tipoPersona, string clavePersona, string idtramite)
       
        {
            string token = ConfigurationManager.AppSettings["TokenServicioExpediente"];
            string url = $"{ConfigurationManager.AppSettings["UrlWebServiceExpedienteConsultaIDTramite"]}?tipo_persona={tipoPersona}&Identidad={clavePersona}&IdTramite={idtramite}&token={token}";
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "GET";
            using (HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.GetEncoding(response.CharacterSet ?? "utf-8")))
                    {
                        string result = reader.ReadToEnd();
                        List<ExpedienteDigital> tmpExpedientesDigitales = JsonConvert.DeserializeObject<List<ExpedienteDigital>>(result);
                        ExpedienteDigital tmpExpedienteDigital = tmpExpedientesDigitales.FirstOrDefault();
                        if (tmpExpedienteDigital == null)
                        {
                            throw new Exception($"Expediente digital de {clavePersona.ToUpper()} no encontrado");
                        }
                        id_ciudadano = tmpExpedienteDigital.id_ciudadano;
                        d_CURP = tmpExpedienteDigital.d_CURP;
                        d_RFC = tmpExpedienteDigital.d_RFC;
                        nombreCompleto = tmpExpedienteDigital.nombreCompleto;
                        nombre = tmpExpedienteDigital.nombre;
                        primerApellido = tmpExpedienteDigital.primerApellido;
                        segundoApellido = tmpExpedienteDigital.segundoApellido;
                        Representante = tmpExpedienteDigital.Representante;
                        id_razonsocial = tmpExpedienteDigital.id_razonsocial;
                        RazonSocial = tmpExpedienteDigital.RazonSocial;
                        FechaNac = tmpExpedienteDigital.FechaNac;
                        Nacionalidad = tmpExpedienteDigital.Nacionalidad;
                        Id_Sexo = tmpExpedienteDigital.Id_Sexo;
                        Sexo = tmpExpedienteDigital.Sexo;
                        Id_LugarNacimiento = tmpExpedienteDigital.Id_LugarNacimiento;
                        LugarNacimiento = tmpExpedienteDigital.LugarNacimiento;
                        Id_estado = tmpExpedienteDigital.Id_estado;
                        Estado = tmpExpedienteDigital.Estado;
                        Id_Municipio = tmpExpedienteDigital.Id_Municipio;
                        Municipio = tmpExpedienteDigital.Municipio;
                        id_localidad = tmpExpedienteDigital.id_localidad;
                        Localidad = tmpExpedienteDigital.Localidad;
                        Colonia = tmpExpedienteDigital.Colonia;
                        CP = tmpExpedienteDigital.CP;
                        Calle = tmpExpedienteDigital.Calle;
                        NoExt = tmpExpedienteDigital.NoExt;
                        NoInt = tmpExpedienteDigital.NoInt;
                        TelCasa = tmpExpedienteDigital.TelCasa;
                        TelCelular = tmpExpedienteDigital.TelCelular;
                        Correo = tmpExpedienteDigital.Correo;
                        FotoPerfilURL = tmpExpedienteDigital.FotoPerfilURL;
                        FotoBase64 = tmpExpedienteDigital.FotoBase64;
                        identidad = tmpExpedienteDigital.identidad;
                        porcentaje = tmpExpedienteDigital.porcentaje;
                        Documentos = tmpExpedienteDigital.Documentos;
                        
                    }
                }
            }
        }
        public string SaveFileExpediente(WS_Expediente.WS_ExpedienteSubirRequest inserter)
        {
            try
            {
                if (inserter != null)
                {
                    WS_Expediente.WSExpedienteSoapClient wSExpediente = new WS_Expediente.WSExpedienteSoapClient();
                    string result = wSExpediente.WS_ExpedienteSubir(
                        inserter.Tipopersona,
                        inserter.Identidad,
                        inserter.token,
                        inserter.IdDocumento,
                        inserter.archivo,
                        inserter.Expedicion,
                        inserter.Validado,
                        inserter.Revisor,
                        inserter.extencion,
                        inserter.folio,
                        inserter.metaDato);

                    if (result.ToLower().Contains("error"))
                    {
                        throw new Exception(result);
                    }
                    return result;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception Ex)
            {
                return null;
            }

        }

        internal void UrlWebServiceExpedienteWhatsAppSend(string Telefono, string Mensaje, string urlwhats)
        {
            string token = ConfigurationManager.AppSettings["TokenServicioExpediente"];

            string url = ConfigurationManager.AppSettings["urlWebServiceExpediente"];

            // Construir el cuerpo de la solicitud SOAP
            string soapMessage = $@"<?xml version=""1.0"" encoding=""utf-8""?>
                <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                  <soap:Body>
                    <WS_WhatsAppSend xmlns=""https://expedientedigital.aguascalientes.gob.mx/"">
                      <Telefono>{WebUtility.HtmlEncode(Telefono)}</Telefono>
                      <Mensaje>{WebUtility.HtmlEncode(Mensaje)}</Mensaje>
                      <url>{WebUtility.HtmlEncode(urlwhats)}</url>
                      <token>{WebUtility.HtmlEncode(token)}</token>
                    </WS_WhatsAppSend>
                  </soap:Body>
                </soap:Envelope>";

            try
            {
                // Crear la solicitud HTTP
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Headers.Add("SOAPAction", "https://expedientedigital.aguascalientes.gob.mx/WS_WhatsAppSend");
                webRequest.ContentType = "text/xml; charset=utf-8";
                webRequest.Method = "POST";

                // Convertir el mensaje SOAP a bytes UTF-8 y escribirlo en el flujo de la solicitud
                byte[] byteArray = Encoding.UTF8.GetBytes(soapMessage);
                webRequest.ContentLength = byteArray.Length;

                using (Stream dataStream = webRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                // Obtener la respuesta
                using (HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string responseXml = reader.ReadToEnd();
                    //Console.WriteLine("Respuesta SOAP: " + responseXml);
                }
            }
            catch (WebException webEx)
            {
                using (StreamReader reader = new StreamReader(webEx.Response.GetResponseStream()))
                {
                    string errorText = reader.ReadToEnd();
                    //Console.WriteLine("Error en la respuesta del servidor: " + errorText);
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}