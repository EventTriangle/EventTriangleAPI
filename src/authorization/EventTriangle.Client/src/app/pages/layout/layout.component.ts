import { Component, OnInit } from '@angular/core';
import { ProfileStateService } from 'src/app/services/state/profile-state.service';
import * as signalR from '@microsoft/signalr';
import {SignalrService} from "../../services/common/signalr.service";

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit {
  constructor(
    protected _profileStateService: ProfileStateService,
    private _signalrService: SignalrService) { }

  public async ngOnInit() {
    const getAuthenticationResult = await this._profileStateService.getAuthenticationAsync();

    if (getAuthenticationResult.authenticated) {
      await this._profileStateService.getProfileAsync();

      await this._signalrService.build();
      this._signalrService.configure();
      await this._signalrService.start();
    }
  }
}
