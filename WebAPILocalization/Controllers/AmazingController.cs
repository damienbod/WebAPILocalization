using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPILocalization.Models;
using ModelErrorCollection = System.Web.Http.ModelBinding.ModelErrorCollection;
using ModelState = System.Web.Http.ModelBinding.ModelState;

namespace WebAPILocalization.Controllers
{
    [RoutePrefix("api/amazing")]
    public class AmazingController : ApiController
    {
        [HttpGet]
        [Route("")]
        public IEnumerable<MyPayload> Get()
        {
            var myPayLoad = new MyPayload
            {
                Description = Resources.AmazingResource.Description,
                Timestamp = DateTime.UtcNow,
                Name = Resources.AmazingResource.Name
            };
            return new [] { myPayLoad };
        }

        [HttpGet]
        [Route("{id}")]
        public MyPayload Get(int id)
        {
            return new MyPayload
            {
                Description = Resources.AmazingResource.Description,
                Timestamp = DateTime.UtcNow,
                Name = Resources.AmazingResource.Name
            };
        }

        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]MyPayload value)
        {
            if (!ModelState.IsValid)
            {
                HttpError error = GetErrors(ModelState, true);
                return Request.CreateResponse(HttpStatusCode.BadRequest, error);

            }

            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        [HttpDelete]
        [Route("{id}")]
        public void Delete(int id)
        {
            var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(Resources.AmazingResource.IdDoesNotExistException + " Id: " +  id),
            };
            throw new HttpResponseException(resp);
        }

        // This method is required because the default BadRequest(Modelstate) adds a english message...
        private HttpError GetErrors(IEnumerable<KeyValuePair<string, ModelState>> modelState, bool includeErrorDetail)
        {
            var modelStateError = new HttpError();
            foreach (KeyValuePair<string, ModelState> keyModelStatePair in modelState)
            {
                string key = keyModelStatePair.Key;
                ModelErrorCollection errors = keyModelStatePair.Value.Errors;
                if (errors != null && errors.Count > 0)
                {
                    IEnumerable<string> errorMessages = errors.Select(error =>
                    {
                        if (includeErrorDetail && error.Exception != null)
                        {
                            return error.Exception.Message;
                        }
                        return String.IsNullOrEmpty(error.ErrorMessage) ? "ErrorOccurred" : error.ErrorMessage;
                    }).ToArray();
                    modelStateError.Add(key, errorMessages);
                }
            }

            return modelStateError;
        }
    }
}
