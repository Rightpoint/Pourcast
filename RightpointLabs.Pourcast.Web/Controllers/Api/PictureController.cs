using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using Glimpse.Core.ClientScript;
using RightpointLabs.Pourcast.Domain.Events;
using RightpointLabs.Pourcast.Domain.Services;
using RightpointLabs.Pourcast.Infrastructure.Services;

namespace RightpointLabs.Pourcast.Web.Controllers.Api
{
    [System.Web.Http.RoutePrefix("api/picture")]
    public class PictureController : ApiController
    {
        private readonly IImageCleanupService _imageCleanupService;

        public PictureController(IImageCleanupService imageCleanupService)
        {
            _imageCleanupService = imageCleanupService;
        }

        public void Taken(string tapId, [FromBody] string dataUrl)
        {
            string intermediateUrl;
            var newDataUrl = _imageCleanupService.CleanUpImage(dataUrl, out intermediateUrl);
            DomainEvents.Raise(new PictureTaken() { TapId = tapId, DataUrl = newDataUrl, IntermediateDataUrl = intermediateUrl, OriginalDataUrl = dataUrl });
        }

    }
}