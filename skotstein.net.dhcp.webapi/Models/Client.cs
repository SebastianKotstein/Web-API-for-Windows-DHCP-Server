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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dhcp;
using Newtonsoft.Json;

namespace skotstein.net.dhcp.webapi.Model
{
    public class Client : Resource
    {

        private string _name;
        private string _addressState;
        private string _ipAddress;
        private string _subnetMask;
        private string _macAddress;
        private bool _leaseExpired;
        private DateTime _leaseExpires; 
        private bool _hasReservation;
        private IList<string> _types = new List<string>();

        [JsonProperty("name")]
        public string Name { get => _name; set => _name = value; }

        [JsonProperty("addressState")]
        public string AddressState { get => _addressState; set => _addressState = value; }

        [JsonProperty("ipAddress")]
        public string IpAddress { get => _ipAddress; set => _ipAddress = value; }

        [JsonProperty("subnetMask")]
        public string SubnetMask { get => _subnetMask; set => _subnetMask = value; }

        [JsonProperty("macAddress")]
        public string MacAddress { get => _macAddress; set => _macAddress = value; }

        [JsonProperty("leaseHasExpired")]
        public bool LeaseExpired { get => _leaseExpired; set => _leaseExpired = value; }

        [JsonProperty("leaseExpiryDate")]
        public DateTime LeaseExpires { get => _leaseExpires; set => _leaseExpires = value; }

        [JsonProperty("hasReservation")]
        public bool HasReservation { get => _hasReservation; set => _hasReservation = value; }

        [JsonProperty("types")]
        public IList<string> Types { get => _types; set => _types = value; }

        public Client()
        {

        }

        public Client(DhcpServerClient dhcpServerClient)
        {
            this.IpAddress = dhcpServerClient.IpAddress.ToString();
            this.SubnetMask = dhcpServerClient.SubnetMask.ToString();
            this.MacAddress = dhcpServerClient.HardwareAddress.Replace(":", "");
            this.LeaseExpires = dhcpServerClient.LeaseExpires;
            this.LeaseExpired = dhcpServerClient.LeaseExpired;
            this.Name = dhcpServerClient.Name;
            this.HasReservation = (((int)dhcpServerClient.Type & (int)DhcpServerClientTypes.Reservation) == 4);
            if (((int)dhcpServerClient.Type & (int)DhcpServerClientTypes.BOOTP) == 2)
            {
                this.Types.Add("BOOTP");
            }
            if (((int)dhcpServerClient.Type & (int)DhcpServerClientTypes.DHCP) == 1)
            {
                this.Types.Add("DHCP");
            }
            if (((int)dhcpServerClient.Type & (int)DhcpServerClientTypes.Reservation) == 4)
            {
                this.Types.Add("Reservation");
            }
            switch (dhcpServerClient.AddressState)
            {
                case DhcpServerClientAddressStates.Active:
                    this.AddressState = "active";
                    break;
                case DhcpServerClientAddressStates.Declined:
                    this.AddressState = "declined";
                    break;
                case DhcpServerClientAddressStates.Offered:
                    this.AddressState = "offered";
                    break;
                case DhcpServerClientAddressStates.Doomed:
                    this.AddressState = "doomed";
                    break;
                case DhcpServerClientAddressStates.Unknown:
                    this.AddressState = "unknown";
                    break;
                default:
                    this.AddressState = "unknown";
                    break;
            }
        }
        
    }
}
