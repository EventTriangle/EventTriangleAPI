import { Injectable } from '@angular/core';
import {BehaviorSubject, firstValueFrom} from "rxjs";
import {IUserDto} from "../../types/interfaces/consumer/IUserDto";
import {UsersApiService} from "../api/users-api.service";

@Injectable({
  providedIn: 'root'
})
export class UsersStateService {
  public wasRequested = false;

  public users$: BehaviorSubject<IUserDto[]> = new BehaviorSubject<IUserDto[]>([]);
  public searchedUsers$: BehaviorSubject<IUserDto[]> = new BehaviorSubject<IUserDto[]>([]);

  constructor(
     private _usersApiService: UsersApiService
  ) {}

  //actions
  public clearSearchedUsers() {
    this.searchedUsers$.next([]);
  }

  //requests
  public async getUsersAsync(limit: number, page: number) {
    const getUsers$ = this._usersApiService.getUsers(limit, page);
    const getUsersResult = await firstValueFrom(getUsers$);

    this.users$.next(getUsersResult.response);
    this.wasRequested = true;

    return getUsersResult;
  }

  public async getSearchUsersAsync(searchText: string, limit: number, page: number) {
    const getSearchUsers$ = this._usersApiService.searchUsers(searchText, limit, page);
    const getSearchUsersResult = await firstValueFrom(getSearchUsers$);

    this.searchedUsers$.next(getSearchUsersResult.response);

    return getSearchUsersResult;
  }
}
