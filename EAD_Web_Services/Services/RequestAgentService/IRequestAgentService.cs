using EAD_Web_Services.Models.RequestAgentModel;

namespace EAD_Web_Services.Services.RequestAgentService
{
    public interface IRequestAgentService
    {
        List<RequestAgent> Get();
        RequestAgent Get(string id);
        RequestAgent Create(RequestAgent requestAgent);
        string Remove(string id);
    }
}
