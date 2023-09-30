import {Injectable} from "@angular/core";
import ApiBaseService from "./api-base.service";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Observable} from "rxjs";
import {IUserDto} from "../../types/interfaces/consumer/IUserDto";
import {IUserSuspendedEvent} from "../../types/interfaces/sender/IUserSuspendedEvent";
import {SuspendUserCommand} from "../../types/requests/SuspendUserCommand";
import {NotSuspendUserCommand} from "../../types/requests/NotSuspendUserCommand";
import {IUserNotSuspendedEvent} from "../../types/interfaces/sender/IUserNotSuspendedEvent";
import {UserRole} from "../../types/enums/UserRole";
import {IUpdateUserRoleEvent} from "../../types/interfaces/sender/IUpdateUserRoleEvent";
import {UpdateUserRoleRequest} from "../../types/requests/UpdateUserRoleRequest";
import {IResult} from "../../types/interfaces/IResult";

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
  public getUsers(limit: number, page: number) : Observable<IResult<IUserDto[]>> {
    return this.httpClient.get<IResult<IUserDto[]>>(
      this.baseUrl + this.consumerUsersRoute + `?limit=${limit}&page=${page}`,
      { withCredentials: true }
    );
  }

  // GET consumer/users/search/{searchText}
  public searchUsers(searchText: string, limit: number, page: number) : Observable<IResult<IUserDto[]>> {
    return this.httpClient.get<IResult<IUserDto[]>>(
      this.baseUrl + this.consumerUsersRoute + `/search/${searchText}` + `?limit=${limit}&page=${page}`,
      { withCredentials: true }
    );
  }

  // POST sender/users
  public suspend(userId: string) : Observable<IResult<IUserSuspendedEvent>> {
    let command : SuspendUserCommand = {
      userId: userId
    }

    return this.httpClient.post<IResult<IUserSuspendedEvent>>(
      this.baseUrl + this.senderUsersRoute,
      command,{ withCredentials: true }
    );
  }

  // DELETE sender/users
  public notSuspend(userId: string) : Observable<IResult<IUserNotSuspendedEvent>> {
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

    return this.httpClient.delete<IResult<IUserNotSuspendedEvent>>(
      this.baseUrl + this.senderUsersRoute,
      options
    );
  }

  // PUT sender/users/role
  public updateUserRole(userId: string, userRole: UserRole) : Observable<IResult<IUpdateUserRoleEvent>> {
    let command : UpdateUserRoleRequest = {
      userId: userId,
      userRole: userRole
    };

    return this.httpClient.put<IResult<IUpdateUserRoleEvent>>(
      this.baseUrl + this.senderUsersRoute,
      command, { withCredentials: true }
    );
  }
}
