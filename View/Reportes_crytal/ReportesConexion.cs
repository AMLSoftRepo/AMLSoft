using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace View.Reportes_crytal
{
    public class ReportesConexion
    {
        public static CrystalDecisions.Shared.ConnectionInfo getConexion()
        {
            /*var strcon = new System.Data.SqlClient.SqlConnectionStringBuilder(
                System.Configuration.ConfigurationManager.
                ConnectionStrings["SQLBDEntities"].ConnectionString);*/

            CrystalDecisions.Shared.ConnectionInfo infocon = new CrystalDecisions.Shared.ConnectionInfo();
            infocon.ServerName = @"DESKTOP-6TMAP1V\SQLSERVER";
            infocon.DatabaseName = "ANTILAVADO";
            infocon.UserID = "lavado";
            infocon.Password = "l@@vad0$2017";

            return infocon;
        }

    }
}