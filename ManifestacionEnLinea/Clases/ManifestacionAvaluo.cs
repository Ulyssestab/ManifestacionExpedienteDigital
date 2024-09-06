using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ManifestacionEnLinea.Clases
{
    public class ManifestacionAvaluo
    {
        public void ActualizarSIS_MC_AV(int ControlFolio, string FOLIOSIC)
        {
            try
            {
                string conexion = System.Configuration.ConfigurationManager.ConnectionStrings["WFTRAMITESConnectionString"].ConnectionString;
                SqlConnection sqlConnection1 = new SqlConnection(conexion);
                string UpdateQuery = "UPDATE WFTRAMITES.dbo.SIS_TRACAT_AC SET CONTROLFOLIO =" + ControlFolio + " WHERE FOLIO_TRAMITE = '" + FOLIOSIC + "' ";

                SqlCommand command = new SqlCommand(UpdateQuery, sqlConnection1);

                sqlConnection1.Open();
                command.ExecuteNonQuery();

                sqlConnection1.Close();

            }
            catch (Exception Ex)
            {

            }
        }
    }
}