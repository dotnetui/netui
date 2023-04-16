using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Essentials
{
    public interface ISyncable : IRecord
    {
        DateTimeOffset CreateTimestamp { get; set; }
        DateTimeOffset SyncTimestamp { get; set; }
        DateTimeOffset UpdateTimestamp { get; set; }
        DateTimeOffset DeleteTimestamp { get; set; }
    }
}
