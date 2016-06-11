using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc.Html;
using Oracle.DataAccess.Client;
using SE2_IMDB.Models.Entity;

namespace SE2_IMDB.Models.Repositories
{
    public class FilmRepo
    {
        public static List<Film> GetFilms()
        {
            List<Film> films = new List<Film>();
            string sql = "SELECT * FROM FILM ORDER BY POPULARITY DESC";
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
                                films.Add(new Film(r["FILMID"], r["TITLE"], r["DESCRIPTION"],
                                    r["RELEASEYEAR"], r["POPULARITY"], r["STORYLINE"], r["PICTURE"]));
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

        public static Film GetFilm(int id)
        {
            Film film = new Film() { ID = id * -1 };
            string sql = "SELECT * FROM FILM WHERE FILMID = :ID";
            if (id == -1337) sql = "SELECT * FROM FILM WHERE FILMID = (SELECT MAX(FILMID) FROM FILM)";
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

                            if (r.Read())
                            {
                                film = new Film(r["FILMID"], r["TITLE"], r["DESCRIPTION"],
                                    r["RELEASEYEAR"], r["POPULARITY"], r["STORYLINE"], r["PICTURE"]);
                            }
                        }
                    }
                    catch (OracleException)
                    {
                        return film;
                    }
                }
            }
            return film;
        }

        public static bool CreateFilm(Film film)
        {
            string sql = "INSERT INTO FILM (TITLE,DESCRIPTION,RELEASEYEAR,STORYLINE) VALUES (:title, :description, :releaseyear, :storyline)";
            using (OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleConnection"].ToString()))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    try
                    {
                        cmd.Parameters.Add(new OracleParameter("title", film.Title));
                        cmd.Parameters.Add(new OracleParameter("description", film.Description));
                        cmd.Parameters.Add(new OracleParameter("releaseyear", film.ReleaseYear));
                        cmd.Parameters.Add(new OracleParameter("storyline", film.StoryLine));
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

        public static bool UpdateFilm(Film film)
        {
            string sql =
                "UPDATE FILM SET TITLE=:title, DESCRIPTION=:description, RELEASEYEAR=:releaseyear, STORYLINE=:storyline, PICTURE=:picture WHERE FILMID=:filmID";
            using (OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleConnection"].ToString()))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    try
                    {
                        cmd.Parameters.Add(new OracleParameter("title", film.Title));
                        cmd.Parameters.Add(new OracleParameter("description", film.Description));
                        cmd.Parameters.Add(new OracleParameter("releaseyear", film.ReleaseYear.ToString()));
                        cmd.Parameters.Add(new OracleParameter("storyline", film.StoryLine));
                        cmd.Parameters.Add(new OracleParameter("picture", film.ImagePath));

                        cmd.Parameters.Add(new OracleParameter("filmID", film.ID));
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

        public static List<Person> GetPersonsFromFilm(int id)
        {
            List<Person> persons = new List<Person>();
            string sql = "SELECT P.PERSONID, P.NAME, P.RATING, R.ROLE FROM PERSON P, ROLE R WHERE P.PERSONID = R.PERSONID AND R.FILMID = :filmid";
            using (OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleConnection"].ToString()))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    try
                    {
                        cmd.Parameters.Add(new OracleParameter("filmid", id));
                        cmd.Prepare();
                        using (OracleDataReader r = cmd.ExecuteReader())
                        {

                            while (r.Read())
                            {
                                persons.Add(new Person(r["PERSONID"], r["RATING"], r["NAME"], "", "") { Role = r["ROLE"].ToString(), inFilm = true});
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

        public static List<Person> GetPersonsNotFromFilm(int id, List<Person> inFilm )
        {
            List<Person> persons = new List<Person>();
            string sql = "SELECT P.PERSONID, P.NAME, P.RATING FROM PERSON P";
            using (OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleConnection"].ToString()))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    try
                    {
                        cmd.Parameters.Add(new OracleParameter("filmid", id));
                        cmd.Prepare();
                        using (OracleDataReader r = cmd.ExecuteReader())
                        {

                            while (r.Read())
                            {
                                persons.Add(new Person(r["PERSONID"], r["RATING"], r["NAME"], "", "") { inFilm = false } ) ;
                            }
                        }
                    }
                    catch (OracleException)
                    {
                        return persons;
                    }
                }
            }

            foreach (var p in persons)
            {
                if (inFilm.Contains(inFilm.Find(x => x.ID == p.ID)))
                {
                    p.inFilm = true;
                    p.Selected = true;
                    p.Role = inFilm.Find(x => x.ID == p.ID).Role;
                }
            }
            return persons;
        }

        public static bool DeleteRoles(int id)
        {
            string sql = "DELETE FROM ROLE WHERE FILMID = :filmID";
            using (OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleConnection"].ToString()))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    try
                    {
                        cmd.Parameters.Add(new OracleParameter("fimlID", id));
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

        public static bool InsertRole(int id, Person person)
        {
            if (person.Role == null) person.Role = "Actor";
            if (person.Role == "" || person.Role == "-") person.Role = "Actor";

            string sql = "INSERT INTO ROLE (FILMID, PERSONID, ROLE) VALUES (:filmID, :personID, :role)";
            using (OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleConnection"].ToString()))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    try
                    {
                        cmd.Parameters.Add(new OracleParameter("filmID", id));
                        cmd.Parameters.Add(new OracleParameter("personID", person.ID));
                        cmd.Parameters.Add(new OracleParameter("role", person.Role));
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

    }
}