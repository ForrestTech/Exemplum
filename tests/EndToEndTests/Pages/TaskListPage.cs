namespace Exemplum.EndToEndTests.Pages
{
    public class TaskListPage : BasePage
    {
        public TaskListPage(IPage page) : base(page)
        {
        }

        protected override string PagePath => "todolists";

        public async Task ValidateCanAccessPage()
        {
            var todoListTitle = await Page.WaitForSelectorAsync("#todo-list-title");
            todoListTitle.Should().NotBeNull();
        }
    }
}