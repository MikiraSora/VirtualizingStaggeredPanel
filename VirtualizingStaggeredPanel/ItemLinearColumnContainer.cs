using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MikiraSora.VirtualizingStaggeredPanel
{
    internal class ItemLinearColumnContainer
    {
        public List<IVirtualGridFlowPanelItemParam> Children { get; } = new();

        private double minOffsetY;
        private double maxOffsetY;
        private int gridItemWidth;

        public ItemLinearColumnContainer(int gridItemWidth)
        {
            this.gridItemWidth = gridItemWidth;
        }

        public double MinOffsetY => minOffsetY + OffsetYBase;
        public double MaxOffsetY => maxOffsetY + OffsetYBase;

        public int OffsetYBase { get; set; } = 0;

        public double DesiredHeight { get; private set; }

        public void Prepend(IVirtualGridFlowPanelItemParam element)
        {
            Children.Insert(0, element);

            var height = gridItemWidth / element.AspectRatio;
            DesiredHeight += height;
            minOffsetY -= height;
        }

        public void Append(IVirtualGridFlowPanelItemParam element)
        {
            Children.Add(element);

            var height = gridItemWidth / element.AspectRatio;
            DesiredHeight += height;
            maxOffsetY += height;
        }

        public override string ToString() => $"Count:{Children.Count} Height:{MinOffsetY}~{MaxOffsetY} Base:{OffsetYBase}";
    }
}
