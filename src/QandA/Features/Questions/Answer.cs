using System;
using QandA.Data;
using QandA.Features.Users;

namespace QandA.Features.Questions
{
	public class Answer : ITrackable
	{
		public int Id { get; set; }

		public string Content { get; set; }

		public User Answerer { get; set; }

		public DateTime Created { get; set; }

		public DateTime LastUpdated { get; set; }
	}
}