import {Component, OnInit} from '@angular/core';
import {animate, query, stagger, style, transition, trigger} from "@angular/animations";
import {ProfileApiService} from "../../services/api/profile-api.service";
import {firstValueFrom} from "rxjs";
import {UserDto} from "../../types/models/consumer/UserDto";
import {Result} from "../../types/models/Result";
import {TransactionsOutletHelper} from "./transactions-outlet.helper";
import {TransactionsApiService} from "../../services/api/transactions-api.service";
import {TransactionDto} from "../../types/models/consumer/TransactionDto";
import {TransactionType} from "../../types/enums/TransactionType";
import {TransactionState} from "../../types/enums/TransactionState";

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
  transactions: TransactionDto[] = [];
  user: UserDto = this._transactionsOutletHelper.generateEmptyUser();
  protected readonly TransactionType = TransactionType;
  protected readonly TransactionState = TransactionState;

  constructor(private _transactionsOutletHelper: TransactionsOutletHelper,
              private _profileApiService: ProfileApiService,
              private _transactionsApiService: TransactionsApiService) {

  }

  async ngOnInit() {
    const getProfileSub$ = this._profileApiService.getProfile();
    const getProfileResult = await firstValueFrom<Result<UserDto>>(getProfileSub$);
    this.user = getProfileResult.response;

    const threeDaysBefore = new Date();
    threeDaysBefore.setDate(threeDaysBefore.getDate() - 3);

    const getTransactionsSub$ =
      this._transactionsApiService.getTransactions(threeDaysBefore, 25);
    const getTransactionsResult = await firstValueFrom<Result<TransactionDto[]>>(getTransactionsSub$);
    this.transactions = getTransactionsResult.response;
  }

  getTransactionClassName(transaction: TransactionDto) : string {
    if(transaction.transactionState === TransactionState.Completed)
    {
      if(transaction.toUserId === this.user.id) {
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
      if(transaction.toUserId === this.user.id) {
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
}
