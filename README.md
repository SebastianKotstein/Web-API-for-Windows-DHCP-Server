# ASP.NET Web API for Windows DHCP Server
An ASP.NET application providing HTTP interfaces for querying information about Windows-based DHCP servers, their scopes, clients, and reservations.
This application uses a .NET wrapper written by Gary Sharp (see [link to project](https://github.com/garysharp/DhcpServerApi)) for accessing Microsoft's native DHCP management API.

## Features
- Read-only Web API with endpoints for querying information about:
  - available Windows-based DHCP servers (including details about server's IP address, options, and version) 
  - IP subnets (aka scopes) served by a particular DHCP server (including IP address, IP ranges, options, ... )
  - clients assigned to a scope (client state, MAC address, assigned IP address, lease expiration details, ... )
  - reservations made for clients in a scope (MAC address, IP address, ... )
- Supports both JSON (default) and XML as media type
- Navigational support through embedded hyperlinks (HATEOAS compliance)

## Prerequisites & Dependencies
- Requires an service account (user) that is member of 'DHCP Users' (local group of the respective DHCP server(s))
