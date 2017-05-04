/**
 * Limiter
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
using Behavior3CSharp.Core;
namespace Behavior3CSharp.Decorators
{

    /**
     * This decorator limit the number of times its child can be called. After a
     * certain number of times, the Limiter decorator returns `FAILURE` without 
     * executing the child.
     *
     * @class Limiter
     * @extends Decorator
    **/
    public class Limiter : Decorator
    {
        /**
         * Node name. Default to `Limiter`.
         *
         * @property name
         * @type {String}
         * @readonly
        **/
        public const string name = "Limiter";

        /**
         * Node title. Default to `Limit X Activations`. Used in Editor.
         *
         * @property title
         * @type {String}
         * @readonly
        **/
        public readonly string title = "Limit <maxLoop> Activations";

        protected int _maxLoop = 1;

        public int MaxLoop
        {
            get { return _maxLoop; }
        }

        private const string maxLoopKey = "maxLoop";

        protected int _loopTimes;

        /**
         * Initialization method. 
         *
         * Settings parameters:
         *
         * - **maxLoop** (*Integer*) Maximum number of repetitions.
         * - **child** (*BaseNode*) The child node.
         *
         * @method initialize
         * @param {Object} settings Object with parameters.
         * @constructor
        **/
        public Limiter(B3Settings settings)
            : base(settings)
        {
            this._maxLoop = int.Parse(_properties[maxLoopKey]);
        }

        /**
         * Open method.
         *
         * @method open
         * @param {Tick} tick A tick instance.
        **/
        protected override void OnOpen(Tick tick)
        {
            _loopTimes = 0;
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

            if (_loopTimes < this._maxLoop)
            {
                B3Status status = this._child.Execute(tick);

                if (status == B3Status.SUCCESS || status == B3Status.FAILURE)
                    _loopTimes++;
                return status;
            }

            return B3Status.FAILURE;
        }
    }
}