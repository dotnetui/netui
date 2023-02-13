namespace Net.UI;

public abstract class CodeCellBase : Widget
{
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public new Color BackgroundColor
    {
        get => (Color)GetValue(BackgroundColorProperty);
        set => SetValue(BackgroundColorProperty, value);
    }

    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text),
        typeof(string),
        typeof(CodeCellBase),
        "",
        BindingMode.TwoWay);

    public static new readonly BindableProperty BackgroundColorProperty = BindableProperty.Create(
        nameof(BackgroundColor),
        typeof(Color),
        typeof(CodeCellBase),
        Color.FromArgb("#00000000"));

    public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
        nameof(TextColor),
        typeof(Color),
        typeof(CodeCellBase),
        Color.FromArgb("#000"));


    public new virtual void Focus()
    {

    }

    public new virtual void Unfocus()
    {

    }
}

public class CodeCell : CodeCellBase
{
    public Entry Entry;

    public CodeCell()
    {
        Content = Entry = new Entry
        {
            BackgroundColor = BackgroundColor,
            TextColor = TextColor,
            Text = Text
        };
        Entry.BindingContext = this;
        Entry.SetBinding(Entry.BackgroundColorProperty, new Binding(nameof(BackgroundColor)));
        Entry.SetBinding(Entry.TextColorProperty, new Binding(nameof(TextColor)));
        Entry.SetBinding(Entry.TextProperty, new Binding(nameof(Text)));
    }

    public override void Focus()
    {
        base.Focus();
        Entry.Focus();
    }

    public override void Unfocus()
    {
        base.Unfocus();
        Entry.Unfocus();
    }
}