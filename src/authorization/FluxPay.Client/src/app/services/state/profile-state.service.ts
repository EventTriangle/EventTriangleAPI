import {Injectable, signal} from "@angular/core";
import {IUserDto} from "../../types/interfaces/consumer/IUserDto";
import {BehaviorSubject, firstValueFrom} from "rxjs";
import {IResult} from "../../types/interfaces/IResult";
import {ProfileApiService} from "../api/profile-api.service";
import {AuthorizationApiService} from "../api/authorization-api.service";
import {IsAuthenticatedResponse} from "../../types/responses/IsAuthenticatedResponse";
import {HttpErrorResponse} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class ProfileStateService {
  private profileRequestVersion = 0;
  private readonly authentication = signal<IsAuthenticatedResponse | null>(null);

  public get wasAuthenticationCheck(): boolean {
    return this.authentication() !== null;
  }

  public get isAuthenticated(): boolean {
    return this.authentication()?.authenticated ?? false;
  }

  public user$: BehaviorSubject<IUserDto | null> = new BehaviorSubject<IUserDto | null>(null);

  constructor(
    private _profileApiService: ProfileApiService,
    private _authorizationApiService: AuthorizationApiService,
  ) {}

  //requests
  public async getAuthenticationAsync() {
    const result = await firstValueFrom(this._authorizationApiService.getIsAuthenticated())
      .catch((error: unknown): IsAuthenticatedResponse => {
        if (error instanceof HttpErrorResponse && error.status === 401) {
          return {authenticated: false};
        }
        throw error;
      });

    this.authentication.set(result);
    return result;
  }

  public async getProfileAsync() {
    const version = ++this.profileRequestVersion;
    const getProfileSub$ = this._profileApiService.getProfile();
    const getProfileResult = await firstValueFrom<IResult<IUserDto>>(getProfileSub$);
    if (version === this.profileRequestVersion) this.user$.next(getProfileResult.response);

    return getProfileResult;
  }
}
