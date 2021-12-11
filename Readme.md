## VirtualizingStaggeredPanel
---

### 简介
此项目提供一个[StaggeredPanel](https://docs.microsoft.com/en-us/windows/communitytoolkit/controls/staggeredpanel)的虚拟化实现. 此面板类继承VirtualizingPanel以及实现IScrollInfo接口。

### 功能和实现状态(没提就是还没测试)
* 用在ItemControls.ItemsPanel - 支持
* ItemControls滚动 - 支持(但因为VirtualizingStaggeredPanel实现了IScrollInfo,因此ItemsControls需要[改改](https://github.com/MikiraSora/VirtualizingStaggeredPanel/blob/master/VirtualizingStaggeredPanel.WPFTest/MainWindow.xaml#L37),且ItemControls不能被ScrollViewer包围)
* ItemControls实时添加Item到列表尾部 - 支持
* ItemControls实时添加Item到列表头部 - 支持(但可能会有显示问题)
* ItemControls实时删除Item - 支持(但可能会有显示问题)
* Panel滚动到指定的Item上 - 支持([示例](https://github.com/MikiraSora/VirtualizingStaggeredPanel/blob/master/VirtualizingStaggeredPanel.WPFTest/MainWindow.xaml.cs#L107))
* 单独使用VirtualizingStaggeredPanel - 暂时不支持
* VirtualizingStaggeredPanel实时改变面板宽度/高度 - 支持(但可能会有显示问题)
* **ItemControls清除全部Items - 支持(但需要调用ForceRefreshContainItems()刷新布局和滚动条)**
* **ItemControls清除部分Items - 不支持(后面可能实现)**

### 食用方式
[![](https://img.shields.io/badge/nuget-VirtualizingStaggeredPanel%20-blue)](https://www.nuget.org/packages/VirtualizingStaggeredPanel)<br>
对应的ItemsControl的ItemSource的元素类，必须是实现了的IVirtualGridFlowPanelItemParam([示例](https://github.com/MikiraSora/VirtualizingStaggeredPanel/blob/master/VirtualizingStaggeredPanel.WPFTest/VirtualGridFlowPanelItemParam.cs)).
<br>
[Example](https://github.com/MikiraSora/VirtualizingStaggeredPanel/blob/master/VirtualizingStaggeredPanel.WPFTest/MainWindow.xaml)<br>
|属性名|默认值|描述|
|--|--|--|
|GridItemWidth|150|表示每一列的固定宽度,像素为单位|
|ScrollOffset|10|表示滑动条一次滚动的位移量,像素为单位|


### Other
根据不愿透露姓名的火瑶瑶大佬的钦点，因为本项目存在很大的局限性,本人整这个项目是因为本人其他项目需求，其他人其他项目想应用本项目前请确保以下几点:
* 你的需求是否**必须需要StaggeredPanel的虚拟化实现**?
* 你的需求能包容本项目的**缺点和限制**吗?
