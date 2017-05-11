using System.Collections;
using System.Collections.Generic;
using BattleServer.Utils;

namespace AStar_2D.Pathfinding
{
    /// <summary>
    /// Internal class used to store more detailed information about a specified <see cref="IPathNode"/> instance.
    /// The class acts as a wrapper and is passed to the active <see cref="AStar_2D.Pathfinding.Algorithm.HeuristicProvider"/> in order to calculate the heuristic and adjacent distances.
    /// </summary>
    public sealed class PathNode : IComparer<PathNode>, IPathNode, IPathNodeIndex
    {
        // Private
        private Index index = Index.zero;
        private IPathNode node = null;
        private int nodeIndex = 0;

        // Public
        /// <summary>
        /// The h value for this node.
        /// </summary>
        public float h = 0;
        /// <summary>
        /// The g value for this node.
        /// </summary>
        public float g = 0;
        /// <summary>
        /// The f value for this node.
        /// </summary>
        public float f = 0;

        // Properties
        /// <summary>
        /// The <see cref="Index"/> that this path node is located at.
        /// </summary>
        public Index Index
        {
            get { return index; }
        }

        /// <summary>
        /// Sorting value used by the algorithm to determine how viable a node is.
        /// </summary>
        public int NodeIndex
        {
            get { return nodeIndex; }
            set { nodeIndex = value; }
        }

        /// <summary>
        /// Is the node walkable.
        /// </summary>
        public bool IsWalkable
        {
            get { return node.IsWalkable; }
        }

        /// <summary>
        /// The weighting value for the node.
        /// </summary>
        public float Weighting
        {
            get { return node.Weighting; }
        }

        /// <summary>
        /// The position in 3D space of the node.
        /// </summary>
        public Vector2 WorldPosition
        {
            get { return node.WorldPosition; }
        }

        // Constructor
        internal PathNode(Index index, IPathNode node)
        {
            this.index = index;
            this.node = node;
        }        

        // Methods
        /// <summary>
        /// Implementation of IComparer interface.
        /// </summary>
        /// <param name="a">The first <see cref="PathNode"/></param>
        /// <param name="b">The second <see cref="PathNode"/></param>
        /// <returns>-1 if a is better than b. 1 if b is better than a. 0 if both are equal</returns>
        public int Compare(PathNode a, PathNode b)
        {
            if(a.f < b.f)
            {
                // A is smaller
                return -1;
            }
            else if(a.f > b.f)
            {
                // A is larger
                return 1;
            }

            return 0;
        }
    }
}
