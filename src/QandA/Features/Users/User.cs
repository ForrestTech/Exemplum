using System;
using QandA.Data;

namespace QandA.Features.Users
{
	public class User : ITrackable
	{
		public int Id { get; set; }

		public string Username { get; set; }

		public string Email { get; set; }

		public DateTime Created { get; set; }

		public DateTime LastUpdated { get; set; }
	}
}