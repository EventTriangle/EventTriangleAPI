import {Injectable} from "@angular/core";
import ApiBaseService from "./api-base.service";
import {HttpClient} from "@angular/common/http";
import {UserDto} from "../../types/models/consumer/UserDto";
import {Observable} from "rxjs";
import {Result} from "../../types/models/Result";

@Injectable({
  providedIn: 'root'
})
export class ProfileApiService extends ApiBaseService {
  private readonly consumerProfileRoute = "consumer/profile";
  private readonly baseUrl: string;

  constructor(private httpClient: HttpClient) {
    super()
    this.baseUrl = super.getUrl();
  }

  // requests

  // GET consumer/profile
  getProfile(): Observable<Result<UserDto>> {
    return this.httpClient.get<Result<UserDto>>(
      this.baseUrl + this.consumerProfileRoute, { withCredentials: true }
    );
  }
}
