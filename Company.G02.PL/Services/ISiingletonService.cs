namespace Company.G02.PL.Services
{
    public interface ISiingletonService
    {
        public Guid Guid { get; set; }
        string GetGuid();
    }
}
