using System.Threading.Tasks;
using Ukor.Configuration;

namespace Ukor.Services
{
    public interface IKeyPressService
    {
        Task HandleKeyPress(Application.KeyPress keyPress);
    }
}