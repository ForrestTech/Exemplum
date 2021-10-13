namespace Exemplum.EndToEndTests
{
    public static class Selector
    {
        public static string Input(string inputName) => $"input[name='{inputName}']";
        public static string Button(string buttonText) => $"//button[text()='{buttonText}']";
    }
}