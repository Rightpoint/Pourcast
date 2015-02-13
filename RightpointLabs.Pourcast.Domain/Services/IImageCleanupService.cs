using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RightpointLabs.Pourcast.Domain.Services
{
    public interface IImageCleanupService
    {
        string CleanUpImage(string rawDataUrl);
    }
}
