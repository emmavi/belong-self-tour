namespace Belong.SelfTours.Infra.Proxies.Interfaces
{
    public interface IHomeProxy
    {
        Task<bool?> IsSelfServiceAllowedAsync(string externalHomeId);
    }
}