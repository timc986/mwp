namespace mwp.WebApi.Helper
{
    public interface IJsonWebTokenGenerator
    {
        string GenerateToken(string userName);
    }
}