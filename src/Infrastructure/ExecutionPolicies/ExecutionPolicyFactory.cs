namespace Exemplum.Infrastructure.ExecutionPolicies
{
    using Application.Common.Policies;
    using Polly;
    using Polly.Contrib.WaitAndRetry;
    using Polly.Extensions.Http;
    using Polly.Registry;
    using Polly.Retry;
    using System;
    using System.Net.Http;

    public static class ExecutionPolicyFactory
    {
        public static void RegisterRetryPolicy(PolicyRegistry registry, IServiceProvider serviceProvider)
        {
            var retry = CreateRetryPolicy();

            registry.Add(ExecutionPolicy.RetryPolicy, retry);
        }

        private static AsyncRetryPolicy<HttpResponseMessage> CreateRetryPolicy()
        {
            //just static config here but this could easily be injected from app config
            var delay = Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromMilliseconds(200),
                retryCount: 5);
            var retry = HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(delay);
            return retry;
        }
    }
}