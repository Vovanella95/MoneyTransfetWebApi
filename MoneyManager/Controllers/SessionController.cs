using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using MoneyManager.Models;
using System.IO;

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
				PhoneNumber bigint,
				CreditCardNumber bigint,
				Ballance decimal,
				Token varchar(255),
				ImageUrl varchar(255),
				Friends varchar(2048));",
				myConnection);

			createUsersCommand.Connection = myConnection;
			await createUsersCommand.ExecuteNonQueryAsync();
			myConnection.Close();
		}

		private async Task SaveImagesAsync(RegisterUserModel userModel)
		{
			var host = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();

			var imageBytes = string.IsNullOrEmpty(userModel.ImageBase64String) ? null : Convert.FromBase64String(userModel.ImageBase64String);
			var backgroundBytes = string.IsNullOrEmpty(userModel.BackgroundImageBase64String) ? null : Convert.FromBase64String(userModel.BackgroundImageBase64String);

			if(!Directory.Exists($"{host}\\Images"))
			{
				Directory.CreateDirectory($"{host}\\Images");
			}

			if (imageBytes != null)
			{
				await SaveImageAsync($"{host}\\Images\\{userModel.Id}-avatar.jpg", imageBytes);
			}

			if (backgroundBytes != null)
			{
				await SaveImageAsync($"{host}\\Images\\{userModel.Id}-background.jpg", backgroundBytes);
			}
		}

		private async Task SaveImageAsync(string fileName, byte[] imageBytes)
		{
			if(System.IO.File.Exists(fileName))
			{
				System.IO.File.Delete(fileName);
			}

			using (var bw = new FileStream(fileName, FileMode.CreateNew))
			{
				await bw.WriteAsync(imageBytes, 0, imageBytes.Length);
			}
		}

		[HttpPost]
		public async Task<ActionResult> Register(RegisterUserModel userModel)
		{
			var allUsers = (await GetAllUsers()).ToList();

			if (allUsers.Any(w => w.Email == userModel.Email))
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var maxId = allUsers.Any() ? allUsers.Max(w => w.Id) : 0;
			userModel.Id = maxId + 1;

			await myConnection.OpenAsync();
			SqlCommand myCommand = new SqlCommand("INSERT INTO Users (Id, UserName, Surname, Email, Password, PhoneNumber, CreditCardNumber, Ballance, ImageUrl) " +
												  $"Values ({userModel.Id},'{userModel.UserName}', '{userModel.Surname}', '{userModel.Email}', '{userModel.Password}', '{userModel.PhoneNumber}', '{userModel.CreditCardNumber}', '{100.4}', '{userModel.ImageUrl}')", myConnection);

			await myCommand.ExecuteNonQueryAsync();

			await SaveImagesAsync(userModel);

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
				PhoneNumber = Int64.Parse(myReader["PhoneNumber"].ToString()),
				CreditCardNumber = Int64.Parse(myReader["CreditCardNumber"].ToString()),
				Token = myReader["Token"].ToString(),
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
					Token = token
				}
			};
		}

		private async Task<IEnumerable<UserModel>> GetAllUsers()
		{
			await myConnection.OpenAsync();

			SqlCommand selectCommand = new SqlCommand($"SELECT * FROM Users", myConnection);

			var myReader = await selectCommand.ExecuteReaderAsync();

			var users = new List<UserModel>();

			while (await myReader.ReadAsync())
			{
				var userModel = new UserModel
				{
					Id = Int32.Parse(myReader["Id"].ToString()),
					UserName = myReader["UserName"].ToString(),
					Surname = myReader["Surname"].ToString(),
					Email = myReader["Email"].ToString(),
					PhoneNumber = Int64.Parse(myReader["PhoneNumber"].ToString()),
					CreditCardNumber = Int64.Parse(myReader["CreditCardNumber"].ToString()),
					Token = myReader["Token"].ToString(),
					Ballance = Double.Parse(myReader["Ballance"].ToString())
				};

				users.Add(userModel);
			}

			myConnection.Close();
			return users;
		}
	}
}