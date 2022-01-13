import React, { Component } from 'react';
import * as signalR from "@microsoft/signalr";

import './custom.css'
import NavMenu from './components/NavMenu';

interface AppState {
  messages: string[];
}


export default class App extends Component<{},AppState> {
  static displayName = App.name;

  constructor(props: AppState) {
    super(props);
    this.state = {
      messages: []
    };
  }

  componentDidMount() {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl("/events")
      .build();

    this.addSignalRListeners(connection);

    connection.start().catch(err => console.log(err));
  }
  
  render() {
    const { messages } = this.state;
    return (
      <div className="container">
        <NavMenu />
        <div className='row'>
          <h1>ATEM Web App</h1>
          <p>This app provides a web API to the Blackmagic ATEM family of vision mixers</p>
          <p>To help you get started, a Swagger page has been set-up <a href="/swagger">here</a></p>
          <p>Eventually, the aim is to have a full web-based UI, API and WebSocket (for Atem events) system covering the full features of the Atem vision mixer</p>
          <p>At the moment this project is limited to just a very basic API</p>
          <p>As this is a dotnetcore app, it should be possible to run this as a server in a docker container, with WINE (for the Switchers API DLL) but I've not tested or even begun to test that yet</p>
        </div>
        <div className='row'>
          <div className='col'>
            {messages?.map(x => <p>{x}</p>)}
          </div>
        </div>
      </div>
    );
  }

  addSignalRListeners = (connection: signalR.HubConnection) => {
    connection.on("updated", (pgmId: number) => {
      this.setState({ messages: [...this.state.messages, pgmId.toString()] });
    });
  }
}
