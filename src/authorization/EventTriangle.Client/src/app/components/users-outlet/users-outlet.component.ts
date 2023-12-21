import {Component, OnInit} from '@angular/core';
import {animate, query, stagger, style, transition, trigger} from "@angular/animations";
import {UsersStateService} from "../../services/state/users-state.service";
import {BehaviorSubject, debounceTime, filter, firstValueFrom} from "rxjs";
import {TextService} from "../../services/common/text.service";
import {IUserDto} from "../../types/interfaces/consumer/IUserDto";
import {ProfileStateService} from "../../services/state/profile-state.service";
import {UserStatus} from "../../types/enums/UserStatus";
import {UsersApiService} from "../../services/api/users-api.service";
import {TransactionsApiService} from "../../services/api/transactions-api.service";
import {ITransactionDto} from "../../types/interfaces/consumer/ITransactionDto";
import {DateService} from "../../services/common/date.service";
import {TransactionState} from "../../types/enums/TransactionState";

@Component({
  selector: 'app-users-outlet',
  templateUrl: './users-outlet.component.html',
  styleUrls: ['./users-outlet.component.scss'],
  animations: [
    trigger('userListAnimation', [
      transition(':enter', [
        query(':enter', style({ marginTop: -5, opacity: 0 }), { optional: true }),
        query(':enter', stagger('50ms', [
          animate('150ms', style({ marginTop: 0, opacity: 1 }))
        ]), { optional: true })
      ])
    ]),
  ]
})
export class UsersOutletComponent implements OnInit {
  //observable
  public users$ = this._usersStateService.users$;
  public currentUserInfo$ = new BehaviorSubject<IUserDto | null>(null);
  public currentUserTransactionsInfo$ = new BehaviorSubject<ITransactionDto[]>([]);
  public searchedUsers$ = this._usersStateService.searchedUsers$;
  public searchText$ = new BehaviorSubject<string>("");

  //state
  public currentUserTransactionsInfoLoader = false;
  public showUserInfoWindow = false;

  //types
  public UserStatus = UserStatus;

  constructor(
      private _usersApiService: UsersApiService,
      protected _usersStateService: UsersStateService,
      protected _transactionsApiService: TransactionsApiService,
      protected _profileStateService: ProfileStateService,
      protected _textService: TextService,
      protected _dateService: DateService
  ) {}

  async ngOnInit() {
    if (!this._profileStateService.isAuthenticated) return;

    await this._usersStateService.getUsersAsync(25, 1);

    this.searchText$
        .pipe(
            filter(x => x.trim() !== ''),
            debounceTime(400))
        .subscribe(async x => await this._usersStateService.getSearchUsersAsync(x, 25, 1));

    this.searchText$
        .pipe(filter(x => x === ''))
        .subscribe(_ => this._usersStateService.clearSearchedUsers())
  }

  //events
  public async onChangeSuspendOrMakeActiveHandler(user: IUserDto) {
    if (user.userStatus.toString() === UserStatus.Suspended.toString()) {
      const suspendUser$ = this._usersApiService.suspend(user.id);
      await firstValueFrom(suspendUser$);
      return;
    }

    const notSuspendUser$ = this._usersApiService.notSuspend(user.id);
    await firstValueFrom(notSuspendUser$);
  }

  public async onClickGetUserTransactionsInfoHandler(userId: string) {
    this.showUserInfoWindow = true;
    let user = this._usersStateService.users$.getValue().find(x => x.id === userId);
    if (!user) {
      user = this._usersStateService.searchedUsers$.getValue().find(x => x.id === userId);
    }
    if (!user) throw new Error("User not found");
    this.currentUserInfo$.next(user);
    this.currentUserTransactionsInfoLoader = true;
    const getTransactionsByUserId$ = this._transactionsApiService.getTransactionsByUserId(userId, new Date(Date.now()), 15);
    const getTransactionsByUserIdResult = await firstValueFrom(getTransactionsByUserId$);
    this.currentUserTransactionsInfo$.next(getTransactionsByUserIdResult.response);
    this.currentUserTransactionsInfoLoader = false;
  }

  //actions
  public closeUserTransactionsInfo() {
    this.showUserInfoWindow = false;
    this.currentUserInfo$.next(null);
    this.currentUserTransactionsInfo$.next([]);
  }

  //common
  public getTransactionClassName(userId: string, transaction: ITransactionDto) : string {
    let user = this._usersStateService.users$.getValue().find(x => x.id === userId);
    if (!user) {
      user = this._usersStateService.searchedUsers$.getValue().find(x => x.id === userId);
    }

    if (!user) throw new Error("User not found");
    if (transaction.transactionState === TransactionState.RolledBack) return "userInfoWindowBodyTransactionRolledBack";
    if (transaction.toUserId === user.id) return 'userInfoWindowBodyTransactionToMe';

    return 'userInfoWindowBodyTransactionFromMe'
  }

  //other
  identifyUserDto(index: number, item: IUserDto){
    return item.id;
  }

  protected readonly console = console;
}
