   M S B A M L   `   `   `  ���� M  IMotionVisualizer3D, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:MotionVisualizer3DMotionVisualizer3D  G CGraphControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA0clr-namespace:GraphControl;assembly=GraphControlGraphControl L HVisualizerControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullP:clr-namespace:VisualizerControl;assembly=VisualizerControlVisualizerControl K GMotionVisualizer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM8clr-namespace:MotionVisualizer;assembly=MotionVisualizerMotionVisualizer 2    ,MotionVisualizer3D.MotionVisualizer3DControl   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     2local clr-namespace:MotionVisualizer3D     Igraphcontrol0clr-namespace:GraphControl;assembly=GraphControl     Xvisualizercontrol:clr-namespace:VisualizerControl;assembly=VisualizerControl     Umotionvisualizer8clr-namespace:MotionVisualizer;assembly=MotionVisualizer       � Title$  
MainWindow��$	��350��$	��525��
���   � � WindowState$ 	Maximized=�.��� 
������� � RowDefinitions �� $��46 ��� $��* �.+��� -   $��	ButtonBar��$��Center=� �� Orientation$ 
Horizontal=�
�������$��0��.+��� -   $��Start_Button��$��10q�$��Start���� $��Time Increment��$��10q�$��Center=�$��Right=��� -   $��TimeIncrementSlider��$��50��$��
5 10 10 10q�$	��.01���� $��
Time Scale��$��Center=��� -   $��TimeScaleSlider��$��50��$��10q�$��1���� $��Time: ��$��Center=��� -   $��	TimeValue��$��0��$��40��$��Center=�$
��5 10q��� $��Auto Camera��$��Center=�$��Right=��� -   $��AutoCameraCheck��$��Center=�$��
5 10 20 10q� d� 	IsChecked$ FalseY��� $��Display��$��Center=�$��Right=��� -   $��DisplayCheck��$��Center=�$��
5 10 20 10q�$
 TrueY��� $��	Slow Draw��$��Center=�$��Right=��� -   $��SlowDrawCheck��$��Center=�$��
5 10 20 10q��� -	   $��5q�.��Save�� -
   $��5q�.��
Screenshot�� -   $��5q�.��Record�� -   $��VisualizerSpot��$��1��� -   $��ViewportGrid��$��1�� � ColumnDefinitions �� $��7* ��� $��3* �.+��� -   $��Viewport��$��0���� -   $��	GraphSpot��
�������$��0 3 3 3q�$��1��
�������