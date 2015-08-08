using System;

namespace MvcTemplate.Services
{
    public interface IService : IDisposable
    {
        String CurrentAccountId { get; set; }
    }
}