using System.Windows.Controls;
using Isometric.Editor.CustomControls;

namespace Isometric.Editor.Extensions
{
    public static class ConstantBoxEvents
    {
        public static TextChangedEventHandler GenerateTextChangedEventHandler(string name)
        {
            return (objectSender, e) =>
            {
                var sender = (TextBox) objectSender;

                GameData.Instance.Constants[name].TrySet(sender.Text);
            };
        }
    }
}