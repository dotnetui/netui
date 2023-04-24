using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.UI
{
    public enum Signals
    {
        PopModal,
        RunOnUI, // Action
        ShowFirstPage,
        ShowPage, // Page
        ShowModalPage, // Page
        AppStart,
        AppSleep,
        AppResume,
        AndroidSafeInsetsUpdate, // Thickness
    }
}
