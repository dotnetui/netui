using Net.Essentials.Vimeo.Models;

using RestSharp;

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo
{
    public class DeviceAuthorizeResponse
    {
        public RestResponse Response { get; set; }
        public ApiToken Token { get; set; }
    }
}
