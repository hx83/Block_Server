using System;
using System.Collections;

using AStar_2D.Pathfinding;

namespace AStar_2D.Threading
{
    internal sealed class AsyncPathRequest
    {
        // Private
        private SearchGrid grid = null;
        private Index start = Index.zero;
        private Index end = Index.zero;
        private Action<Path, PathRequestStatus> callback = null;
        private DiagonalMode diagonal = DiagonalMode.Diagonal;
        private long timeStamp = 0;

        // Properties
        public SearchGrid Grid
        {
            get { return grid; }
        }

        public Index Start
        {
            get { return start; }
        }

        public Index End
        {
            get { return end; }
        }

        public DiagonalMode Diagonal
        {
            get { return diagonal; }
        }

        internal Action<Path, PathRequestStatus> Callback
        {
            get { return callback; }
        }

        internal long TimeStamp
        {
            get { return timeStamp; }
        }

        // Constructor
        public AsyncPathRequest(SearchGrid grid, Index start, Index end, Action<Path, PathRequestStatus> callback)
        {
            this.grid = grid;
            this.start = start;
            this.end = end;
            this.callback = callback;

            // Create a time stamp
            timeStamp = DateTime.UtcNow.Ticks;
        }

        public AsyncPathRequest(SearchGrid grid, Index start, Index end, DiagonalMode diagonal, Action<Path, PathRequestStatus> callback)
        {
            this.grid = grid;
            this.start = start;
            this.end = end;
            this.diagonal = diagonal;
            this.callback = callback;

            // Create a time stamp
            timeStamp = DateTime.UtcNow.Ticks;
        }
    }
}
