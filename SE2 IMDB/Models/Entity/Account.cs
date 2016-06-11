using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using SE2_IMDB.Models.Repositories;

namespace SE2_IMDB.Models.Entity
{
	public class Account
	{
		public Account()
		{
			
		}

		[Required]
		public string Username { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		public string Role { get; set; }
		public int ID { get; set; }


		public static Account GetID(HttpContext context)
		{
			if (context.Request.Cookies["SessionData"] != null)
			{
				string ID = context.Request.Cookies["SessionData"]["AccountID"];

				context.Response.Cookies["SessionData"]["AccountID"] = ID;
				context.Response.Cookies["SessionData"].Expires = DateTime.Now.AddHours(3);
				return LoginRepo.GetAccount(ID.ToInt());
			}
			return new Account() {Role = "", ID = 0};
		}

		public static bool Authenticate(string username, string password, HttpContext context)
		{
			string salt = LoginRepo.GetSalt(username);
			string encPassword = Encrypt(password, salt);
			Account account = LoginRepo.ValidateCredentials(username, encPassword);
			if (account.Role != "" && account.Role != "BANNED" && account.ID != 0)
			{
				context.Response.Cookies["SessionData"]["AccountID"] = account.ID.ToString();
				context.Response.Cookies["SessionData"].Expires = DateTime.Now.AddHours(3);
				return true;
			}
			else return false;
		}

		private static string Encrypt(string Password, string salt = "FIDBoM")
		{
			byte[] MessageBytes = Encoding.UTF8.GetBytes(string.Concat(Password, salt));
			SHA512Managed SHhash = new SHA512Managed();
			byte[] HashValue = SHhash.ComputeHash(MessageBytes);
			string strHex = "";
			foreach (byte b in HashValue) { strHex += String.Format("{0:x2}", b); }
			return strHex;
		}

	}
}