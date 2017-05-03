namespace Behavior3CSharp
{
    public enum B3Status
    {
        /**
         * Returned when a criterion has been met by a condition node or an action node
         * has been completed successfully.
         * 
         * @property SUCCESS
         * @type {Integer}
         */
        SUCCESS,

        /**
         * Returned when a criterion has not been met by a condition node or an action 
         * node could not finish its execution for any reason.
         * 
         * @property FAILURE
         * @type {Integer}
         */
        FAILURE,

        /**
         * Returned when an action node has been initialized but is still waiting the 
         * its resolution.
         * 
         * @property FAILURE
         * @type {Integer}
         */
        RUNNING,

        /**
         * Returned when some unexpected error happened in the tree, probably by a 
         * programming error (trying to verify an undefined variable). Its use depends 
         * on the final implementation of the leaf nodes.
         * 
         * @property ERROR
         * @type {Integer}
         */
        ERROR,
    }
}
