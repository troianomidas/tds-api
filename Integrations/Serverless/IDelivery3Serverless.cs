namespace WebApi.Integrations.Serverless;

public interface IDelivery3Serverless
{
    Task RefreshWorkflowOrderAsync(int storeId);
}