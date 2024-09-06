using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ManifestacionEnLinea.DataModel;

namespace ManifestacionEnLinea.Clases
{
    public class Clase
    {
        GDB01001DataContext ContextoGDB = new GDB01001DataContext();
        WFTRAMITESDataContext ContextWftramites = new WFTRAMITESDataContext();

        public SIS_PC_CLAVE_CATASTRAL Predio_Clave_Catastral(string CuentaOriginal)
        {
            try
            {
                string muni = "GDB01";
                string NombreGDB = "";
                if (CuentaOriginal.Length == 17)
                {
                    NombreGDB = muni + "0" + CuentaOriginal.Substring(0, 2);
                }
                else
                {
                    NombreGDB = muni + CuentaOriginal.Substring(5, 3);
                }
                string conexionGDB = System.Configuration.ConfigurationManager.ConnectionStrings[NombreGDB + "ConnectionString"].ConnectionString;
                ContextoGDB = new GDB01001DataContext(conexionGDB);

                if (CuentaOriginal.Length == 17)
                {

                    SIS_PC_CLAVE_CATASTRAL Info = (from record in ContextoGDB.SIS_PC_CLAVE_CATASTRAL
                                                   where record.CVE_CAT_ORI == CuentaOriginal && record.STATUSREGISTROTABLA == "ACTIVO"
                                                   select record).FirstOrDefault();
                    return Info;
                }
                else
                {
                    SIS_PC_CLAVE_CATASTRAL Info = (from record in ContextoGDB.SIS_PC_CLAVE_CATASTRAL
                                                   where record.CVE_CAT_EST == CuentaOriginal && record.STATUSREGISTROTABLA == "ACTIVO"
                                                   select record).FirstOrDefault();
                    return Info;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public SIS_PC_CENTROIDES CoordenadaCentro(string CuentaEstandar)
        {
            if (CuentaEstandar == null)
            {
                return null;
            }
            else
            {


                try
                {
                    string muni = "GDB01";
                    string NombreGDB = "";
                    if (CuentaEstandar.Length == 17)
                    {
                        NombreGDB = muni + "0" + CuentaEstandar.Substring(0, 2);
                    }
                    else
                    {
                        NombreGDB = muni + CuentaEstandar.Substring(5, 3);
                    }
                    string conexionGDB = System.Configuration.ConfigurationManager.ConnectionStrings[NombreGDB + "ConnectionString"].ConnectionString;
                    ContextoGDB = new GDB01001DataContext(conexionGDB);

                    if (CuentaEstandar.Length == 17)
                    {

                        SIS_PC_CENTROIDES Info = (from record in ContextoGDB.SIS_PC_CENTROIDES
                                                  where record.CVE_CAT_EST == CuentaEstandar && record.STATUSREGISTROTABLA == "ACTIVO"
                                                  select record).FirstOrDefault();
                        return Info;
                    }
                    else
                    {
                        SIS_PC_CENTROIDES Info = (from record in ContextoGDB.SIS_PC_CENTROIDES
                                                  where record.CVE_CAT_EST == CuentaEstandar && record.STATUSREGISTROTABLA == "ACTIVO"
                                                  select record).FirstOrDefault();
                        return Info;
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

        }
        public SIS_PC_SUPERFICIES2 InfoSuperficie(string CuentaOriginal)
        {
            try
            {
                string muni = "GDB01";
                string NombreGDB = "";
                if (CuentaOriginal.Length == 17)
                {
                    NombreGDB = muni + "0" + CuentaOriginal.Substring(0, 2);
                }
                else
                {
                    NombreGDB = muni + CuentaOriginal.Substring(5, 3);
                }

                string conexionGDB = System.Configuration.ConfigurationManager.ConnectionStrings[NombreGDB + "ConnectionString"].ConnectionString;
                ContextoGDB = new GDB01001DataContext(conexionGDB);
                if (CuentaOriginal.Length == 17)
                {

                    SIS_PC_SUPERFICIES2 Info = (from record in ContextoGDB.SIS_PC_SUPERFICIES2
                                                   where record.CVE_CAT_ORI == CuentaOriginal && record.STATUSREGISTROTABLA == "ACTIVO"
                                                   select record).FirstOrDefault();
                    return Info;
                }
                else
                {
                    SIS_PC_SUPERFICIES2 Info = (from record in ContextoGDB.SIS_PC_SUPERFICIES2
                                                   where record.CVE_CAT_EST == CuentaOriginal && record.STATUSREGISTROTABLA == "ACTIVO"
                                                   select record).FirstOrDefault();
                    return Info;
                }
            }
            catch
            {
                return null;
            }
        }

        public SIS_PC_CONSTRUCCIONES2 InfoConstrucciones(string CuentaOriginal)
        {
            try
            {
                string muni = "GDB01";
                string NombreGDB = "";
                if (CuentaOriginal.Length == 17)
                {
                    NombreGDB = muni + "0" + CuentaOriginal.Substring(0, 2);
                }
                else
                {
                    NombreGDB = muni + CuentaOriginal.Substring(5, 3);
                }
                string conexionGDB = System.Configuration.ConfigurationManager.ConnectionStrings[NombreGDB + "ConnectionString"].ConnectionString;
                ContextoGDB = new GDB01001DataContext(conexionGDB);

                if (CuentaOriginal.Length == 17)
                {

                    SIS_PC_CONSTRUCCIONES2 Info = (from record in ContextoGDB.SIS_PC_CONSTRUCCIONES2
                                                   where record.CVE_CAT_ORI == CuentaOriginal && record.STATUSREGISTROTABLA == "ACTIVO"
                                                select record).FirstOrDefault();
                    return Info;
                }
                else
                {
                    SIS_PC_CONSTRUCCIONES2 Info = (from record in ContextoGDB.SIS_PC_CONSTRUCCIONES2
                                                   where record.CVE_CAT_EST == CuentaOriginal && record.STATUSREGISTROTABLA == "ACTIVO"
                                                select record).FirstOrDefault();
                    return Info;
                }
            }
            catch
            {
                return null;
            }
        }

        public SIS_TRACAT_ABSTENCIONES ValidarAbstencion(string CuentaOriginal)
        {
            try
            {
                string muni = "GDB01";
                string NombreGDB = "";
                if (CuentaOriginal.Length == 17)
                {
                    NombreGDB = muni + "0" + CuentaOriginal.Substring(0, 2);
                }
                else
                {
                    NombreGDB = muni + CuentaOriginal.Substring(5, 3);
                }
                string conexionGDB = System.Configuration.ConfigurationManager.ConnectionStrings[NombreGDB + "ConnectionString"].ConnectionString;
                ContextoGDB = new GDB01001DataContext(conexionGDB);

                if (CuentaOriginal.Length == 17)
                {

                    SIS_TRACAT_ABSTENCIONES Info = (from record in ContextWftramites.SIS_TRACAT_ABSTENCIONES
                                                    where record.CVE_CAT_ORI == CuentaOriginal && record.STATUSREGISTROTABLA == "ACTIVO"
                                                    select record).FirstOrDefault();
                    return Info;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public SIS_TRACAT_MC ValidarSolicitudMC(string CuentaOriginal)
        {
            try
            {
                string muni = "GDB01";
                string NombreGDB = "";
                if (CuentaOriginal.Length == 17)
                {
                    NombreGDB = muni + "0" + CuentaOriginal.Substring(0, 2);
                }
                else
                {
                    NombreGDB = muni + CuentaOriginal.Substring(5, 3);
                }

                string conexionGDB = System.Configuration.ConfigurationManager.ConnectionStrings[NombreGDB + "ConnectionString"].ConnectionString;
                ContextoGDB = new GDB01001DataContext(conexionGDB);
                if (CuentaOriginal.Length == 17)
                {

                    SIS_TRACAT_MC Info = (from record in ContextWftramites.SIS_TRACAT_MC
                                          where record.CVE_CAT_ORI == CuentaOriginal && record.STATUSREGISTROTABLA == "ACTIVO"
                                                        select record).FirstOrDefault();
                    return Info;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public SIS_TRACAT_MC_AV  ValidarSolicitudMCAV(string CuentaOriginal)
        {
            try
            {
                string muni = "GDB01";
                string NombreGDB = "";
                if (CuentaOriginal.Length == 17)
                {
                    NombreGDB = muni + "0" + CuentaOriginal.Substring(0, 2);
                }
                else
                {
                    NombreGDB = muni + CuentaOriginal.Substring(5, 3);
                }

                string conexionGDB = System.Configuration.ConfigurationManager.ConnectionStrings[NombreGDB + "ConnectionString"].ConnectionString;
                ContextoGDB = new GDB01001DataContext(conexionGDB);
                if (CuentaOriginal.Length == 17)
                {

                    SIS_TRACAT_MC_AV Info = (from record in ContextWftramites.SIS_TRACAT_MC_AV
                                             where record.CVE_CAT_ORI == CuentaOriginal && record.STATUSREGISTROTABLA == "ACTIVO"
                                                        select record).FirstOrDefault();
                    return Info;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public SIS_TRACAT_AC ValidarSolicitudAV(string CuentaOriginal)
        {
            try
            {
                string muni = "GDB01";
                string NombreGDB = "";
                if (CuentaOriginal.Length == 17)
                {
                    NombreGDB = muni + "0" + CuentaOriginal.Substring(0, 2);
                }
                else
                {
                    NombreGDB = muni + CuentaOriginal.Substring(5, 3);
                }
                string conexionGDB = System.Configuration.ConfigurationManager.ConnectionStrings[NombreGDB + "ConnectionString"].ConnectionString;
                ContextoGDB = new GDB01001DataContext(conexionGDB);

                if (CuentaOriginal.Length == 17)
                {

                    SIS_TRACAT_AC Info = (from record in ContextWftramites.SIS_TRACAT_AC
                                          where record.CVE_CAT_ORI == CuentaOriginal && record.STATUSREGISTROTABLA == "ACTIVO"
                                             select record).FirstOrDefault();
                    return Info;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Tramite ValidarTramite(string CuentaOriginal)
        {
            try
            {
                string muni = "GDB01";
                string NombreGDB = "";
                if (CuentaOriginal.Length == 17)
                {
                    NombreGDB = muni + "0" + CuentaOriginal.Substring(0, 2);
                }
                else
                {
                    NombreGDB = muni + CuentaOriginal.Substring(5, 3);
                }
                string conexionGDB = System.Configuration.ConfigurationManager.ConnectionStrings[NombreGDB + "ConnectionString"].ConnectionString;
                ContextoGDB = new GDB01001DataContext(conexionGDB);

                if (CuentaOriginal.Length == 17)
                {

                    Tramite Info = (from record in ContextWftramites.Tramite
                                    where record.ClaveCatastralOriginal == CuentaOriginal && record.FK_CatEstatus == 2
                                    select record).FirstOrDefault();
                    return Info;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public FOLIOPAGOAVALUO BuscarPagoActivo(string CuentaOriginal)
        {
            try
            {
                FOLIOPAGOAVALUO Info = (from record in ContextWftramites.FOLIOPAGOAVALUO
                                        where record.CUENTACATASTRAL == CuentaOriginal && record.STATUSREGISTROTABLA == "ACTIVO"
                                     select record).FirstOrDefault();
                return Info;



            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public FOLIOPAGOAVALUO CrearReferenciaPagoAvaluo(string ClaveCatastral)
        {
            string Municipio = ClaveCatastral.Substring(0, 2);

            try
            {
                FOLIOPAGOAVALUO Info = (from record in ContextWftramites.FOLIOPAGOAVALUO
                                        where record.REFERENCIAFOLIO.StartsWith("AE0" + Municipio)
                                        orderby record.ID descending
                                        select record).FirstOrDefault();
                return Info;



            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string CrearReferenciaPagoAvaluoNuevo(string Referencia)
        {
            string FOLIONUEVO = string.Empty;
            string Municipio = Referencia.Substring(2, 3);
            DateTime fechahoy = DateTime.Now;
            string anio = fechahoy.Year.ToString();
            if (Referencia == "")
            {
                FOLIONUEVO = "AE" + Municipio + anio + "000001";

            }
            else
            {
                string numerodeFolio = Referencia.Substring(9, 6);
                int ConvertirNumeroFolio = Convert.ToInt32(numerodeFolio);

                if (ConvertirNumeroFolio < 9)
                {
                    int Incremento = ConvertirNumeroFolio + 1;
                    FOLIONUEVO = "AE" + Municipio + anio + "00000" + Incremento.ToString();
                }
                if (ConvertirNumeroFolio >= 9 && ConvertirNumeroFolio < 99)
                {
                    int Incremento = ConvertirNumeroFolio + 1;
                    FOLIONUEVO = "AE" + Municipio + anio + "0000" + Incremento.ToString();
                }
                if (ConvertirNumeroFolio >= 99 && ConvertirNumeroFolio < 999)
                {
                    int Incremento = ConvertirNumeroFolio + 1;
                    FOLIONUEVO = "AE" + Municipio + anio + "000" + Incremento.ToString();
                }
                if (ConvertirNumeroFolio >= 9999 && ConvertirNumeroFolio < 9999)
                {
                    int Incremento = ConvertirNumeroFolio + 1;
                    FOLIONUEVO = "AE" + Municipio + anio + "00" + Incremento.ToString();
                }
                if (ConvertirNumeroFolio >= 9999 && ConvertirNumeroFolio < 99999)
                {
                    int Incremento = ConvertirNumeroFolio + 1;
                    FOLIONUEVO = "AE" + Municipio + anio + "0" + Incremento.ToString();
                }
                if (ConvertirNumeroFolio >= 99999 && ConvertirNumeroFolio < 999999)
                {
                    int Incremento = ConvertirNumeroFolio + 1;
                    FOLIONUEVO = "AE" + Municipio + anio + Incremento.ToString();
                }
            }
            return FOLIONUEVO;
        }

        public void InsertarPago(string CuentaOriginal, string FolioPago, string NombreConstribuyente)
        {
            
            string Municipio = CuentaOriginal.Substring(0, 2);
            DateTime fechahoy = DateTime.Now;

            FOLIOPAGOAVALUO PagoAvaluo = new FOLIOPAGOAVALUO();
            PagoAvaluo.STATUSREGISTROTABLA = "ACTIVO";
            PagoAvaluo.CUENTACATASTRAL = CuentaOriginal;
            PagoAvaluo.REFERENCIAFOLIO = FolioPago;
            PagoAvaluo.FECHAALTA = fechahoy;
            PagoAvaluo.NOMBRE = NombreConstribuyente;
            PagoAvaluo.ESTADO = "PENDIENTE";


            ContextWftramites.FOLIOPAGOAVALUO.InsertOnSubmit(PagoAvaluo);
            ContextWftramites.SubmitChanges();
        }

        public void actualizarPago(string clavecatastral)
        {
            string conexion = System.Configuration.ConfigurationManager.ConnectionStrings["WFTRAMITESConnectionString"].ConnectionString;
            System.Data.SqlClient.SqlConnection sqlConnection1 = new System.Data.SqlClient.SqlConnection(conexion);
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;

            cmd.CommandText = "UPDATE FOLIOPAGOAVALUO SET STATUSREGISTROTABLA ='ELIMINADO'  WHERE CUENTACATASTRAL = '"+ clavecatastral + "' ";
            cmd.Connection = sqlConnection1;
            sqlConnection1.Open();
            cmd.ExecuteNonQuery();
            sqlConnection1.Close();
        }

        public bool Probarconexion()
        {
            try
            {

                string conexionGDB = System.Configuration.ConfigurationManager.ConnectionStrings["GDB01001ConnectionString"].ConnectionString;
                System.Data.SqlClient.SqlConnection sqlConnection1 = new System.Data.SqlClient.SqlConnection(conexionGDB);
                sqlConnection1.Open();
                sqlConnection1.Close();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public COORDENADAS_MANIFESTACION_AVALUO BuscarCoordenada(string CuentaOriginal)
        {
            try
            {
                COORDENADAS_MANIFESTACION_AVALUO Info = (from record in ContextWftramites.COORDENADAS_MANIFESTACION_AVALUO
                                                         where record.CVE_CAT_ORI == CuentaOriginal && record.STATUSREGISTROTABLA == "ACTIVO"
                                        select record).FirstOrDefault();
                return Info;



            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void InsertarCoordenadas(string CuentaOriginal, string Latitud, string Longitud, string UTMX, string UTMY)
        {
            try
            {

                COORDENADAS_MANIFESTACION_AVALUO Coordenada = new COORDENADAS_MANIFESTACION_AVALUO();
                Coordenada.STATUSREGISTROTABLA = "ACTIVO";
                Coordenada.CVE_CAT_ORI = CuentaOriginal;
                Coordenada.LATITUD = Latitud;
                Coordenada.LONGITUD = Longitud;
                Coordenada.UTM_X = UTMX;
                Coordenada.UTM_Y = UTMY;

                ContextWftramites.COORDENADAS_MANIFESTACION_AVALUO.InsertOnSubmit(Coordenada);
                ContextWftramites.SubmitChanges();
            }
            catch(Exception ex)
            {

            }
            
        }
        public void ActualizarCoordenadas(string CuentaOriginal, string Latitud, string Longitud, string CooordenadaUtmX,string CoordenadaUtmY)
        {
            try
            {
                string conexion = System.Configuration.ConfigurationManager.ConnectionStrings["WFTRAMITESConnectionString"].ConnectionString;
                System.Data.SqlClient.SqlConnection sqlConnection1 = new System.Data.SqlClient.SqlConnection(conexion);
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.CommandText = "UPDATE COORDENADAS_MANIFESTACION_AVALUO SET LATITUD='"+Latitud+"',LONGITUD='"+ Longitud + "',UTM_X='"+ CooordenadaUtmX + "',UTM_Y='"+ CoordenadaUtmY + "'   WHERE CVE_CAT_ORI = '" + CuentaOriginal + "' ";
                cmd.Connection = sqlConnection1;
                sqlConnection1.Open();
                cmd.ExecuteNonQuery();
                sqlConnection1.Close();

            }
            catch(Exception ex)
            {

            }
            

        }


    }
}