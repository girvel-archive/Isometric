using System.Windows.Controls;
using System.Windows.Media;

namespace Isometric.Editor.Extensions
{
    public static class ConstantBoxEvents
    {
        public static TextChangedEventHandler GenerateTextChangedEventHandler(string name)
        {
            return (objectSender, e) =>
            {
                var sender = (TextBox) objectSender;

                sender.Background = GameData.Instance.Constants[name].TrySet(sender.Text)
                    ? Brushes.White
                    : Brushes.MistyRose;
            };
        }
    }
}