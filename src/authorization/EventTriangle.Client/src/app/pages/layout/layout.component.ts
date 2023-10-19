import { Component, OnInit } from '@angular/core';
import { ProfileStateService } from 'src/app/services/state/profile-state.service';
import * as signalR from '@microsoft/signalr';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit {
  constructor(
    protected _profileStateService: ProfileStateService) { }

  public async ngOnInit() {
    const getAuthenticationResult = await this._profileStateService.getAuthenticationAsync();

    if (getAuthenticationResult.authenticated) {
      await this._profileStateService.getProfileAsync();

      const connectionBuilder = new signalR.HubConnectionBuilder();
      const connection = connectionBuilder
          .configureLogging(signalR.LogLevel.Information)
          .withUrl("https://localhost:7000/consumer" + "/notify")
          .build();

      await connection.start();

      console.log(connection);

      await connection.send("Join");
    }
  }
}
