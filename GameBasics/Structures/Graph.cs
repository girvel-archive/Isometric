using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GameBasics.Structures
{
    [Serializable]
    public class Graph<T> : IEnumerable<GraphNode<T>>
    {
        // TODO CheckIdentity : bool /// does graph can contain one object twice

        public GraphNode<T> Root {
            get { return _root; }
            set {
                if (!Nodes.Contains(value))
                {
                    Nodes.Add(value);
                }
                _root = value;
            }
        }

        public List<GraphNode<T>> Nodes = new List<GraphNode<T>>();
        private GraphNode<T> _root;


        public IEnumerator<GraphNode<T>> GetEnumerator()
        {
            return Nodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }



        public GraphNode<T>[] Find(T item)
        {
            return
                (from node in this
                where node.Value.Equals(item)
                select node).ToArray();
        }

        public GraphNode<T>[] Find(Predicate<T> predicate)
        {
            return
                (from node in this
                    where predicate(node.Value)
                    select node).ToArray();
        } 
    }
}