﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Foundation;
using UIKit;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Specifics = Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using SizeF = CoreGraphics.CGSize;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Net.UI;
using Net.UI.iOS.Renderers;

[assembly: ExportRenderer(typeof(Button2), typeof(Button2Renderer))]
namespace Net.UI.iOS.Renderers
{
    public class Button2Renderer : ButtonRenderer
    {
        bool firstTime = true;

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            var customButton = e.NewElement as Button2;
            var thisButton = Control as UIButton;

            if (firstTime)
            {
                firstTime = false;
                thisButton.TouchDown += ThisButton_TouchDown;
                thisButton.TouchUpOutside += ThisButton_TouchUpOutside;
                thisButton.TouchCancel += ThisButton_TouchCancel;
                thisButton.TouchUpInside += ThisButton_TouchUpInside;
            }

            try
            {
                if (customButton?.TextAlignment == TextAlignment.Center)
                {
                    thisButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
                }
                else if (customButton?.TextAlignment == TextAlignment.Start)
                {
                    thisButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
                }
                else if (customButton?.TextAlignment == TextAlignment.End)
                {
                    thisButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Right;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Custom button null err: {ex}");
            }
        }

        private void ThisButton_TouchCancel(object sender, EventArgs e)
        {
            (Element as Button2).OnReleased();
        }

        private void ThisButton_TouchUpInside(object sender, EventArgs e)
        {
            (Element as Button2).OnReleased();
            (Element as Button2).OnClicked();
        }

        private void ThisButton_TouchUpOutside(object sender, EventArgs e)
        {
            (Element as Button2).OnReleased();
        }

        private void ThisButton_TouchDown(object sender, EventArgs e)
        {
            (Element as Button2).OnPressed();
        }
    }
}