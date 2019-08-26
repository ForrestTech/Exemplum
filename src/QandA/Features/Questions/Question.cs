using System;
using System.Collections.Generic;
using QandA.Data;
using QandA.Features.Users;

namespace QandA.Features.Questions
{
	public class Question : ITrackable
	{
		private Question()
		{}

		public Question(string title, string questionContent, User questioner)
		{
			if (string.IsNullOrWhiteSpace(title))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(title));
			if (string.IsNullOrWhiteSpace(questionContent))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(questionContent));

			Title = title;
			QuestionContent = questionContent;
			Questioner = questioner ?? throw new ArgumentNullException(nameof(questioner));
		}

		public Question(string title, string questionContent, int questionerId)
		{
			if (string.IsNullOrWhiteSpace(title))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(title));
			if (string.IsNullOrWhiteSpace(questionContent))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(questionContent));
			if (questionerId <= 0) throw new ArgumentOutOfRangeException(nameof(questionerId));

			Title = title;
			QuestionContent = questionContent;
			QuestionerId = questionerId;
		}

		public int Id { get; private set; }

		public string Title { get; private set; }

		public string QuestionContent { get; private set; }

		public int QuestionerId { get; set; }

		public User Questioner { get; private set; }

		public ICollection<Answer> Answers { get; set; } = new List<Answer>();

		public DateTime Created { get; set; }

		public DateTime LastUpdated { get; set; }
	}
}