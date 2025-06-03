// Create a client to localhost on "default" namespace

using BrewUp.Mediator.WorkflowActivities;
using BrewUp.Mediator.Workflows;
using Temporalio.Client;
using Temporalio.Worker;

TemporalClient temporalClient = await TemporalClient.
    ConnectAsync(new TemporalClientConnectOptions("localhost:7233"));

// Cancellation token to shutdown Worker on ctrl+c
using var tokenSource = new CancellationTokenSource();
Console.CancelKeyPress += (_, eventArgs) =>
{
    tokenSource.Cancel();
    eventArgs.Cancel = true;
};

var activities = new SalesOrderActivities();

WorkflowOptions workflowOptions = new()
{
    TaskQueue = "brewup-tasks",
    RetryPolicy = new Temporalio.Common.RetryPolicy
    {
        MaximumAttempts = 3,
        InitialInterval = TimeSpan.FromSeconds(1),
        MaximumInterval = TimeSpan.FromSeconds(10),
        BackoffCoefficient = 2,
    },
};

// Create Worker
using var worker = new TemporalWorker(
    temporalClient,
    new TemporalWorkerOptions("brewup-tasks").
        AddAllActivities(activities).
        AddWorkflow<SalesOrderWorkflow>());

// Run Worker until cancelled
Console.WriteLine("Running worker");
try
{
    await worker.ExecuteAsync(tokenSource.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Worker cancelled");
}