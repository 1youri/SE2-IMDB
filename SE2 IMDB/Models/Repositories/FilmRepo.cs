using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Oracle.DataAccess.Client;
using SE2_IMDB.Models.Entity;

namespace SE2_IMDB.Models.Repositories
{
    public class FilmRepo
    {
        public static List<Film> GetFilms()
        {
            string sql = "SELECT * FROM FILM ORDER BY POPULARITY";
            using (OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleConnection"].ToString()))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    try
                    {
                        using (OracleDataReader r = cmd.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                yield new Film(r["FILMID"].ToInt(),r["RELEASEYEAR")
                            }
                            //    return new Comment(id, reader["COMMENTTEXT"].ToString(), reader["COMMENTER"].ToString());
                            //else return new Comment(-1, "COMMENT NOT FOUND", "NULL");
                        }
                    }
                    catch (OracleException)
                    {
                        return new Comment(-1, "COMMENT NOT FOUND", "NULL");
                    }
                }
            }

        }
    }
}