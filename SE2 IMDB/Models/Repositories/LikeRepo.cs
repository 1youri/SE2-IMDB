using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Oracle.DataAccess.Client;
using SE2_IMDB.Models.Entity;

namespace SE2_IMDB.Models.Repositories
{
    public class LikeRepo
    {
        public static int checkLike(int accountid, int PersoonID = 0, int FilmID = 0)
        {
            if (PersoonID != 0) FilmID = 0;
            string sql;
            if (PersoonID > 0) sql = @"SELECT * FROM ""LIKE"" WHERE ACCOUNTID = :accountID AND PERSONID = :ID";
            else if (FilmID > 0) sql = @"SELECT * FROM ""LIKE"" WHERE ACCOUNTID = :accountID AND FILMID = :ID";
            else sql = @"SELECT * FROM ""LIKE"" WHERE 1=0";
            using (OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleConnection"].ToString()))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    try
                    {
                        cmd.Parameters.Add(new OracleParameter("accountID", accountid));
                        cmd.Parameters.Add(new OracleParameter("ID", PersoonID + FilmID));
                        using (OracleDataReader r = cmd.ExecuteReader())
                        {

                            if (r.Read())
                            {
                                return r["VALUE"].ToInt();
                            }
                        }
                    }
                    catch (OracleException)
                    {
                        return 0;
                    }
                }
            }
            return 0;
        }

        public static bool UpdateLike(int accountid, int value, int PersoonID = 0, int FilmID = 0)
        {

            if (PersoonID != 0) FilmID = 0;

            string sql1 = "";
            if (PersoonID > 0) sql1 = @"DELETE FROM ""LIKE"" WHERE ACCOUNTID = :accountID AND PERSONID = :ID";
            else if (FilmID > 0) sql1 = @"DELETE FROM ""LIKE"" WHERE ACCOUNTID = :accountID AND FILMID = :ID";
            string sql2 = "";
            if (PersoonID > 0) sql2 = @"INSERT INTO ""LIKE""(PERSONID, ACCOUNTID, VALUE) VALUES (:ID, :accountid, :value)";
            else if (FilmID > 0) sql2 = @"INSERT INTO ""LIKE""(FILMID, ACCOUNTID, VALUE) VALUES (:ID, :accountid, :value)";

            if (sql1 != "" && sql2 != "")
            {
                using (
                    OracleConnection conn =
                        new OracleConnection(ConfigurationManager.ConnectionStrings["OracleConnection"].ToString()))
                {
                    try
                    {
                        conn.Open();
                        using (OracleCommand cmd = new OracleCommand(sql1, conn))
                        {
                            cmd.Parameters.Add(new OracleParameter("accountID", accountid));
                            cmd.Parameters.Add(new OracleParameter("ID", PersoonID + FilmID));
                            cmd.ExecuteNonQuery();
                        }

                        using (OracleCommand cmd2 = new OracleCommand(sql2, conn))
                        {

                            cmd2.Parameters.Add(new OracleParameter("ID", PersoonID + FilmID));
                            cmd2.Parameters.Add(new OracleParameter("accountid", accountid));
                            cmd2.Parameters.Add(new OracleParameter("value", value));
                            cmd2.ExecuteNonQuery();
                        }
                        return true;
                    }
                    catch (OracleException)
                    {
                        return false;
                    }
                }
            }
            return false;
        }
    }
}