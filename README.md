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






