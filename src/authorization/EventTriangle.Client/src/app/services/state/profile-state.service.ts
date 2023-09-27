import {Injectable} from "@angular/core";
import {UserDto} from "../../types/models/consumer/UserDto";
import {firstValueFrom} from "rxjs";
import {Result} from "../../types/models/Result";
import {ProfileApiService} from "../api/profile-api.service";
import {AuthorizationApiService} from "../api/authorization-api.service";
import {IsAuthenticatedResponse} from "../../types/responses/IsAuthenticatedResponse";
import {HttpErrorResponse} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class ProfileStateService {
  public wasAuthenticationCheck = false;
  public isAuthenticated = false;

  public user: UserDto | null = null;

  constructor(
      private _profileApiService: ProfileApiService,
      private _authorizationApiService: AuthorizationApiService,
  ) {}

  public async getAuthenticationAsync() {
    const getAuthenticationSub$ = this._authorizationApiService.getIsAuthenticated();
    const getAuthenticationResult = await firstValueFrom<IsAuthenticatedResponse>(getAuthenticationSub$)
        .catch((e: HttpErrorResponse) => {
          const getAuthenticationResult = e.error as IsAuthenticatedResponse;

          this.wasAuthenticationCheck = true;
          this.isAuthenticated = getAuthenticationResult.authenticated;

          return getAuthenticationResult;
        });

    this.wasAuthenticationCheck = true;
    this.isAuthenticated = getAuthenticationResult.authenticated;

    return getAuthenticationResult;
  }

  public async getProfileAsync() {
    const getProfileSub$ = this._profileApiService.getProfile();
    const getProfileResult = await firstValueFrom<Result<UserDto>>(getProfileSub$);
    this.user = getProfileResult.response;

    return getProfileResult;
  }
}
