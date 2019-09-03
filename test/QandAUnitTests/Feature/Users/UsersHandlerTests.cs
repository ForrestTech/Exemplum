using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using QandA.Data;
using QandA.Features.Users;
using Xunit;

namespace QandAUnitTests.Feature.Users
{
    public class UsersHandlerTests
    {
	    [Fact]
	    public async Task ListUsers_Returns_Paged_Users()
	    {
			//This is an example test.  If test setup becomes more complex than this we should move to using autofixture to setup test SUTs.  We should also use it to generate some test data 
		 //   var options = new DbContextOptionsBuilder<DatabaseContext>()
			//    .UseInMemoryDatabase(databaseName: "Test1")
			//    .Options;

		 //   using (var context = new DatabaseContext(options))
		 //   {
			//    var user = new User { Email = "test@sample.com", Username = "test" };
			//    context.Users.Add(user);
			//    await context.SaveChangesAsync();
		 //   }

			//using (var context = new DatabaseContext(options))
		 //   {
			//    var sut = new List(context);

			//    var result = await sut.Handle(new ListUsersRequest(), CancellationToken.None);

			//    result.Items.Count.Should().Be(1);
		 //   }
	    }
	}
}
