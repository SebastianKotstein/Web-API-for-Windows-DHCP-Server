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
using Dhcp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace skotstein.net.dhcp.webapi.Model
{
    public class Server : Resource
    {
        private string _name;
        private string _ipAddress;
        private string _version;
        private IList<DhcpOption> _options = new List<DhcpOption>();

        [JsonProperty("name")]
        public string Name { get => _name; set => _name = value; }

        [JsonProperty("ipAddress")]
        public string IpAddress { get => _ipAddress; set => _ipAddress = value; }

        [JsonProperty("version")]
        public string Version { get => _version; set => _version = value; }

        [JsonProperty("options")]
        public IList<DhcpOption> Options { get => _options; set => _options = value; }

        public Server()
        {

        }

        public Server(DhcpServer dhcpServer)
        {
            this.Name = dhcpServer.Name;
            this.IpAddress = dhcpServer.IpAddress.ToString();
            switch (dhcpServer.Version)
            {
                case DhcpServerVersions.Windows2000:
                    this.Version = "Windows 2000";
                    break;
                case DhcpServerVersions.Windows2003:
                    this.Version = "Windows 2003 / 2003 R2";
                    break;
                case DhcpServerVersions.Windows2008:
                    this.Version = "Windows 2008";
                    break;
                case DhcpServerVersions.Windows2008R2:
                    this.Version = "Windows 2008 R2";
                    break;
                case DhcpServerVersions.Windows2012:
                    this.Version = "Windows 2012";
                    break;
                case DhcpServerVersions.Windows2012R2:
                    this.Version = "Windows 2012 R2";
                    break;
                case DhcpServerVersions.WindowsNT310:
                    this.Version = "Windows NT 3.1";
                    break;
                case DhcpServerVersions.WindowsNT350:
                    this.Version = "Windows NT 3.5";
                    break;
                case DhcpServerVersions.WindowsNT351:
                    this.Version = "Windows NT 3.51";
                    break;
                case DhcpServerVersions.WindowsNT40:
                    this.Version = "Windows NT 4.0";
                    break;
                default:
                    this.Version = "Windows DHCP Server";
                    break;
            }
            foreach (DhcpServerOptionValue dhcpServerOptionValue in dhcpServer.AllGlobalOptionValues)
            {
                DhcpOption option = new DhcpOption();
                option.OptionId = dhcpServerOptionValue.OptionId;
                option.Label = dhcpServerOptionValue.Option.Name;
                foreach (DhcpServerOptionElement dhcpServerOptionElement in dhcpServerOptionValue.Values)
                {
                    option.Values.Add(dhcpServerOptionElement.ValueFormatted);
                }
                this.Options.Add(option);
            }
        }

        public bool IsMatchingFilter(string name, string ip, string version)
        {
            return IsEntityPropertyContainingQueryParameterValue(this.Name, name)
                && IsEntityPropertyContainingQueryParameterValue(this.IpAddress, ip)
                && IsEntityPropertyContainingQueryParameterValue(this.Version, version);
        }
    }
}
