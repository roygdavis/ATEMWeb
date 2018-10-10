# ATEMWeb
Enables a REST API for the Blackmagic ATEM switchers.

Also provides a SignalR (WebSockets) endpoint for ATEM events - if another ATEM Switcher Control Panel is connected, you'll see the inputs change through messages within the WebSocket connection.

Limitations: currently only allows selecting the PGM source.

# Quick start
You'll need to install version 7.5.2 of the Blackmagic Switchers application, found here: https://www.blackmagicdesign.com/support/family/atem-live-production-switchers

Install .Net runtime 4.6.1

Compile & run.

http://<host>:<port>/index.html will providea simple web page showing the ATEM events

Only one API method is currently implemented:
http://<host>:<port>/api/api/?Function=fade&Duration=2&Input=6

The "Input=6" parameter is the ATEM input number.  Blackmagic have assigned internal source (solid colour, black, bars, etc) with higher integer values (1000,5000, etc).  I'll add a list of those to this in the future.

# Future plans
Provide a full and feature-rich API to provide complete access to the ATEM via a REST API.
Mimic the Vmix API thumbprint
