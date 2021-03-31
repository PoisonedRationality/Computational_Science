using DongUtility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace GraphData
{
    public class GraphDataManager
    {
        private readonly UpdatingFunctions updatingFunctions = new UpdatingFunctions();

        public GraphDataPacket GetData() => updatingFunctions.GetData();

        private List<IGraphPrototype> graphs = new List<IGraphPrototype>();
        public IEnumerable<IGraphPrototype> Graphs => graphs;

        public void WriteGraphHeader(BinaryWriter bw)
        {
            bw.Write(graphs.Count);
            foreach (var graph in graphs)
            {
                var graphType = graph.GetGraphType();
                bw.Write((byte)graphType);
                graph.WriteToFile(bw);
            }
        }

        public void CopyGraphsFrom(GraphDataManager other)
        {
            graphs.AddRange(other.graphs);
            updatingFunctions.AddFunctions(other.updatingFunctions);
        }

        public delegate double BasicFunction();
        public delegate List<double> ListFunction();
        public delegate Vector VectorFunc();
        public struct BasicFunctionPair
        {
            public BasicFunctionPair(BasicFunction xFunc, BasicFunction yFunc)
            {
                XFunc = xFunc;
                YFunc = yFunc;
            }
            public BasicFunction XFunc { get; set; }
            public BasicFunction YFunc { get; set; }
        }

        public void AddSingleGraph(string name, Color color, BasicFunction xFunc, BasicFunction yFunc,
            string xAxis, string yAxis)
        {
            var info = new TimelineInfo(new TimelinePrototype(name, color),
                new BasicFunctionPair(xFunc, yFunc));

            AddGraph(new List<TimelineInfo>() { info }, xAxis, yAxis);
        }

        public void AddHist(int nBins, Color color, ListFunction allDataFunc, string xAxis)
        {
            graphs.Add(new HistogramPrototype(nBins, color, xAxis));

            void function(GraphDataPacket ds)
            {
                ds.AddSet(allDataFunc());
            }

            updatingFunctions.AddFunction(function);
        }

        public delegate string TextFunction();
        public void AddText(string title, Color color, TextFunction textFunc)
        {
            graphs.Add(new TextPrototype(title, color));

            void function(GraphDataPacket ds)
            {
                ds.AddTextData(textFunc());
            }

            updatingFunctions.AddFunction(function);
        }

        public class TimelineInfo
        {
            public TimelinePrototype Timeline { get; set; }
            public BasicFunctionPair Functions { get; set; }
            public TimelineInfo(TimelinePrototype timeline, BasicFunctionPair function)
            {
                Timeline = timeline;
                Functions = function;
            }
        }

        public void AddGraph(IEnumerable<TimelineInfo> timelines, string xAxis, string yAxis)
        {
            var graph = new GraphPrototype(xAxis, yAxis);
            foreach (var timeline in timelines)
            {
                graph.AddTimeline(timeline.Timeline);
            }
            graphs.Add(graph);

            void function(GraphDataPacket ds)
            {
                foreach (var timeline in timelines)
                {
                    ds.AddData(timeline.Functions.XFunc());
                    ds.AddData(timeline.Functions.YFunc());
                }
            }
            updatingFunctions.AddFunction(function);
        }

        public void Add3DGraph(string name, BasicFunction funcX, VectorFunc funcY, string xAxis, string yAxis)
        {
            var xVec = new TimelineInfo(new TimelinePrototype("x " + name, Color.Red),
                new BasicFunctionPair(funcX, () => funcY().X));

            var yVec = new TimelineInfo(new TimelinePrototype("y " + name, Color.Green),
                new BasicFunctionPair(funcX, () => funcY().Y));

            var zVec = new TimelineInfo(new TimelinePrototype("z " + name, Color.Blue),
                new BasicFunctionPair(funcX, () => funcY().Z));

            AddGraph(new List<TimelineInfo> { xVec, yVec, zVec }, xAxis, yAxis);

        }

        public void AddLeaderBoard(IEnumerable<LeaderBarPrototype> leaderBars, ListFunction function)
        {
            graphs.Add(new LeaderBoardPrototype(leaderBars));

            void newFunction(GraphDataPacket ds)
            {
                ds.AddSet(function());
            }

            updatingFunctions.AddFunction(newFunction);
        }
    }
}
