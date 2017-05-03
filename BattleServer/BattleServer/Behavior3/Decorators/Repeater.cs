/**
 * Repeater
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
 * Repeater is a decorator that repeats the tick signal until the child node 
 * return `RUNNING` or `ERROR`. Optionally, a maximum number of repetitions 
 * can be defined.
 *
 * @class Repeater
 * @extends Decorator
**/
    public class Repeater : Decorator
    {
        /**
         * Node name. Default to `Repeater`.
         *
         * @property name
         * @type {String}
         * @readonly
        **/
        public const string name = "Repeater";

        /**
         * Node title. Default to `Repeat XXx`. Used in Editor.
         *
         * @property title
         * @type {String}
         * @readonly
        **/
        public readonly string title = "Repeat <maxLoop>x";

        protected int _maxLoop = -1;
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
         * - **maxLoop** (*Integer*) Maximum number of repetitions. Default to -1 
         *                           (infinite).
         * - **child** (*BaseNode*) The child node.
         * 
         * @method initialize
         * @param {Object} settings Object with parameters.
         * @constructor
        **/
        public Repeater(B3Settings settings)
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
        **/
        protected override B3Status OnTick(Tick tick)
        {
            if (this._child == null)
            {
                return B3Status.ERROR;
            }

            B3Status status = B3Status.SUCCESS;

            while (this._maxLoop < 0 || _loopTimes < this._maxLoop)
            {
                status = this._child.Execute(tick);

                if (status == B3Status.SUCCESS || status == B3Status.FAILURE)
                    _loopTimes++;
                else
                    break;
            }

            return status;
        }
    }
}