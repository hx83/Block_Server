using System.Collections;

using AStar_2D.Pathfinding;
using AStar_2D.Pathfinding.Algorithm;
using BattleServer.Utils;
using System;

namespace AStar_2D
{
    /// <summary>
    /// Represents an AStar search space.
    /// The base class for all component grids.
    /// </summary>
    public abstract class AStarAbstractGrid
    {
        // Public
        /// <summary>
        /// When enabled, pathfinding requests may be handled by a background worker thread to remove load on the main thread.
        /// </summary>
        public bool allowThreading = true;
        /// <summary>
        /// When enabled, diagonal paths may be considered during the pathfinding operation.
        /// </summary>
        public DiagonalMode diagonalMovement = DiagonalMode.Diagonal;
        /// <summary>
        /// The spaicing between the nodes. 
        /// </summary>
        public int nodeSpacing = 1;

        // Properties
        /// <summary>
        /// Access a node at the specified index.
        /// </summary>
        /// <param name="x">The X component of the index</param>
        /// <param name="y">The Y component fo the index</param>
        /// <returns>The path node at the specified index or null if the grid has not been correctly initialized</returns>
        public abstract IPathNode this[int x, int y] { get; }

        /// <summary>
        /// The heuristic method that is used by the pathfinding algorithm.
        /// The default heuiristic is Euclidean.
        /// </summary>
        public abstract HeuristicProvider Provider { set; }

        /// <summary>
        /// The current width of the search space or 0 if it has not been correctly initialized.
        /// </summary>
        public abstract int Width { get; }

        /// <summary>
        /// The current height of the search space or 0 if it has not been correctly initialized.
        /// </summary>
        public abstract int Height { get; }

        /// <summary>
        /// Can the grid accept pathfinding requests.
        /// </summary>
        public abstract bool IsReady { get; }


        // Methods
        public abstract void constructGrid(IPathNode[,] grid);

        /// <summary>
        /// Attempts to find the corrosponding <see cref="Index"/> from a position in 3D space.
        /// This method is very expensive and performs a distance check for every node in the seach space. 
        /// This method will always return the <see cref="Index"/> that is the shortest distance from the specified position even if this position is well outside the bounds of the grid.
        /// </summary>
        /// <param name="worldPosition">The position in 3D space to try and find an index for</param>
        /// <returns>The closest <see cref="Index"/> to the specified world position</returns>
        public abstract Index findNearestIndex(Vector2 worldPosition);

        /// <summary>
        /// Attempts to search this grid for a path between the start and end points.
        /// </summary>
        /// <param name="start">The <see cref="Index"/> into the search space representing the start position</param>
        /// <param name="end">The <see cref="Index"/> into the search space representing the end position</param>
        /// <param name="callback">The <see cref="PathRequestDelegate"/> method to call when the algorithm has completed</param>
        public abstract void findPath(Index start, Index end, Action<Path, PathRequestStatus> callback);

        /// <summary>
        /// Attempts to search this grid for a path between the start and end points.
        /// </summary>
        /// <param name="start">The <see cref="Index"/> into the search space representing the start position</param>
        /// <param name="end">The <see cref="Index"/> into the search space representing the end position</param>
        /// <param name="diagonal">The diagonal mode used when finding paths</param>
        /// <param name="callback">The <see cref="PathRequestDelegate"/> method to call whe the algorithm has completed</param>
        public abstract void findPath(Index start, Index end, DiagonalMode diagonal, Action<Path, PathRequestStatus> callback);

        /// <summary>
        /// Attempts to find a <see cref="Path"/> between the start and end points and returns the result on completion.
        /// </summary>
        /// <param name="start">The <see cref="Index"/> into the search space representing the start position</param>
        /// <param name="end">The <see cref="Index"/> into the search space representing the end position</param>
        /// <returns>The <see cref="Path"/> that was found or null if the algorithm failed</returns>
        public abstract Path findPathImmediate(Index start, Index end);

        /// <summary>
        /// Attempts to find a <see cref="Path"/> between the start and end points and returns the result on completion.
        /// </summary>
        /// <param name="start">The <see cref="Index"/> into the search space representing the start position</param>
        /// <param name="end">The <see cref="Index"/> into the search space representing the end position</param>
        /// <param name="diagonal">The diagonal mode used when finding paths</param>
        /// <returns>The <see cref="Path"/> that was found or null if the algorithm failed</returns>
        public abstract Path findPathImmediate(Index start, Index end, DiagonalMode diagonal);

        /// <summary>
        /// Attempts to find a <see cref="Path"/> between the start and end points and returns the result on completion.
        /// </summary>
        /// <param name="start">The <see cref="Index"/> into the search space representing the start position</param>
        /// <param name="end">The <see cref="Index"/> into the search space representing the end position</param>
        /// <param name="status">The <see cref="PathRequestStatus"/> describing the state of the result</param>
        /// <param name="diagonal">The diagonal mode used when finding paths</param>
        /// <returns>The <see cref="Path"/> that was found or null if the algorithm failed</returns>
        public abstract Path findPathImmediate(Index start, Index end, out PathRequestStatus status, DiagonalMode diagonal);
    }
}
