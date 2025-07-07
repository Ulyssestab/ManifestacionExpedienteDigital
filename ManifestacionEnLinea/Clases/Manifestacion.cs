using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ManifestacionEnLinea.Clases
{
    public class Manifestacion
    {

        public void ActualizarSIS_MC(int ControlFolio, string FOLIOSIC)
        {
            try
            {
                string conexion = System.Configuration.ConfigurationManager.ConnectionStrings["WFTRAMITESConnectionString"].ConnectionString;
                SqlConnection sqlConnection1 = new SqlConnection(conexion);
                string UpdateQuery = "UPDATE WFTRAMITES.dbo.SIS_TRACAT_MC SET CONTROLFOLIO =" + ControlFolio + " WHERE FOLIO_TRAMITE = '" + FOLIOSIC + "' ";

                SqlCommand command = new SqlCommand(UpdateQuery, sqlConnection1);

                sqlConnection1.Open();
                command.ExecuteNonQuery();

                sqlConnection1.Close();

            }
            catch (Exception Ex)
            {

            }
        }

        public bool InsertarSolicitudMCAV(SolicitudMCAV solicitud)
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["WFTRAMITESConnectionString"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                    INSERT INTO SIS_TRACAT_SOLICITUD_MC_AV (
                        STATUSREGISTROTABLA,
                        ALTAREGISTROTABLA,
                        USUARIOALTA,
                        FOLIO_TRAMITE,
                        TIPO_TRAMITE,
                        CVE_CAT_EST,
                        CVE_CAT_ORI,
                        CVE_PREDIAL,
                        OBSERVACIONES,
                        TRAMITADOR,
                        SOLICITANTE,
                        PROPIETARIO,
                        UBICACION,
                        NOTIFICACION,
                        NOTIFICACION_RECHAZO,
                        AVALUO,
                        FOLIO_PAGO_AVALUO,
                        IGUAL_CARTOGRAFIA,
                        CORREOELECTRONICO
                        -- Agrega más campos aquí si es necesario
                    )
                    VALUES (
                        @STATUSREGISTROTABLA,
                        @ALTAREGISTROTABLA,
                        @USUARIOALTA,
                        @FOLIO_TRAMITE,
                        @TIPO_TRAMITE,
                        @CVE_CAT_EST,
                        @CVE_CAT_ORI,
                        @CVE_PREDIAL,
                        @OBSERVACIONES,
                        @TRAMITADOR,
                        @SOLICITANTE,
                        @PROPIETARIO,
                        @UBICACION,
                        @NOTIFICACION,
                        @NOTIFICACION_RECHAZO,
                        @AVALUO,
                        @FOLIO_PAGO_AVALUO,
                        @IGUAL_CARTOGRAFIA,
                        @CORREOELECTRONICO
                    )";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    // Agregar parámetros
                    cmd.Parameters.AddWithValue("@STATUSREGISTROTABLA", (object)solicitud.STATUSREGISTROTABLA ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ALTAREGISTROTABLA", (object)solicitud.ALTAREGISTROTABLA ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@USUARIOALTA", (object)solicitud.USUARIOALTA ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@FOLIO_TRAMITE", (object)solicitud.FOLIO_TRAMITE ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@TIPO_TRAMITE", (object)solicitud.TIPO_TRAMITE ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CVE_CAT_EST", (object)solicitud.CVE_CAT_EST ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CVE_CAT_ORI", (object)solicitud.CVE_CAT_ORI ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CVE_PREDIAL", (object)solicitud.CVE_PREDIAL ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@OBSERVACIONES", (object)solicitud.OBSERVACIONES ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@TRAMITADOR", (object)solicitud.TRAMITADOR ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@SOLICITANTE", (object)solicitud.SOLICITANTE ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@PROPIETARIO", (object)solicitud.PROPIETARIO ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@UBICACION", (object)solicitud.UBICACION ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@NOTIFICACION", (object)solicitud.NOTIFICACION ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@NOTIFICACION_RECHAZO", solicitud.NOTIFICACION_RECHAZO);
                    cmd.Parameters.AddWithValue("@AVALUO", (object)solicitud.AVALUO ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@FOLIO_PAGO_AVALUO", (object)solicitud.FOLIO_PAGO_AVALUO ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IGUAL_CARTOGRAFIA", (object)solicitud.IGUAL_CARTOGRAFIA ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CORREOELECTRONICO", (object)solicitud.CORREOELECTRONICO ?? DBNull.Value);
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                Console.WriteLine("Error al insertar solicitud: " + ex.Message);
                return false;
            }
        }

    }
}