import {Injectable} from "@angular/core";
import ApiBaseService from "./api-base.service";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {UserDto} from "../../types/models/UserDto";

@Injectable({
  providedIn: 'root'
})
export class UsersApiService extends ApiBaseService {
  private readonly usersRout = "api/users";
  private readonly baseUrl: string;

  constructor(private httpClient: HttpClient) {
    super()
    this.baseUrl = super.getUrl();
  }

  // requests

  // GET api/users
  public getUsers(limit: number, page: number) : Observable<UserDto[]> {
    return this.httpClient.get<UserDto[]>(
      this.baseUrl + this.usersRout + `?limit=${limit}&page=${page}`
    );
  }
}
