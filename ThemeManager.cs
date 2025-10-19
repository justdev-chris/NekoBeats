using System;
using System.Collections.Generic;

namespace NekoBeats
{
    public static class ThemeManager
    {
        public static event Action<ITheme> ThemeChanged;
        private static ITheme _currentTheme;

        public static ITheme CurrentTheme
        {
            get => _currentTheme;
            set
            {
                _currentTheme = value;
                ThemeChanged?.Invoke(value);
            }
        }

        static ThemeManager()
        {
            // Default theme
            CurrentTheme = new GradientBarsTheme();
        }
    }
}
