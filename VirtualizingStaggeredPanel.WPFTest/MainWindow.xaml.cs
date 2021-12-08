using MikiraSora.VirtualizingStaggeredPanel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace VirtualizingStaggeredPanel.WPFTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<VirtualGridFlowPanelItemParam> FakeImages { get; } = new ObservableCollection<VirtualGridFlowPanelItemParam>(
            new Size[] {
            new(100,200),
            new(200,300),
            new(100,250),
            new(50,400),
            new(90,400),
            new(700,700),
            new(400,200),
            new(200,300),
            new(100,250),
            new(200,300),
            new(100,250),
            new(50,400),
            new(90,400),
            new(700,700),
            new(400,200),
            new(200,300),
            new(100,250),
            new(50,400),
            new(200,300),
            new(100,250),
            new(50,400),
            new(200,300),
            new(100,250),
            new(200,300),
            new(100,250),
            new(50,400),
            new(90,400),
            new(700,700),
            new(400,200),
            new(200,300),
            new(100,250),
            new(50,400),
            new(200,300),
            new(100,250),
            new(50,400),
        }.Select((x, i) => new VirtualGridFlowPanelItemParam()
        {
            AspectRatio = x.Width / x.Height,
            Index = i
        }));

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FakeImages.Add(new VirtualGridFlowPanelItemParam()
            {
                Index = FakeImages.Count,
                AspectRatio = 1
            });
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            FakeImages.Insert(0,new VirtualGridFlowPanelItemParam()
            {
                Index = FakeImages.Count,
                AspectRatio = 0.5
            });
        }
    }
}
