﻿using System;
using System.Collections.Generic;

namespace MoneyManager.Models
{
	public class TransactionModel
	{
		public int Id { get; set; }
		public DateTime DeadlineDate { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime CreationDate { get; set; }
		public UserModel Owner { get; set; }
		public IEnumerable<UserModel> Collaborators { get; set; }
		public IEnumerable<UserModel> Finished { get; set; }
		public IEnumerable<UserModel> InProgress { get; set; }
		public double Coast { get; set; }
		public string ImageUrl => "http://" + System.Web.HttpContext.Current.Request.Url.Host + $"/Images?path={Id}-transaction.jpg";
	}
}