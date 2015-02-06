using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using RightpointLabs.Pourcast.Domain.Events;

namespace RightpointLabs.Pourcast.Web.Controllers.Api
{
    [System.Web.Http.RoutePrefix("api/picture")]
    public class PictureController : ApiController
    {
        public void Taken([FromBody] string dataUrl)
        {
            DomainEvents.Raise(new PictureTaken() { DataUrl = dataUrl });
        }
    }
}