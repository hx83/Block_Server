using System;

using AStar_2D.Collections;
using AStar_2D.Pathfinding.Algorithm;
using BattleServer.Utils;

namespace AStar_2D.Pathfinding
{
    public enum DiagonalMode
    {
        /// <summary>
        /// Paths containing diagonal movement will not be considered.
        /// </summary>
        NoDiagonal,
        /// <summary>
        /// Paths containing diagonal movement will be considered.
        /// </summary>
        Diagonal,
        /// <summary>
        /// Paths containing diagonal movement will be considered but situations where corner cutting may occur will default to non-diagonal movement.
        /// </summary>
        DiagonalNoCutting,
    }

    /// <summary>
    /// Represents a grid that can be used for pathfinding.
    /// Use this class if you want to achieve pathfinding without relying on Unity components.
    /// </summary>
    public class SearchGrid
    {
        // Private
        private NodeQueue<PathNode> orderedMap = null;
        private OpenNodeMap<PathNode> closedMap = null;
        private OpenNodeMap<PathNode> openMap = null;
        private OpenNodeMap<PathNode> runtimeMap = null;
        
        private PathNode[,] nodeGrid = null;
        private PathNode[,] searchGrid = null;
        private PathNode[] adjacentNodes = new PathNode[8]; // Allocate once and reuse (8 possible directions)
        private int width = 0;
        private int height = 0;
        private float nodeSpacing = 0.2f;

        // Public
        /// <summary>
        /// The heuristic method to use.
        /// </summary>
        public HeuristicProvider provider = HeuristicProvider.defaultProvider;

        // Properties
        /// <summary>
        /// Attempts to access an element of the grid at the specified index.
        /// </summary>
        /// <param name="x">The X component of the index</param>
        /// <param name="y">The Y component of the index</param>
        /// <returns>The <see cref="IPathNode"/> at the specified index</returns>
        public IPathNode this[int x, int y]
        {
            get { return nodeGrid[x, y]; }
        }

        /// <summary>
        /// The current width of the grid.
        /// </summary>
        public int Width
        {
            get { return width; }
        }

        /// <summary>
        /// The current height of the grid.
        /// </summary>
        public int Height
        {
            get { return height; }
        }

        /// <summary>
        /// The distance between 2 nodes. Settings this value can dramaticaly increase the performance of <see cref="findNearestIndex(Vector3)"/>.
        /// As it will prevent an exhaustive search if the method has already found the best matching node.
        /// Only set this value if the user node grid has equal spacing in both the X and Y axis.
        /// </summary>
        public float NodeSpacing
        {
            get { return nodeSpacing; }
            set { nodeSpacing = value; }
        }

        // Methods
        /// <summary>
        /// Allocates the search area based on an input array.
        /// </summary>
        /// <param name="inputGrid">The input array used for searching</param>
        public void constructGrid(IPathNode[,] inputGrid)
        {
            // Make sur ethe input is acceptable
            validateInputGrid(inputGrid);

            // Get sizes
            width = inputGrid.GetLength(0);
            height = inputGrid.GetLength(1);

            // Cache and allocate
            nodeGrid = new PathNode[width, height];
            searchGrid = new PathNode[width, height];

            closedMap = new OpenNodeMap<PathNode>(width, height);
            openMap = new OpenNodeMap<PathNode>(width, height);
            runtimeMap = new OpenNodeMap<PathNode>(width, height);
            orderedMap = new NodeQueue<PathNode>(new PathNode(Index.zero, null));

            // Create the grid wrapper
            for(int x = 0; x < width; x++)
            {
                for(int y = 0; y < height; y++)
                {
                    // Create a wrapper path node over the interface
                    nodeGrid[x, y] = new PathNode(new Index(x, y), inputGrid[x, y]);
                }
            }
        }

        /// <summary>
        /// Attempts to locate the <see cref="Index"/> that is closest to the specified world position.
        /// This method is very expensive and performs a distance check for every node in the grid.
        /// Should not be used for very large grids.
        /// </summary>
        /// <param name="worldPosition">The input position</param>
        /// <returns>A <see cref="Index"/> that best represents the specified world position</returns>
        public Index findNearestIndex(Vector2 worldPosition)
        {
            Index index = new Index(0, 0);
            float closest = float.MaxValue;
            float sqrSpacing = (float)Math.Pow(nodeSpacing, 2);

            // Process each node
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector2 a = nodeGrid[x, y].WorldPosition;

                    float elementX = (a.X - worldPosition.X);
                    float elementY = (a.Y - worldPosition.Y);

                    // Calculate the square distance
                    float sqrDistance = (elementX * elementX + elementY * elementY);

                    // Check for smaller
                    if(sqrDistance < closest)
                    {
                        index = nodeGrid[x, y].Index;
                        closest = sqrDistance;

                        // Check if we can consider this as the best
                        if (sqrDistance < sqrSpacing)
                            return index;
                    }
                }
            }

            return index;
        }


        /// <summary>
        /// Launches the pathfinding algorithm and attempts to find a <see cref="Path"/> between the start and end <see cref="Index"/>.
        /// </summary>
        /// <param name="start">The start position for the search</param>
        /// <param name="end">The end position for the search</param>
        /// <param name="diagonal">The diagonal mod used when finding paths</param>
        /// <param name="callback">The <see cref="PathRequestDelegate"/> method to call on completion</param>
        public void findPath(Index start, Index end, DiagonalMode diagonal, Action<Path, PathRequestStatus> callback)
        {
            // Already at the destination
            if (start.Equals(end))
            {
                callback(null, PathRequestStatus.SameStartEnd);
                return;
            }

            // Get the nodes
            PathNode startNode = nodeGrid[start.X, start.Y];
            PathNode endNode = nodeGrid[end.X, end.Y];

            // Clear all previous data
            clearSearchData();

            // Starting scores
            startNode.g = 0;
            startNode.h = provider.heuristic(startNode, endNode);
            startNode.f = startNode.h;

            // Add the start node
            openMap.add(startNode);
            runtimeMap.add(startNode);
            orderedMap.push(startNode);
            
            while(openMap.Count > 0)
            {
                // Get the front value
                PathNode value = orderedMap.pop();

                if(value == endNode)
                {
                    // We have found the path
                    Path result = constructPath(searchGrid[endNode.Index.X, endNode.Index.Y]);

                    // Last node
                    result.push(endNode, endNode.Index);

                    // Trigger the delegate with success
                    callback(result, PathRequestStatus.PathFound);

                    // Exit the method
                    return;
                }
                else
                {
                    openMap.remove(value);
                    closedMap.add(value);

                    // Fill our array with surrounding nodes
                    constructAdjacentNodes(value, adjacentNodes, diagonal);

                    // Process each neighbor
                    foreach(PathNode adjacent in adjacentNodes)
                    {
                        bool isBetter = false;

                        // Skip null nodes
                        if (adjacent == null)
                            continue;

                        // Make sure the node is walkable
                        if (adjacent.IsWalkable == false)
                            continue;

                        // Make sure it has not already been excluded
                        if (closedMap.contains(adjacent) == true)
                            continue;

                        // Check for custom exclusion descisions
                        if (validateConnection(value, adjacent) == false)
                            continue;

                        // Calculate the score for the node
                        float score = runtimeMap[value].g + provider.adjacentDistance(value, adjacent);
                        bool added = false;

                        // Make sure it can be added to the open map
                        if(openMap.contains(adjacent) == false)
                        {
                            openMap.add(adjacent);
                            isBetter = true;
                            added = true;
                        }
                        else if(score < runtimeMap[adjacent].g)
                        {
                            // The score is better
                            isBetter = true;
                        }
                        else
                        {
                            // The score is not better
                            isBetter = false;
                        }

                        // CHeck if a better score has been found
                        if(isBetter == true)
                        {
                            // Update the search grid
                            searchGrid[adjacent.Index.X, adjacent.Index.Y] = value;

                            // Add the adjacent node
                            if (runtimeMap.contains(adjacent) == false)
                                runtimeMap.add(adjacent);

                            // Update the score values for the node
                            runtimeMap[adjacent].g = score;
                            runtimeMap[adjacent].h = provider.heuristic(adjacent, endNode);
                            runtimeMap[adjacent].f = runtimeMap[adjacent].g + runtimeMap[adjacent].h;

                            // CHeck if we added to the open map
                            if(added == true)
                            {
                                // Push the adjacent node to the set
                                orderedMap.push(adjacent);
                            }
                            else
                            {
                                // Refresh the set
                                orderedMap.refresh(adjacent);
                            }
                        }
                    }

                }
            } // End while

            // Failure
            callback(null, PathRequestStatus.PathNotFound);
        }

        /// <summary>
        /// When overriden, allows custom checks to be implemented to determine whether neghboring nodes are able to connect to a specific node.
        /// </summary>
        /// <param name="center">The center node that is currently being validated</param>
        /// <param name="neighbor">The neghboring node that is being checked</param>
        /// <returns>Should return true when the connection between the neighbors is allowed and false whent he connection is not allowed</returns>
        public virtual bool validateConnection(PathNode center, PathNode neighbor)
        {
            // Default behaviour - all nodes are valid and can be included
            return true;
        }

        private int constructAdjacentNodes(PathNode center, PathNode[] nodes, DiagonalMode diagonal)
        {
            // Get the center node
            Index node = center.Index;

            // Clear the shared array so that old data is not used
            for (int i = 0; i < nodes.Length; i++)
                nodes[i] = null;

            int index = 0;

            // Check if diagonal movements can be used
            if (diagonal != DiagonalMode.NoDiagonal)
            {
                // Cache the adjacent nodes
                PathNode left = safeGetNode(node.X - 1, node.Y);
                PathNode right = safeGetNode(node.X + 1, node.Y);
                PathNode top = safeGetNode(node.X, node.Y + 1);
                PathNode bottom = safeGetNode(node.X, node.Y - 1);

                bool canAdd = true;

                // Bottom left
                {
                    canAdd = true;

                    if (diagonal == DiagonalMode.DiagonalNoCutting)
                    {
                        // Left cutting
                        if (left != null && left.IsWalkable == false)
                            canAdd = false;

                        // Bottom cutting
                        if (bottom != null && bottom.IsWalkable == false)
                            canAdd = false;
                    }

                    // Make sure the diagonal movement is allowed
                    if (canAdd == true)
                        nodes[index++] = safeGetNode(node.X - 1, node.Y - 1);
                } // End bottom left

                // Top right
                {
                    canAdd = true;

                    if (diagonal == DiagonalMode.DiagonalNoCutting)
                    {
                        // Right cutting
                        if (right != null && right.IsWalkable == false)
                            canAdd = false;

                        // Top cutting
                        if (top != null && top.IsWalkable == false)
                            canAdd = false;
                    }

                    // Make sure the diagonal movement is allowed
                    if (canAdd == true)
                        nodes[index++] = safeGetNode(node.X + 1, node.Y + 1);
                } // End top right

                // Top Left
                {
                    canAdd = true;

                    if (diagonal == DiagonalMode.DiagonalNoCutting)
                    {
                        // Left cutting
                        if (left != null && left.IsWalkable == false)
                            canAdd = false;

                        // Top cutting
                        if (top != null && top.IsWalkable == false)
                            canAdd = false;
                    }

                    // Make sure the diagonal movement is allowed
                    if (canAdd == true)
                        nodes[index++] = safeGetNode(node.X - 1, node.Y + 1);
                } // End top left

                // Bottom right
                {
                    canAdd = true;

                    if (diagonal == DiagonalMode.DiagonalNoCutting)
                    {
                        // Right cutting
                        if (right != null && right.IsWalkable == false)
                            canAdd = false;

                        // Bottom cutting
                        if (bottom != null && bottom.IsWalkable == false)
                            canAdd = false;
                    }

                    // Make sure the diagonal movement is allowed
                    if (canAdd == true)
                        nodes[index++] = safeGetNode(node.X + 1, node.Y - 1);
                } // End bottom right
            }

            // Bottom
            nodes[index++] = safeGetNode(node.X, node.Y - 1);

            // Left
            nodes[index++] = safeGetNode(node.X - 1, node.Y);

            // Right
            nodes[index++] = safeGetNode(node.X + 1, node.Y);

            // Top
            nodes[index++] = safeGetNode(node.X, node.Y + 1);

            return index + 1;
        }

        private Path constructPath(PathNode current)
        {
            // Create the path
            Path path = new Path();

            // Call the dee construct method
            deepConstructPath(current, path);            

            return path;
        }

        private void deepConstructPath(PathNode current, Path output)
        {
            // Get the node from the search grid
            PathNode node = searchGrid[current.Index.X, current.Index.Y];

            // Make sure we have a valid node
            if (node != null)
            {
                // Call through reccursive
                deepConstructPath(node, output);
            }

            // Push the node to the path
            output.push(current, current.Index);
        }

        private void validateInputGrid(IPathNode[,] grid)
        {
            // CHeck for null arrays
            if (grid == null)
                throw new ArgumentException("A search grid cannot be created from a null reference");

            // Check for 0 lenght arrays
            if (grid.GetLength(0) == 0 || grid.GetLength(1) == 0)
                throw new ArgumentException("A search grid cannot be created because one or more dimensions have a length of 0");
        }

        private PathNode safeGetNode(int x, int y)
        {
            // Validate index
            if (x >= 0 && x < width &&
                y >= 0 && y < height)
                return nodeGrid[x, y];

            return null;
        }

        private void clearSearchData()
        {
            // Reset all data
            closedMap.clear();
            openMap.clear();
            runtimeMap.clear();
            orderedMap.clear();

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    searchGrid[x, y] = null;
        }
    }
}