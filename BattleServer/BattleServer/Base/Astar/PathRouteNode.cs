using System.Collections;
using System;
using BattleServer.Utils;

namespace AStar_2D
{
    /// <summary>
    /// Represents a single node in a <see cref="Path"/>.
    /// Used by an agent to move towards this nodes position.
    /// </summary>
    public sealed class PathRouteNode : IPathNode
    {
        // Private
        private const float distanceTolerance = 0.1f;

        private IPathNode reference = null;
        private Index index = Index.zero;

        // Properties
        /// <summary>
        /// The <see cref="Index"/> that this node is located at.
        /// </summary>
        public Index Index
        {
            get { return index; }
        }

        /// <summary>
        /// Can this path node be traversed.
        /// </summary>
        public bool IsWalkable
        {
            get { return reference.IsWalkable; }
        }

        /// <summary>
        /// The weighting value for this node.
        /// </summary>
        public float Weighting
        {
            get { return reference.Weighting; }
        }

        /// <summary>
        /// The position in world space of this node.
        /// </summary>
        public Vector2 WorldPosition
        {
            get { return reference.WorldPosition; }
        }

        // Constructor
        /// <summary>
        /// Parameter constructor.
        /// </summary>
        /// <param name="reference">The underlying <see cref="IPathNode"/> that this node is wrapping</param>
        /// <param name="index">The <see cref="Index"/> location of this node</param>
        public PathRouteNode(IPathNode reference, Index index)
        {
            this.reference = reference;
            this.index = index;
        }

        // Methods
        /// <summary>
        /// Checks whether the specified transform is within a predetermined distance of the node.
        /// </summary>
        /// <param name="transform">The transform to check</param>
        /// <returns>True if the specified transform has arrived at this node</returns>
        public bool hasArrived(Vector2 position)
        {
            // Check if we are within tolerance of the last node
            return isWithinDistanceOfNode(reference, position);
        }

        private bool isWithinDistanceOfNode(IPathNode node, Vector2 position)
        {
            // Calculate the distance to the node
            float distance = Vector2.DistanceSquare(position, node.WorldPosition);

            // Check if we are within tolerance
            return distance < distanceTolerance * distanceTolerance;
        }
    }
}
