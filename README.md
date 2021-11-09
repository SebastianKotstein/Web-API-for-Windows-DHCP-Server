# ASP.NET Web API for Windows DHCP Server
An ASP.NET application providing HTTP interfaces for querying information about Windows-based DHCP servers, their scopes, clients, and reservations.
This application uses a .NET wrapper written by Gary Sharp (see [link to project](https://github.com/garysharp/DhcpServerApi)) for accessing Microsoft's native DHCP management API.

## Features
- Read-only Web API with endpoints for querying:
  - the list of available **DHCP Servers** including details about their IP addresses, options, and versions 
  - list of **Scopes** served by a particular DHCP server,including IP addresses, IP ranges, options, etc.
  - **Clients** assigned to a scope including client states, MAC addresses, assigned IP addresses, lease expirations, etc.
  - **Reservations** made for clients in a scope (MAC addresses, IP address, etc. )
- Supports both JSON (default) and XML as media type
- Navigational support through embedded hyperlinks (HATEOAS compliance)

## Prerequisites & Dependencies
- Requires an service account (user) that is member of 'DHCP Users' (local group of the respective DHCP server(s))
