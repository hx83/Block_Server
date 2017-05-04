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

using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
namespace Behavior3CSharp
{
    public class B3Settings
    {
        private string _id;
        private string _name;
        private string _title;
        private string _description;
        private List<string> _children;
        private Dictionary<string, string> _properties;

        public B3Settings()
        {
            _id = B3.CreateUUID();
            _children = new List<string>();
            _properties = new Dictionary<string, string>();

        }

        public string Id
        {
            get
            {
                return _id;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public string Title
        {
            get
            {
                return _title;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
        }

        public List<string> ChildKeys
        {
            get
            {
                return _children;
            }
        }

        public Dictionary<string, string> Properties
        {
            get
            {
                return _properties;
            }
        }

        public void Parse(JsonData data)
        {
            _id = (string)data["id"];
            _name = (string)data["name"];
            _title = (string)data["title"];
            _description = (string)data["description"];
            if (data.Keys.Contains("children"))
            {
                int count = data["children"].Count;
                for (int i = 0; i < count; i++)
                {
                    _children.Add((string)data["children"][i]);
                }
            }
            foreach (string propKey in data["properties"].Keys)
            {
                JsonData propertyData = data["properties"];
                _properties.Add(propKey, propertyData[propKey].ToString());
            }
        }
    }
}