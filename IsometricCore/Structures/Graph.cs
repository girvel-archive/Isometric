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
            set {
                if (!_nodes.Contains(value))
                {
                    _nodes.Add(value);
                }
                _root = value;
            }
        }

        public GraphNode<T>[] NodesCopy => _nodes.ToArray();

        private List<GraphNode<T>> _nodes = new List<GraphNode<T>>();
        private GraphNode<T> _root;


        
        /// <summary>
        /// Serialization ctor. Don't use it in code!
        /// </summary>
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

        /// <summary>
        /// Tries to add node to node list;
        /// </summary>
        /// <returns><c>true</c>, if add node was tryed, <c>false</c> if list already contains it.</returns>
        /// <param name="item">new item</param>
        public bool TryAddNode(GraphNode<T> item)
        {
            if (!_nodes.Contains(item))
            {
                _nodes.Add(item);
                return true;
            }

            return false;
        }

        public bool TryRemoveNode(GraphNode<T> node)
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