using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;

namespace wpf_snake.Controls   // <-- módosítsd a saját namespace-edre!
{
    public class GameTextBox : TextBox
    {
        public Brush BorderColor { get; set; } = Brushes.Lime;
        public Brush BackgroundColorCustom { get; set; } = Brushes.Black;
        public Brush TextColor { get; set; } = Brushes.Lime;
        public double BorderSize { get; set; } = 1;
        public Thickness InnerPadding { get; set; } = new Thickness(6);

        public GameTextBox()
        {
            Background = Brushes.Transparent; // mert mi rajzoljuk
            Foreground = TextColor;

            BorderThickness = new Thickness(0);
            FontFamily = new FontFamily("Courier New");
            FontWeight = FontWeights.Bold;
            FontSize = 14;

            Padding = InnerPadding;

            CaretBrush = TextColor;
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            double w = ActualWidth;
            double h = ActualHeight;

            // Háttér
            dc.DrawRectangle(BackgroundColorCustom, null, new Rect(0, 0, w, h));

            // Keret
            var pen = new Pen(BorderColor, BorderSize);
            dc.DrawRectangle(null, pen, new Rect(BorderSize / 2, BorderSize / 2,
                                                 w - BorderSize, h - BorderSize));
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            InvalidateVisual();
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            InvalidateVisual();
        }

        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnGotKeyboardFocus(e);
            CaretBrush = TextColor;
        }

        protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnLostKeyboardFocus(e);
            CaretBrush = TextColor;
        }
    }
}
