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
		/// Serialization ctor.
		/// </summary>
        [Obsolete("Using serialization ctor", true)]
		public GraphNode() {}

		public GraphNode(T value, Graph<T> parentGraph)
        {
            Value = value;
            ParentGraph = parentGraph;
            Children = new List<GraphNode<T>>();
        } 



        public void Add(GraphNode<T> child)
        {
            #if !DEBUG
            ParentGraph.TryAddNode(child);
            #else
            if (!ParentGraph.TryAddNode(child)
                && Children.Contains(child) 
                && ParentGraph.CheckIdentity)
            {
                throw new ArgumentException("Node already has this child");
            }
            #endif

            Children.Add(child);
        }



        public bool IsParentOf(T item)
        {
            return Children.Any(c => c.Value.Equals(item));
        }
    }
}