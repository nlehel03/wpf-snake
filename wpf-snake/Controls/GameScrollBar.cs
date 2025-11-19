using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace wpf_snake.Controls   // !!! igazítsd a saját namespace-edhez !!!
{
    public class GameScrollBar : Control
    {
        private bool _dragging;
        private double _dragOffset;   // egér–thumb offset drag közben

        static GameScrollBar()
        {
            // fontos, hogy kapjon háttérképet (különben átlátszó marad)
            BackgroundProperty.OverrideMetadata(
                typeof(GameScrollBar),
                new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender));
        }

        public GameScrollBar()
        {
            Width = 14;
            Background = Brushes.Black;
            TrackBrush = new SolidColorBrush(Color.FromRgb(0, 128, 0));
            ThumbBrush = Brushes.Lime;
            BorderBrush = Brushes.Lime;

            LargeChange = 5;
            SmallChange = 1;
        }

        // --- DependencyProperty-k: Value, Maximum, TrackBrush, ThumbBrush, BorderBrush ---

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                nameof(Value),
                typeof(int),
                typeof(GameScrollBar),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender, OnValueChanged, CoerceValue));

        public int Value
        {
            get => (int)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        private static object CoerceValue(DependencyObject d, object baseValue)
        {
            var bar = (GameScrollBar)d;
            int v = (int)baseValue;
            if (v < 0) v = 0;
            if (v > bar.Maximum) v = bar.Maximum;
            return v;
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var bar = (GameScrollBar)d;
            bar.ValueChanged?.Invoke(bar, EventArgs.Empty);
        }

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register(
                nameof(Maximum),
                typeof(int),
                typeof(GameScrollBar),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender, OnMaximumChanged));

        public int Maximum
        {
            get => (int)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var bar = (GameScrollBar)d;
            if (bar.Maximum < 0)
                bar.Maximum = 0;
            if (bar.Value > bar.Maximum)
                bar.Value = bar.Maximum;
        }

        public Brush TrackBrush
        {
            get => (Brush)GetValue(TrackBrushProperty);
            set => SetValue(TrackBrushProperty, value);
        }

        public static readonly DependencyProperty TrackBrushProperty =
            DependencyProperty.Register(
                nameof(TrackBrush),
                typeof(Brush),
                typeof(GameScrollBar),
                new FrameworkPropertyMetadata(Brushes.DarkGreen, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush ThumbBrush
        {
            get => (Brush)GetValue(ThumbBrushProperty);
            set => SetValue(ThumbBrushProperty, value);
        }

        public static readonly DependencyProperty ThumbBrushProperty =
            DependencyProperty.Register(
                nameof(ThumbBrush),
                typeof(Brush),
                typeof(GameScrollBar),
                new FrameworkPropertyMetadata(Brushes.Lime, FrameworkPropertyMetadataOptions.AffectsRender));

        public new Brush BorderBrush
        {
            get => (Brush)GetValue(BorderBrushProperty);
            set => SetValue(BorderBrushProperty, value);
        }

        public static readonly new DependencyProperty BorderBrushProperty =
            DependencyProperty.Register(
                nameof(BorderBrush),
                typeof(Brush),
                typeof(GameScrollBar),
                new FrameworkPropertyMetadata(Brushes.Lime, FrameworkPropertyMetadataOptions.AffectsRender));

        // WinForms-ból áthozott logika szerinti lépések
        public int LargeChange { get; set; } = 5;
        public int SmallChange { get; set; } = 1;

        public event EventHandler? ValueChanged;

        // --- Rajzolás (WPF-es megfelelője az OnPaint-nek) ---

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            double w = ActualWidth;
            double h = ActualHeight;

            // háttér
            dc.DrawRectangle(Background, null, new Rect(0, 0, w, h));

            // track
            Rect trackRect = new Rect(3, 3, Math.Max(0, w - 6), Math.Max(0, h - 6));
            dc.DrawRectangle(TrackBrush, null, trackRect);

            // thumb
            var (thumbTop, thumbHeight) = GetThumbRect();
            Rect thumbRect = new Rect(3, thumbTop, Math.Max(0, w - 6), thumbHeight);
            dc.DrawRectangle(ThumbBrush, null, thumbRect);

            // border
            var pen = new Pen(BorderBrush, 1);
            dc.DrawRectangle(null, pen, new Rect(0.5, 0.5, w - 1, h - 1));
        }

        private (double top, double height) GetThumbRect()
        {
            double total = ActualHeight - 6; // 3-3 px margó
            if (total <= 0)
                return (3, 10);

            int page = Math.Max(1, LargeChange);
            double thumbHeight = Math.Max(14, total * page / (Maximum + page));
            double maxTop = Math.Max(0, total - thumbHeight);

            double topOffset = (Maximum == 0) ? 0 : maxTop * Value / Maximum;
            double top = 3 + topOffset;

            return (top, thumbHeight);
        }

        // --- Egérkezelés (drag, kattintás, görgetés) ---

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            Focus(); // hogy kaphasson wheel-t

            var pos = e.GetPosition(this);
            var (thumbTop, thumbHeight) = GetThumbRect();
            Rect thumbRect = new Rect(3, thumbTop, Math.Max(0, ActualWidth - 6), thumbHeight);

            if (thumbRect.Contains(pos))
            {
                _dragging = true;
                _dragOffset = pos.Y - thumbTop;
                CaptureMouse();
            }
            else
            {
                // page up / page down
                if (pos.Y < thumbTop)
                    Value = Math.Max(0, Value - LargeChange);
                else if (pos.Y > thumbTop + thumbHeight)
                    Value = Math.Min(Maximum, Value + LargeChange);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (_dragging)
            {
                var pos = e.GetPosition(this);
                double total = ActualHeight - 6;
                var (_, thumbHeight) = GetThumbRect();
                double maxTop = Math.Max(0, total - thumbHeight);

                double newTop = pos.Y - _dragOffset;
                newTop = Math.Max(3, Math.Min(3 + maxTop, newTop));

                double thumbPos = newTop - 3;
                Value = (maxTop == 0) ? 0 : (int)Math.Round(thumbPos * Maximum / maxTop);
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            if (_dragging)
            {
                _dragging = false;
                ReleaseMouseCapture();
            }
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            int steps = e.Delta / 120; // WPF-ben is 120 a default
            if (steps != 0)
            {
                Value = Math.Max(0, Math.Min(Maximum, Value - steps * SmallChange));
            }
        }
    }
}
