import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IsAuthenticatedResponse } from '../../types/responses/IsAuthenticatedResponse';
import ApiBaseService from './api-base.service';
import {Observable} from "rxjs";
import {AzureAdAuthResponse} from "../../types/responses/AzureAdAuthResponse";
import {Result} from "../../types/models/Result";

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
  public getLogoutPathForRedirection = () => this.baseUrl + this.authorizationRoute + "logout";

  //requests

  // GET api/isAuthenticated
  public getIsAuthenticated() : Observable<IsAuthenticatedResponse> {
    return this.httpClient.get<IsAuthenticatedResponse>(
      this.baseUrl + this.authorizationRoute + "isAuthenticated",
      { withCredentials: true }
    );
  }

  // GET api/logout
  public logout() : Observable<any> {
    return this.httpClient.get<any>(
      this.baseUrl + this.authorizationRoute + "logout",
      { withCredentials: true }
    );
  }

  // GET api/token
  public getAuthorizationData() : Observable<Result<AzureAdAuthResponse>> {
    return this.httpClient.get<Result<AzureAdAuthResponse>>(
      this.baseUrl + this.authorizationRoute + "token"
    );
  }

  // POST api/token?refreshToken=
  public refreshToken(refreshToken: string) : Observable<Result<AzureAdAuthResponse>> {
    return this.httpClient.post<Result<AzureAdAuthResponse>>(
      this.baseUrl + this.authorizationRoute + `token?refreshToken=${refreshToken}`,
      {},
      { withCredentials: true }
    );
  }

}
