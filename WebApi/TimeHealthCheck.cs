using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WebApi;

public class TimeHealthCheck : IHealthCheck
{
    private readonly TimeSpan threshold = TimeSpan.FromSeconds(30);

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new())
    {
        var currentTime = DateTime.Now;
        var data = new Dictionary<string, object>
        {
            {"CurrentTime", currentTime},
            {"Threshold", this.threshold}
        };

        return DateTime.Now.Second > this.threshold.Seconds
            ? HealthCheckResult.Degraded("I cannot operate in the second half of the minute", data: data)
            : HealthCheckResult.Healthy(data: data);
    }
}