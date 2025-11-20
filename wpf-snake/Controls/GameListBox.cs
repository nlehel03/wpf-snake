using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace wpf_snake.Controls
{
    public class GameListBox : ListBox
    {
        public GameListBox()
        {
            Background = Brushes.Black;
            Foreground = Brushes.Lime;
            BorderBrush = Brushes.Lime;
            BorderThickness = new Thickness(2);
            FontFamily = new FontFamily("Courier New");
            FontSize = 14;
            SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Auto);

            var itemStyle = new Style(typeof(ListBoxItem));
            itemStyle.Setters.Add(new Setter(BackgroundProperty, Brushes.Black));
            itemStyle.Setters.Add(new Setter(ForegroundProperty, Brushes.Green));
            itemStyle.Setters.Add(new Setter(PaddingProperty, new Thickness(4)));

            var selectedTrigger = new Trigger { Property = IsSelectedProperty, Value = true };
            selectedTrigger.Setters.Add(new Setter(BackgroundProperty, Brushes.Green));
            selectedTrigger.Setters.Add(new Setter(ForegroundProperty, Brushes.Black));
            itemStyle.Triggers.Add(selectedTrigger);

            ItemContainerStyle = itemStyle;
        }
    }
}
