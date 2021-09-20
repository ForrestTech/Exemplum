namespace Exemplum.WebApp.Extensions
{
    using Microsoft.JSInterop;
    using System.Threading.Tasks;

    public static class JsRuntimeExtensions
    {
        /// <summary>
        /// A very naive retry forever for JS functions that return a success status.
        /// </summary>
        public static async Task RetryInvokeAsync(this IJSRuntime runtime, 
            string identifier, object argument, int retryFor = 50)
        {
            var attempts = 1;
            var result = false;
            while (result == false && attempts <= retryFor)
            {
                result = await runtime.InvokeAsync<bool>(identifier, argument);

                attempts++;
            }
        }
    }
}