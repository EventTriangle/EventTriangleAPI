import { Component } from '@angular/core';
import {animate, style, transition, trigger} from "@angular/animations";
import { AuthorizationApiService } from 'src/app/services/api/authorization-api.service';
import {ProfileStateService} from "../../services/state/profile-state.service";
import {UserRole} from "../../types/enums/UserRole";

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
  animations: [
    trigger("navbarLinkAnimation", [
      transition(":enter", [
        style({transform: 'translateY(-10px)', opacity: 0}),
        animate(".3s", style({transform: 'translateY(0px)', opacity: 1}))
      ])
    ])
  ]
})
export class NavbarComponent {
  //observable
  public user$ = this._profileStateService.user$;

  //types
  public UserRole = UserRole;

  constructor(
      private _authorizationApiService: AuthorizationApiService,
      protected _profileStateService: ProfileStateService
      ) {}

  async getLogout() {
    window.location.href = this._authorizationApiService.getLogoutPathForRedirection();
  }
}
