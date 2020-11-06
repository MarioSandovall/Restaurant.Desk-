using Model.Models;

namespace Service.Interfaces
{
    public interface IConfigService
    {
        void LoadTheme();
        ConfigApp GetConfiguration();
        void UpdateProperty(string propertyName, object value);
    }
}
