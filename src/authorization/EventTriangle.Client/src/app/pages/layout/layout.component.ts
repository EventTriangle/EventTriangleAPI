import { Component, OnInit } from '@angular/core';
import { AuthorizationApiService } from 'src/app/services/api/authorization-api.service';
import { ProfileStateService } from 'src/app/services/state/profile-state.service';
import {animate, query, stagger, state, style, transition, trigger} from "@angular/animations";

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss'],
  animations: [
    trigger('loginWindowAnimation', [
      transition(':enter', [
        style({ opacity: 0 }),
        animate('.4s 250ms', style({opacity: 1 }))
      ]),
    ])
  ]
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

  async getLogin() {
    window.location.href = this._authorizationApiService.getLoginPathForRedirection();
  }
}
