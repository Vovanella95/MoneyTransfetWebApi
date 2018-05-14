using MoneyManager.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;

namespace MoneyManager.Controllers
{
	public class TransactionsController : Controller
	{
		SqlConnection myConnection = new SqlConnection("Server=tcp:circleserver.database.windows.net,1433;Initial Catalog=mediastoredb;Persist Security Info=False;User ID=uladzimir_paliukhovich;Password=Remember_me95;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

		public ActionResult Index()
		{
			return View();
		}

		public async Task<ActionResult> All()
		{
			var transactions = (await GetAllTransactions());
			var maped = await Map(transactions);

			return new JsonResult
			{
				Data = maped,
				JsonRequestBehavior = JsonRequestBehavior.AllowGet
			};
		}

		public async Task<ActionResult> Add(AddTransactionModel transaction)
		{
			var allTransactions = await GetAllTransactions();

			var email = Request.Headers["X-USERNAME"];
			var token = Request.Headers["X-TOKEN"];

			var user = (await new UsersController().GetAllUsers()).FirstOrDefault(w => w.Email == email);

			if (user == null || user.Token != token)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var id = allTransactions.Any() ? allTransactions.Max(w => w.Id) + 1 : 0;

			var data = Map(transaction);
			data.Id = id;
			data.OwnerId = user.Id;
			data.CreationDate = DateTime.UtcNow.ToString();

			await myConnection.OpenAsync();
			SqlCommand myCommand = new SqlCommand("INSERT INTO Transactions (Id, Coast, CollaboratorsIds, CreationDate, DeadlineDate, Description, FinishedIds, InProgressIds, OwnerId, Title) " +
												  $"Values ({data.Id},'{data.Coast}', '{data.CollaboratorsIds}', '{data.CreationDate}', '{data.DeadlineDate}', N'{data.Description}', '{data.FinishedIds}', '{data.InProgressIds}', '{data.OwnerId}', N'{data.Title}')", myConnection);

			await myCommand.ExecuteNonQueryAsync();

			await SaveImagesAsync(transaction);

			return new HttpStatusCodeResult(HttpStatusCode.OK);
		}

		private async Task SaveImagesAsync(AddTransactionModel transactionModel)
		{
			var host = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();

			var imageBytes = string.IsNullOrEmpty(transactionModel.ImageBase64String) ? null : Convert.FromBase64String(transactionModel.ImageBase64String);

			if (!Directory.Exists($"{host}\\Images"))
			{
				Directory.CreateDirectory($"{host}\\Images");
			}

			if (imageBytes != null)
			{
				await SaveImageAsync($"{host}\\Images\\{transactionModel.Id}-transaction.jpg", imageBytes);
			}
		}

		private async Task SaveImageAsync(string fileName, byte[] imageBytes)
		{
			if (System.IO.File.Exists(fileName))
			{
				System.IO.File.Delete(fileName);
			}

			using (var bw = new FileStream(fileName, FileMode.Create))
			{
				await bw.WriteAsync(imageBytes, 0, imageBytes.Length);
			}
		}

		private async Task<IEnumerable<TransactionDataModel>> GetAllTransactions()
		{
			await myConnection.OpenAsync();

			SqlCommand selectCommand = new SqlCommand($"SELECT * FROM Transactions", myConnection);

			var myReader = await selectCommand.ExecuteReaderAsync();

			var transactions = new List<TransactionDataModel>();

			while (await myReader.ReadAsync())
			{
				transactions.Add(ReadTransaction(myReader));
			}

			myReader.Close();
			myConnection.Close();

			return transactions;
		}

		private TransactionDataModel ReadTransaction(SqlDataReader reader)
		{
			var title = reader["Title"];
			byte[] utf8Bytes = Encoding.UTF8.GetBytes(title.ToString().ToCharArray());

			String str2 = Encoding.UTF8.GetString(utf8Bytes);


			return new TransactionDataModel
			{
				Id = Int32.Parse(reader["Id"].ToString()),
				Coast = Double.Parse(reader["Coast"].ToString()),
				CollaboratorsIds = reader["CollaboratorsIds"].ToString(),
				FinishedIds = reader["FinishedIds"].ToString(),
				InProgressIds = reader["FinishedIds"].ToString(),
				CreationDate = reader["CreationDate"].ToString(),
				DeadlineDate = reader["DeadlineDate"].ToString(),
				Description = reader["Description"].ToString(),
				OwnerId = Int32.Parse(reader["OwnerId"].ToString()),
				Title = str2
			};
		}

		public async Task<IEnumerable<TransactionModel>> Map(IEnumerable<TransactionDataModel> collection)
		{
			var allUsers = await (new UsersController()).GetAllUsers();
			var resultCollection = new List<TransactionModel>();

			foreach (var data in collection)
			{
				var collaboratorsIds = JsonConvert.DeserializeObject<int[]>(data.CollaboratorsIds);
				var finishedIds = JsonConvert.DeserializeObject<int[]>(data.FinishedIds);
				var inProgressIds = JsonConvert.DeserializeObject<int[]>(data.InProgressIds);

				var owner = allUsers.First(w => w.Id == data.OwnerId);
				owner.Friends = null;
				resultCollection.Add(new TransactionModel
				{
					Coast = data.Coast,
					Title = data.Title,
					Description = data.Description,
					Collaborators = allUsers.Where(w => collaboratorsIds != null && collaboratorsIds.Contains(w.Id)),
					Id = data.Id,
					CreationDate = DateTime.Parse(data.CreationDate),
					DeadlineDate = DateTime.Parse(data.DeadlineDate),
					InProgress = allUsers.Where(w => inProgressIds != null && inProgressIds.Contains(w.Id)),
					Owner = owner,
					Finished = allUsers.Where(w => finishedIds != null && finishedIds.Contains(w.Id))
				});
			}
			return resultCollection;
		}

		public TransactionDataModel Map(AddTransactionModel model)
		{
			return new TransactionDataModel
			{
				Coast = model.Coast,
				CollaboratorsIds = JsonConvert.SerializeObject(model.CollaboratorsIds),
				CreationDate = model.CreationDate.ToString(),
				DeadlineDate = model.DeadlineDate.ToString(),
				Description = model.Description,
				FinishedIds = JsonConvert.SerializeObject(model.FinishedIds),
				InProgressIds = JsonConvert.SerializeObject(model.InProgressIds),
				Title = model.Title
			};
		}

		[HttpGet]
		public async Task<ActionResult> My(bool isClosedOnly = false, bool isOpenedOnly = false, int? byUserId = null)
		{
			var email = Request.Headers["X-USERNAME"];
			var token = Request.Headers["X-TOKEN"];

			var user = (await new UsersController().GetAllUsers()).FirstOrDefault(w => w.Email == email);

			if (user == null || user.Token != token)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var transactions = (await Map(await GetAllTransactions())).Where(w => w.Owner.Id == user.Id || w.Collaborators.Any(ww => ww.Id == user.Id));

			if (isClosedOnly)
			{
				transactions = transactions.Where(w => w.IsClosed);
			}
			else if (isOpenedOnly)
			{
				transactions = transactions.Where(w => !w.IsClosed);
			}

			if (byUserId != null)
			{
				transactions = transactions.Where(w => w.Owner.Id == byUserId || w.Collaborators.Any(ww => ww.Id == byUserId));
			}

			return new JsonResult
			{
				JsonRequestBehavior = JsonRequestBehavior.AllowGet,
				Data = transactions
			};
		}

		[HttpPost]
		public async Task<ActionResult> Collaborate(int id)
		{
			var email = Request.Headers["X-USERNAME"];
			var token = Request.Headers["X-TOKEN"];

			var user = (await new UsersController().GetAllUsers()).FirstOrDefault(w => w.Email == email);
			var transaction = (await GetAllTransactions()).FirstOrDefault(w => w.Id == id);
			if (user == null || user.Token != token || transaction == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var collaboratorsIds = JsonConvert.DeserializeObject<int[]>(transaction.CollaboratorsIds);
			var inProgressIds = JsonConvert.DeserializeObject<int[]>(transaction.InProgressIds).ToList();

			if (collaboratorsIds.All(w => w != user.Id))
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "you have no participation at this transaction");
			}

			if (!inProgressIds.Contains(user.Id))
			{
				inProgressIds.Add(user.Id);
			}

			await myConnection.OpenAsync();
			SqlCommand updateTokenCommand = new SqlCommand($"UPDATE Transactions SET CollaboratorsIds = '{JsonConvert.SerializeObject(inProgressIds)}' WHERE Id = '{transaction.Id}'", myConnection);
			await updateTokenCommand.ExecuteNonQueryAsync();

			myConnection.Close();

			return new HttpStatusCodeResult(200);
		}

		[HttpPost]
		public async Task<ActionResult> Approve(int transactionId, int userId)
		{
			var email = Request.Headers["X-USERNAME"];
			var token = Request.Headers["X-TOKEN"];

			var user = (await new UsersController().GetAllUsers()).FirstOrDefault(w => w.Email == email);
			var transaction = (await GetAllTransactions()).FirstOrDefault(w => w.Id == transactionId);

			if (user == null || user.Token != token || transaction == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var collaboratorsIds = JsonConvert.DeserializeObject<int[]>(transaction.CollaboratorsIds);
			var finishedIds = JsonConvert.DeserializeObject<int[]>(transaction.FinishedIds).ToList();

			if (transaction.OwnerId != user.Id || collaboratorsIds.All(w => w != userId))
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "you have no participation at this transaction");
			}

			if (!finishedIds.Contains(userId))
			{
				finishedIds.Add(userId);
			}

			await myConnection.OpenAsync();
			SqlCommand updateTokenCommand = new SqlCommand($"UPDATE Transactions SET FinishedIds = '{JsonConvert.SerializeObject(finishedIds)}' WHERE Id = '{transaction.Id}'", myConnection);
			await updateTokenCommand.ExecuteNonQueryAsync();

			myConnection.Close();

			return new HttpStatusCodeResult(200);
		}
	}
}