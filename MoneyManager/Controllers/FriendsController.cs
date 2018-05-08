using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using MoneyManager.Models;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace MoneyManager.Controllers
{
    public class FriendsController : Controller
    {
	    SqlConnection myConnection = new SqlConnection("Server=tcp:circleserver.database.windows.net,1433;Initial Catalog=mediastoredb;Persist Security Info=False;User ID=uladzimir_paliukhovich;Password=Remember_me95;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

		public ActionResult Index()
        {
            return View();
        }

		[HttpPost]
	    public async Task<ActionResult> Add(int friendId)
	    {
			var email = Request.Headers["X-USERNAME"];
			var token = Request.Headers["X-TOKEN"];

			var user = (await new UsersController().GetAllUsers()).FirstOrDefault(w => w.Email == email);

			if (user == null || user.Token != token)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

		    var userFriends = string.IsNullOrEmpty(user.Friends)
			    ? new List<int>()
			    : JsonConvert.DeserializeObject<List<int>>(user.Friends);

		    if (userFriends.Any(w => w == friendId))
		    {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			userFriends.Add(friendId);

		    await myConnection.OpenAsync();
			var serializedValue = JsonConvert.SerializeObject(userFriends);

			SqlCommand updateTokenCommand = new SqlCommand($"UPDATE Users SET Friends = '{serializedValue}' WHERE Email = '{user.Email}'", myConnection);
			await updateTokenCommand.ExecuteNonQueryAsync();

			myConnection.Close();

			return new HttpStatusCodeResult(HttpStatusCode.OK);
		}

		[HttpPost]
		public async Task<ActionResult> Remove(int friendId)
		{
			var email = Request.Headers["X-USERNAME"];
			var token = Request.Headers["X-TOKEN"];

			var user = (await new UsersController().GetAllUsers()).FirstOrDefault(w => w.Email == email);

			if (user == null || user.Token != token)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var userFriends = string.IsNullOrEmpty(user.Friends)
				? new List<int>()
				: JsonConvert.DeserializeObject<List<int>>(user.Friends);

			if (userFriends.All(w => w == friendId))
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			userFriends.Remove(friendId);

			await myConnection.OpenAsync();

			SqlCommand updateTokenCommand = new SqlCommand($"UPDATE Users SET Friends = '{JsonConvert.SerializeObject(userFriends)}' WHERE Email = '{user.Email}'", myConnection);
			await updateTokenCommand.ExecuteNonQueryAsync();

			myConnection.Close();

			return new HttpStatusCodeResult(HttpStatusCode.OK);
		}

	    public async Task<ActionResult> All()
	    {
			var email = Request.Headers["X-USERNAME"];
			var token = Request.Headers["X-TOKEN"];

			var user = (await new UsersController().GetAllUsers()).FirstOrDefault(w => w.Email == email);

			if (user == null || user.Token != token)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var userFriends = string.IsNullOrEmpty(user.Friends)
			    ? new List<int>()
			    : JsonConvert.DeserializeObject<List<int>>(user.Friends);

		    var friends = (await GetAllUsers()).Where(w => userFriends.Contains(w.Id));

		    return new JsonResult
		    {
				Data = friends,
				JsonRequestBehavior = JsonRequestBehavior.AllowGet
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
				    Friends = myReader["Friends"].ToString(),
					Ballance = Double.Parse(myReader["Ballance"].ToString())
			    };

			    users.Add(userModel);
		    }
			myReader.Close();
		    myConnection.Close();
		    return users;
	    }
	}
}