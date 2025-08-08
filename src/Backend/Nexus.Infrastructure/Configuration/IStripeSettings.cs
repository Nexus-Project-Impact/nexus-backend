using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Infrastructure.Configuration
{
    public interface IStripeSettings
    {
        string? SecretKey { get; set; }
        string? PublishableKey { get; set; }
        string? WebhookSecret { get; set; }
    }
}
