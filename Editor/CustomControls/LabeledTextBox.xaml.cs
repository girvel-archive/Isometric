namespace Isometric.Editor.CustomControls
{
    /// <summary>
    /// Логика взаимодействия для LabeledTextBox.xaml
    /// </summary>
    public partial class LabeledTextBox
    {
        public string LabelText
        {
            get { return Label.Content.ToString(); }
            set { Label.Content = value; }
        }

        public string Text
        {
            get { return TextBox.Text; }
            set { TextBox.Text = value; }
        }



        public LabeledTextBox()
        {
            InitializeComponent();
            
        }
    }
}
