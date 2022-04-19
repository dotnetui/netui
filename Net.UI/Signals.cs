namespace Net.UI;

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