import {Injectable} from "@angular/core";
import ApiBaseService from "./api-base.service";
import {HttpClient} from "@angular/common/http";
import {IUserDto} from "../../types/interfaces/consumer/IUserDto";
import {Observable} from "rxjs";
import {IResult} from "../../types/interfaces/IResult";
import {ConfigService} from "../common/config.service";

@Injectable({
  providedIn: 'root'
})
export class ProfileApiService extends ApiBaseService {
  private readonly consumerProfileRoute = "consumer/profile";
  private readonly baseUrl: string;

  constructor(
      private httpClient: HttpClient,
      private _configService: ConfigService) {
    super()
    this.baseUrl = _configService.getServerUrl();
  }

  // requests

  // GET consumer/profile
  getProfile() : Observable<IResult<IUserDto>> {
    return this.httpClient.get<IResult<IUserDto>>(
      this.baseUrl + this.consumerProfileRoute,
      { withCredentials: true }
    );
  }

  // GET consumer/profile/{userId}
  getProfileById(userId: string) : Observable<IResult<IUserDto>> {
    return this.httpClient.get<IResult<IUserDto>>(
      this.baseUrl + this.consumerProfileRoute + `/${userId}`,
      { withCredentials: true }
    )
  }
}
