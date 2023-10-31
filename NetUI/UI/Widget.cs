using Net.Essentials;
using Net.Essentials.Systems;

using System.Windows.Input;

namespace Net.UI;

public class Widget : ContentView
{
    Thickness? originalPadding;
    bool parentWasNull = true;
    WeakReference<WidgetPage> pagePointer = null;

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public ImageSource Icon
    {
        get => (ImageSource)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public ImageSource SelectedIcon
    {
        get => (ImageSource)GetValue(SelectedIconProperty);
        set => SetValue(SelectedIconProperty, value);
    }

    public bool IsVisibleAsTab
    {
        get => (bool)GetValue(IsVisibleAsTabProperty);
        set => SetValue(IsVisibleAsTabProperty, value);
    }

    public bool PadTop
    {
        get => (bool)GetValue(PadTopProperty);
        set => SetValue(PadTopProperty, value);
    }

    public bool PadBottom
    {
        get => (bool)GetValue(PadBottomProperty);
        set => SetValue(PadBottomProperty, value);
    }

    public ICommand ShowTabCommand
    {
        get => (ICommand)GetValue(ShowTabCommandProperty);
        set => SetValue(ShowTabCommandProperty, value);
    }

    public static readonly BindableProperty PadTopProperty = BindableProperty.Create(
        nameof(PadTop),
        typeof(bool),
        typeof(Widget),
        false,
        BindingMode.TwoWay,
        propertyChanged: (bindable, oldVal, newVal) =>
        {
            (bindable as Widget).AdjustPadding();
        });

    public static readonly BindableProperty IsVisibleAsTabProperty = BindableProperty.Create(
        nameof(IsVisibleAsTab),
        typeof(bool),
        typeof(Widget),
        true,
        BindingMode.TwoWay);

    public static readonly BindableProperty PadBottomProperty = BindableProperty.Create(
        nameof(PadBottom),
        typeof(bool),
        typeof(Widget),
        false,
        BindingMode.TwoWay,
        propertyChanged: (bindable, oldVal, newVal) =>
        {
            (bindable as Widget).AdjustPadding();
        });

    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
        nameof(Title),
        typeof(string),
        typeof(Widget),
        string.Empty,
        BindingMode.TwoWay);

    public static readonly BindableProperty IconProperty = BindableProperty.Create(
        nameof(Icon),
        typeof(ImageSource),
        typeof(Widget),
        null,
        BindingMode.TwoWay);

    public static readonly BindableProperty SelectedIconProperty = BindableProperty.Create(
        nameof(SelectedIcon),
        typeof(ImageSource),
        typeof(Widget),
        null,
        BindingMode.TwoWay);

    public static readonly BindableProperty ShowTabCommandProperty = BindableProperty.Create(
        nameof(ShowTabCommand),
        typeof(ICommand),
        typeof(Widget),
        null,
        BindingMode.TwoWay);

    public void AdjustPadding()
    {
        if (originalPadding == null)
        {
            originalPadding = Padding;
        }

        var pageMargin = NotchSystem.Instance.GetPageMargin();
        var top = PadTop ? pageMargin.Top : originalPadding.Value.Top;
        var bottom = PadBottom ? pageMargin.Bottom : originalPadding.Value.Bottom;
        Padding = new Thickness(
            originalPadding.Value.Left,
            top,
            originalPadding.Value.Right,
            bottom);
    }

    void TryHook(WidgetPage page)
    {
        if (parentWasNull && Parent != null)
        {
            page = page ?? GetPage();
            if (page != null)
            {
                parentWasNull = false;
                pagePointer = new WeakReference<WidgetPage>(page);
                page.OnAppeared += Page_OnAppeared;
                page.OnDisappeared += Page_OnDisappeared;
                TriggerStart();
            }
        }
        else if (!parentWasNull && Parent == null)
        {
            if (pagePointer.TryGetTarget(out var lastPage))
            {
                lastPage.OnDisappeared -= Page_OnDisappeared;
                lastPage.OnAppeared -= Page_OnAppeared;
            }
            parentWasNull = true;
            TriggerStop();
        }

        AdjustPadding();
    }

    protected override void OnParentSet()
    {
        base.OnParentSet();
        TryHook(null);
    }

    public void HookToAlivePage(WidgetPage page)
    {
        TryHook(page);
    }

    protected override void ChangeVisualState()
    {
        base.ChangeVisualState();
    }

    private void Page_OnDisappeared(object sender, EventArgs e)
    {
        TriggerStop();
    }

    private void Page_OnAppeared(object sender, EventArgs e)
    {
        TriggerStart();
    }

    public virtual void OnStart()
    {

    }

    public virtual void OnStop()
    {

    }

    bool isStarted = false;
    public void TriggerStart()
    {
        if (!isStarted)
        {
            isStarted = true;
            OnStart();
        }
    }

    public void TriggerStop()
    {
        if (isStarted)
        {
            isStarted = false;
            OnStop();
        }
    }

    public WidgetPage GetPage()
    {
        if (Parent is WidgetPage p) return p;

        var el = this as Element;
        while (true)
        {
            if (el == null || el.Parent == null) return null;
            if (el.Parent is WidgetPage page) return page;
            el = el.Parent;
        }
    }

    public bool GetIsVisibleRecursive()
    {
        var el = (View)this;
        while (el != null)
        {
            if (!el.IsVisible) return false;
            el = el.Parent as View;
        }
        return true;
    }

    WeakReference<TinyViewModel> lastContext = null;
    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        if (lastContext != null && lastContext.TryGetTarget(out var lastVm) &&
            lastVm is MauiViewModel lastMvm)
            lastMvm?.OnUnbind(this);
        if (BindingContext is TinyViewModel vm)
        {
            if (vm is MauiViewModel mvm) mvm.OnBind(this);
            lastContext = new WeakReference<TinyViewModel>(vm);
        }
    }
}