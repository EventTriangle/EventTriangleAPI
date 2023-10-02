import {Component, OnInit} from '@angular/core';
import {animate, query, stagger, style, transition, trigger} from "@angular/animations";
import {ProfileApiService} from "../../services/api/profile-api.service";
import {firstValueFrom} from "rxjs";
import {IResult} from "../../types/interfaces/IResult";
import {TransactionsApiService} from "../../services/api/transactions-api.service";
import {ITransactionDto} from "../../types/interfaces/consumer/ITransactionDto";
import {TransactionType} from "../../types/enums/TransactionType";
import {TransactionState} from "../../types/enums/TransactionState";
import {ICreateTransactionUserToUserEvent} from "../../types/interfaces/sender/ICreateTransactionUserToUserEvent";
import {TransactionsStateService} from "../../services/state/transactions-state.service";
import {ProfileStateService} from "../../services/state/profile-state.service";
import {TextService} from "../../services/common/text.service";
import {DateService} from "../../services/common/date.service";

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
  //observable
  public user$ = this._profileStateService.user$;
  public transactions$ = this._transactionsStateService.transactions$;

  amount: number = 0;
  toUserId: string = '';

  protected readonly TransactionType = TransactionType;
  protected readonly TransactionState = TransactionState;

  constructor(
    private _profileApiService: ProfileApiService,
    private _transactionsApiService: TransactionsApiService,
    protected _transactionsStateService: TransactionsStateService,
    protected _profileStateService: ProfileStateService,
    protected _textService: TextService,
    protected _dateService: DateService) {}

  async ngOnInit() {
    if (!this._profileStateService.isAuthenticated) return;

    const date = new Date();
    await this._transactionsStateService.getTransactionsAsync(date, 25);
  }

  getTransactionClassName(transaction: ITransactionDto) : string {
    const userProfile = this._profileStateService.user$.getValue();

    if (!userProfile) throw new Error("User is null");
    if (transaction.transactionState === TransactionState.RolledBack) return "transactionItemRolledBack";
    if (transaction.toUserId === userProfile.id) return 'transactionItemToMe';

    return 'transactionItemFromMe'
  }

  getTransactionInfoClassName(transaction: ITransactionDto) : string {
    const userProfile = this._profileStateService.user$.getValue();

    if (!userProfile) throw new Error("User is null");
    if (transaction.transactionState === TransactionState.RolledBack) return "transactionItemRolledBackInfo";
    if (transaction.toUserId === userProfile.id) return 'transactionItemToMeInfo';

    return 'transactionItemFromMeInfo'
  }

  async sendMoneyToUser(toUserId: string, amount: number) : Promise<void> {
    const sendMoneyToUserSub$ = this._transactionsApiService.createTransactionUserToUser(toUserId,amount);
    await firstValueFrom<IResult<ICreateTransactionUserToUserEvent>>(sendMoneyToUserSub$);
    await this.ngOnInit();
  }

  //other
  identifyTransactionDto(index: number, item: ITransactionDto){
    return item.id;
  }
}
