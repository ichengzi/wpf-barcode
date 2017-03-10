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

namespace WpfBarCode
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<string, string> codeTypes = new Dictionary<string, string>()
        {
            { "Code39", "Code39" },
            { "Code128A", "Code128A" },
            { "Code128B", "Code128B" },
            { "Code128C", "Code128C" },
        };

        public MainWindow()
        {
            InitializeComponent();
            codeTypeCombobox.DataContext = codeTypes;
            codeTypeCombobox.SelectedValue = "Code39";

            var code = "1234567890";
            var lists = new BarcodeEngine() { CodeType = "Code128A",BarWidth = 2 }.Generate(code);
            var canvas = new czBarCodeCanvas(lists,50);
            sp.Children.Add(canvas);
        }

        private void barWithSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            barcode.BarWidth = e.NewValue;
        }

        private void showTextCheckBox_Click(object sender, RoutedEventArgs e)
        {
            barcode.TextVisibility = showTextCheckBox.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
        }

        private void codeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            barcode.Code = codeTextBox.Text;
        }

        private void codeTypeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            barcode.CodeType = codeTypeCombobox.SelectedValue.ToString();
        }

        private void code39WideBarRateSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            barcode.Code39WideRate = e.NewValue;
        }
    }

    public class czBarCodeCanvas : Canvas
    {
        private List<BarcodeItem> barCodeItems;
        
        public czBarCodeCanvas(List<BarcodeItem> barCodeItems,double height)
        {
            this.barCodeItems = barCodeItems;
            this.Height = height;
        }

        protected override void OnRender(DrawingContext dc)
        {
            try
            {
                base.OnRender(dc);
                czRender(dc);
            }
            catch (Exception ex)
            {
                //
            }
        }

        private void czRender(DrawingContext dc)
        {
            double x = 0;
            foreach (var bar in barCodeItems)
            {
                var rect = new Rect(x, 0, bar.Width, this.Height);
                dc.DrawRectangle(bar.Color,null,rect);
                x += bar.Width;
            }
        }
    }
}
