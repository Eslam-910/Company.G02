using Microsoft.AspNetCore.SignalR;

namespace Company.G02.PL.Services
{
    public class SiingletonService:ISiingletonService
    {
        public Guid Guid { get; set; }

        public SiingletonService()
        {
            Guid = Guid.NewGuid();
        }
        public string GetGuid()
        {
            return Guid.ToString();
        }
    }
}
