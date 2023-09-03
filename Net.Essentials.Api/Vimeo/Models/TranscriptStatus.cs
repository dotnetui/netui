using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    public enum TranscriptStatus
    {
        Unspecified = 0,
        Unknown,
        Completed,
        Failed,
        InProgress,
        LanguageNotSupported,
        NoSpeech,
        NotStarted
    }
}
