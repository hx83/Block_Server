using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("Assembly-CSharp-Editor")]

namespace AStar_2D
{
    /// <summary>
    /// Maintains a collection of all existing instances of the <see cref="AStarGrid"/> class.
    /// This class will be expanded in the future.
    /// </summary>
    public static class AStarGridManager
    {
        // Private
        private static List<AStarAbstractGrid> activeGrids = new List<AStarAbstractGrid>();

        // Properties
        /// <summary>
        /// Attempts to access the default grid, typically the grid that is created first.
        /// </summary>
        public static AStarAbstractGrid DefaultGrid
        {
            get { return (activeGrids.Count > 0) ? activeGrids[0] : null; }
        }

        // Methods
        internal static void registerGrid(AStarAbstractGrid grid)
        {
            // Add the grid to the active grids
            activeGrids.Add(grid);
        }

        internal static void unregisterGrid(AStarAbstractGrid grid)
        {
            // Remove the grid from the list
            activeGrids.Remove(grid);
        }
    }
}
