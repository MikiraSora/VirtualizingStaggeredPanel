using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace MikiraSora.VirtualizingStaggeredPanel
{
    public class VirtualizingStaggeredPanel : VirtualizingPanel, IScrollInfo
    {
        #region DP

        public int GridItemWidth
        {
            get { return (int)GetValue(GridItemWidthProperty); }
            set { SetValue(GridItemWidthProperty, value); }
        }

        public int GridItemMarginWidth
        {
            get { return (int)GetValue(GridItemMarginWidthProperty); }
            set { SetValue(GridItemMarginWidthProperty, value); }
        }

        public static readonly DependencyProperty GridItemMarginWidthProperty =
            DependencyProperty.Register("GridItemMarginWidth", typeof(int), typeof(VirtualizingStaggeredPanel), new PropertyMetadata(10));

        public static readonly DependencyProperty GridItemWidthProperty =
            DependencyProperty.Register("GridItemWidth", typeof(int), typeof(VirtualizingStaggeredPanel), new PropertyMetadata(150));

        #endregion

        #region IScrollInfo

        //鼠标每一次滚动 UI上的偏移
        public static readonly DependencyProperty ScrollOffsetProperty = DependencyProperty.RegisterAttached("ScrollOffset", typeof(int), typeof(VirtualizingStaggeredPanel), new PropertyMetadata(10));

        public int ScrollOffset
        {
            get { return (int)GetValue(ScrollOffsetProperty); }
            set { SetValue(ScrollOffsetProperty, value); }
        }

        public bool CanHorizontallyScroll { get; set; }
        public bool CanVerticallyScroll { get; set; }

        private Size extent = new Size(0, 0);
        public double ExtentWidth => extent.Width;
        public double ExtentHeight => extent.Height;

        private Size viewPort = new Size(0, 0);
        public double ViewportWidth => viewPort.Width;
        public double ViewportHeight => viewPort.Height;

        private TranslateTransform trans = new TranslateTransform();

        private Point offset;
        public double HorizontalOffset => offset.X;
        public double VerticalOffset => offset.Y;

        public ScrollViewer ScrollOwner { get; set; }

        public void LineDown()
        {
            SetVerticalOffset(VerticalOffset + ScrollOffset);
        }

        public void LineLeft()
        {
            throw new NotImplementedException();
        }

        public void LineRight()
        {
            throw new NotImplementedException();
        }

        public void LineUp()
        {
            this.SetVerticalOffset(this.VerticalOffset - this.ScrollOffset);
        }

        public Rect MakeVisible(Visual visual, Rect rectangle)
        {
            return new Rect();
        }

        public void MouseWheelDown()
        {
            this.SetVerticalOffset(this.VerticalOffset + this.ScrollOffset);
        }

        public void MouseWheelLeft()
        {
            throw new NotImplementedException();
        }

        public void MouseWheelRight()
        {
            throw new NotImplementedException();
        }

        public void MouseWheelUp()
        {
            this.SetVerticalOffset(this.VerticalOffset - this.ScrollOffset);
        }

        public void PageDown()
        {
            this.SetVerticalOffset(this.VerticalOffset + this.viewPort.Height);
        }

        public void PageLeft()
        {
            throw new NotImplementedException();
        }

        public void PageRight()
        {
            throw new NotImplementedException();
        }

        public void PageUp()
        {
            this.SetVerticalOffset(this.VerticalOffset - this.viewPort.Height);
        }

        public void SetHorizontalOffset(double offset)
        {
            throw new NotImplementedException();
        }

        public void SetVerticalOffset(double offset)
        {
            if (offset < 0 || this.viewPort.Height >= this.extent.Height)
                offset = 0;
            else
                if (offset + this.viewPort.Height >= this.extent.Height)
                offset = this.extent.Height - this.viewPort.Height;

            this.offset.Y = offset;
            ScrollOwner?.InvalidateScrollInfo();
            trans.Y = -offset;
            InvalidateMeasure();
        }

        #endregion

        ItemLinearColumnContainer[] containers = default;

        Dictionary<UIElement, (IVirtualGridFlowPanelItemParam, int)> elementMap = new();

        ItemsControl refItemsControl;
        ItemsControl RefItemsControl => refItemsControl ??= ItemsControl.GetItemsOwner(this);

        IEnumerable<IVirtualGridFlowPanelItemParam> ItemSource => (RefItemsControl?.HasItems ?? false) ? RefItemsControl.ItemsSource.OfType<IVirtualGridFlowPanelItemParam>() : Enumerable.Empty<IVirtualGridFlowPanelItemParam>();

        private bool CheckAndReBuildContainers(Size availableSize)
        {
            var width = GridItemWidth;
            var col = Math.Max(0, (int)((double.IsNaN(availableSize.Width) ? 0 : availableSize.Width) / width));

            if (col == (containers?.Length ?? -1))
                return false;

            if (containers != null)
                foreach (var item in containers.SelectMany(x => x.Children))
                    item.__HasInserted = false;

            containers = new ItemLinearColumnContainer[col];

            for (int i = 0; i < containers.Length; i++)
                containers[i] = new ItemLinearColumnContainer(width);

            return true;
        }

        private void CheckItemSourceChanged()
        {
            foreach (var newAppended in ItemSource.Reverse().TakeWhile(x => !x.__HasInserted).Reverse())
            {
                //todo 处理新增的
                newAppended.__HasInserted = false;
                var minOne = containers.OrderBy(x => x.MaxOffsetY).FirstOrDefault();
                if (minOne != null)
                {
                    minOne?.Append(newAppended);
                    newAppended.__HasInserted = true;
                }
            }

            foreach (var newPrepended in ItemSource.TakeWhile(x => !x.__HasInserted))
            {
                newPrepended.__HasInserted = false;
                //todo 处理前面加的
                var maxOne = containers.OrderByDescending(x => x.MinOffsetY).FirstOrDefault();
                if (maxOne != null)
                {
                    maxOne?.Prepend(newPrepended);
                    newPrepended.__HasInserted = true;
                }
            }

            var i = -1;
            var itor = ItemSource.GetEnumerator();

            do
            {
                i++;
                if (!itor.MoveNext())
                    break;
                itor.Current.__ItemIndex = i;
            } while (true);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var isRebuild = CheckAndReBuildContainers(availableSize);

            UIElementCollection children = this.InternalChildren;
            IItemContainerGenerator generator = this.ItemContainerGenerator;

            CheckItemSourceChanged();
            var minOffsetY = containers.OrderBy(x => x.MinOffsetY).FirstOrDefault()?.MinOffsetY ?? 0;
            var maxOffsetY = containers.OrderByDescending(x => x.MaxOffsetY).FirstOrDefault()?.MaxOffsetY ?? 0;
            var colCalcResult = containers.Select((x, i) => (GetVisibleItemParams(ref availableSize, x, minOffsetY, maxOffsetY), x, i)).ToArray();

            var visibleParams = colCalcResult
                .SelectMany(x => x.Item1.paramList.Select(y => (y, x.x, x.i)))
                .OrderBy(x => x.y.__ItemIndex)
                .ToArray();

            var prevItemIndex = int.MinValue;
            IDisposable currentGeneratorStatus = default;

            RemoveInternalChildRange(0, children.Count);

            elementMap.Clear();

            foreach ((var param, var col, var i) in visibleParams)
            {
                if (param.__ItemIndex - prevItemIndex != 1)
                {
                    //not next. (re)generate UIElement generator.
                    currentGeneratorStatus?.Dispose();
                    var startPosi = generator.GeneratorPositionFromIndex(param.__ItemIndex);
                    currentGeneratorStatus = generator.StartAt(startPosi, GeneratorDirection.Forward, true);
                }

                var childElement = generator.GenerateNext(out var newlyRealized) as UIElement;
                elementMap[childElement] = (param, i);

                AddInternalChild(childElement);

                if (newlyRealized)
                {
                    generator.PrepareItemContainer(childElement);
                }

                childElement.Measure(availableSize);
                prevItemIndex = param.__ItemIndex;
            }

            currentGeneratorStatus?.Dispose();

            UpdateScrollInfo(availableSize);//availableSize更新后，更新滚动条
            Debug.WriteLine($"called MeasureOverride() {InternalChildren.Count} {availableSize}");

            var height = maxOffsetY - minOffsetY;
            var size = availableSize;
            size.Height = double.IsInfinity(availableSize.Height) ? height : availableSize.Height;

            return size;
        }

        private void UpdateScrollInfo(Size availableSize)
        {
            var extent = CalculateExtent(availableSize);
            if (extent != this.extent)
            {
                this.extent = extent;
                ScrollOwner?.InvalidateScrollInfo();
            }
            if (availableSize != viewPort)
            {
                viewPort = availableSize;
                ScrollOwner?.InvalidateScrollInfo();
            }
        }

        private Size CalculateExtent(Size availableSize)
        {
            return new(availableSize.Width, containers.OrderByDescending(x => x.DesiredHeight).FirstOrDefault()?.DesiredHeight ?? 0);
        }

        private (List<IVirtualGridFlowPanelItemParam> paramList, double calcColMinY, double calcColMaxY) GetVisibleItemParams(ref Size availableSize, ItemLinearColumnContainer col, double minOffsetY, double maxOffsetY)
        {
            var paramList = new List<IVirtualGridFlowPanelItemParam>();
            var calcColMinY = double.MaxValue;
            var calcColMaxY = double.MinValue;
            var width = GridItemWidth;

            //build view bound.
            var bound = new Rect(0, VerticalOffset, int.MaxValue, availableSize.Height);
            var y = 0d;

            for (int i = 0; i < col.Children.Count; i++)
            {
                var param = col.Children[i];
                var bottom = col.MinOffsetY - minOffsetY + y;
                var height = width / param.AspectRatio;

                var paramRect = new Rect(0, bottom, width, height);
                if (bound.IntersectsWith(paramRect))
                {
                    calcColMinY = Math.Min(calcColMinY, y);
                    paramList.Add(param);
                    calcColMaxY = Math.Max(calcColMaxY, y + height);
                }
                y += height;
            }

            return (paramList, calcColMinY, calcColMaxY);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            //获取最低的偏移量，用到后面的全局y轴计算。
            var minBaseOffsetY = containers.OrderBy(x => x.MinOffsetY).FirstOrDefault()?.MinOffsetY ?? 0;
            var width = GridItemWidth;
            Debug.WriteLine($"called ArrangeOverride() minBaseOffsetY:{minBaseOffsetY}");

            foreach (UIElement uiElement in InternalChildren)
            {
                if (!(elementMap.TryGetValue(uiElement, out var zz) && zz is (var param, var colIndex) && containers.ElementAtOrDefault(colIndex) is ItemLinearColumnContainer col))
                    continue;

                //计算x
                var x = colIndex * width;

                //step 1:计算出此param在此col的相对位置,得到y1
                var y1 = col.MinOffsetY + col.Children.TakeWhile(x => x != param).Select(x => width / x.AspectRatio).Aggregate(0d, (a, b) => a + b);

                //step 2:将y1从[col.MinOffsetY,col.MaxOffsetY]范围偏移到以0作minBaseOffsetY的y轴值,得到y2
                var y2 = y1 - minBaseOffsetY;

                //step 2:将y2添加滚动条的偏差值,得到y3
                var y3 = y2 - VerticalOffset;

                var height = width / param.AspectRatio;
                uiElement.Arrange(new Rect(x, y3, width, height));
            }

            Debug.WriteLine($"called ArrangeOverride() done");
            UpdateScrollInfo(finalSize);
            return finalSize;
        }
    }
}
