using System.Collections;
using System.Collections.Generic;
using System;

namespace AStar_2D.Threading
{
    internal sealed class ThreadManager : IEnumerable<WorkerThread>
    {
        // Private
        private static readonly float threadSpawnThreshold = 0.6f;
        private static readonly int minWorkerThreads = 1;
        private static ThreadManager manager = null;
        private List<WorkerThread> threads = new List<WorkerThread>();

        // Public
        public static readonly int maxAllowedWorkerThreads = 3;
        public int maxWorkerThreads = 3;
        public int maxIdleFrames = 240;

        // Properties
        public static ThreadManager Active
        {
            get
            {
                // Launch the manager
                launchIfRequired();

                return manager;
            }
        }

        public int ActiveThreads
        {
            get { return threads.Count; }
        }

        // Methods
        public static void launchIfRequired()
        {
            // CHeck for valid manager
            if (manager != null)
                return;
            // Store a reference
            manager = new ThreadManager();
        }

        public void Update()
        {
            // Make sure there is always atleast 1 thread
            if(maxWorkerThreads <= 0)
                maxWorkerThreads = minWorkerThreads;

            // Process messages for this frame
            foreach (WorkerThread thread in threads)
                thread.processMessageQueue();

            terminateIdleWorkers();
        }

        public void OnDestroy()
        {
            // Process each thread
            foreach(WorkerThread thread in threads)
            {
                // Dispatch each message immediatley
                while (thread.IsMessageQueueEmpty == false)
                    thread.processMessageQueue();

                // Terminate the thread
                thread.endOrAbort();
            }

            // Clear the list
            threads.Clear();

            // Reset the reference
            manager = null;
        }

        public void asyncRequest(AsyncPathRequest request)
        {
            // Get the worker
            WorkerThread thread = findSuitableWorker();

            // Push the request
            thread.asyncRequest(request);
        }

        public bool hasThread(WorkerThread thread)
        {
            return threads.Contains(thread);
        }

        public int getThreadID(WorkerThread thread)
        {
            return threads.IndexOf(thread);
        }

        private WorkerThread findSuitableWorker()
        {
            // Make sure there is a worker to handle the request
            if (threads.Count == 0)
                return spawnThread();

            // Try to find a suitable thread
            WorkerThread candidate = threads[0];
            float best = 1;

            foreach(WorkerThread thread in threads)
            {
                if(thread.ThreadLoad < best)
                {
                    candidate = thread;
                    best = thread.ThreadLoad;
                }
            }

            // Check for no candidate
            if (best >= threadSpawnThreshold)
            {
                // Check if we can spawn a new thread
                if (threads.Count < maxWorkerThreads)
                {
                    // Create a new worker for the request
                    return spawnThread();
                }
            }

            return candidate;
        }

        private WorkerThread spawnThread()
        {
            // Create a new worker
            WorkerThread thread = new WorkerThread(threads.Count);

            // Register
            threads.Add(thread);

            // Begin
            thread.launch();

            return thread;
        }

        private void terminateIdleWorkers()
        {
            int totalThreads = threads.Count;

            // We cant termainte the remaining threads
            if (totalThreads <= minWorkerThreads)
                return;

            // Process the list of threads
            for(int i = 0; i < threads.Count; i++)
            {
                // Check if a thread is idleing
                if(threads[i].ThreadLoad == 0)
                {
                    if (threads[i].IdleFrames > maxIdleFrames)
                    {
                        // Triger routine
                        threadTerminateRoutine(threads[i]);

                        // Remove from list
                        threads.RemoveAt(i);
                    }
                }
            }
        }

        private IEnumerator threadTerminateRoutine(WorkerThread thread)
        {
            // Make sure all messages are dispatched before killing the thread
            while(thread.IsMessageQueueEmpty == false)
            {
                // Process thr threads messages
                thread.processMessageQueue();

                // Wait for next frame
                yield return null;
            }

            // We can now kill the thread
            thread.endOrAbort();
        }

        public IEnumerator<WorkerThread> GetEnumerator()
        {
            return threads.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return threads.GetEnumerator();
        }
    }
}
