using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    public class Play
    {
        public object Dash { get; set; }
        public object Hls { get; set; }
        public object Progressive { get; set; }
        public string Status { get; set; }
    }
}
