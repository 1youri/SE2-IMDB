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
            List<Film> Films = new List<Film>();
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
                                Films.Add(new Film(r["FILMID"], r["TITLE"],r["DESCRIPTION"],
                                    r["RELEASEYEAR"], r["POPULARITY"], r["STORYLINE"],r["PICTURE"]));
                            }
                        }
                    }
                    catch (OracleException)
                    {
                        return Films;
                    }
                }
            }
            return Films;
        }

        public static Film GetFilm(int id)
        {
            Film film = new Film() {ID = id*-1};
            string sql = "SELECT * FROM FILM WHERE FILMID = :ID";
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
            //string sql =
            //    "UPDATE FILM SET TITLE=:title, DESCRIPTION=:description, RELEASEYEAR=:releaseyear, STORYLINE=:storyline WHERE FILMID=:ID";
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
                        cmd.Parameters.Add(new OracleParameter("releaseyear", 2017/*film.ReleaseYear.ToString()*/));
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
    }
}