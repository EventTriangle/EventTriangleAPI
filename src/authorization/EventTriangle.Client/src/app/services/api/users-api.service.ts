import {Injectable} from "@angular/core";
import ApiBaseService from "./api-base.service";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Observable} from "rxjs";
import {UserDto} from "../../types/models/consumer/UserDto";
import {UserSuspendedEvent} from "../../types/models/sender/UserSuspendedEvent";
import {SuspendUserCommand} from "../../types/requests/SuspendUserCommand";
import {NotSuspendUserCommand} from "../../types/requests/NotSuspendUserCommand";
import {UserNotSuspendedEvent} from "../../types/models/sender/UserNotSuspendedEvent";
import {UserRole} from "../../types/enums/UserRole";
import {UpdateUserRoleEvent} from "../../types/models/sender/UpdateUserRoleEvent";
import {UpdateUserRoleRequest} from "../../types/requests/UpdateUserRoleRequest";
import {Result} from "../../types/models/Result";

@Injectable({
  providedIn: 'root'
})
export class UsersApiService extends ApiBaseService {
  private readonly consumerUsersRoute = "consumer/users";
  private readonly senderUsersRoute = "sender/users";
  private readonly baseUrl: string;

  constructor(private httpClient: HttpClient) {
    super()
    this.baseUrl = super.getUrl();
  }

  // requests

  // GET consumer/users
  public getUsers(limit: number, page: number) : Observable<Result<UserDto[]>> {
    return this.httpClient.get<Result<UserDto[]>>(
      this.baseUrl + this.consumerUsersRoute + `?limit=${limit}&page=${page}`,
      { withCredentials: true }
    );
  }

  // GET consumer/users/search/{searchText}
  public searchUsers(searchText: string, limit: number, page: number) : Observable<Result<UserDto[]>> {
    return this.httpClient.get<Result<UserDto[]>>(
      this.baseUrl + this.consumerUsersRoute + `/search/${searchText}` + `?limit=${limit}&page=${page}`,
      { withCredentials: true }
    );
  }

  // POST sender/users
  public suspend(userId: string) : Observable<Result<UserSuspendedEvent>> {
    let command : SuspendUserCommand = {
      userId: userId
    }

    return this.httpClient.post<Result<UserSuspendedEvent>>(
      this.baseUrl + this.senderUsersRoute,
      command,{ withCredentials: true }
    );
  }

  // DELETE sender/users
  public notSuspend(userId: string) : Observable<Result<UserNotSuspendedEvent>> {
    let command : NotSuspendUserCommand = {
      userId: userId
    };
    let options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      }),
      withCredentials: true,
      body: command
    };

    return this.httpClient.delete<Result<UserNotSuspendedEvent>>(
      this.baseUrl + this.senderUsersRoute,
      options
    );
  }

  // PUT sender/users/role
  public updateUserRole(userId: string, userRole: UserRole) : Observable<Result<UpdateUserRoleEvent>> {
    let command : UpdateUserRoleRequest = {
      userId: userId,
      userRole: userRole
    };

    return this.httpClient.put<Result<UpdateUserRoleEvent>>(
      this.baseUrl + this.senderUsersRoute,
      command, { withCredentials: true }
    );
  }
}
