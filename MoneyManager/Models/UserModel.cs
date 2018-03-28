namespace MoneyManager.Models
{
	public class UserModel
	{
		public int Id { get; set; }
		public string Email { get; set; }
		public string UserName { get; set; }
		public string Surname { get; set; }
		public int PhoneNumber { get; set; }
		public int CreditCardNumber { get; set; }
		public double Ballance { get; set; }
		public string Token { get; set; }
		public string ImageUrl { get; set; }
	}
}