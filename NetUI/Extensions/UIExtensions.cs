using Microsoft.Maui.Controls.Compatibility;

namespace Net.UI;

public static class UIExtensions
{
    public static Point GetScreenCoords(this VisualElement view)
    {
        var result = new Point(view.X + view.TranslationX, view.Y + view.TranslationY);

        var parent = view.Parent as VisualElement;
        while (parent != null)
        {
            result = result.Offset(parent.X + parent.TranslationX, parent.Y + parent.TranslationY);
            if (parent is ScrollView scrollView)
            {
                result = result.Offset(-scrollView.ScrollX, -scrollView.ScrollY);
            }
            parent = parent.Parent as VisualElement;
        }
        return result;
    }

    public static T FindParentOfType<T>(this IView view) where T : IView
    {
        var parent = view.Parent as IView;
        while (parent != null)
        {
            if (parent is T t)
                return t;
            parent = parent.Parent as IView;
        }
        return default;
    }

    public static Page GetPage(this Element view)
    {
        var el = view;
        while (true)
        {
            if (el == null || el.Parent == null) return null;
            if (el.Parent is Page page) return page;
            el = el.Parent;
        }
    }

    public static Page WithPage(this View view)
    {
        return new WidgetPage
        {
            Content = view
        };
    }

    public static IEnumerable<View> GetAllChildren(this VisualElement source)
    {
        if (source != null)
        {
            if (source is ContentView contentView)
            {
                yield return contentView.Content;
                if (contentView.Content != null)
                {
                    foreach (View view in contentView.Content.GetAllChildren())
                    {
                        yield return view;
                    }
                }

            }
            else
            {
                if (source is Layout<View> viewLayout)
                {
                    foreach (View child in viewLayout.Children)
                    {
                        yield return child;
                        foreach (View view in child.GetAllChildren())
                            yield return view;
                    }
                }
                else
                {
                    if (source is ContentPage contentPage)
                    {
                        yield return contentPage.Content;
                        foreach (View view in contentPage.Content.GetAllChildren())
                            yield return view;
                    }
                }
            }
        }
    }
}