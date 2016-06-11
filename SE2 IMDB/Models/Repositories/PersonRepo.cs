using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Oracle.DataAccess.Client;
using SE2_IMDB.Models.Entity;

namespace SE2_IMDB.Models.Repositories
{
    public class PersonRepo
    {
        public static List<Person> GetPersons()
        {
            List<Person> persons = new List<Person>();
            string sql = "SELECT * FROM PERSON ORDER BY RATING DESC";
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
                                persons.Add(new Person(r["PERSONID"], r["RATING"], r["NAME"], r["DESCRIPTION"], r["PICTURE"]));
                            }
                        }
                    }
                    catch (OracleException)
                    {
                        return persons;
                    }
                }
            }
            return persons;
        }

        public static Person GetPerson(int id)
        {
            Person person = new Person() { ID = id * -1 };
            string sql = "SELECT * FROM PERSON WHERE PERSONID = :ID";
            if (id == -1337) sql = "SELECT * FROM PERSON WHERE PERSONID = (SELECT MAX(PERSONID) FROM PERSON";
            using (OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleConnection"].ToString()))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    try
                    {
                        cmd.Parameters.Add(new OracleParameter("ID", id));
                        using (OracleDataReader r = cmd.ExecuteReader())
                        {

                            while (r.Read())
                            {
                                person = new Person(r["PERSONID"], r["RATING"], r["NAME"], r["DESCRIPTION"], r["PICTURE"]);
                            }
                        }
                    }
                    catch (OracleException)
                    {
                        return person;
                    }
                }
            }
            return person;
        }

        public static bool CreatePerson(Person person)
        {
            string sql = "INSERT INTO PERSON (NAME, DESCRIPTION, PICTURE) VALUES (:name, :description, :picture)";
            using (OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleConnection"].ToString()))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    try
                    {
                        cmd.Parameters.Add(new OracleParameter("name", person.Name));
                        cmd.Parameters.Add(new OracleParameter("description", person.Description));
                        cmd.Parameters.Add(new OracleParameter("picture", person.ImagePath));
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (OracleException)
                    {
                        return false;
                    }
                }
            }
        }

        public static bool UpdatePerson(Person person)
        {
            string sql =
                "UPDATE PERSON SET NAME=:name, DESCRIPTION=:description, PICTURE=:picture WHERE PERSONID=:personID";
            using (OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleConnection"].ToString()))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    try
                    {
                        cmd.Parameters.Add(new OracleParameter("name", person.Name));
                        cmd.Parameters.Add(new OracleParameter("description", person.Description));
                        cmd.Parameters.Add(new OracleParameter("picture", person.ImagePath));

                        cmd.Parameters.Add(new OracleParameter("personID", person.ID));
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (OracleException)
                    {
                        return false;
                    }
                }
            }
        }

        public static List<Film> GetPlayingFilms(int personID)
        {
            List<Film> films = new List<Film>();
            string sql = "SELECT * FROM FILM F, ROLE R WHERE F.FILMID = R.FILMID AND R.PERSONID = :personID ORDER BY POPULARITY DESC";
            using (OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleConnection"].ToString()))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    try
                    {
                        cmd.Parameters.Add(new OracleParameter("personID", personID));

                        cmd.Prepare();
                        using (OracleDataReader r = cmd.ExecuteReader())
                        {

                            while (r.Read())
                            {
                                films.Add(new Film(r["FILMID"], r["TITLE"], r["DESCRIPTION"], 
                                    r["RELEASEYEAR"], r["POPULARITY"], "", "") {Role = r["ROLE"].ToString()});
                            }
                        }
                    }
                    catch (OracleException)
                    {
                        return films;
                    }
                }
            }
            return films;
        }
    }
}