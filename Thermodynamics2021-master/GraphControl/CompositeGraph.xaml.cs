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
using DongUtility;
using GraphData;

namespace GraphControl
{
    /// <summary>
    /// Interaction logic for CompositeGraph.xaml
    /// </summary>
    public partial class CompositeGraph : UserControl, IUpdating
    {
        public List<IUpdating> graphs = new List<IUpdating>();

        public CompositeGraph()
        {
            InitializeComponent();
        }

        public void AddObject(UserControl control)
        {
            AddToGraphList(control);
        }

        public IUpdating GetGraph(int index)
        {
            return graphs[index];
        }

        public int GetNGraphs()
        {
            return graphs.Count;
        }

        public void AddGraph(UserControl graph)
        {
            AddToGraphList(graph);
        }

       
        private void AddToGraphList(UserControl control)
        {
            GraphPanel.RowDefinitions.Add(new RowDefinition());
            GraphPanel.Children.Add(control);
            Grid.SetRow(control, GraphPanel.RowDefinitions.Count - 1);

            if (control is IUpdating)
            {
                graphs.Add((IUpdating)control);
            }
        }

        public void Update(GraphDataPacket data)
        {
            foreach (var graph in graphs)
            {
                graph.Update(data);
            }
        }

        public void Clear()
        {
            graphs.Clear();
            GraphPanel.Children.Clear();
            GraphPanel.RowDefinitions.Clear();
        }
    }
}
