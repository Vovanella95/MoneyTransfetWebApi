using System;

namespace MoneyManager.Models
{
	public class TransactionDataModel
	{
		public int Id { get; set; }
		public string DeadlineDate { get; set; }
		public string OngoingDate { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string CreationDate { get; set; }
		public int OwnerId { get; set; }
		public string CollaboratorsIds { get; set; }
		public string FinishedIds { get; set; }
		public string InProgressIds { get; set; }
		public double Coast { get; set; }
	}
}