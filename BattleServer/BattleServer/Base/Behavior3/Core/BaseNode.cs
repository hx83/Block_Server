/**
 * BaseNode
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
     * The BaseNode class is used as super class to all nodes in BehaviorJS. It 
     * comprises all common variables and methods that a node must have to execute.
     *
     * **IMPORTANT:** Do not inherit from this class, use `b3.Composite`, 
     * `b3.Decorator`, `b3.Action` or `b3.Condition`, instead.
     *
     * The attributes are specially designed to serialization of the node in a JSON
     * format. In special, the `parameters` attribute can be set into the visual 
     * editor (thus, in the JSON file), and it will be used as parameter on the 
     * node initialization at `BehaviorTree.load`.
     * 
     * BaseNode also provide 5 callback methods, which the node implementations can
     * override. They are `enter`, `open`, `tick`, `close` and `exit`. See their 
     * documentation to know more. These callbacks are called inside the `_execute`
     * method, which is called in the tree traversal.
     * 
     * @class BaseNode
    **/
    public class BaseNode
    {

        /**
         * Node ID.
         *
         * @property id
         * @type {String}
         * @readonly
        **/
        protected string _id;
        public string Id
        {
            get { return _id; }
        }

        /**
         * Node category. Must be `b3.COMPOSITE`, `b3.DECORATOR`, `b3.ACTION` or 
         * `b3.CONDITION`. This is defined automatically be inheriting the 
         * correspondent class.
         *
         * @property category
         * @type constant
         * @readonly
        **/
        protected string _category;
        public string Category
        {
            get { return _category; }
        }
        /**
         * Node title.
         *
         * @property title
         * @type {String}
         * @optional
         * @readonly
        **/
        protected string _title;
        public string Title
        {
            get { return _title; }
        }
        /**
         * Node description.
         *
         * @property description
         * @type {String}
         * @optional
         * @readonly
        **/
        protected string _description;
        public string Description
        {
            get { return _description; }
        }

        /**
         * A dictionary (key, value) describing the node properties. Useful for 
         * defining custom variables inside the visual editor.
         *
         * @property properties
         * @type {Object}
         * @readonly
        **/
        protected Dictionary<string, string> _properties;

        protected bool _isOpen;
        public bool IsOpen
        {
            get { return _isOpen; }
        }

        protected List<string> _childKeys;

        public List<string> ChildKeys
        {
            get { return _childKeys; }
        }

        protected B3Settings _settings;



        /**
         * Initialization method.
         *
         * @method initialize
         * @constructor
        **/
        public BaseNode(B3Settings settings)
        {
            _settings = settings;
            this._id = settings.Id;
            this._description = settings.Description;
            this._title = settings.Title;
            this._properties = settings.Properties;
            this._childKeys = settings.ChildKeys;
        }

        /**
         * This is the main method to propagate the tick signal to this node. This 
         * method calls all callbacks: `enter`, `open`, `tick`, `close`, and 
         * `exit`. It only opens a node if it is not already open. In the same 
         * way, this method only close a node if the node  returned a status 
         * different of `b3.RUNNING`.
         *
         * @method _execute
         * @param {Tick} tick A tick instance.
         * @returns {Constant} The tick state.
         * @protected
        **/
        public B3Status Execute(Tick tick)
        {
            /* ENTER */
            this.Enter(tick);

            /* OPEN */
            if (!_isOpen)
            {
                this.Open(tick);
            }

            /* TICK */
            B3Status status = this.Tick(tick);

            /* CLOSE */
            if (status != B3Status.RUNNING)
            {
                this.Close(tick);
            }

            /* EXIT */
            this.Exit(tick);

            return status;
        }

        /**
         * Wrapper for enter method.
         *
         * @method _enter
         * @param {Tick} tick A tick instance.
         * @protected
        **/
        public void Enter(Tick tick)
        {
            tick.EnterNode(this);
            this.OnEnter(tick);
        }

        /**
         * Wrapper for open method.
         *
         * @method _open
         * @param {Tick} tick A tick instance.
         * @protected
        **/
        public void Open(Tick tick)
        {
            tick.OpenNode(this);
            _isOpen = true;
            this.OnOpen(tick);
        }

        /**
         * Wrapper for tick method.
         *
         * @method _tick
         * @param {Tick} tick A tick instance.
         * @returns {Constant} A state constant.
         * @protected
        **/
        public B3Status Tick(Tick tick)
        {
            tick.TickNode(this);
            return this.OnTick(tick);
        }

        /**
         * Wrapper for close method.
         *
         * @method _close
         * @param {Tick} tick A tick instance.
         * @protected
        **/
        public void Close(Tick tick)
        {
            tick.CloseNode(this);
            _isOpen = false;
            this.OnClose(tick);
        }

        /**
         * Wrapper for exit method.
         *
         * @method _exit
         * @param {Tick} tick A tick instance.
         * @protected
        **/
        public void Exit(Tick tick)
        {
            tick.ExitNode(this);
            this.OnExit(tick);
        }

        /**
         * Enter method, override this to use. It is called every time a node is 
         * asked to execute, before the tick itself.
         *
         * @method enter
         * @param {Tick} tick A tick instance.
        **/
        protected virtual void OnEnter(Tick tick)
        {

        }

        /**
         * Open method, override this to use. It is called only before the tick 
         * callback and only if the not isn't closed.
         *
         * Note: a node will be closed if it returned `b3.RUNNING` in the tick.
         *
         * @method open
         * @param {Tick} tick A tick instance.
        **/
        protected virtual void OnOpen(Tick tick)
        {

        }

        /**
         * Tick method, override this to use. This method must contain the real 
         * execution of node (perform a task, call children, etc.). It is called
         * every time a node is asked to execute.
         *
         * @method tick
         * @param {Tick} tick A tick instance.
        **/
        protected virtual B3Status OnTick(Tick tick)
        {
            return B3Status.SUCCESS;
        }

        /**
         * Close method, override this to use. This method is called after the tick
         * callback, and only if the tick return a state different from 
         * `b3.RUNNING`.
         *
         * @method close
         * @param {Tick} tick A tick instance.
        **/
        protected virtual void OnClose(Tick tick)
        {

        }

        /**
         * Exit method, override this to use. Called every time in the end of the 
         * execution.
         *
         * @method exit
         * @param {Tick} tick A tick instance.
        **/
        protected virtual void OnExit(Tick tick)
        {

        }
    }
}