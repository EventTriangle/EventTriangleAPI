import {Component, OnInit} from '@angular/core';
import {animate, query, stagger, style, transition, trigger} from "@angular/animations";
import {ProfileApiService} from "../../services/api/profile-api.service";
import {firstValueFrom} from "rxjs";
import {UserDto} from "../../types/models/consumer/UserDto";
import {Result} from "../../types/models/Result";
import {TransactionsApiService} from "../../services/api/transactions-api.service";
import {TransactionDto} from "../../types/models/consumer/TransactionDto";
import {TransactionType} from "../../types/enums/TransactionType";
import {TransactionState} from "../../types/enums/TransactionState";
import {CreateTransactionUserToUserEvent} from "../../types/models/sender/CreateTransactionUserToUserEvent";
import {TransactionsStateService} from "../../services/state/transactions-state.service";
import {ProfileStateService} from "../../services/state/profile-state.service";

@Component({
  selector: 'app-transactions-outlet',
  templateUrl: './transactions-outlet.component.html',
  styleUrls: ['./transactions-outlet.component.scss'],
  animations: [
    trigger('walletAnimation', [
      transition(':enter', [
        style({opacity: 0}),
        animate('.5s', style({opacity: 1}))
      ])
    ]),
    trigger('transactionItemAnimation', [
      transition(':enter', [
        query(':enter', style({transform: 'translateY(-5px)', opacity: 0}), {optional: true}),
        query(':enter', stagger('50ms', [
          animate('200ms', style({transform: 'translateY(0)', opacity: 1}))
        ]), {optional: true})
      ])
    ]),
    trigger('rightBarAnimation', [
      transition(':enter', [
        style({transform: 'translateY(10px)', opacity: 0}),
        animate('.25s', style({transform: 'translateY(0px)', opacity: 1}))
      ])
    ])
  ]
})
export class TransactionsOutletComponent implements OnInit {
  amount: number = 0;
  toUserId: string = '';

  protected readonly TransactionType = TransactionType;
  protected readonly TransactionState = TransactionState;

  constructor(private _profileApiService: ProfileApiService,
              private _transactionsApiService: TransactionsApiService,
              protected _transactionsStateService: TransactionsStateService,
              protected _profileStateService: ProfileStateService) {

  }

  async ngOnInit() {
    const getProfileSub$ = this._profileApiService.getProfile();
    const getProfileResult = await firstValueFrom<Result<UserDto>>(getProfileSub$);
    this._profileStateService.user = getProfileResult.response;

    const threeDaysBefore = new Date();
    threeDaysBefore.setDate(threeDaysBefore.getDate() - 3);

    const getTransactionsSub$ =
      this._transactionsApiService.getTransactions(threeDaysBefore, 25);
    const getTransactionsResult = await firstValueFrom<Result<TransactionDto[]>>(getTransactionsSub$);
    this._transactionsStateService.transactions = getTransactionsResult.response;
  }

  getTransactionClassName(transaction: TransactionDto) : string {
    if(transaction.transactionState === TransactionState.Completed)
    {
      if(transaction.toUserId === this._profileStateService.user.id) {
        return 'transactionItemToMe';
      } else {
        return 'transactionItemFromMe'
      }
    } else {
      return 'transactionItemRolledBack';
    }
  }

  getTransactionInfoClassName(transaction: TransactionDto) : string {
    if(transaction.transactionState === TransactionState.Completed)
    {
      if(transaction.toUserId === this._profileStateService.user.id) {
        return 'transactionItemToMeInfo';
      } else {
        return 'transactionItemFromMeInfo'
      }
    } else {
      return 'transactionItemRolledBackInfo';
    }
  }

  async getUserNameById(userId: string) : Promise<string> {
    const getUserProfileByIdSub$ = this._profileApiService.getProfileById(userId);
    const getUserProfileResponse = await firstValueFrom<Result<UserDto>>(getUserProfileByIdSub$);
    const user = getUserProfileResponse.response;

    return user.email.split('@')[0];
  }

  async sendMoneyToUser(toUserId: string, amount: number) : Promise<void> {
    const sendMoneyToUserSub$ =
      this._transactionsApiService.createTransactionUserToUser(toUserId,amount);
    await firstValueFrom<Result<CreateTransactionUserToUserEvent>>(sendMoneyToUserSub$);
    await this.ngOnInit();
  }
}
