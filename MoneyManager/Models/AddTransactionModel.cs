using System.Collections.Generic;

namespace MoneyManager.Models
{
	public class AddTransactionModel : TransactionModel
	{
		public IEnumerable<int> CollaboratorsIds { get; set; }
		public IEnumerable<int> FinishedIds { get; set; }
		public IEnumerable<int> InProgressIds { get; set; }
		public string ImageBase64String { get; set; }

		public string Email { get; set; }
		public string Token { get; set; }
	}
}