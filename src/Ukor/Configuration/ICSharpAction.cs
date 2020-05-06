using System.Threading.Tasks;

namespace Ukor.Configuration
{
    public interface ICSharpAction
    {
        Task DoActionAsync(Application application);
    }
}
