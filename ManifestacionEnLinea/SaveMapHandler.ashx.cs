using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ManifestacionEnLinea
{
    /// <summary>
    /// Descripción breve de SaveMapHandler
    /// </summary>
    public class SaveMapHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                context.Response.ContentType = "application/json";

                using (var reader = new StreamReader(context.Request.InputStream))
                {
                    var json = reader.ReadToEnd();
                    var data = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Dictionary<string, string>>(json);

                    if (data != null && data.ContainsKey("imageData"))
                    {
                        var imageData = data["imageData"];
                        var filePath = HttpContext.Current.Server.MapPath("~/SavedMaps/mapa.png");

                        byte[] imageBytes = Convert.FromBase64String(imageData);
                        File.WriteAllBytes(filePath, imageBytes);

                        context.Response.StatusCode = 200;
                        context.Response.Write("{\"status\":\"success\"}");
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                        context.Response.Write("{\"status\":\"error\", \"message\":\"Invalid data\"}");
                    }
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.Write("{\"status\":\"error\", \"message\":\"" + ex.Message + "\"}");
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}