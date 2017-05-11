using System.Collections;
using BattleServer.Utils;

namespace AStar_2D
{
    /// <summary>
    /// The interface that must be implemented by the game in order for pathfinding functionality to be available.
    /// </summary>
    public interface IPathNode
    {
        // Properties
        /// <summary>
        /// Can the node be traversed.
        /// </summary>
        bool IsWalkable { get; }

        /// <summary>
        /// A normalized weighting value used to give specific paths higher costs, making them less likley to be selected.
        /// </summary>
        float Weighting { get; }

        /// <summary>
        /// The position of the node in the world. Used by agents for path traversal.
        /// </summary>
        Vector2 WorldPosition { get; }
    }

    internal interface IPathNodeIndex
    {
        // Properties
        Index Index { get; }

        int NodeIndex { get; set; }
    }
}
