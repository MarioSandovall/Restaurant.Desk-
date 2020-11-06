using System.Threading.Tasks;

namespace Business.Interfaces.Login
{
    public interface ISoftwareUpdateViewModel
    {
        Task<bool> IsNewAppVersionAsync();

        void CheckForUpdates();
    }
}
