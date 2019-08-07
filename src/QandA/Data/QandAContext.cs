using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QandA.Data.Configuration;
using QandA.Features.Questions;
using QandA.Features.Users;

namespace QandA.Data
{
	public class QandAContext : DbContext
	{
		public QandAContext(DbContextOptions options)
			: base(options)
		{
		}

		public DbSet<Question> Questions { get; set; }
		public DbSet<User> User { get; set; }
		public DbSet<Answer> Answer { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new QuestionConfiguration());
			modelBuilder.ApplyConfiguration(new AnswerConfiguration());
			modelBuilder.ApplyConfiguration(new UserConfiguration());
		}

		public override int SaveChanges(bool acceptAllChangesOnSuccess)
		{
			OnBeforeSaving();
			return base.SaveChanges(acceptAllChangesOnSuccess);
		}

		public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
		{
			OnBeforeSaving();
			return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
		}

		private void OnBeforeSaving()
		{
			var entries = ChangeTracker.Entries();
			foreach (var entry in entries)
			{
				if (entry.Entity is ITrackable trackable)
				{
					var now = DateTime.UtcNow;
					switch (entry.State)
					{
						case EntityState.Modified:
							trackable.LastUpdated = now;
							
							break;

						case EntityState.Added:
							trackable.Created = now;
							trackable.LastUpdated = now;
							break;
					}
				}
			}
		}
	}
}