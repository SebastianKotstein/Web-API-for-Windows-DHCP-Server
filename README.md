# ASP.NET Web API for Windows DHCP Server
An ASP.NET application providing HTTP interfaces for querying information about Windows-based DHCP servers, their scopes, clients, and reservations.
This application uses a .NET wrapper written by Gary Sharp (see [link to project](https://github.com/garysharp/DhcpServerApi)) for accessing Microsoft's native DHCP management API.

## Features
- Read-only Web API with endpoints for querying:
  - the list of available **DHCP Servers** including details about their IP addresses, options, and versions 
  - list of **Scopes** served by a particular DHCP server, including IP addresses, IP ranges, options, etc.
  - **Clients** assigned to a scope including client states, MAC addresses, assigned IP addresses, lease expirations, etc.
  - **Reservations** made for clients in a scope (MAC addresses, IP address, etc. )
- Supports both JSON (default) and XML as media type
- Navigational support through embedded hyperlinks (HATEOAS compliance)
- OpenAPI offline documentation ([Link](https://github.com/SebastianKotstein/Web-API-for-Windows-DHCP-Server/blob/master/OpenAPI.yml))

## Prerequisites
- Requires a Windows-based machine running Microsoft's IIS web server plattform for hosting this ASP.NET application. This Windows machine must be in the same domain as the DHCP server(s). The application has been successfully deployed and tested on Windows Server 2016 and IIS 10.
- Requires an service account (user) that is member of the local group `DHCP Users`. The ASP.NET application must be executed with the identity of this service account (see the [Installation Guide](https://github.com/SebastianKotstein/Web-API-for-Windows-DHCP-Server/wiki/Installation-Guide) for setting up the account and enabling the required ASP.NET feature called impersonation.

## Dependencies
- ASP.NET Web API
- [DhcpServerApi](https://github.com/garysharp/DhcpServerApi) by Gary Sharp (MIT License)
- [Newtonsoft.Json](https://www.newtonsoft.com/json) by James Newton-King (MIT License)

## Installation Guide
See [Installation Guide](https://github.com/SebastianKotstein/Web-API-for-Windows-DHCP-Server/wiki/Installation-Guide) in Wiki

## Limitations & Known Issues
- Due to a bug in ASP.NET a path parameter cannot be substituted with a string that contains hypens (e.g. `dhcp-00.mydomain.com`) when this path parameter is the last segment of the path (e.g. `https://dhcp-api.mydomain.local/servers/dhcp-00.mydomain.com` for the templated URI `https://dhcp-api.mydomain.local/servers/{serverName}`). Hence, as a workaround, it is possible to add a trailing slash to the URI (i.e. `https://dhcp-api.mydomain.local/servers/dhcp-00.mydomain.com/`), which is bad style in terms of REST, in order to trigger the intended endpoint. We, therefore, decided to add a trailing slash to all advertised URIs referencing a server resource as these URIs are likely to contain hyphens. 






