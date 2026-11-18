import { Component } from '@angular/core';
import {AuthorizationApiService} from "../../services/api/authorization-api.service";
import {animate, style, transition, trigger} from "@angular/animations";

@Component({
  selector: 'app-login-window',
  templateUrl: './login-window.component.html',
  styleUrls: ['./login-window.component.scss'],
  animations: [
    trigger('loginWindowAnimation', [
      transition(':enter', [
        style({ opacity: 0 }),
        animate('.4s 250ms', style({opacity: 1 }))
      ]),
    ])
  ]
})
export class LoginWindowComponent {
  constructor(
      protected _authorizationApiService: AuthorizationApiService) {}

  async getLogin() {
    window.location.href = this._authorizationApiService.getLoginPathForRedirection();
  }
}
