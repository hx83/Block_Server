/**
 * MaxTime
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

using Base.Utils;
// namespace:
using Behavior3CSharp.Core;
namespace Behavior3CSharp.Decorators
{

    /**
     * The MaxTime decorator limits the maximum time the node child can execute. 
     * Notice that it does not interrupt the execution itself (i.e., the child must
     * be non-preemptive), it only interrupts the node after a `RUNNING` status.
     *
     * @class MaxTime
     * @extends Decorator
    **/
    public class MaxTime : Decorator
    {
        /**
         * Node name. Default to `MaxTime`.
         *
         * @property name
         * @type {String}
         * @readonly
        **/
        public const string name = "MaxTime";

        /**
         * Node title. Default to `Max XXms`. Used in Editor.
         *
         * @property title
         * @type {String}
         * @readonly
        **/
        public readonly string title = "Max <maxTime>ms";

        protected double _maxTime = 0;

        protected double _startTime = 0;

        private const string maxTimeKey = "maxTime";

        /**
         * Initialization method.
         *
         * Settings parameters:
         *
         * - **maxTime** (*Integer*) Maximum time a child can execute.
         * - **child** (*BaseNode*) The child node.
         *
         * @method initialize
         * @param {Object} settings Object with parameters.
         * @constructor
        **/
        public MaxTime(B3Settings settings)
            : base(settings)
        {
            this._maxTime = float.Parse(_properties[maxTimeKey]);
        }

        /**
         * Open method.
         *
         * @method open
         * @param {Tick} tick A tick instance.
        **/
        protected override void OnOpen(Tick tick)
        {
            _startTime = LocalTime.NowMilliseconds;
        }

        /**
         * Tick method.
         *
         * @method tick
         * @param {Tick} tick A tick instance.
         * @returns {Constant} A state constant.
        **/
        protected override B3Status OnTick(Tick tick)
        {
            if (this._child == null)
            {
                return B3Status.ERROR;
            }

            double currTime = LocalTime.NowMilliseconds;

            B3Status status = this._child.Execute(tick);
            if (currTime - _startTime > this._maxTime)
            {
                return B3Status.FAILURE;
            }

            return status;
        }
    }
}
