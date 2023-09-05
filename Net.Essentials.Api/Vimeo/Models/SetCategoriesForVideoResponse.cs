using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    public enum SetCategoriesForVideoResponse
    {
        None,
        Created,
        UserDoesntOwnVideo,
        NotFound,
        Error
    }
}
