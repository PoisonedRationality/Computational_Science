using GraphData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphControl
{
    /// <summary>
    /// Interaction logic for UpdatingText.xaml
    /// </summary>
    public partial class UpdatingText : UserControl, IUpdating
    {
        public UpdatingText()
        {
            InitializeComponent();
        }

        public string Title
        {
            get
            {
                return TitleBlock.Text;
            }
            set
            {
                TitleBlock.Text = value;
            }
        }
        public Color Color
        {
            get
            {
                SolidColorBrush br = TitleBlock.Foreground as SolidColorBrush;
                return br.Color;
            }
            set
            {
                Brush newBrush = new SolidColorBrush(value);
                TitleBlock.Foreground = newBrush;
                TextBlock.Foreground = newBrush;
            }
        }

        public void Update(GraphDataPacket text)
        {
            TextBlock.Text = text.GetTextData();
        }
    }
}
