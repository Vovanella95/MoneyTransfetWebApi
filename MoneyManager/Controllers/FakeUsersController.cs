using MoneyManager.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MoneyManager.Controllers
{
	public class FakeUsersController : Controller
	{
		public IEnumerable<Tuple<RegisterUserModel, string, string>> Users = new[]
		{
			new Tuple<RegisterUserModel, string, string>(
				new RegisterUserModel
			{
				UserName = "Владислав",
				Ballance = 18.4,
				CreditCardNumber = 1267120816081929,
				Email = "vlad_nagibator12@mail.ru",
				Password = "1234abcd",
				PhoneNumber = 375292466780,
				Surname = "Русский"
			},
				"https://pp.userapi.com/c834200/v834200714/109b45/D7DmwZ1lvaA.jpg",
				"https://image.freepik.com/free-vector/nice-background-with-polygonal-shapes_1159-452.jpg"),
			new Tuple<RegisterUserModel, string, string>(
				new RegisterUserModel
			{
				UserName = "Ekaterina",
				Ballance = 22.89,
				CreditCardNumber = 66103332303024343,
				Email = "ekaterina_kuznetsova@mail.ru",
				Password = "1234abcd",
				PhoneNumber = 375291239634,
				Surname = "Kuzniatsova"
			},
				"https://pp.userapi.com/c840237/v840237138/81c57/is1zywWZrto.jpg",
				"https://png.pngtree.com/thumb_back/fw800/back_pic/00/12/29/04563b77c1ddb3e.jpg"),
			new Tuple<RegisterUserModel, string, string>(
				new RegisterUserModel
			{
				UserName = "Ivan",
				Ballance = 0.02,
				CreditCardNumber = 1145434533024354,
				Email = "ivan_17@epam.com",
				Password = "1234abcd",
				PhoneNumber = 375291518035,
				Surname = "Vabishchevich"
			},
				"https://pp.userapi.com/c636522/v636522469/71123/Ao7LC6pp7w0.jpg",
				"https://i.pinimg.com/originals/b8/bf/ed/b8bfed43813adf07f33eaf85d260a2bd.jpg"),
			new Tuple<RegisterUserModel, string, string>(
				new RegisterUserModel
			{
				UserName = "Rodion",
				Ballance = 30.02,
				CreditCardNumber = 11435843938284432,
				Email = "rodion_geforevich@yandex.ru",
				Password = "1234abcd",
				PhoneNumber = 375291518035,
				Surname = "Geforsevich"
			},
				"https://pp.userapi.com/c840120/v840120247/84a39/Hz68o9keFpw.jpg",
				"http://picsoverflow.com/wp-content/uploads/2016/05/Corn-Fields.jpg"),
			new Tuple<RegisterUserModel, string, string>(
				new RegisterUserModel
			{
				UserName = "Aliona",
				Ballance = 30.02,
				CreditCardNumber = 4916453610102931,
				Email = "alena1616@yandex.ru",
				Password = "1234abcd",
				PhoneNumber = 375291518035,
				Surname = "Supurunchik"
			},
				"https://pp.userapi.com/c830608/v830608476/914aa/kml7FXXZIo8.jpg",
				"https://photosharingsites.files.wordpress.com/2014/11/autumn-leaf-orange-yellow-red-nice-backgrounds-free-wallpapers-pictures-images-photos.jpg?w=474&h=316"),
			new Tuple<RegisterUserModel, string, string>(
				new RegisterUserModel
			{
				UserName = "Georgie",
				Ballance = 120.98,
				CreditCardNumber = 1515676789892020,
				Email = "george_superkot@yandex.ru",
				Password = "1234abcd",
				PhoneNumber = 375291518035,
				Surname = "Paliukhovich"
			},
				"https://pp.userapi.com/c403517/v403517557/8c76/AA1Q07ItNYA.jpg",
				"https://st2.depositphotos.com/1017908/11135/i/950/depositphotos_111357656-stock-photo-cat-laying-on-the-road.jpg"),
			new Tuple<RegisterUserModel, string, string>(
				new RegisterUserModel
			{
				UserName = "Mikhail",
				Ballance = 12.001,
				CreditCardNumber = 1515676789892020,
				Email = "mikhail@yandex.ru",
				Password = "1234abcd",
				PhoneNumber = 375291518035,
				Surname = "Pakaluik"
			},
				"https://pp.userapi.com/c403517/v403517557/8c76/AA1Q07ItNYA.jpg",
				"http://media.moddb.com/cache/images/downloads/1/82/81610/thumb_620x2000/Wallpaper_-_37.jpg")
		};

		public async Task<ActionResult> Index()
		{
			await new SessionController().Initialize();

			var host = AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "\\Images";

			if (Directory.Exists(host))
			{
				Directory.Delete(host, true);
			}

			foreach (var item in Users)
			{
				await RegisterUserAsync(item);
			}

			return await new UsersController().All();
		}

		private async Task RegisterUserAsync(Tuple<RegisterUserModel, string, string> userModel)
		{
			var logoImageBytes = new WebClient().DownloadData(new Uri(userModel.Item2));
			userModel.Item1.ImageBase64String = Convert.ToBase64String(logoImageBytes);

			var backgroundImageBytes = new WebClient().DownloadData(new Uri(userModel.Item3));
			userModel.Item1.BackgroundImageBase64String = Convert.ToBase64String(backgroundImageBytes);

			await new SessionController().Register(userModel.Item1);
		}
	}
}