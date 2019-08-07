using System;
using System.Collections.Generic;
using QandA.Data;
using QandA.Features.Users;

namespace QandA.Features.Questions
{
	public class Question : ITrackable
	{
		public int Id { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public User Questioner { get; set; }

		public DateTime Created { get; set; }

		public DateTime LastUpdated { get; set; }

		public List<Answer> Answers { get; set; }
	}
}