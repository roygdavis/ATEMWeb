import React, { Component } from 'react';

export class Home extends Component {
  static displayName = Home.name;

  render () {
      return (
          <div>
              <h1>ATEM Web App</h1>
              <p>This app provides a web API to the Blackmagic ATEM family of vision mixers</p>
              <p>To help you get started, a Swagger page has been set-up <a href="/swagger">here</a></p>
              <p>Eventually, the aim is to have a full web-based UI, API and WebSocket (for Atem events) system covering the full features of the Atem vision mixer</p>
              <p>At the moment this project is limited to just a very basic API</p>
              <p>As this is a dotnetcore app, it should be possible to run this as a server in a docker container, with WINE (for the Switchers API DLL) but I've not tested or even begun to test that yet</p>
          </div>
      );
  }
}
