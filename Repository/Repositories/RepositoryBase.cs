using Service.Interfaces;

namespace Repository.Repositories
{
    public class RepositoryBase
    {
        public string Controller { get; }
        public IWebService WebService { get; }

        public RepositoryBase(IWebService webService, string controller)
        {
            Controller = controller;
            WebService = webService;
        }
    }
}
