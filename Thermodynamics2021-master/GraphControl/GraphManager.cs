using GraphData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using static WPFUtility.UtilityFunctions;

namespace GraphControl
{
    public class GraphManager
    {
        private IGraphDataInterface dataManager;
        private CompositeGraph graphs;

        public GraphManager(IGraphDataInterface dataManager, CompositeGraph graphs)
        {
            this.dataManager = dataManager;
            this.graphs = graphs;
        }

        public void Initialize()
        {
            var prototypes = dataManager.Graphs;
            foreach (var prototype in prototypes)
            {
                var control = GetGraphFromPrototype(prototype);
                graphs.AddGraph(control);
            }
        }

        private UserControl GetGraphFromPrototype(IGraphPrototype prototype)
        {
            switch (prototype.GetGraphType())
            {
                case IGraphPrototype.GraphType.Graph:
                    var protoGraph = prototype as GraphPrototype;
                    var gu = new GraphUnderlying(protoGraph.XAxisTitle, protoGraph.YAxisTitle);
                    foreach (var timeline in protoGraph.Timelines)
                    {
                        var newTimeline = new Timeline(timeline.Name, ConvertColor(timeline.Color));
                        gu.AddTimeline(newTimeline);
                    }
                    return new Graph(gu);

                case IGraphPrototype.GraphType.Histogram:
                    var protoHist = prototype as HistogramPrototype;
                    var hist = new Histogram(protoHist.NBins, ConvertColor(protoHist.Color),
                        protoHist.XAxisTitle);
                    return new Graph(hist);

                case IGraphPrototype.GraphType.Text:
                    var protoText = prototype as TextPrototype;
                    var text = new UpdatingText { 
                        Title = protoText.Title, 
                        Color = ConvertColor(protoText.Color)
                    };
                    return text;

                case IGraphPrototype.GraphType.LeaderBoard:
                    var protoBoard = prototype as LeaderBoardPrototype;
                    var leaderBoard = new LeaderBoardControl();
                    foreach (var leaderBar in protoBoard.Prototypes)
                    {
                        leaderBoard.AddEntry(leaderBar.Name, ConvertColor(leaderBar.Color));
                    }
                    return leaderBoard;

                default:
                    throw new NotImplementedException("Should never reach here");
            }
        }

        public void AddData()
        {
            graphs.Update(dataManager.GetData());
        }
    }
}
