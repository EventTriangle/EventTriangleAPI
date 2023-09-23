import { Component, OnInit } from '@angular/core';
import { AuthorizationApiService } from 'src/app/services/api/authorization-api.service';
import { ProfileStateService } from 'src/app/services/state/profile-state.service';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit {
  constructor(
    private _authorizationApiService: AuthorizationApiService,
    protected _profileStateService: ProfileStateService) { }

  public ngOnInit(): void {
    const request = this._authorizationApiService.getIsAuthenticated();

    request.subscribe({
      next: (res) => {
        this._profileStateService.wasAuthenticationCheck = true;
        this._profileStateService.isAuthenticated = res.authenticated
      },
      error: _ => {
        this._profileStateService.wasAuthenticationCheck = true;
        this._profileStateService.isAuthenticated = false;
      }
    });
  }
}
