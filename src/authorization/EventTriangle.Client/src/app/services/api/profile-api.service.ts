import {Injectable} from "@angular/core";
import ApiBaseService from "./api-base.service";
import {HttpClient} from "@angular/common/http";
import {UserDto} from "../../types/models/consumer/UserDto";
import {Observable} from "rxjs";

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

  getProfile(): Observable<UserDto> {
    return this.httpClient.get<UserDto>(
      this.baseUrl + this.consumerProfileRoute
    );
  }
}
