namespace BrewUp.Mediator.WorkflowModels;

public record OrderConfirmation(
    string OrderId,
    string OrderNumber,
    string Status,
    long BillingTimestamp,
    decimal Amount);