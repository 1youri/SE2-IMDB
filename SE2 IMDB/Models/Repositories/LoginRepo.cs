using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Oracle.DataAccess.Client;
using SE2_IMDB.Models.Entity;

namespace SE2_IMDB.Models.Repositories
{
    public class LoginRepo
    {
        public static Account ValidateCredentials(string username, string password)
        {
            List<Film> films = new List<Film>();
            string sql = "SELECT * FROM ACCOUNT WHERE USERNAME = :username AND PASSWORD = :password";
            using (OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleConnection"].ToString()))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("username", username));
                    cmd.Parameters.Add(new OracleParameter("password", password));
                    try
                    {
                        using (OracleDataReader r = cmd.ExecuteReader())
                        {

                            if (r.Read())
                            {
                                return new Account() { ID = r["ACCOUNTID"].ToInt(), Role = r["ACCOUNTTYPE"].ToString() };
                            }
                            return new Account() { ID = 0, Role = "" };
                        }
                    }
                    catch (OracleException)
                    {
                        return new Account() { ID = 0, Role = "" };
                    }
                }
            }
        }

        public static Account GetAccount(int accountID)
        {
            List<Film> films = new List<Film>();
            string sql = "SELECT * FROM ACCOUNT WHERE ACCOUNTID = :accountID";
            using (OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleConnection"].ToString()))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("accountID", accountID));
                    try
                    {
                        using (OracleDataReader r = cmd.ExecuteReader())
                        {

                            if (r.Read())
                            {
                                return new Account() { ID = r["ACCOUNTID"].ToInt(), Role = r["ACCOUNTTYPE"].ToString() };
                            }
                            return new Account() { ID = 0, Role = "" };
                        }
                    }
                    catch (OracleException)
                    {
                        return new Account() { ID = 0, Role = "" };
                    }
                }
            }
        }

        public static string GetSalt(string username)
        {
            List<Film> films = new List<Film>();
            string sql = "SELECT SALT FROM ACCOUNT WHERE USERNAME = :username";
            using (OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleConnection"].ToString()))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("username", username));
                    try
                    {
                        using (OracleDataReader r = cmd.ExecuteReader())
                        {

                            if (r.Read())
                            {
                                return r["SALT"].ToString();
                            }
                            return "";
                        }
                    }
                    catch (OracleException)
                    {
                        return "";
                    }
                }
            }
        }
    }
}