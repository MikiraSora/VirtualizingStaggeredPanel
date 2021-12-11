using MikiraSora.VirtualizingStaggeredPanel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VirtualizingStaggeredPanel.WPFTest
{
    public class VirtualGridFlowPanelItemParam : IVirtualGridFlowPanelItemParam
    {
        public int Index { get; set; }

        public double AspectRatio { get; set; }
        public bool __HasInserted { get; set; }
        public int __ItemIndex { get; set; }

        public override string ToString() => $"Index:{Index} __ItemIndex:{__ItemIndex} AspectRatio:{AspectRatio}";
    }
}
