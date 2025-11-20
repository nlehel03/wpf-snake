using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace wpf_snake.Controls   
{
    public class GameListBox : ListBox
    {
        public GameListBox()
        {
            // Alap kinézet – megfelel a WinForms verziódnak
            Background = Brushes.Black;
            Foreground = Brushes.Lime;
            BorderBrush = Brushes.Lime;
            BorderThickness = new Thickness(2);
            FontFamily = new FontFamily("Courier New");
            FontSize = 14;
            SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Auto);

            // ItemContainerStyle – hogyan néz ki egy sor
            var style = new Style(typeof(ListBoxItem));

            style.Setters.Add(new Setter(BackgroundProperty, Brushes.Black));
            style.Setters.Add(new Setter(ForegroundProperty, Brushes.Green));
            style.Setters.Add(new Setter(PaddingProperty, new Thickness(4)));

            // Kijelölt elem: zöld háttér, fekete szöveg
            var trigger = new Trigger
            {
                Property = IsSelectedProperty,
                Value = true
            };
            trigger.Setters.Add(new Setter(BackgroundProperty, Brushes.Green));
            trigger.Setters.Add(new Setter(ForegroundProperty, Brushes.Black));

            style.Triggers.Add(trigger);

            ItemContainerStyle = style;
        }
    }
}
