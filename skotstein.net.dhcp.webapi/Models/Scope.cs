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
using System.Net;
using System.Threading.Tasks;

namespace skotstein.net.dhcp.webapi.Model
{
    public class Scope : Resource 
    {
        private string _name;
        private string _ipAddress;
        private string _subnetMask;
        private string _comment;
        private string _scopeState;
        private long _leaseDuration;
        private IpRange _ipRange = null;
        private IList<IpRange> _excludedIpRanges = new List<IpRange>();
        private IList<DhcpOption> _options = new List<DhcpOption>();

        [JsonProperty("name")]
        public string Name { get => _name; set => _name = value; }

        [JsonProperty("ipAddress")]
        public string IpAddress { get => _ipAddress; set => _ipAddress = value; }

        [JsonProperty("subnetMask")]
        public string SubnetMask { get => _subnetMask; set => _subnetMask = value; }

        [JsonProperty("leaseDurationInSeconds")]
        public long LeaseDuration { get => _leaseDuration; set => _leaseDuration = value; }

        [JsonProperty("comment")]
        public string Comment { get => _comment; set => _comment = value; }

        [JsonProperty("state")]
        public string ScopeState { get => _scopeState; set => _scopeState = value; }

        [JsonProperty("ipRange")]
        public IpRange IpRange { get => _ipRange; set => _ipRange = value; }
        [JsonProperty("excludedIpRanges")]
        public IList<IpRange> ExcludedIpRanges { get => _excludedIpRanges; set => _excludedIpRanges = value; }

        [JsonProperty("options")]
        public IList<DhcpOption> Options { get => _options; set => _options = value; }

        public Scope()
        {

        }

        public Scope(DhcpServerScope dhcpServerScope)
        {
            this.Comment = dhcpServerScope.Comment;
            this.IpAddress = dhcpServerScope.Address.ToString();
            this.SubnetMask = dhcpServerScope.Mask.ToString();
            this.Name = dhcpServerScope.Name;
            this.LeaseDuration = (long)dhcpServerScope.LeaseDuration.TotalSeconds;

            switch (dhcpServerScope.State)
            {
                case DhcpServerScopeState.Disabled:
                    this.ScopeState = "disabled";
                    break;
                case DhcpServerScopeState.DisabledSwitched:
                    this.ScopeState = "disabledSwitched";
                    break;
                case DhcpServerScopeState.Enabled:
                    this.ScopeState = "enabled";
                    break;
                case DhcpServerScopeState.EnabledSwitched:
                    this.ScopeState = "enabledSwitched";
                    break;
                case DhcpServerScopeState.InvalidState:
                    this.ScopeState = "invalidState";
                    break;
                default:
                    this.ScopeState = "unknown";
                    break;

            }

            if (dhcpServerScope.IpRange != null)
            {
                this.IpRange = new IpRange();
                this.IpRange.StartIpAddress = dhcpServerScope.IpRange.StartAddress.ToString();
                this.IpRange.EndIpAddress = dhcpServerScope.IpRange.EndAddress.ToString();
            }

            if (dhcpServerScope.ExcludedIpRanges != null)
            {
                foreach (DhcpServerIpRange dhcpServerIpRange in dhcpServerScope.ExcludedIpRanges)
                {
                    IpRange ipRange = new IpRange();
                    ipRange.StartIpAddress = dhcpServerIpRange.StartAddress.ToString();
                    ipRange.EndIpAddress = dhcpServerIpRange.EndAddress.ToString();
                    this.ExcludedIpRanges.Add(ipRange);
                }
            }
            foreach (DhcpServerOptionValue dhcpServerOptionValue in dhcpServerScope.AllOptionValues)
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

        public bool IsMatchingFilter(string name, string ip, string state)
        {
            return IsEntityPropertyContainingQueryParameterValue(this.Name, name)
                && IsEntityPropertyContainingQueryParameterValue(this.IpAddress, ip)
                && IsEntityPropertyEqualQueryParameterValue(this.ScopeState, state);
        }
    }
}
