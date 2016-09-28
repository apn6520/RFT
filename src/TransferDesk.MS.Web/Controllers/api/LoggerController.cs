using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TransferDesk.Contracts.Logging;

namespace TransferDesk.MS.Web.Controllers.api
{
    public class LoggerController : ApiController
    {
        private readonly ILogger _logger;

        public LoggerController(ILogger logger)
        {
           _logger = logger;
        }
        [HttpPost]
        public IHttpActionResult Log(string logMessage)
        {
            _logger.Log(logMessage,"");
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
