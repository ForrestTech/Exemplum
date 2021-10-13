namespace Exemplum.EndToEndTests.Steps
{
    using Pages;
    using System.Threading.Tasks;
    using TechTalk.SpecFlow;

    [Binding]
    public class TaskListSteps
    {
        public TaskListPage TaskListPage { get; }

        public TaskListSteps(TaskListPage taskListPage)
        {
            TaskListPage = taskListPage;
        }

        [When(@"the user navigates to the task lists")]
        public async Task WhenTheUserNavigatesToTheTaskLists()
        {
            await TaskListPage.Navigate();
        }

        [Then(@"the user can access the task lists page")]
        public async Task ThenTheUserCanAccessTheTaskListsPage()
        {
            await TaskListPage.ValidateCanAccessPage();
        }
    }
}