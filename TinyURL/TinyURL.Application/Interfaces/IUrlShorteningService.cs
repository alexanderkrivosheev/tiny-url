namespace TinyURL.Core.Interfaces
{
    public interface IUrlShorteningService
    {
        string CreateShortUrl(string longUrl, string customAlias = null);
        bool DeleteShortUrl(string shortUrl);
        string GetLongUrl(string shortUrl);
        int GetClickCount(string shortUrl);
    }
}
