using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    public class Capabilities
    {
        public bool HasLiveSubscription { get; set; }
        public bool HasEnterpriseLihp { get; set; }
        public bool HasSvvTimecodedComments { get; set; }
        public bool HasSimplifiedEnterpriseAccount { get; set; }
    }
}
