using MoneyManager.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
				Surname = "Мудрый"
			},
				"https://pp.userapi.com/c834200/v834200714/109b45/D7DmwZ1lvaA.jpg",
				"https://image.freepik.com/free-vector/nice-background-with-polygonal-shapes_1159-452.jpg"),
			new Tuple<RegisterUserModel, string, string>(
				new RegisterUserModel
			{
				UserName = "Екатерина",
				Ballance = 22.89,
				CreditCardNumber = 66103332303024343,
				Email = "ekaterina_kuznetsova@mail.ru",
				Password = "1234abcd",
				PhoneNumber = 375291239634,
				Surname = "Кузнецова"
			},
				"https://pp.userapi.com/c840237/v840237138/81c57/is1zywWZrto.jpg",
				"https://png.pngtree.com/thumb_back/fw800/back_pic/00/12/29/04563b77c1ddb3e.jpg"),
			new Tuple<RegisterUserModel, string, string>(
				new RegisterUserModel
			{
				UserName = "Иван",
				Ballance = 0.02,
				CreditCardNumber = 1145434533024354,
				Email = "ivan_17@epam.com",
				Password = "1234abcd",
				PhoneNumber = 375291518035,
				Surname = "Вабищевич"
			},
				"https://pp.userapi.com/c636522/v636522469/71123/Ao7LC6pp7w0.jpg",
				"https://i.pinimg.com/originals/b8/bf/ed/b8bfed43813adf07f33eaf85d260a2bd.jpg"),
			new Tuple<RegisterUserModel, string, string>(
				new RegisterUserModel
			{
				UserName = "Родион",
				Ballance = 30.02,
				CreditCardNumber = 11435843938284432,
				Email = "rodion_geforevich@yandex.ru",
				Password = "1234abcd",
				PhoneNumber = 375291518035,
				Surname = "Джифорсевич"
			},
				"https://pp.userapi.com/c840120/v840120247/84a39/Hz68o9keFpw.jpg",
				"http://picsoverflow.com/wp-content/uploads/2016/05/Corn-Fields.jpg"),
			new Tuple<RegisterUserModel, string, string>(
				new RegisterUserModel
			{
				UserName = "Алена",
				Ballance = 30.02,
				CreditCardNumber = 4916453610102931,
				Email = "alena1616@yandex.ru",
				Password = "1234abcd",
				PhoneNumber = 375291518035,
				Surname = "Супурунчик"
			},
				"https://pp.userapi.com/c830608/v830608476/914aa/kml7FXXZIo8.jpg",
				"https://photosharingsites.files.wordpress.com/2014/11/autumn-leaf-orange-yellow-red-nice-backgrounds-free-wallpapers-pictures-images-photos.jpg?w=474&h=316"),
			new Tuple<RegisterUserModel, string, string>(
				new RegisterUserModel
			{
				UserName = "Георгий",
				Ballance = 120.98,
				CreditCardNumber = 1515676789892020,
				Email = "george_superkot@yandex.ru",
				Password = "1234abcd",
				PhoneNumber = 375291518035,
				Surname = "Святокотович"
			},
				"https://pp.userapi.com/c403517/v403517557/8c76/AA1Q07ItNYA.jpg",
				"https://st2.depositphotos.com/1017908/11135/i/950/depositphotos_111357656-stock-photo-cat-laying-on-the-road.jpg"),
			new Tuple<RegisterUserModel, string, string>(
				new RegisterUserModel
			{
				UserName = "Михаил",
				Ballance = 12.001,
				CreditCardNumber = 1515676789892020,
				Email = "mikhail@yandex.ru",
				Password = "1234abcd",
				PhoneNumber = 375291518035,
				Surname = "Пакалюк"
			},
				"https://pp.userapi.com/c403517/v403517557/8c76/AA1Q07ItNYA.jpg",
				"http://media.moddb.com/cache/images/downloads/1/82/81610/thumb_620x2000/Wallpaper_-_37.jpg"),
			new Tuple<RegisterUserModel, string, string>(
				new RegisterUserModel
			{
				UserName = "Стас",
				CreditCardNumber = 1515676789894455,
				Email = "stas_love_you@yandex.ru",
				Password = "1234abcd",
				PhoneNumber = 375291518035,
				Surname = "Михайлов"
			},
				"https://viva.ua/storage/crop/stars/avatar_54_max.jpg",
				"https://gl.weburg.net/00/events/10/45565/original/7428719.jpg"),
			new Tuple<RegisterUserModel, string, string>(
				new RegisterUserModel
			{
				UserName = "Саске",
				CreditCardNumber = 2341676789892020,
				Email = "sasuke_kill_konoha@yandex.ru",
				Password = "1234abcd",
				PhoneNumber = 375291518435,
				Surname = "Учиха"
			},
				"https://pbs.twimg.com/profile_images/690572318955999232/oo51w9fA.jpg",
				"https://99px.ru/sstorage/53/2016/11/tmb_184514_6112.jpg")
		};

		public IEnumerable<Tuple<AddTransactionModel, string, int>> Transactions = new[]
		{
			new Tuple<AddTransactionModel, string, int>(
				new AddTransactionModel
				{
					CollaboratorsIds = new[]{9,1,2,3 },
					Coast = 124,
					DeadlineDate = DateTime.UtcNow + TimeSpan.FromHours(15),
					Title = "Бочка китайской капусты \"Чуан-хай\" для дяди Коли",
					Description = "Китайская капуста \"Чуан-хай\" состоит из 18 видов специй и капусты, дарит свежесть и ты чувствуешь себя человеком",
				}, "https://gotovim-doma.ru/images/recipe/7/c8/7c8e7e0d65a24741dfad4ae0a878b92c.jpg",
				9),
			new Tuple<AddTransactionModel, string, int>(
				new AddTransactionModel
				{
					CollaboratorsIds = new[]{1,2,4 },
					Coast = 15,
					OngoingDate = DateTime.UtcNow + TimeSpan.FromHours(16),
					DeadlineDate = DateTime.UtcNow + TimeSpan.FromHours(36.12),
					Title = "Билеты в кино на Дэдпул 2",
					Description = "Встречаемся возле кинотеатра на 1 этаже за минут 10 до начала",
				}, "https://ru.diez.md/wp-content/uploads/2018/01/Deadpool-2-moldova10.jpg",
				1),
			new Tuple<AddTransactionModel, string, int>(
				new AddTransactionModel
				{
					CollaboratorsIds = new[]{5},
					Coast = 22,
					DeadlineDate = DateTime.UtcNow + TimeSpan.FromHours(28.4),
					Title = "Деньги Васяну чтоб тот не сдох от голода",
				}, "http://berg.com.ua/wp-content/cashTEST1.jpg",
				2),
			new Tuple<AddTransactionModel, string, int>(
				new AddTransactionModel
				{
					CollaboratorsIds = new[]{9,4,5,6},
					Coast = 300,
					OngoingDate = DateTime.UtcNow + TimeSpan.FromHours(72),
					DeadlineDate = DateTime.UtcNow + TimeSpan.FromHours(20),
					Title = "Посещение аниме клуба в кругу друзей",
					Description = "Аниме клуб находится на улице \"Анимешная\" д.15 2 этаж, а там видно будет",
				}, "https://static.tgstat.ru/public/images/channels/_0/6c/6c03774b093e79107a3afbe73df8a4a3.jpg",
				4),
			new Tuple<AddTransactionModel, string, int>(
				new AddTransactionModel
				{
					CollaboratorsIds = new[]{2,6,1,4,3},
					Coast = 18,
					OngoingDate = DateTime.UtcNow + TimeSpan.FromHours(420),
					DeadlineDate = DateTime.UtcNow + TimeSpan.FromHours(398),
					Title = "Подарок Стасику на ДР",
					Description = "Короч было решено подарить Стасику новый крест на шею, а то старый совсем изновился",
				}, "https://avatars.mds.yandex.net/get-pdb/777813/28781493-de5b-458c-843f-09df9aee3f92/s800",
				4),
			new Tuple<AddTransactionModel, string, int>(
				new AddTransactionModel
				{
					CollaboratorsIds = new[]{2,6},
					Coast = 125,
					DeadlineDate = DateTime.UtcNow + TimeSpan.FromHours(136),
					Title = "Игра Shadow of the Tomb Raider PS4",
				}, "https://media.senscritique.com/media/000017705532/source_big/Shadow_of_the_Tomb_Raider.jpg",
				6),
			new Tuple<AddTransactionModel, string, int>(
				new AddTransactionModel
				{
					CollaboratorsIds = new[]{9,4,3},
					Coast = 1000000,
					OngoingDate = DateTime.UtcNow + TimeSpan.FromHours(115),
					DeadlineDate = DateTime.UtcNow + TimeSpan.FromHours(7 * 24),
					Title = "Штраф за ограбление магазина в пятницу ночью",
					Description = "Без комментариев, штраф делим на троих",
				}, "http://mvdlnr.ru/upload/editor/images/29fb78ac13dd81dd606dd3d237b33256%5B1%5D.jpg",
				9),
			new Tuple<AddTransactionModel, string, int>(
				new AddTransactionModel
				{
					CollaboratorsIds = new[]{9,1,2,3,4,5,6},
					Coast = 2.5,
					DeadlineDate = DateTime.UtcNow + TimeSpan.FromHours(14 * 24),
					Title = "Долг за 10 пачек ролтона",
					Description = "Вы испортили мой ролтон когда я вас оставил на 5 минут. Так что с вас куча денег",
				}, "http://otzyvy.pro/image.php?nocache=1&img=uploads/reviews/2017-07/46bcb7ed086f2d7757dc446b6521878e.2",
				5),
			new Tuple<AddTransactionModel, string, int>(
				new AddTransactionModel
				{
					CollaboratorsIds = new[]{9,1,2,3,4,5},
					Coast = 220.8,
					DeadlineDate = DateTime.UtcNow + TimeSpan.FromHours(312),
					Title = "Деньги за поездку в Ирак",
				}, "https://cdni.rt.com/russian/images/2017.03/article/58d1602dc36188ea398b46f8.jpg",
				4),
			new Tuple<AddTransactionModel, string, int>(
				new AddTransactionModel
				{
					CollaboratorsIds = new[]{9},
					Coast = 5,
					DeadlineDate = DateTime.UtcNow + TimeSpan.FromHours(102),
					Title = "Благодарность за хладнокровное убийство паука",
					Description = "Помнишь я убил паука в твоей комнате? Ты не заходил туда неделю, а я его грохнул. С тебя 5 баксов",
				}, "http://simple-fauna.ru/wp-content/uploads/2017/01/pauk-volk.jpg",
				1),
			new Tuple<AddTransactionModel, string, int>(
				new AddTransactionModel
				{
					CollaboratorsIds = new[]{4,5,6},
					Coast = 50,
					DeadlineDate = DateTime.UtcNow + TimeSpan.FromHours(102),
					Title = "На бензин",
					Description = "Окей, я завезу вас в лес, но с вас бензин 95-й для моей ласточки",
				}, "https://www.belrynok.by/wp-content/uploads/2017/09/7e.jpg",
				1),
			new Tuple<AddTransactionModel, string, int>(
				new AddTransactionModel
				{
					CollaboratorsIds = new[]{2,6,1},
					Coast = 18,
					DeadlineDate = DateTime.UtcNow + TimeSpan.FromHours(102),
					Title = "Распечатка фальшивых купюр",
					Description = "Деньги за распечатку фальшивых денег для вашего подозрительного сомнительного мероприятия",
				}, "https://i.ytimg.com/vi/R73luLB9RBs/hqdefault.jpg",
				6)
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

			var allUsers = await new UsersController().GetAllUsers();

			foreach (var item in Transactions)
			{
				await RegisterTransactionAsync(item.Item1, item.Item2, allUsers.First(w => w.Id == item.Item3));
			}

			return new HttpStatusCodeResult(HttpStatusCode.OK);
		}

		private async Task RegisterUserAsync(Tuple<RegisterUserModel, string, string> userModel)
		{
			var logoImageBytes = new WebClient().DownloadData(new Uri(userModel.Item2));
			userModel.Item1.ImageBase64String = Convert.ToBase64String(logoImageBytes);

			var backgroundImageBytes = new WebClient().DownloadData(new Uri(userModel.Item3));
			userModel.Item1.BackgroundImageBase64String = Convert.ToBase64String(backgroundImageBytes);

			await new SessionController().Register(userModel.Item1);
		}

		private async Task RegisterTransactionAsync(AddTransactionModel addTransactionModel, string imageUrl, UserModel owner)
		{
			try
			{
				var logoImageBytes = new WebClient().DownloadData(new Uri(imageUrl));
				addTransactionModel.ImageBase64String = Convert.ToBase64String(logoImageBytes);
				await new TransactionsController().AddUsingBot(addTransactionModel, owner);
			}
			catch (Exception)
			{
				//ignore
			}
		}
	}
}