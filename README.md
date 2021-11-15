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
- Requires a Windows-based machine running Microsoft's IIS web server plattform for hosting this ASP.NET application. This Windows machine must be in the same domain as the DHCP server(s). The application has been successfully deployed and tested on Windows Server 2016 and IIS 10.
- Requires an service account (user) that is member of the local group `DHCP Users`. The ASP.NET application must be executed with the identity of this service account. See the instructions below for setting up such an account and enabling the required ASP.NET feature called impersonation.

## Installation Guide
Follow these steps in order to deploy the ASP.NET Web API for Windows DHCP Server:

### Prepare a Windows Host
Prepare a Windows-based machine (e.g. Windows Server 2016) and install IIS on this machine (detailed instructions for the installation of IIS under Windows Server 2016 can be found [here](https://www.rootusers.com/how-to-install-iis-in-windows-server-2016/). Make sure that the prepared host is in the same domain as the DHCP server(s), which should be accessed via the API, and can reach them via network.

### Create a Service Account with Required Permissions
For querying DHCP status information through Microsoft's native DHCP management API, we require a user account that has at least permission to read DHCP server settings, i.e. that is member of the local group `DHCP Users`, which is a local group . The following procedure describes the creation of such a user account as well as the required steps of making this user member of `DHCP Users`. Depending on your enterprise setup, the following instructions might vary from the required steps in your setup.

On your domain controller, open 'Active Directory Users and Computers' and create a domain user, e.g. `dhcpservice@mydomain.com`. Make sure that the password of this account never expires by enabling the respective checkbox.  

Since `DHCP Users` is per default a local but not a domain group, we have to sign to the DHCP server host and open 'Computer Management' and then navigate to 'Local Users and Groups'. Open the 'Groups' folder and double click on 'DHCP Users'. Search for the created user account and add it as a group member to 'DHCP Users'. You may have to repeat these steps for all DHCP servers in your setup. Moreover, after adding the user account to the group 'DHCP Users', it might be necessary to sign in with this user account to a Windows machine in your domain such that these changes are applied.






