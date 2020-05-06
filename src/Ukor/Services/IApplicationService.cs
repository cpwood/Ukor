using System.Threading.Tasks;
using Ukor.Configuration;

namespace Ukor.Services
{
    public interface IApplicationService
    {
        Task DoActionAsync(Application application);
    }
}