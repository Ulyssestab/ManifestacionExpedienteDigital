using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ManifestacionEnLinea.Clases
{
    public class RevisionPago
    {
        public string ConsultaPagoTramiteAC(string cfdipagoref)
        {
            List<string> TramitePago = new List<string>();
            string cve_cat_ori = string.Empty;
            string folioref = string.Empty;
            string foliotram = string.Empty;

            try
            {
                string nombreBD = "WFTRAMITES";
                string conexionGDBTL = System.Configuration.ConfigurationManager.ConnectionStrings[nombreBD + "ConnectionString"].ConnectionString;
                SqlConnection cnnconsulta;
                cnnconsulta = new SqlConnection(conexionGDBTL);
                cnnconsulta.Open();
                String sqlBuscarPago= @"SELECT CVE_CAT_ORI,FOLIO_PAGO_AVALUO,FOLIO_TRAMITE FROM [" + nombreBD + "].[dbo].[SIS_TRACAT_AC] WHERE FOLIO_PAGO_AVALUO='"+ cfdipagoref + "' AND STATUSREGISTROTABLA='ACTIVO'" ;
                SqlCommand cmdTramites = new SqlCommand(sqlBuscarPago, cnnconsulta);
                SqlDataReader readerTramites = cmdTramites.ExecuteReader();

                while (readerTramites.Read())
                {
                    cve_cat_ori = readerTramites.GetString(0);
                    folioref = readerTramites.GetString(1);
                    foliotram = readerTramites.GetString(2);

                    TramitePago.Add(cve_cat_ori + ", " + folioref + ", " + foliotram);


                }
                cnnconsulta.Close();

                if (TramitePago.Count > 0)
                {
                    return "bloquear";
                }
                else
                {
                    return "ok";
                }
            }
            catch
            {
                return null;
            }
        }

        public string ConsultaPagoTramiteMC_AV(string cfdipagoref)
        {
            List<string> TramitePago = new List<string>();
            string cve_cat_ori = string.Empty;
            string folioref = string.Empty;
            string foliotram = string.Empty;

            try
            {
                string nombreBD = "WFTRAMITES";
                string conexionGDBTL = System.Configuration.ConfigurationManager.ConnectionStrings[nombreBD + "ConnectionString"].ConnectionString;
                SqlConnection cnnconsulta;
                cnnconsulta = new SqlConnection(conexionGDBTL);
                cnnconsulta.Open();
                String sqlBuscarPago = @"SELECT CVE_CAT_ORI,FOLIO_PAGO_AVALUO,FOLIO_TRAMITE FROM [" + nombreBD + "].[dbo].[SIS_TRACAT_MC_AV] WHERE FOLIO_PAGO_AVALUO='" + cfdipagoref + "' AND STATUSREGISTROTABLA='ACTIVO'";
                SqlCommand cmdTramites = new SqlCommand(sqlBuscarPago, cnnconsulta);
                SqlDataReader readerTramites = cmdTramites.ExecuteReader();

                while (readerTramites.Read())
                {
                    cve_cat_ori = readerTramites.GetString(0);
                    folioref = readerTramites.GetString(1);
                    foliotram = readerTramites.GetString(2);

                    TramitePago.Add(cve_cat_ori + ", " + folioref + ", " + foliotram);


                }
                cnnconsulta.Close();

                if (TramitePago.Count > 0)
                {
                    return "bloquear";
                }
                else
                {
                    return "ok";
                }
            }
            catch
            {
                return null;
            }
        }

        public string ConsultaEjercido(string cfdipagoref)
        {
            List<string> TramitePago = new List<string>();
            string cve_cat_ori = string.Empty;
            string folioref = string.Empty;
            string foliotram = string.Empty;

            try
            {
                string nombreBD = "WFTRAMITES";
                string conexionGDBTL = System.Configuration.ConfigurationManager.ConnectionStrings[nombreBD + "ConnectionString"].ConnectionString;
                SqlConnection cnnconsulta;
                cnnconsulta = new SqlConnection(conexionGDBTL);
                cnnconsulta.Open();
                String sqlBuscarPago = @"SELECT CVE_CAT_ORI,REFERENCIAFOLIO FROM [" + nombreBD + "].[dbo].[FOLIOPAGOAVALUO] WHERE REFERENCIAFOLIO='" + cfdipagoref + "' AND ESTADO='FOLIO UTILIZADO'";
                SqlCommand cmdTramites = new SqlCommand(sqlBuscarPago, cnnconsulta);
                SqlDataReader readerTramites = cmdTramites.ExecuteReader();

                while (readerTramites.Read())
                {
                    cve_cat_ori = readerTramites.GetString(0);
                    folioref = readerTramites.GetString(1);
                    foliotram = readerTramites.GetString(2);

                    TramitePago.Add(cve_cat_ori + ", " + folioref + ", " + foliotram);


                }
                cnnconsulta.Close();

                if (TramitePago.Count > 0)
                {
                    return "bloquear";
                }
                else
                {
                    return "ok";
                }
            }
            catch
            {
                return null;
            }
        }

    }
}