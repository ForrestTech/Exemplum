using System;
using System.Collections.Generic;
using QandA.Data;
using QandA.Features.Questions;

namespace QandA.Features.Users
{
	public class User : ITrackable
	{
		public int Id { get; set; }

		public string Username { get; set; }

		public string Email { get; set; }

		public List<Question> Questions { get; set; }

		public List<Answer> Answers { get; set; }

		public DateTime Created { get; set; }

		public DateTime LastUpdated { get; set; }
	}
}