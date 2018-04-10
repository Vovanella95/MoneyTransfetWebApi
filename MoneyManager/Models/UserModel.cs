namespace MoneyManager.Models
{
	public class UserModel
	{
		public int Id { get; set; }
		public string Email { get; set; }
		public string UserName { get; set; }
		public string Surname { get; set; }
		public long PhoneNumber { get; set; }
		public long CreditCardNumber { get; set; }
		public double Ballance { get; set; }
		public string Token { get; set; }
		public string ImageUrl => "http://" + System.Web.HttpContext.Current.Request.Url.Host + $"/Images?path={Id}-avatar.jpg";
		public string BackgroundImageUrl => "http://" + System.Web.HttpContext.Current.Request.Url.Host + $"/Images?path={Id}-background.jpg";
		public string Friends { get; set; }
	}
}