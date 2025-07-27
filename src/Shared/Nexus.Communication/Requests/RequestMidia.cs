using Microsoft.AspNetCore.Http;

namespace Nexus.Communication.Requests
{
    public  class RequestMidia
    {
        public string? Name { get; set; }

        public IFormFile? Image { get; set; }

    }
}
