import { Component, OnInit } from '@angular/core';
import { ProfileStateService } from 'src/app/services/state/profile-state.service';

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

    if (getAuthenticationResult.authenticated) await this._profileStateService.getProfileAsync();
  }
}
