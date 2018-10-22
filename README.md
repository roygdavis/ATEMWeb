# ATEMWeb
Enables a REST API for the Blackmagic ATEM switchers.

Also provides a SignalR (WebSockets) endpoint for ATEM events - if another ATEM Switcher Control Panel is connected, you'll see the inputs change through messages within the WebSocket connection.

Limitations: currently only allows selecting the PGM source.

# Quick start
You'll need to install version 7.5.2 of the Blackmagic Switchers application, found here: https://www.blackmagicdesign.com/support/family/atem-live-production-switchers

Install .Net runtime 4.6.1

Edit web.config with the IP of your ATEM.

Compile & run.

# API endpoints
http://host:port/index.html will provide simple web page showing the ATEM events (only PGM change events are currently notified)

http://host:port/api/atem/mixeffects
This will return all the M/E banks you have. The zero-based index number returned will be required in the following calls (or, just use zero if you only have the 1 ME)

http://host:port/api/atem/mixeffects/{me_id}/pgm
Returns the current input id for the PGM bus on the specified me_id (actually returns the entire state for the specified me_id)

http://host:port/api/atem/mixeffects/{meId}/pgm/{pgm_id}
Change the PGM output for the specified {me_id) to the input number specified by pgm_id. me_id is usually zero, unless you have a 2M/E ATEM and want to change the PGM output of the 2nd one, in which case it'll be a one (obvs)

http://host:port/api/atem/mixeffects/{me_id}/pvw
Returns the input id of the PVW bus on the specified me_id (and entire state of the specified me_id)

http://host:port/api/atem/mixeffects/{me_id}/pvw/{pvw_id}
Change the PVW output for the {me_id) to the input number specified by pvw_id. me_id is usually zero, unless you have a 2M/E ATEM and want to change the PVW output of the 2nd one, in which case it'll be a one (obvs)

Notes on input numbers...
Blackmagic have assigned internal source (solid colour, black, bars, etc) with higher integer values (1000,5000, etc).  I'll add an API call to grab a list of those in a future release.

# Future plans
Provide a full and feature-rich API to provide complete access to the ATEM via a REST API.
Mimic the Vmix API thumbprint
