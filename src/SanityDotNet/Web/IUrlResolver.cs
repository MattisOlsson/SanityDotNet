using System;
using System.Globalization;
using System.Threading.Tasks;
using SanityDotNet.Models;

namespace SanityDotNet.Web
{
    public interface IUrlResolver
    {
        Task<string> GetUrl(ISanityDocument document);
        Task<string> GetUrl(Guid documentId);
        Task<string> GetUrl(ISanityDocument document, CultureInfo language);
        Task<string> GetUrl(Guid documentId, CultureInfo language);
    }
}