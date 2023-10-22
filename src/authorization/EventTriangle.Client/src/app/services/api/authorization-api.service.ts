import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IsAuthenticatedResponse } from '../../types/responses/IsAuthenticatedResponse';
import ApiBaseService from './api-base.service';
import {Observable} from "rxjs";
import {AzureAdAuthResponse} from "../../types/responses/AzureAdAuthResponse";
import {IResult} from "../../types/interfaces/IResult";
import {ConfigService} from "../common/config.service";

@Injectable({
  providedIn: 'root'
})
export class AuthorizationApiService extends ApiBaseService {
  private readonly authorizationRoute = "api/";
  private readonly baseUrl: string;

  constructor(
      private _httpClient: HttpClient,
      private _configService: ConfigService
  ) {
    super()
    this.baseUrl = _configService.getServerUrl();
  }

  //paths
  public getLoginPathForRedirection = () => this.baseUrl + this.authorizationRoute + "login";
  public getLogoutPathForRedirection = () => this.baseUrl + this.authorizationRoute + "logout";

  //requests

  // GET api/isAuthenticated
  public getIsAuthenticated() : Observable<IsAuthenticatedResponse> {
    return this._httpClient.get<IsAuthenticatedResponse>(
      this.baseUrl + this.authorizationRoute + "isAuthenticated",
      { withCredentials: true }
    );
  }

  // GET api/logout
  public logout() : Observable<any> {
    return this._httpClient.get<any>(
      this.baseUrl + this.authorizationRoute + "logout",
      { withCredentials: true }
    );
  }

  // GET api/token
  public getAuthorizationData() : Observable<IResult<AzureAdAuthResponse>> {
    return this._httpClient.get<IResult<AzureAdAuthResponse>>(
      this.baseUrl + this.authorizationRoute + "token"
    );
  }

  // POST api/token?refreshToken=
  public refreshToken(refreshToken: string) : Observable<IResult<AzureAdAuthResponse>> {
    return this._httpClient.post<IResult<AzureAdAuthResponse>>(
      this.baseUrl + this.authorizationRoute + `token?refreshToken=${refreshToken}`,
      {},
      { withCredentials: true }
    );
  }

}
