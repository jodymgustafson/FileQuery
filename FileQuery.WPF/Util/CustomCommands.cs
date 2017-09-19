using System.Windows.Input;

namespace FileQuery.Wpf.Util
{
    static class CustomCommands
    {
        public static readonly RoutedUICommand Exit = new RoutedUICommand(
                                "Exit",
                                "Exit",
                                typeof(CustomCommands),
                                new InputGestureCollection()
                                {
                                    new KeyGesture(Key.F4, ModifierKeys.Alt)
                                }
                        );

        public static readonly RoutedUICommand Execute = new RoutedUICommand(
                                "Execute Query",
                                "Execute",
                                typeof(CustomCommands),
                                new InputGestureCollection()
                                {
                                    new KeyGesture(Key.X, ModifierKeys.Alt)
                                }
                        );

        public static readonly RoutedUICommand AddExcludePath = new RoutedUICommand(
                                "Add _Exclude Path",
                                "Exclude",
                                typeof(CustomCommands)
                        );
    }
}
