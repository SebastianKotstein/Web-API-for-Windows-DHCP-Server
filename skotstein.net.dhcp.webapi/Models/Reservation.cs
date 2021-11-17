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
    public class Reservation : Resource
    {
        private string _ipAddress;
        private string _subnetMask;
        private string _macAddress;
        private IList<string> _allowedClientTypes = new List<string>();
        private IList<DhcpOption> _options = new List<DhcpOption>();

        [JsonProperty("ipAddress")]
        public string IpAddress { get => _ipAddress; set => _ipAddress = value; }

        [JsonProperty("subnetMask")]
        public string SubnetMask { get => _subnetMask; set => _subnetMask = value; }

        [JsonProperty("macAddress")]
        public string MacAddress { get => _macAddress; set => _macAddress = value; }

        [JsonProperty("allowedClientTypes")]
        public IList<string> AllowedClientTypes { get => _allowedClientTypes; set => _allowedClientTypes = value; }

        [JsonProperty("options")]
        public IList<DhcpOption> Options { get => _options; set => _options = value; }

        public Reservation()
        {

        }

        public Reservation(DhcpServerScopeReservation dhcpServerScopeReservation)
        {
            this.IpAddress = dhcpServerScopeReservation.IpAddress.ToString();
            this.MacAddress = dhcpServerScopeReservation.HardwareAddress.Replace(":", "");
            this.SubnetMask = dhcpServerScopeReservation.IpAddressMask.ToString();


            if (((int)dhcpServerScopeReservation.AllowedClientTypes & (int)DhcpServerClientTypes.BOOTP) == 2)
            {
                this.AllowedClientTypes.Add("BOOTP");
            }
            if (((int)dhcpServerScopeReservation.AllowedClientTypes & (int)DhcpServerClientTypes.DHCP) == 1)
            {
                this.AllowedClientTypes.Add("DHCP");
            }
            foreach (DhcpServerOptionValue dhcpServerOptionValue in dhcpServerScopeReservation.AllOptionValues)
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

        public bool IsMatchingFilter(string ip, string macAddress)
        {
            if (macAddress != null)
            {
                macAddress = macAddress.Replace(":", "");
            }

            return IsEntityPropertyContainingQueryParameterValue(this.IpAddress, ip)
                && IsEntityPropertyContainingQueryParameterValue(this.MacAddress, macAddress);
        }
    }
}
