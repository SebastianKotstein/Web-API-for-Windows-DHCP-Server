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
using skotstein.net.dhcp.webapi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace skotstein.net.dhcp.webapi.BusinessLogic
{
    /// <summary>
    /// Implements the business logic for querying DHCP servers, scopes, clients, and reservations.
    /// </summary>
    public class DhcpService
    {
        /// <summary>
        /// Returns a collection of all available DHCP servers.
        /// </summary>
        /// <returns>collection of all available DHCP servers</returns>
        public Servers GetServers()
        {
            Servers servers = new Servers();
            foreach(DhcpServer dhcpServer in DhcpServer.Servers)
            {
                Server server = new Server(dhcpServer);
                servers.Items.Add(server);
            }
            return servers;
        }

        /// <summary>
        /// Returns the first <see cref="DhcpServer"/> instance having the passed server name or null, if no such server exists.
        /// </summary>
        /// <param name="serverName">name of the server</param>
        /// <returns>instance having the passed server name or null</returns>
        private DhcpServer GetServerAsDhcpServer(string serverName)
        {
            foreach (DhcpServer dhcpServer in DhcpServer.Servers)
            {
                if (IsStringEqual(serverName, dhcpServer.Name))
                {
                    return dhcpServer;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns true if a <see cref="Server"/> exists that has the passed server name, else false.
        /// </summary>
        /// <param name="serverName">name of the server</param>
        /// <returns>true or false</returns>
        public bool HasServer(string serverName)
        {
            return GetServer(serverName) != null;
        }

        /// <summary>
        /// Returns the first <see cref="Server"/> instance having the passed server name or null, if no such server exists.
        /// </summary>
        /// <param name="serverName">name of the server</param>
        /// <returns>instance having the passed server name or null</returns>
        public Server GetServer(string serverName)
        {
            DhcpServer dhcpServer = GetServerAsDhcpServer(serverName);
            if(dhcpServer != null)
            {
                return new Server(dhcpServer);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns a collection of all scopes that are served by the DHCP server having the passed name.
        /// The method returns null, if no such DHCP server exists.
        /// </summary>
        /// <param name="serverName">name of the server</param>
        /// <returns>collection of scopes or null</returns>
        public Scopes GetScopes(string serverName)
        {
            DhcpServer dhcpServer = GetServerAsDhcpServer(serverName);
            if(dhcpServer != null)
            {
                Scopes scopes = new Scopes();
                foreach(DhcpServerScope dhcpServerScope in dhcpServer.Scopes)
                {
                    Scope scope = new Scope(dhcpServerScope);
                    scopes.Items.Add(scope);
                }
                return scopes;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the <see cref="DhcpServerScope"/> that has the passed scope name and is served by the DHCP server having the passed server name.
        /// The method returns null, if either no such DHCP server or scope exists.
        /// </summary>
        /// <param name="serverName">name of the server</param>
        /// <param name="scopeName">name of the scope</param>
        /// <returns>scope having the passed name or null</returns>
        private DhcpServerScope GetScopeAsDhcpServerScope(string serverName, string scopeName)
        {
            DhcpServer dhcpServer = GetServerAsDhcpServer(serverName);
            if (dhcpServer != null)
            {
                foreach (DhcpServerScope dhcpServerScope in dhcpServer.Scopes)
                {
                    if (IsStringEqual(scopeName, dhcpServerScope.Name))
                    {
                        return dhcpServerScope;
                    }
                }
                return null;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns true if a <see cref="Scope"/> exists that has the passed scope name as well as a DHCP server that has the passed server name and serves this scope, else false.
        /// </summary>
        /// <param name="serverName">name of the server</param>
        /// <param name="scopeName">name of the scope</param>
        /// <returns>true or false</returns>
        public bool HasScope(string serverName, string scopeName)
        {
            return GetScope(serverName, scopeName) != null;
        }

        /// <summary>
        /// Returns the <see cref="Scope"/> that has the passed scope name and is served by the DHCP server having the passed server name.
        /// The method returns null, if either no such DHCP server or scope exists.
        /// </summary>
        /// <param name="serverName">name of the server</param>
        /// <param name="scopeName">name of the scope</param>
        /// <returns>scope having the passed name or null</returns>
        public Scope GetScope(string serverName, string scopeName)
        {
            DhcpServerScope dhcpServerScope = GetScopeAsDhcpServerScope(serverName, scopeName);
            if(dhcpServerScope != null)
            {
                return new Scope(dhcpServerScope);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns a collection of all clients that are served by the DHCP server having the passed server name and belongs to the scope having the passed scope name.
        /// The method returns null, if no such DHCP server and/or scope exists.
        /// </summary>
        /// <param name="serverName">name of the server</param>
        /// <param name="scopeName">name of the scope</param>
        /// <returns>collection of clients or null</returns>
        public Clients GetClients(string serverName, string scopeName)
        {
            DhcpServerScope dhcpServerScope = GetScopeAsDhcpServerScope(serverName, scopeName);
            if (dhcpServerScope != null)
            {
                Clients clients = new Clients();
                foreach (DhcpServerClient dhcpServerClient in dhcpServerScope.Clients)
                {
                    Client client = new Client(dhcpServerClient);
                    clients.Items.Add(client);
                }
                return clients;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the <see cref="DhcpServerClient"/> that has the passed MAC address and belongs to the scope having the passed scope name and is served by the DHCP server having the passed server name.
        /// The method returns null, if either no such DHCP server, scope and/or client exists.
        /// </summary>
        /// <param name="serverName">name of the server</param>
        /// <param name="scopeName">name of the scope</param>
        /// <param name="macAddress">MAC address</param>
        /// <returns>client or null</returns>
        private DhcpServerClient GetClientAsDhcpServerClient(string serverName, string scopeName, string macAddress)
        {
            DhcpServerScope dhcpServerScope = GetScopeAsDhcpServerScope(serverName, scopeName);
            if (dhcpServerScope != null)
            {
                foreach (DhcpServerClient dhcpServerClient in dhcpServerScope.Clients)
                {
                    if (IsMacAddressEqual(macAddress, dhcpServerClient.HardwareAddress))
                    {
                        return dhcpServerClient;
                    }
                }
                return null;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns true if a <see cref="Client"/> exists that has the passed MAC address and is associated to a scope having the passed scope name that is served by a DHCP server that has the passed server name, else false.
        /// </summary>
        /// <param name="serverName">name of the server</param>
        /// <param name="scopeName">name of the scope</param>
        /// <param name="macAddress">MAC address</param>
        /// <returns>true or false</returns>
        public bool HasClient(string serverName, string scopeName, string macAddress)
        {
            return GetClient(serverName, scopeName, macAddress) != null;
        }

        /// <summary>
        /// Returns the <see cref="Client"/> that has the passed MAC address and belongs to the scope having the passed scope name and is served by the DHCP server having the passed server name.
        /// The method returns null, if either no such DHCP server, scope and/or client exists.
        /// </summary>
        /// <param name="serverName">name of the server</param>
        /// <param name="scopeName">name of the scope</param>
        /// <param name="macAddress">MAC address</param>
        /// <returns>client or null</returns>
        public Client GetClient(string serverName, string scopeName, string macAddress)
        {
            DhcpServerClient dhcpServerClient = GetClientAsDhcpServerClient(serverName, scopeName, macAddress);
            if(dhcpServerClient != null)
            {
                return new Client(dhcpServerClient);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns a collection of all reservations that are served by the DHCP server having the passed server name and belongs to the scope having the passed scope name.
        /// The method returns null, if no such DHCP server and/or scope exists.
        /// </summary>
        /// <param name="serverName">name of the server</param>
        /// <param name="scopeName">name of the scope</param>
        /// <returns>collection of reservations or null</returns>
        public Reservations GetReservations(string serverName, string scopeName)
        {
            DhcpServerScope dhcpServerScope = GetScopeAsDhcpServerScope(serverName, scopeName);
            if (dhcpServerScope != null)
            {
                Reservations reservations = new Reservations();
                foreach (DhcpServerScopeReservation dhcpServerScopeReservation in dhcpServerScope.Reservations)
                {
                    Reservation reservation = new Reservation(dhcpServerScopeReservation);
                    reservations.Items.Add(reservation);
                }
                return reservations;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns true if a <see cref="Reservation"/> exists that has the passed MAC address and is associated to a scope having the passed scope name that is served by a DHCP server that has the passed server name, else false.
        /// </summary>
        /// <param name="serverName">name of the server</param>
        /// <param name="scopeName">name of the scope</param>
        /// <param name="macAddress">MAC address</param>
        /// <returns>true or false</returns>
        public bool HasReservation(string serverName, string scopeName, string macAddress)
        {
            return GetReservation(serverName, scopeName, macAddress) != null;
        }

        /// <summary>
        /// Returns the <see cref="Reservation"/> that is for passed MAC address and belongs to the scope having the passed scope name and is served by the DHCP server having the passed server name.
        /// The method returns null, if either no such DHCP server, scope and/or client exists.
        /// </summary>
        /// <param name="serverName">name of the server</param>
        /// <param name="scopeName">name of the scope</param>
        /// <param name="macAddress">MAC address</param>
        /// <returns>reservation or null</returns>
        public Reservation GetReservation(string serverName, string scopeName, string macAddress)
        {
            DhcpServerScopeReservation dhcpServerScopeReservation = GetReservationAsDhcpServerScopeReservation(serverName, scopeName, macAddress);
            if (dhcpServerScopeReservation != null)
            {
                return new Reservation(dhcpServerScopeReservation);
            }
            else
            {
                return null;
            }
        }

        // <summary>
        /// Returns the <see cref="DhcpServerScopeReservation"/> that is for passed MAC address and belongs to the scope having the passed scope name and is served by the DHCP server having the passed server name.
        /// The method returns null, if either no such DHCP server, scope and/or reservation exists.
        /// </summary>
        /// <param name="serverName">name of the server</param>
        /// <param name="scopeName">name of the scope</param>
        /// <param name="macAddress">MAC address</param>
        /// <returns>reservation or null</returns>
        private DhcpServerScopeReservation GetReservationAsDhcpServerScopeReservation(string serverName, string scopeName, string macAddress)
        {
            DhcpServerScope dhcpServerScope = GetScopeAsDhcpServerScope(serverName, scopeName);
            if (dhcpServerScope != null)
            {
                foreach (DhcpServerScopeReservation dhcpServerScopeReservation in dhcpServerScope.Reservations)
                {
                    if (IsMacAddressEqual(macAddress, dhcpServerScopeReservation.HardwareAddress))
                    {
                        return dhcpServerScopeReservation;
                    }
                }
                return null;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Compares to strings and returns true if they are equal regardless case-sensitivity and trailing/leading whitespaces
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>true if equals</returns>
        private bool IsStringEqual(string a, string b)
        {
            return a.Trim().ToLower().CompareTo(b.Trim().ToLower()) == 0;
        }

        /// <summary>
        /// Compares to MAC addresses and returns true if they are equal regardless case-sensitivity and trailing/leading whitespaces, with and without ':'
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>true if equals</returns>
        private bool IsMacAddressEqual(string a, string b)
        {
            return a.Replace(":","").Trim().ToLower().CompareTo(b.Replace(":", "").Trim().ToLower()) == 0;
        }
    }
}
