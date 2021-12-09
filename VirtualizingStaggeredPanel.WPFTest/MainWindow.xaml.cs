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
            for (int i = 0; i < 100000; i++)
            {
                FakeImages.Add(new VirtualGridFlowPanelItemParam()
                {
                    Index = FakeImages.Count,
                    AspectRatio = 1
                });
            }
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
            FakeImages.Insert(0, new VirtualGridFlowPanelItemParam()
            {
                Index = FakeImages.Count,
                AspectRatio = 0.5
            });
        }

        static T GetChildRecursively<T>(DependencyObject root, object specifyDataCtx = default) where T : FrameworkElement
        {
            if (root is T d && (specifyDataCtx == default || d.DataContext == specifyDataCtx))
                return d;
            var c = VisualTreeHelper.GetChildrenCount(root);
            for (int i = 0; i < c; i++)
            {
                var w = GetChildRecursively<T>(VisualTreeHelper.GetChild(root, i), specifyDataCtx);
                if (w != null)
                    return w;
            }
            return null;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var panel = GetChildRecursively<MikiraSora.VirtualizingStaggeredPanel.VirtualizingStaggeredPanel>(DisplayList,null);
            var item = FakeImages.FirstOrDefault(x => x.Index == 20);

            var scrollOffset = panel.FindScrollOffsetByItem(item);
            if (scrollOffset.HasValue)
                panel.ScrollOwner?.ScrollToVerticalOffset(scrollOffset.Value);
        }
    }
}
