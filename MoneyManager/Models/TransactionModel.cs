using System;
using System.Collections.Generic;
using System.Linq;

namespace MoneyManager.Models
{
	public class TransactionModel
	{
		public int Id { get; set; }
		public DateTime DeadlineDate { get; set; }
		public bool IsClosed => Collaborators != null && FinishedIds != null && Collaborators.Count() == FinishedIds.Count();
		public double SingleCost => Collaborators != null ? Coast / Math.Max(Collaborators.Count(), 1) : 0;
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime CreationDate { get; set; }
		public UserModel Owner { get; set; }
		public IEnumerable<UserModel> Collaborators { get; set; }
		public IEnumerable<int> FinishedIds { get; set; }
		public IEnumerable<int> InProgressIds { get; set; }
		public double Coast { get; set; }
		public DateTime? OngoingDate { get; set; }
		public string ImageUrl => "http://" + System.Web.HttpContext.Current.Request.Url.Host + $"/Images?path={Id}-transaction.jpg";
	}
}