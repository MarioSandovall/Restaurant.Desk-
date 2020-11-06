using System;

namespace Service.Interfaces
{
    public interface ILogService
    {
        void Write(Exception ex);
    }
}
