using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oracle.DataAccess.Client;
using SE2_IMDB.Models.Entity;
using System.Configuration;

namespace SE2_IMDB.Models.Repositories
{
    public class CommentRepo
    {
        public static Comment GetComment(int id)
        {
            string sql = "SELECT * FROM THREADCOMMENT WHERE COMMENTID = :ID";
            using (OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleConnection"].ToString()))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("ID", id));
                    cmd.Prepare();
                    try
                    {
                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                                return new Comment(id, reader["COMMENTTEXT"].ToString(), reader["COMMENTER"].ToString());
                            else return new Comment(-1, "COMMENT NOT FOUND", "NULL");
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