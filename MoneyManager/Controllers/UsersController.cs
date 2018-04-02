using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MoneyManager.Models;

namespace MoneyManager.Controllers
{
    public class UsersController : Controller
    {
	    SqlConnection myConnection = new SqlConnection("Server=tcp:circleserver.database.windows.net,1433;Initial Catalog=mediastoredb;Persist Security Info=False;User ID=uladzimir_paliukhovich;Password=Remember_me95;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

		public ActionResult Index()
        {
            return View();
        }

	    private async Task<IEnumerable<UserModel>> GetAllUsers()
	    {
		    await myConnection.OpenAsync();

		    SqlCommand selectCommand = new SqlCommand($"SELECT * FROM Users", myConnection);

		    var myReader = await selectCommand.ExecuteReaderAsync();

		    var users = new List<UserModel>();

			while(await myReader.ReadAsync())
		    {
			    var userModel = new UserModel
			    {
				    Id = Int32.Parse(myReader["Id"].ToString()),
				    UserName = myReader["UserName"].ToString(),
				    Surname = myReader["Surname"].ToString(),
				    Email = myReader["Email"].ToString(),
				    PhoneNumber = Int32.Parse(myReader["PhoneNumber"].ToString()),
				    CreditCardNumber = Int32.Parse(myReader["CreditCardNumber"].ToString()),
				    Token = myReader["Token"].ToString(),
					ImageUrl = myReader["ImageUrl"].ToString(),
				    Ballance = Double.Parse(myReader["Ballance"].ToString())
			    };

				users.Add(userModel);
			}

		    return users;
	    }

	    public async Task<ActionResult> All()
	    {
		    return new JsonResult
		    {
				Data = await GetAllUsers(),
				JsonRequestBehavior = JsonRequestBehavior.AllowGet
			};
	    }

	    public async Task<ActionResult> Search(string query)
	    {
		    var allUsers = await GetAllUsers();

		    var neededUsers = allUsers.Where(w => IsUserMatch(w, query));

		    return new JsonResult
		    {
				Data = neededUsers,
				JsonRequestBehavior = JsonRequestBehavior.AllowGet
		    };
	    }

	    private bool IsUserMatch(UserModel user, string query)
	    {
		    var queryWords = query.Split(' ');

		    return queryWords.Any(w =>
			    user.UserName.Contains(w) || user.Surname.Contains(w) || user.Email.Contains(w) ||
			    user.PhoneNumber.ToString().Contains(w));
	    }
    }
}