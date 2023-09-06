using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    public class Preset
    {
        public object Metadata { get; set; }
        public string Name { get; set; }
        public EmbedSettings Settings { get; set; }
        public string Uri { get; set; }
        public User User { get; set; }
    }
}
