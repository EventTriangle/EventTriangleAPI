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
  public getUsers(limit: number, page: number) : Observable<UserDto[]> {
    return this.httpClient.get<UserDto[]>(
      this.baseUrl + this.consumerUsersRoute + `?limit=${limit}&page=${page}`
    );
  }

  // GET consumer/users/search/{searchText}
  public searchUsers(searchText: string, limit: number, page: number) : Observable<UserDto[]> {
    return this.httpClient.get<UserDto[]>(
      this.baseUrl + this.consumerUsersRoute + `/search/${searchText}` + `?limit=${limit}&page=${page}`
    );
  }

  // POST sender/users
  public suspend(userId: string) : Observable<UserSuspendedEvent> {
    let command = new SuspendUserCommand(userId);

    return this.httpClient.post<UserSuspendedEvent>(
      this.baseUrl + this.senderUsersRoute,
      command
    );
  }

  // DELETE sender/users
  public notSuspend(userId: string) : Observable<UserNotSuspendedEvent> {
    let command = new NotSuspendUserCommand(userId);
    let options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      }),
      body: command
    };

    return this.httpClient.delete<UserNotSuspendedEvent>(
      this.baseUrl + this.senderUsersRoute,
      options
    );
  }

  // PUT sender/users/role
  public updateUserRole(userId: string, userRole: UserRole) : Observable<UpdateUserRoleEvent> {
    let command =  new UpdateUserRoleRequest(userId, userRole);
    let options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      }),
      body: command
    };

    return this.httpClient.put<UpdateUserRoleEvent>(
      this.baseUrl + this.senderUsersRoute,
      options
    );
  }
}
