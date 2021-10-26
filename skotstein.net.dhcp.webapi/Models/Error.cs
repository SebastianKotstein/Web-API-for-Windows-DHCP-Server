// MIT License
//
// Copyright (c) 2021 Sebastian Kotstein
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE
using Newtonsoft.Json;
using skotstein.net.dhcp.webapi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace skotstein.net.dhcp.webapi.Models
{
    public class Error : Resource
    {
        private int _code;
        private string _message;
        private DateTime _time;

        [JsonProperty("code")]
        public int Code { get => _code; set => _code = value; }

        [JsonProperty("message")]
        public string Message { get => _message; set => _message = value; }

        [JsonProperty("time")]
        public DateTime Time { get => _time; set => _time = value; }
    }
}