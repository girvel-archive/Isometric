using System;
using System.Collections.Generic;
using System.Linq;

namespace GameBasics.Structures
{
    [Serializable]
    public class GraphNode<T>
    {
        public T Value { get; set; }
        public Graph<T> ParentGraph { get; set; }
        public List<GraphNode<T>> Children { get; set; }



		/// <summary>
		/// Serialization ctor. Don't use it in code!
		/// </summary>
		public GraphNode() {}

		public GraphNode(T value, Graph<T> parentGraph)
        {
            Value = value;
            ParentGraph = parentGraph;
            Children = new List<GraphNode<T>>();
        } 



        public void Add(GraphNode<T> child)
        {
            if (!ParentGraph.Nodes.Contains(child))
            {
                ParentGraph.Nodes.Add(child);
            }
            Children.Add(child);
        }



        public bool IsParentOf(T item)
        {
            return Children.Any(c => c.Value.Equals(item));
        }
    }
}