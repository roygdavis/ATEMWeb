# ATEMWeb
Enables a REST API for the Blackmagic ATEM switchers.

Also provides a SignalR (WebSockets) endpoint for ATEM events - if another ATEM Switcher Control Panel is connected, you'll see the inputs change through messages within the WebSocket connection.

Limitations: currently only allows selecting the PGM source.

# Quick start
1. You'll need to install version 7.5.2 of the Blackmagic Switchers application, found here: https://www.blackmagicdesign.com/support/family/atem-live-production-switchers
2. Install dotnet v6.0
3. Compile & run with ```dotnet run```
4. The default url is [https://localhost:5001/](https://localhost:5001/) if you haven't already, you'll need to install the localhost security certificate from dotnet.
5. Swagger is the best way to interact with the API and is available at [https://localhost:5001/swagger/index.html](https://localhost:5001/swagger/index.html)
6. Call the ```/api/atem/connect``` method  to connect to your ATEM.  If it's connected via USB then leave the address parameter blank, otherwise supply the IP address of the ATEM.
7. A proper user interface is in the works, providing a similar feel and functionality offered by the ATEM Switcher application but that's very much a work in progress. 

# API endpoints
Swagger is enabled at [https://localhost:5001/swagger/index.html](https://localhost:5001/swagger/index.html) allowing a complete catalog of the API's available.
GET requests return values, POST requests assert a change (i.e. is an instruction to the ATEM to do something).

POST [http://host:port/api/atem/mixeffects](https://localhost:5001/api/atem/Connect)
Use this to connect to your ATEM.  Address property should be blank if you're connected via USB, otherwise use the ATEM's IP address.

GET [https://localhost:5001/api/atem/mixeffects/1/GetProgramInput](https://localhost:5001/api/atem/mixeffects/1/GetProgramInput)
Returns the current PGM input number.  If you have more than one M/E bank on your mixer then replace the ```1``` with the M/E bank number you wish to query.

POST [https://localhost:5001/api/atem/mixeffects/1/SetProgramInput/1](https://localhost:5001/api/atem/mixeffects/1/SetProgramInput/1)
Change the PGM output for the specified M/E block to the input number specified by the value after ```SetProgramInput```.

Review the Swagger page for an exhaustive list of the API's available.  Please note that some are still work in progress!

# Future plans
- Provide a full and feature-rich API to provide complete access to the ATEM via a REST API.
- Provide a feature-rich web UI experience, with event feedback from the ATEM
- Mimic the Vmix API.
