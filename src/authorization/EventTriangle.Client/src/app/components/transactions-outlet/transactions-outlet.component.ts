import {
  ChangeDetectorRef,
  Component,
  ElementRef,
  OnInit,
  ViewChild,
} from '@angular/core';
import {animate, query, stagger, style, transition, trigger} from "@angular/animations";
import {debounceTime, firstValueFrom, fromEvent} from "rxjs";
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
import {ContactsStateService} from "../../services/state/contacts-state.service";

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
    trigger('transactionsAnimation', [
      transition(':enter', [
        query(':enter', style({transform: 'translateY(-5px)', opacity: 0}), {optional: true}),
        query(':enter', stagger('50ms', [
          animate('200ms', style({transform: 'translateY(0)', opacity: 1}))
        ]), {optional: true})
      ])
    ]),
    trigger('transactionItemAnimation', [
      transition(':enter', [
        style({ height: 0, opacity: 0, padding: 0, margin: 0 }),
        animate('.25s', style({ height: "*", opacity: "*", padding: "*", margin: "*" }))
      ])
    ]),
    trigger('rightBarAnimation', [
      transition(':enter', [
        style({transform: 'translateY(10px)', opacity: 0}),
        animate('.25s', style({transform: 'translateY(0px)', opacity: 1}))
      ])
    ]),
    trigger("transactionListLoaderAnimation", [
      transition(":enter", [
        style({transform: 'translateY(10px)', opacity: 0}),
        animate('.25s', style({transform: 'translateY(0px)', opacity: 1}))
      ]),
      transition(":leave", [
        style({transform: 'translateY(0px)', opacity: 1}),
        animate('.25s', style({transform: 'translateY(10px)', opacity: 0}))
      ])
    ])
  ]
})
export class TransactionsOutletComponent implements OnInit {
  @ViewChild("transactionListRef") transactionListRef!: ElementRef;

  //observable
  public user$ = this._profileStateService.user$;
  public transactions$ = this._transactionsStateService.transactions$;

  //common
  public amount: string = '';
  public toUserId: string = '';
  public contacts: string[] = [];

  //state
  public transactionListLoader = false;

  //types
  protected readonly TransactionType = TransactionType;
  protected readonly TransactionState = TransactionState;

  constructor(
    private _transactionsApiService: TransactionsApiService,
    private _changeDetectorRef: ChangeDetectorRef,
    protected _transactionsStateService: TransactionsStateService,
    protected _profileStateService: ProfileStateService,
    protected _contactStateService: ContactsStateService,
    protected _textService: TextService,
    protected _dateService: DateService) {}

  async ngOnInit() {
    if (!this._profileStateService.isAuthenticated) return;

    await this._contactStateService.getContactsAsync();
    this.contacts = this._contactStateService.contacts$.getValue().map(x => x.contactId);

    const date = new Date();
    if (this.transactions$.getValue().length === 0) await this._transactionsStateService.getTransactionsAsync(date, 20);

    this._changeDetectorRef.detectChanges();

    fromEvent(this.transactionListRef.nativeElement, "scroll")
      .pipe(
        debounceTime(400))
      .subscribe(async e => {
        const event = e as Event;
        const target = event.target as HTMLElement;
        if (target.offsetHeight + target.scrollTop >= target.scrollHeight - 1) {
          this.transactionListLoader = true;
          const transactions = this._transactionsStateService.transactions$.getValue();
          const lastTransaction = transactions[transactions.length - 1];
          const date = new Date(lastTransaction.createdAt);
          await this._transactionsStateService.getTransactionsAsync(date, 20);
          this.transactionListLoader = false;
        }
      });
  }

  //common
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

  async sendMoneyToUser() : Promise<void> {
    if (isNaN(+this.amount) || +this.amount <= 0) throw new Error("Amount value is incorrect");

    const sendMoneyToUserSub$ = this._transactionsApiService.createTransactionUserToUser(this.toUserId.trim(), +this.amount);

    this.amount = "";

    await firstValueFrom<IResult<ICreateTransactionUserToUserEvent>>(sendMoneyToUserSub$);
    await this.ngOnInit();
  }

  //other
  identifyTransactionDto(index: number, item: ITransactionDto){
    return item.id;
  }
}
