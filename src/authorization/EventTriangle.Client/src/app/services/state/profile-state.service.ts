import {Injectable} from "@angular/core";
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
  public wasAuthenticationCheck = false;
  public isAuthenticated = false;

  public user$: BehaviorSubject<IUserDto | null> = new BehaviorSubject<IUserDto | null>(null);

  constructor(
    private _profileApiService: ProfileApiService,
    private _authorizationApiService: AuthorizationApiService,
  ) {}

  //actions
  public plusToBalance(value: number) {
    const user = this.user$.getValue();

    if (!user) throw new Error("User is not defined");

    user.wallet.balance += value;

    this.user$.next(user);
  }

  public minusFromBalance(value: number) {
    const user = this.user$.getValue();

    if (!user) throw new Error("User is not defined");

    user.wallet.balance -= value;

    this.user$.next(user);
  }

  //requests
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
    const getProfileResult = await firstValueFrom<IResult<IUserDto>>(getProfileSub$);
    this.user$.next(getProfileResult.response);

    return getProfileResult;
  }
}
