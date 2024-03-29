namespace Exemplum.EndToEndTests.Pages;

public abstract class BasePage
{
    protected BasePage(IPage page)
    {
        Page = page;
    }

    protected abstract string PagePath { get; }

    protected IPage Page { get; }

    public async Task Navigate()
    {
        await Page.GotoAsync(Path(PagePath));
    }

    protected static string Path(string relativePath)
    {
        return $"{Url.Root}/{relativePath}";
    }
}