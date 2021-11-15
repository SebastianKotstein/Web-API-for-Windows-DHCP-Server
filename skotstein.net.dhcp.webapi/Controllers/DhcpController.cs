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
using skotstein.net.dhcp.webapi.BusinessLogic;
using skotstein.net.dhcp.webapi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web;
using skotstein.net.dhcp.webapi.Models;

namespace skotstein.net.dhcp.webapi.Controllers
{
    public class DhcpController : ApiController
    {
        private DhcpService _service;

        public DhcpController()
        {
            _service = new DhcpService();
            
        }

        /// <summary>
        /// Returns a resource containing a hyperlink to the list of servers.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("", Name = "get_base")]
        public IHttpActionResult GetBase()
        {
            Resource resource = new Resource();
            resource.AddHyperlink("servers", Url.Link("get_servers", new { }));
            return Ok(resource);
        }

        /// <summary>
        /// Returns a store of all available DHCP servers.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("servers", Name = "get_servers")]
        public IHttpActionResult GetServers()
        {
            Servers servers = _service.GetServers();
            foreach(Server server in servers.Items)
            {
                server.AddHyperlink("item", Url.Link("get_server", new { serverName = server.Name+"/" }));
            }
            servers.AddHyperlink("self", Url.Link("get_servers", new { }));
            return Ok(servers);
        }

        /// <summary>
        /// Returns the DHCP server that has the specified server name.
        /// Note: Due to a bug in ASP.NET it is not possible to subsitute path parameters with strings containing '-'.
        /// As a workaround, as '-' might be contained in a server name, make sure to add a trailing '/' after the server name.
        /// </summary>
        /// <param name="serverName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("servers/{serverName}", Name = "get_server")]
        public IHttpActionResult GetServer(string serverName)
        {
            Server server = _service.GetServer(serverName);
            if(server != null)
            {
                server.AddHyperlink("self", Url.Link("get_server", new { serverName = server.Name + "/" }));
                server.AddHyperlink("collection", Url.Link("get_servers", new { }));
                server.AddHyperlink("scopes", Url.Link("get_scopes", new { serverName = server.Name }));
                return Ok(server);
            }
            else
            {
                return Content(HttpStatusCode.NotFound, CreateError(404, "Server '"+serverName+"' not found"));
            }
        }

        /// <summary>
        /// Returns all scopes handled by the specified DHCP server.
        /// </summary>
        /// <param name="serverName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("servers/{serverName}/scopes", Name = "get_scopes")]
        public IHttpActionResult getScopes(string serverName)
        {
            Server server = _service.GetServer(serverName);
            if (server != null)
            {
                Scopes scopes = _service.GetScopes(serverName);
                foreach(Scope scope in scopes.Items)
                {
                    scope.AddHyperlink("item", Url.Link("get_scope", new { serverName = server.Name, scopeName=scope.Name }));
                }
                scopes.AddHyperlink("self", Url.Link("get_scopes", new { serverName = server.Name }));
                scopes.AddHyperlink("server", Url.Link("get_server", new { serverName = server.Name + "/" }));
                return Ok(scopes);
            }
            else
            {
                return Content(HttpStatusCode.NotFound, CreateError(404, "Server '" + serverName + "' not found"));
            }
        }

        /// <summary>
        /// Returns a specified scope handled by the specified DHCP server.
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="scopeName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("servers/{serverName}/scopes/{scopeName}", Name = "get_scope")]
        public IHttpActionResult getScope(string serverName, string scopeName)
        {
            Server server = _service.GetServer(serverName);
            if(server != null)
            {
                Scope scope = _service.GetScope(serverName,scopeName);
                if(scope != null)
                {
                    scope.AddHyperlink("self", Url.Link("get_scope", new { serverName = server.Name, scopeName = scope.Name }));
                    scope.AddHyperlink("collection", Url.Link("get_scopes", new { serverName = server.Name }));
                    scope.AddHyperlink("clients", Url.Link("get_clients", new { serverName = server.Name, scopeName = scope.Name }));
                    scope.AddHyperlink("reservations", Url.Link("get_reservations", new { serverName = server.Name, scopeName = scope.Name }));
                    scope.AddHyperlink("server", Url.Link("get_server", new { serverName = server.Name + "/" }));
                    return Ok(scope);
                }
                else
                {
                    return Content(HttpStatusCode.NotFound, CreateError(404, "Scope '" + scopeName + "' not found"));
                }
            }
            else
            {
                return Content(HttpStatusCode.NotFound, CreateError(404, "Server '" + serverName + "' not found"));
            }
        }

        /// <summary>
        /// Returns the list of known clients that are assigned to the specified scope handled by the specified DHCP server.
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="scopeName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("servers/{serverName}/scopes/{scopeName}/clients", Name = "get_clients")]
        public IHttpActionResult GetClients(string serverName, string scopeName)
        {
            Server server = _service.GetServer(serverName);
            if (server != null)
            {
                Scope scope = _service.GetScope(serverName, scopeName);
                if (scope != null)
                {
                    Clients clients = _service.GetClients(serverName, scopeName);
                    foreach(Client client in clients.Items)
                    {
                        client.AddHyperlink("item", Url.Link("get_client", new { serverName = server.Name, scopeName = scope.Name, macAddress = client.MacAddress }));
                    }
                    clients.AddHyperlink("self", Url.Link("get_clients", new { serverName = server.Name, scopeName = scope.Name }));
                    clients.AddHyperlink("scope", Url.Link("get_scope", new { serverName = server.Name, scopeName = scope.Name }));
                    clients.AddHyperlink("server", Url.Link("get_server", new { serverName = server.Name + "/" }));
                    return Ok(clients);
                }
                else
                {
                    return Content(HttpStatusCode.NotFound, CreateError(404, "Scope '" + scopeName + "' not found"));
                }
            }
            else
            {
                return Content(HttpStatusCode.NotFound, CreateError(404, "Server '" + serverName + "' not found"));
            }
        }

        /// <summary>
        /// Returns the list of reservations assigned to specified scope handled by the specified DHCP server.
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="scopeName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("servers/{serverName}/scopes/{scopeName}/reservations", Name = "get_reservations")]
        public IHttpActionResult GetReservations(string serverName, string scopeName)
        {
            Server server = _service.GetServer(serverName);
            if (server != null)
            {
                Scope scope = _service.GetScope(serverName, scopeName);
                if (scope != null)
                {
                    Reservations reservations = _service.GetReservations(serverName, scopeName);
                    foreach (Reservation reservation in reservations.Items)
                    {
                        reservation.AddHyperlink("item", Url.Link("get_reservation", new { serverName = server.Name, scopeName = scope.Name, macAddress = reservation.MacAddress }));
                    }
                    reservations.AddHyperlink("self", Url.Link("get_reservations", new { serverName = server.Name, scopeName = scope.Name }));
                    reservations.AddHyperlink("scope", Url.Link("get_scope", new { serverName = server.Name, scopeName = scope.Name }));
                    reservations.AddHyperlink("server", Url.Link("get_server", new { serverName = server.Name + "/" }));
                    return Ok(reservations);
                }
                else
                {
                    return Content(HttpStatusCode.NotFound, CreateError(404, "Scope '" + scopeName + "' not found"));
                }
            }
            else
            {
                return Content(HttpStatusCode.NotFound, CreateError(404, "Server '" + serverName + "' not found"));
            }
        }

        /// <summary>
        /// Returns the specified client that is assigned to the specified scope handled by the specified DHCP server.
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="scopeName"></param>
        /// <param name="macAddress"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("servers/{serverName}/scopes/{scopeName}/clients/{macAddress}", Name = "get_client")]
        public IHttpActionResult GetClient(string serverName, string scopeName, string macAddress)
        {
            Server server = _service.GetServer(serverName);
            if (server != null)
            {
                Scope scope = _service.GetScope(serverName, scopeName);
                if (scope != null)
                {
                    Client client = _service.GetClient(serverName, scopeName, macAddress);
                    if(client != null)
                    {
                        client.AddHyperlink("self", Url.Link("get_client", new { serverName = server.Name, scopeName = scope.Name, macAddress = client.MacAddress }));
                        client.AddHyperlink("collection", Url.Link("get_clients", new { serverName = server.Name, scopeName = scope.Name }));
                        client.AddHyperlink("scope", Url.Link("get_scope", new { serverName = server.Name, scopeName = scope.Name }));
                        client.AddHyperlink("server", Url.Link("get_server", new { serverName = server.Name + "/" }));
                        if (client.HasReservation && _service.HasReservation(serverName, scopeName, client.MacAddress))
                        {
                            client.AddHyperlink("reservation", Url.Link("get_reservation", new { serverName = server.Name, scopeName = scope.Name, macAddress = client.MacAddress }));
                        }
                        return Ok(client);
                    }
                    else
                    {
                        return Content(HttpStatusCode.NotFound, CreateError(404, "Client '" + macAddress + "' not found"));
                    }
                }
                else
                {
                    return Content(HttpStatusCode.NotFound, CreateError(404, "Scope '" + scopeName + "' not found"));
                }
            }
            else
            {
                return Content(HttpStatusCode.NotFound, CreateError(404, "Server '" + serverName + "' not found"));
            }
        }

        /// <summary>
        /// Returns the reservation that is issued for the client having the passed MAC address and is assigned to the specified scope handled by the specified DHCP server.
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="scopeName"></param>
        /// <param name="macAddress"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("servers/{serverName}/scopes/{scopeName}/reservations/{macAddress}", Name = "get_reservation")]
        public IHttpActionResult GetReservation(string serverName, string scopeName, string macAddress)
        {
            Server server = _service.GetServer(serverName);
            if (server != null)
            {
                Scope scope = _service.GetScope(serverName, scopeName);
                if (scope != null)
                {
                    Reservation reservation = _service.GetReservation(serverName, scopeName, macAddress);
                    if(reservation != null)
                    {
                        reservation.AddHyperlink("self", Url.Link("get_reservation", new { serverName = server.Name, scopeName = scope.Name, macAddress = reservation.MacAddress }));
                        reservation.AddHyperlink("collection", Url.Link("get_reservations", new { serverName = server.Name, scopeName = scope.Name }));
                        reservation.AddHyperlink("scope", Url.Link("get_scope", new { serverName = server.Name, scopeName = scope.Name }));
                        reservation.AddHyperlink("server", Url.Link("get_server", new { serverName = server.Name + "/" }));
                        if (_service.HasClient(serverName, scopeName, reservation.MacAddress))
                        {
                            reservation.AddHyperlink("client", Url.Link("get_client", new { serverName = server.Name, scopeName = scope.Name, macAddress = reservation.MacAddress }));
                        }
                        return Ok(reservation);
                    }
                    else
                    {
                        return Content(HttpStatusCode.NotFound, CreateError(404, "Reservation '" + macAddress + "' not found"));
                    }
                    
                }
                else
                {
                    return Content(HttpStatusCode.NotFound, CreateError(404, "Scope '" + scopeName + "' not found"));
                }
            }
            else
            {

                return Content(HttpStatusCode.NotFound, CreateError(404, "Server '" + serverName + "' not found"));
            }

        }

        private Error CreateError(int statusCode, string message)
        {
            Error error = new Error()
            {
                Code = statusCode,
                Message = message,
                Time = DateTime.UtcNow
            };
            error.AddHyperlink("base", Url.Link("get_base", new { }));
            return error;
        }
    }
}
