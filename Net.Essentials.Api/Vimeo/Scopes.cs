using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo
{
    [Flags]
    public enum Scopes
    {
        Public = 0,
        Private = 1,
        Purchased = Private | 2,
        Create = Private | 4,
        Edit = Private | 8,
        Delete = Private | 16,
        Interact = Private | 32,
        Stats = Private | 64,
        Upload = Private | 128,
        PromoCodes = Private | 256,
        VideoFiles = Private | 512,
        All = Private | Purchased | Create | Edit | Delete | Interact | Stats | Upload | PromoCodes | VideoFiles,
        Basic = Public | Purchased | Create | Edit | Delete | Interact | Stats
    }
}
