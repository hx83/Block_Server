using System.Collections;

using AStar_2D.Pathfinding;
using AStar_2D.Pathfinding.Algorithm;
using AStar_2D.Threading;
using BattleServer.Utils;
using System;
using System.Diagnostics;

namespace AStar_2D
{
    // Enum
    /// <summary>
    /// Status value used to deternime the outcome of a pathfinding request. 
    /// </summary>
    public enum PathRequestStatus
    {
        /// <summary>
        /// The provided <see cref="Index"/> referenced a node that was outside the bounds of the search grid.
        /// </summary>
        InvalidIndex = 0,
        /// <summary>
        /// The provided start and end <see cref="Index"/> represent the same node in the search space. No path will be generated.
        /// </summary>
        SameStartEnd,
        /// <summary>
        /// The search grid has not been correctly initialized and is unable to handle requests.
        /// </summary>
        GridNotReady,
        /// <summary>
        /// A path to the destination could not be found. The path must be blocked by unwalkable nodes.
        /// </summary>
        PathNotFound,
        /// <summary>
        /// A path to the destination was found.
        /// </summary>
        PathFound,
    }

    /// <summary>
    /// Represents an AStar search space.
    /// This component must be attached to a game object and can be inherited if requried. See <see cref="AStar_2D.Demo.TileManager"/> for an example.
    /// This component makes use of the default <see cref="SearchGrid"/> implementation. In order to make use of a custom impelemtation take a look at the generic <see cref="AStarGrid{T}"/> and the demo <see cref="AStar_2D.Demo.SelectiveSearchGrid"/> scripts.
    /// The <see cref="SearchGrid"/> class can be used directly if you dont need grid to be a component.
    /// </summary>
    public class AStarGrid : AStarGrid<SearchGrid>
    {
        // Empty class
    }

    /// <summary>
    /// Represents an AStar search space.
    /// This component must be attached to a game object and can be inherited if required. See <see cref="AStar_2D.Demo.TileManager"/> for an example.
    /// The component accepts generic arguments of the <see cref="SearchGrid"/> class and allows custom <see cref="SearchGrid"/> derived types to be used.
    /// For more information take a look at the <see cref="AStar_2D.Demo.SelectiveSearchGrid"/> class.
    /// <typeparam name="T">The generic type to use for the underlying algorithm implementation. The type must inherit from <see cref="SearchGrid"/></typeparam>
    /// </summary>
    public class AStarGrid<T> : AStarAbstractGrid where T : SearchGrid, new()
    {
        // Private
        private T searchGrid = new T();
        private bool isReady = false;       

        // Properties
        /// <summary>
        /// Access a node at the specified index.
        /// </summary>
        /// <param name="x">The X component of the index</param>
        /// <param name="y">The Y component fo the index</param>
        /// <returns>The path node at the specified index or null if the grid has not been correctly initialized</returns>
        public override IPathNode this[int x, int y]
        {
            get { return (verifyReady() == false) ? null : searchGrid[x, y]; }
        }

        /// <summary>
        /// The heuristic method that is used by the pathfinding algorithm.
        /// The default heuiristic is Euclidean.
        /// </summary>
        public override HeuristicProvider Provider
        {
            set { if (verifyReady() == true) searchGrid.provider = value; }
        }

        /// <summary>
        /// The current width of the search space or 0 if it has not been correctly initialized.
        /// </summary>
        public override int Width
        {
            get { return (verifyReady() == false) ? 0 : searchGrid.Width; }
        }

        /// <summary>
        /// The current height of the search space or 0 if it has not been correctly initialized.
        /// </summary>
        public override int Height
        {
            get { return (verifyReady() == false) ? 0 : searchGrid.Height; }
        }

        /// <summary>
        /// Can the grid accept pathfinding requests.
        /// </summary>
        public override bool IsReady
        {
            get { return isReady; }
        }

        // Methods
        /// <summary>
        /// Called by Unity.
        /// Can be overriden but be sure to call base.Awake() to ensure that the grid is correctly initialized.
        /// </summary>
        public virtual void Awake()
        {
            // Register this grid with the grid manager
            AStarGridManager.registerGrid(this);
        }

        /// <summary>
        /// Called by Unity.
        /// Can be overriden but be sure to call base.OnDestroy() to ensure that the grid is successfully destroyed.
        /// </summary>
        public virtual void OnDestroy()
        {
            // Unregister this grid as it is about to be destroyed
            AStarGridManager.unregisterGrid(this);
        }

        /// <summary>
        /// The main method that is used to initialize the grid with user data.
        /// The provided data must be a two dimensional array of class instances that implement the <see cref="IPathNode"/> interface.
        /// The array must be correctly initialized with no null elements.
        /// The array cannot have any dimensions of zero length.
        /// </summary>
        /// <param name="grid">The user input grid</param>
        public override void constructGrid(IPathNode[,] grid)
        {
            // Initialize the search grid
            searchGrid.constructGrid(grid);
            isReady = true;
        }

        /// <summary>
        /// Attempts to find the corrosponding <see cref="Index"/> from a position in 3D space.
        /// This method is very expensive and performs a distance check for every node in the seach space. 
        /// This method will always return the <see cref="Index"/> that is the shortest distance from the specified position even if this position is well outside the bounds of the grid.
        /// </summary>
        /// <param name="worldPosition">The position in 3D space to try and find an index for</param>
        /// <returns>The closest <see cref="Index"/> to the specified world position</returns>
        public override Index findNearestIndex(Vector2 worldPosition)
        {
            // Make sure the grid is ready
            if (verifyReady() == false) return null;

            // Pass the call through to the raw grid
            return searchGrid.findNearestIndex(worldPosition);
        }

        public static int findNodeDistance(Index start, Index end, DiagonalMode diagonal)
        {
            // Include start node
            int count = 1;

            // Change in x 
            int deltaX = Math.Abs(start.X - end.X);

            // Change in y
            int deltaY = Math.Abs(start.Y - end.Y);

            if (diagonal == DiagonalMode.NoDiagonal)
            {
                // Add the x and y offset
                count += (deltaX + deltaY);
            }
            else
            {
                // Get the smallest delta
                int smallest = deltaX;

                // Check if y is smaller
                if (deltaY < smallest)
                    smallest = deltaY;

                // Get the number of diagonal steps
                int diagonalSteps = Math.Abs(deltaX - deltaY);

                // Add the diagonal offset
                count += (diagonalSteps + smallest);
            }

            return count;
        }


        /// <summary>
        /// Attempts to search this grid for a path between the start and end points.
        /// </summary>
        /// <param name="start">The <see cref="Index"/> into the search space representing the start position</param>
        /// <param name="end">The <see cref="Index"/> into the search space representing the end position</param>
        /// <param name="callback">The <see cref="PathRequestDelegate"/> method to call when the algorithm has completed</param>
        public override void findPath(Index start, Index end, Action<Path, PathRequestStatus> callback)
        {
            // Call through
            findPath(start, end, diagonalMovement, callback);
        }


        /// <summary>
        /// Attempts to search this grid for a path between the start and end points.
        /// </summary>
        /// <param name="start">The <see cref="Index"/> into the search space representing the start position</param>
        /// <param name="end">The <see cref="Index"/> into the search space representing the end position</param>
        /// <param name="diagonal">The diagonal mode used when finding paths</param>
        /// <param name="callback">The <see cref="PathRequestDelegate"/> method to call whe the algorithm has completed</param>
        public override void findPath(Index start, Index end, DiagonalMode diagonal, Action<Path, PathRequestStatus> callback)
        {
            // Make sure the grid is ready
            if (verifyReady() == false)
            {
                callback(null, PathRequestStatus.GridNotReady);
                return;
            }

            // Check if threading is enabled
            if(allowThreading == true)
            {
                // Create a request
                AsyncPathRequest request = new AsyncPathRequest(searchGrid, start, end, diagonal, (Path path, PathRequestStatus status) =>
                {
                    // Invoke callback
                    callback(path, status);
                });

                // Dispatch the request
                ThreadManager.Active.asyncRequest(request);
            }
            else
            {
                PathRequestStatus status;

                // Run the task immediatley
                Path result = findPathImmediate(start, end, out status, diagonal);

                // Trigger callback
                callback(result, status);
            }
        }

        /// <summary>
        /// Attempts to find a <see cref="Path"/> between the start and end points and returns the result on completion.
        /// </summary>
        /// <param name="start">The <see cref="Index"/> into the search space representing the start position</param>
        /// <param name="end">The <see cref="Index"/> into the search space representing the end position</param>
        /// <returns>The <see cref="Path"/> that was found or null if the algorithm failed</returns>
        public override Path findPathImmediate(Index start, Index end)
        {
            // Call through
            return findPathImmediate(start, end, diagonalMovement);
        }

        /// <summary>
        /// Attempts to find a <see cref="Path"/> between the start and end points and returns the result on completion.
        /// </summary>
        /// <param name="start">The <see cref="Index"/> into the search space representing the start position</param>
        /// <param name="end">The <see cref="Index"/> into the search space representing the end position</param>
        /// <param name="diagonal">The diagonal mode used when finding paths</param>
        /// <returns>The <see cref="Path"/> that was found or null if the algorithm failed</returns>
        public override Path findPathImmediate(Index start, Index end, DiagonalMode diagonal)
        {
            PathRequestStatus status = PathRequestStatus.InvalidIndex;

            // Call through
            return findPathImmediate(start, end, out status, diagonal);
        }

        /// <summary>
        /// Attempts to find a <see cref="Path"/> between the start and end points and returns the result on completion.
        /// </summary>
        /// <param name="start">The <see cref="Index"/> into the search space representing the start position</param>
        /// <param name="end">The <see cref="Index"/> into the search space representing the end position</param>
        /// <param name="status">The <see cref="PathRequestStatus"/> describing the state of the result</param>
        /// <param name="diagonal">The diagonal mode used when finding a path</param>
        /// <returns>The <see cref="Path"/> that was found or null if the algorithm failed</returns>
        public override Path findPathImmediate(Index start, Index end, out PathRequestStatus status, DiagonalMode diagonal)
        {
            // Make sure the grid is ready
            if (verifyReady() == false)
            {
                status = PathRequestStatus.GridNotReady;
                return null;
            }

            // Store a temp path
            Path path = null;
            PathRequestStatus temp = PathRequestStatus.InvalidIndex;

            // Find a path
            searchGrid.findPath(start, end, diagonal, (Path result, PathRequestStatus resultStatus) =>
            {
                // Store the status
                temp = resultStatus;

                // Make sure the path was found
                if (resultStatus == PathRequestStatus.PathFound)
                {
                    path = result;
                }
            });

            status = temp;
            return path;
        }

        private bool verifyReady()
        {
            // Make sure we can accept requests
            if (isReady == false)
            {

                Debug.WriteLine(string.Format("AStar Grid '{0}' is not ready to receive path requests, Make sure you construct the grid before hand"));
                return false;
            }

            return true;
        }
    }
}
