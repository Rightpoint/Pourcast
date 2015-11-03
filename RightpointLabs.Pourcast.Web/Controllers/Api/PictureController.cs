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
        private readonly IFaceRecognitionService _faceRecognitionService;

        public PictureController(IFaceRecognitionService faceRecognitionService)
        {
            _faceRecognitionService = faceRecognitionService;
        }

        public void Taken(string tapId, [FromBody] string dataUrl)
        {
            string intermediateUrl, newDataUrl;
            bool addedOverlay;
            var faces = _faceRecognitionService.ProcessImage(dataUrl, out intermediateUrl, out newDataUrl, out addedOverlay);
            DomainEvents.Raise(new PictureTaken() { TapId = tapId, DataUrl = newDataUrl, IntermediateDataUrl = intermediateUrl, OriginalDataUrl = dataUrl, Faces = faces, AddedOverlay = addedOverlay });
        }

    }
}