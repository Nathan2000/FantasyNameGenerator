namespace Gui.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    public class CrossButton : Button
    {
        static CrossButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(CrossButton),
                new FrameworkPropertyMetadata(typeof(CrossButton)));
        }
    }
}
