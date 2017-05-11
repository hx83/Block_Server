using System.Text;
using System.Collections;
using System.Collections.Generic;
using System;

using AStar_2D.Pathfinding;
using BattleServer.Utils;

namespace AStar_2D
{
    /// <summary>
    /// Represents a series of nodes that make up a path to the destination.
    /// Provides additional helper methods for path traversal.
    /// </summary>
    public sealed class Path : IEnumerable<PathRouteNode>
    {
        // Private       
        private Queue<PathRouteNode> nodes = new Queue<PathRouteNode>();
        private PathRouteNode lastNode = null;
        private PathRouteNode currentNode = null;

        // Properties
        /// <summary>
        /// Can the path be reached.
        /// </summary>
        public bool IsReachable
        {
            get { return allNodesWalkable(); }
        }

        /// <summary>
        /// Does the path contain any nodes.
        /// </summary>
        public bool IsEmpty
        {
            get { return nodes.Count == 0; }
        }

        /// <summary>
        /// The number of nodes that make up this path.
        /// </summary>
        public int NodeCount
        {
            get { return nodes.Count; }
        }

        /// <summary>
        /// The last node in the path.
        /// </summary>
        public PathRouteNode LastNode
        {
            get { return lastNode; }
        }

        // Constructor
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Path() { }

        // Methods
        internal void push(IPathNode node, Index index)
        {
            // COnstruct the route node
            PathRouteNode route = new PathRouteNode(node, index);

            // Set the start node
            if (IsEmpty == true)
                currentNode = route;

            // Add the node
            lastNode = route;
            nodes.Enqueue(route);
        }

        /// <summary>
        /// Access the next node in the path.
        /// This method will remove the retuend value from its internal data so the result must be managed by the caller.
        /// </summary>
        /// <returns>A <see cref="PathRouteNode"/> representing the next node in the path</returns>
        public PathRouteNode getNextNode()
        {
            // Make sure the path is not empty
            if (IsEmpty == false)
            {
                // Get the next node
                currentNode = nodes.Dequeue();

                // Return the node
                return currentNode;
            }

            // No more nodes available so return the very last node
            return lastNode;
        }

        /// <summary>
        /// Peek at the next node in the path.
        /// This method will not modify the state of the path.
        /// </summary>
        /// <returns>A <see cref="PathRouteNode"/> representing the next node in the path</returns>
        public PathRouteNode peekNextNode()
        {
            // Peek at the first node
            if (IsEmpty == false)
                return nodes.Peek();

            // No more nodes available
            return lastNode;
        }

        /// <summary>
        /// Checks whether the specified transform is within a predetermined distance of the last node.
        /// </summary>
        /// <param name="transform">The transform to check</param>
        /// <returns>True if the specified transform has arrived at the last node</returns>
        public bool hasArrivedAtLast(Vector2 position)
        {
            // CHeck if we are within tolerance of the current node
            return lastNode.hasArrived(position);
        }

        private bool allNodesWalkable()
        {
            // If any path is not walkable then exits early with failure
            foreach(IPathNode node in nodes)
                if (node.IsWalkable == false)
                    return false;
            
            // The path can be walked until the end
            return true;
        }

        /// <summary>
        /// Overriden to string method.
        /// </summary>
        /// <returns>This <see cref="Path"/> as a string representation</returns>
        public override string ToString()
        {
            return ToString(false);
        }

        /// <summary>
        /// Additional to string method.
        /// </summary>
        /// <param name="detailed">Should detailed information for the path be included</param>
        /// <returns>This <see cref="Path"/> as a string representation</returns>
        public string ToString(bool detailed)
        {
            if(detailed == true)
            {
                StringBuilder builder = new StringBuilder();
                
                builder.AppendLine(string.Format("Path: ({0})", nodes.Count));

                foreach (IPathNode node in nodes)
                    builder.AppendLine(string.Format("\tNode: ({0}, {1})", node.WorldPosition.X, node.WorldPosition.Y));

                return builder.ToString(); 
            }
            else
            {
                return string.Format("Path: ({0})", nodes.Count);
            }
        }

        /// <summary>
        /// IEnumerator implementation.
        /// </summary>
        /// <returns>The enumerator for the inner collection</returns>
        public IEnumerator<PathRouteNode> GetEnumerator()
        {
            return nodes.GetEnumerator();
        }

        /// <summary>
        /// IEnumerator implementation.
        /// </summary>
        /// <returns>The enumerator for the inner collection</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return nodes.GetEnumerator();
        }
    }
}
