//   Sri Lanka Institute of Information Technology
//   Year  :  4th Year 2nd Semester
//   Module Code  :  SE4040
//   Module  :  Enterprise Application Development
//   Student Id Number  :  IT20032838
//   Name  :  Devsrini Savidya A.S.

using EAD_Web_Services.Models.RequestAgentModel;

namespace EAD_Web_Services.Services.RequestAgentService
{
    /// <summary>
    /// Interface for the RequestAgent Service
    /// </summary>
    public interface IRequestAgentService
    {
        List<RequestAgent> Get();
        RequestAgent Get(string id);
        RequestAgent Create(RequestAgent requestAgent);
        string Remove(string id);
    }
}
