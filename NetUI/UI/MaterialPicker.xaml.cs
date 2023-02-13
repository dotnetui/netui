namespace Net.UI;

public partial class MaterialPicker
{
    public class Item
    {
        public object Payload { get; set; }
        public string Display { get; set; }
    }

    List<Item> items = new();

    public object SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public IEnumerable<object> ItemsSource
    {
        get => (IEnumerable<object>)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public IEnumerable<string> ItemTexts
    {
        get => (List<string>)GetValue(ItemTextsProperty);
        set => SetValue(ItemTextsProperty, value);
    }

    public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(
            nameof(Placeholder),
            typeof(string),
            typeof(MaterialPicker));

    public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create(
            nameof(ItemsSource),
            typeof(IEnumerable<object>),
            typeof(MaterialPicker),
            propertyChanged: (bindable, oldVal, newVal) =>
            {
                if (bindable is MaterialPicker picker)
                    picker.BuildItemTexts();
            });

    public static readonly BindableProperty ItemTextsProperty =
        BindableProperty.Create(
            nameof(ItemTexts),
            typeof(List<string>),
            typeof(MaterialPicker),
            propertyChanged: (bindable, oldVal, newVal) =>
            {
                if (bindable is MaterialPicker picker)
                    picker.BuildItemTexts();
            });

    public static readonly BindableProperty SelectedItemProperty =
        BindableProperty.Create(
            nameof(SelectedItem),
            typeof(object),
            typeof(MaterialPicker),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: (bindable, oldVal, newVal) =>
            {
                if (bindable is MaterialPicker picker)
                    picker.LoadPickerWithSelectedItem();
            });

    public MaterialPicker()
    {
        InitializeComponent();
        grid.BindingContext = this;
        tap.Tapped += (s, e) =>
        {
            if (items == null || items.Count == 0) return;
            picker.Focus();
        };
    }

    void BuildItemTexts()
    {
        items.Clear();
        var lstItemsSource = ItemsSource?.ToList();
        var lstItemTexts = ItemTexts?.ToList();
        if (lstItemsSource != null && lstItemsSource.Count > 0)
            for (int i = 0; i < lstItemsSource.Count; i++)
            {
                items.Add(new Item
                {
                    Payload = lstItemsSource[i],
                    Display =
                        (ItemTexts != null && lstItemTexts.Count > i) ?
                            lstItemTexts[i] :
                            lstItemsSource[i]?.ToString() ?? "null"
                });
            }
        picker.ItemsSource = items;
    }

    void picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (items != null && picker.SelectedIndex < items.Count)
        {
            var payload = items[picker.SelectedIndex].Payload;
            if (payload != SelectedItem)
                SelectedItem = payload;
        }
    }

    void LoadPickerWithSelectedItem()
    {
        if (items == null || items.Count == 0)
            return;

        if (items != null && picker.SelectedIndex < items.Count && picker.SelectedIndex >= 0)
        {
            var payload = items[picker.SelectedIndex].Payload;
            if (payload == SelectedItem)
                return;
        }

        int i = 0;
        for (; i < items.Count; i++)
        {
            if (SelectedItem == items[i].Payload)
            {
                picker.SelectedIndex = i;
                return;
            }
        }
    }
}