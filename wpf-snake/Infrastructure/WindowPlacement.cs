using System.Windows;

namespace wpf_snake.Infrastructure
{
    internal static class WindowPlacement
    {
        private static Rect? _lastBounds;
        private static WindowState _lastState = WindowState.Normal;

        public static void Apply(Window window)
        {
            window.WindowStartupLocation = WindowStartupLocation.Manual;

            if (_lastBounds.HasValue)
            {
                var b = _lastBounds.Value;
                window.Left = b.Left;
                window.Top = b.Top;
                window.Width = b.Width;
                window.Height = b.Height;
                window.WindowState = _lastState;
            }

            window.LocationChanged += (_, __) => Capture(window);
            window.SizeChanged += (_, __) => Capture(window);
            window.StateChanged += (_, __) => Capture(window);
        }

        private static void Capture(Window window)
        {
            _lastState = window.WindowState;
            var rect = window.WindowState == WindowState.Normal
                ? new Rect(window.Left, window.Top, window.Width, window.Height)
                : window.RestoreBounds;
            _lastBounds = rect;
        }
    }
}