using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MikiraSora.VirtualizingStaggeredPanel
{
    public interface IVirtualGridFlowPanelItemParam
    {
        /// <summary>
        /// 横宽比,AspectRatio = Width/Height
        /// </summary>
        public double AspectRatio { get; set; }

        /// <summary>
        /// DONT EDIT IT AND IGNORE IT
        /// </summary>
        public bool __HasInserted { get; set; }

        /// <summary>
        /// DONT EDIT IT AND IGNORE IT
        /// </summary>
        public int __ItemIndex { get; set; }
    }
}
