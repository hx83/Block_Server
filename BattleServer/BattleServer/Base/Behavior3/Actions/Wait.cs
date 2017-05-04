/**
 * Wait
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
using System.Diagnostics;
namespace Behavior3CSharp.Actions
{

    /**
     * Wait a few seconds.
     *
     * @class Wait
     * @extends Action
    **/

    public class Wait : Action
    {
        /**
         * Node name. Default to `Wait`.
         *
         * @property name
         * @type {String}
         * @readonly
        **/
        public const string name = "Wait";

        /**
         * Node title. Default to `Wait XXms`. Used in Editor.
         *
         * @property title
         * @type {String}
         * @readonly
        **/
        public const string title = "Wait <milliseconds>ms";

        private double _endTime;

        private double _startTime;

        private const string millisecondsKey = "milliseconds";

        /**
         * Initialization method.
         *
         * Settings parameters:
         *
         * - **milliseconds** (*Integer*) Maximum time, in milliseconds, a child
         *                                can execute.
         *
         * @method initialize
         * @param {Object} settings Object with parameters.
         * @constructor
        **/
        public Wait(B3Settings settings)
            : base(settings)
        {
            this._endTime = float.Parse(_properties[millisecondsKey]);
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
            double currTime = LocalTime.NowMilliseconds;

            if (currTime - _startTime > this._endTime)
            {
                return B3Status.SUCCESS;
            }

            return B3Status.RUNNING;
        }


    }
}