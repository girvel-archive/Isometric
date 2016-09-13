using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace IsometricCore.Structures
{
    [Serializable]
    public class Graph<T> : IEnumerable<GraphNode<T>>
    {
        public bool CheckIdentity { get; }

        public GraphNode<T> Root {
            get { return _root; }
            set
            {
                if (_root == value)
                    return;

                if (_root != null)
                {
                    _nodes.Remove(_root);
                }

                if (value != null)
                {
                    _nodes.Add(value);
                }

                _root = value;
            }
        }

        public GraphNode<T>[] NodesCopy => _nodes.ToArray();

        private readonly List<GraphNode<T>> _nodes = new List<GraphNode<T>>();
        private GraphNode<T> _root;


        
        [Obsolete("using serialization ctor", true)]
        public Graph() {}

        public Graph(bool checkIdentity)
        {
            CheckIdentity = checkIdentity;
        }



        public IEnumerator<GraphNode<T>> GetEnumerator()
        {
            return _nodes.GetEnumerator();
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

        public GraphNode<T> SetRoot(T nodeElement)
        {
            return Root = new GraphNode<T>(nodeElement, this);
        }
        
        internal bool TryAddNode(GraphNode<T> item)
        {
            if (!_nodes.Contains(item))
            {
                _nodes.Add(item);
                return true;
            }

            return false;
        }

        internal bool TryRemoveNode(GraphNode<T> node)
        {
            if (_nodes.Contains(node))
            {
                _nodes.Remove(node);
                return true;
            }

            return false;
        }
    }
}