import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IIsAuthenticatedResponse } from 'src/app/types/responses/IIsAuthenticatedResponse';
import ApiBaseService from './api-base.service';
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class AuthorizationApiService extends ApiBaseService {
  private readonly authorizationRoute = "api/";
  private readonly baseUrl: string;

  constructor(private httpClient: HttpClient) {
    super()
    this.baseUrl = super.getUrl();
  }

  //paths
  public getLoginPathForRedirection = () => this.baseUrl + this.authorizationRoute + "login";

  //requests
  public getIsAuthenticated() : Observable<IIsAuthenticatedResponse> {
    return this.httpClient.get<IIsAuthenticatedResponse>(this.baseUrl + this.authorizationRoute + "isAuthenticated",
      { withCredentials: true });
  }
}
