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
* 单独使用VirtualizingStaggeredPanel - 暂时不支持
* VirtualizingStaggeredPanel实时改变面板宽度/高度 - 支持(但可能会有显示问题)

### 食用方式
[Example](https://github.com/MikiraSora/VirtualizingStaggeredPanel/blob/master/VirtualizingStaggeredPanel.WPFTest/MainWindow.xaml)<br>
|属性名|默认值|描述|
|--|--|--|
|GridItemWidth|150|表示每一列的固定宽度,像素为单位|
|ScrollOffset|10|表示滑动条一次滚动的位移量,像素为单位|