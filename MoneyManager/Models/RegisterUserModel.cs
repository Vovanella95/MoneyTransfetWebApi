namespace MoneyManager.Models
{
	public class RegisterUserModel : UserModel
	{
		public string Password { get; set; }
		public string ImageBase64String { get; set; }
		public string BackgroundImageBase64String { get; set; }
	}
}