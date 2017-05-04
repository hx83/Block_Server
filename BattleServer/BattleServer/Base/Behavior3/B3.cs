/**
 * b3
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

using System;
namespace Behavior3CSharp
{

    /**
     * Behavior3JS
     * ===========
     *
     * * * *
     * 
     * **Behavior3JS** is a Behavior Tree library written in JavaScript. It 
     * provides structures and algorithms that assist you in the task of creating 
     * intelligent agents for your game or application. Check it out some features 
     * of Behavior3JS:
     * 
     * - Based on the work of (Marzinotto et al., 2014), in which they propose a 
     *   **formal**, **consistent** and **general** definition of Behavior Trees;
     * - **Optimized to control multiple agents**: you can use a single behavior 
     *   tree instance to handle hundreds of agents;
     * - It was **designed to load and save trees in a JSON format**, in order to 
     *   use, edit and test it in multiple environments, tools and languages;
     * - A **cool visual editor** which you can access online;
     * - Several **composite, decorator and action nodes** available within the 
     *   library. You still can define your own nodes, including composites and 
     *   decorators;
     * - **Completely free**, the core module and the visual editor are all published
     *   under the MIT License, which means that you can use them for your open source
     *   and commercial projects;
     * - **Lightweight**, only 11.5KB!
     * 
     * Visit http://behavior3js.guineashots.com to know more!
     *
     * 
     * Core Classes and Functions
     * --------------------------
     * 
     * This library include the following core structures...
     *
     * **Public:**
     * 
     * - **BehaviorTree**: the structure that represents a Behavior Tree;
     * - **Blackboard**: represents a "memory" in an agent and is required to to 
     *   run a `BehaviorTree`;
     * - **Composite**: base class for all composite nodes;
     * - **Decorator**: base class for all decorator nodes;
     * - **Action**: base class for all action nodes;
     * - **Condition**: base class for all condition nodes;
     *
     * **Internal:**
     * 
     * 
     * - **Tick**: used as container and tracking object through the tree during 
     *   the tick signal;
     * - **BaseNode**: the base class that provide all common node features;
     * 
     * *Some classes are used internally on Behavior3JS, but you may need to access
     * its functionalities eventually, specially the `Tick` object.*
     *
     * 
     * Nodes Included 
     * --------------
     *
     * **Composite Nodes**: 
     * 
     * - Sequence;
     * - Priority;
     * - MemSequence;
     * - MemPriority;
     * 
     * 
     * **Decorators**: 
     * 
     * - Inverter;
     * - Limiter
     * - MaxTime;
     * - Repeater;
     * - RepeaterUntilFailure;
     * - RepeaterUntilSuccess;
     *
     * 
     * **Actions**:
     * 
     * - Succeeder;
     * - Failer;
     * - Error;
     * - Runner;
     * - Wait.
     * 
     * @module Behavior3JS
     * @main Behavior3JS
    **/
    public class B3
    {

        /**
         * List of all constants in Behavior3JS.
         *
         * @class Constants
        **/

        /**
         * Version of the library.
         * 
         * @property VERSION
         * @type {String}
         */
        public string VERSION = "0.1.0";

        /**
         * Describes the node category as Composite.
         * 
         * @property COMPOSITE
         * @type {String}
         */
        public const string COMPOSITE = "composite";

        /**
         * Describes the node category as Decorator.
         * 
         * @property DECORATOR
         * @type {String}
         */
        public const string DECORATOR = "decorator";

        /**
         * Describes the node category as Action.
         * 
         * @property ACTION
         * @type {String}
         */
        public const string ACTION = "action";

        /**
         * Describes the node category as Condition.
         * 
         * @property CONDITION
         * @type {String}
         */
        public const string CONDITION = "condition";


        /**
         * List of internal and helper functions in Behavior3JS.
         * 
         * @class Utils
        **/

        public static string CreateUUID()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}