using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using COVID19Tracker.ServerData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace COVID19TrackerWebService.Controllers
{
    [ApiController]
    public class LocationRecieverController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("api/loglocation")]
        public ActionResult<bool> LogLocation([FromBody]ServerLocationPacket locationData)
        {
            // TODO: Add to the segmented database
            return true;
        }

    }
}