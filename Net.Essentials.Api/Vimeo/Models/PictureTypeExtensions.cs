using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    public static class PictureTypeExtensions
    {
        public static PictureType ToPictureType(string pictureTypeString)
        {
            if (Enum.TryParse<PictureType>(pictureTypeString, true, out var result))
                return result;
            return PictureType.Unknown;
        }

        public static string ToPictureTypeString(this PictureType pictureType)
        {
            if (pictureType == PictureType.Unknown) return null;
            return pictureType.ToString().ToLower();
        }
    }
}
