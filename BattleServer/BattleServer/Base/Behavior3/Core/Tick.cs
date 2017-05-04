/**
 * Tick
 *
 * Copyright (c) 2014 Renato de Pontes Pereira.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to 
 * deal in the Software without restriction, including without limitation the 
 * rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
 * sell copies of the Software, and to permit persons to whom the Software is 
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
 * IN THE SOFTWARE.
**/

/**
 * @module Behavior3JS
 **/

// namespace:
using System.Collections.Generic;
namespace Behavior3CSharp.Core
{

    /**
     * A new Tick object is instantiated every tick by BehaviorTree. It is passed 
     * as parameter to the nodes through the tree during the traversal.
     * 
     * The role of the Tick class is to store the instances of tree, debug, target
     * and blackboard. So, all nodes can access these informations.
     * 
     * For internal uses, the Tick also is useful to store the open node after the 
     * tick signal, in order to let `BehaviorTree` to keep track and close them 
     * when necessary.
     *
     * This class also makes a bridge between nodes and the debug, passing the node
     * state to the debug if the last is provided.
     *
     * @class Tick
    **/
    public class Tick
    {
        /**
         * The tree reference.
         * 
         * @property tree
         * @type {b3.BehaviorTree}
         * @readOnly
         */
        private BehaviorTree _tree;
        public BehaviorTree Tree
        {
            get { return _tree; }
            set { _tree = value; }
        }
        /**
         * The debug reference.
         * 
         * @property debug
         * @type {Object}
         * @readOnly
         */
        private object _debug;
        public object Debug
        {
            get { return _debug; }
        }

        /**
         * The target object reference.
         * 
         * @property target
         * @type {Object}
         * @readOnly
         */
        private object _target;
        public object Target
        {
            get { return _target; }
            set { _target = value; }
        }
        /**
         * The list of open nodes. Update during the tree traversal.
         * 
         * @property _openNodes
         * @type {Array}
         * @protected
         * @readOnly
         */
        private List<BaseNode> _openNodes;
        public List<BaseNode> OpenNodes
        {
            get { return _openNodes; }
        }
        /**
         * The number of nodes entered during the tick. Update during the tree 
         * traversal.
         * 
         * @property _nodeCount
         * @type {Integer}
         * @protected
         * @readOnly
         */
        private int _nodeCount;
        public int NodeCount
        {
            get { return _nodeCount; }
        }
        /**
         * Initialization method.
         *
         * @method initialize
         * @constructor
        **/
        public Tick()
        {
            // set by BehaviorTree
            this._tree = null;
            this._debug = null;
            this._target = null;

            // updated during the tick signal
            this._openNodes = new List<BaseNode>();
            this._nodeCount = 0;
        }

        /**
         * Called when entering a node (called by BaseNode).
         *
         * @method _enterNode
         * @param {Object} node The node that called this method.
         * @protected
        **/
        public void EnterNode(BaseNode node)
        {
            //this._nodeCount++;
            //this._openNodes.Add(node);

            // TODO: call debug here
        }

        /**
         * Callback when opening a node (called by BaseNode). 
         *
         * @method _openNode
         * @param {Object} node The node that called this method.
         * @protected
        **/
        public void OpenNode(BaseNode node)
        {
            // TODO: call debug here
        }

        /**
         * Callback when ticking a node (called by BaseNode).
         *
         * @method _tickNode
         * @param {Object} node The node that called this method.
         * @protected
        **/
        public void TickNode(BaseNode node)
        {
            // TODO: call debug here
        }

        /**
         * Callback when closing a node (called by BaseNode).
         *
         * @method _closeNode
         * @param {Object} node The node that called this method.
         * @protected
        **/
        public void CloseNode(BaseNode node)
        {
            // TODO: call debug here
            //this._openNodes.Remove(node);
        }

        /**
         * Callback when exiting a node (called by BaseNode).
         *
         * @method _exitNode
         * @param {Object} node The node that called this method.
         * @protected
        **/
        public void ExitNode(BaseNode node)
        {
            // TODO: call debug here
        }
    }
}