using System;
using System.Data.SqlClient;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using MoneyManager.Models;
using Newtonsoft.Json;

namespace MoneyManager.Controllers
{
	public class SessionController : Controller
	{
		SqlConnection myConnection = new SqlConnection("Server=tcp:circleserver.database.windows.net,1433;Initial Catalog=mediastoredb;Persist Security Info=False;User ID=uladzimir_paliukhovich;Password=Remember_me95;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

		public ActionResult Index()
		{
			return View();
		}

		public async Task<ActionResult> Initialize()
		{
			await CreateUsersTable();
			return null;
		}

		private async Task CreateUsersTable()
		{
			await myConnection.OpenAsync();

			SqlCommand deleteUsersCommand = new SqlCommand("DROP TABLE Users;", myConnection);
			deleteUsersCommand.Connection = myConnection;

			try
			{
				await deleteUsersCommand.ExecuteNonQueryAsync();
			}
			catch (Exception e)
			{

			}

			SqlCommand createUsersCommand = new SqlCommand(@"
				CREATE TABLE Users (
				Id int,
				UserName varchar(255),
				Surname varchar(255),
				Email varchar(255),
				Password varchar(255),
				PhoneNumber int,
				CreditCardNumber int,
				Ballance decimal,
				Token varchar(255),
				ImageUrl varchar(255));",
				myConnection);

			createUsersCommand.Connection = myConnection;
			await createUsersCommand.ExecuteNonQueryAsync();
			myConnection.Close();
		}

		[HttpPost]
		public async Task<ActionResult> Register(RegisterUserModel userModel)
		{
			await myConnection.OpenAsync();
			SqlCommand myCommand = new SqlCommand("INSERT INTO Users (Id, UserName, Surname, Email, Password, PhoneNumber, CreditCardNumber, Ballance, ImageUrl) " +
												  $"Values ({0},'{userModel.UserName}', '{userModel.Surname}', '{userModel.Email}', '{userModel.Password}', '{userModel.PhoneNumber}', '{userModel.CreditCardNumber}', '{100.4}', '{userModel.ImageUrl}')", myConnection);

			await myCommand.ExecuteNonQueryAsync();

			myConnection.Close();

			return await SignIn(new LoginUserModel
			{
				Email = userModel.Email,
				Password = userModel.Password
			});
		}

		[HttpPost]
		public async Task<ActionResult> SignIn(LoginUserModel user)
		{
			await myConnection.OpenAsync();

			SqlCommand selectCommand = new SqlCommand($"SELECT * FROM Users WHERE Email = '{user.Email}'", myConnection);

			var myReader = await selectCommand.ExecuteReaderAsync();

			await myReader.ReadAsync();

			var userModel = new RegisterUserModel
			{
				Id = Int32.Parse(myReader["Id"].ToString()),
				UserName = myReader["UserName"].ToString(),
				Password = myReader["Password"].ToString(),
				Surname = myReader["Surname"].ToString(),
				Email = myReader["Email"].ToString(),
				PhoneNumber = Int32.Parse(myReader["PhoneNumber"].ToString()),
				CreditCardNumber = Int32.Parse(myReader["CreditCardNumber"].ToString()),
				Token = myReader["Token"].ToString(),
				ImageUrl = myReader["ImageUrl"].ToString(),
				Ballance = Double.Parse(myReader["Ballance"].ToString())
			};

			myReader.Close();

			if (userModel.Password != user.Password)
			{
				return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
			}

			var token = Guid.NewGuid().ToString();

			SqlCommand updateTokenCommand = new SqlCommand($"UPDATE Users SET Token = '{token}' WHERE Email = '{user.Email}'", myConnection);
			await updateTokenCommand.ExecuteNonQueryAsync();

			myConnection.Close();
			return new JsonResult
			{
				Data = new UserModel
				{
					Email = userModel.Email,
					Id = userModel.Id,
					CreditCardNumber = userModel.CreditCardNumber,
					Surname = userModel.Surname,
					PhoneNumber = userModel.PhoneNumber,
					Ballance = userModel.Ballance,
					UserName = userModel.UserName,
					Token = userModel.Token,
					ImageUrl = userModel.ImageUrl
				}
			};
		}

	}
}