using System.Collections.Generic;

namespace MoneyManager.Models
{
	public class AddTransactionModel : TransactionModel
	{
		public IEnumerable<int> CollaboratorsIds { get; set; }
		public string ImageBase64String { get; set; }
	}
}