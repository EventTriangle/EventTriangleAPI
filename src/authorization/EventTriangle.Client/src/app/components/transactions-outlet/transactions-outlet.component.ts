import {
  ChangeDetectorRef,
  Component,
  OnInit,
} from '@angular/core';
import {animate, query, stagger, style, transition, trigger} from "@angular/animations";
import {BehaviorSubject, debounceTime, filter, firstValueFrom, Subject} from "rxjs";
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
import {UserStatus} from "../../types/enums/UserStatus";
import {ErrorMessageConstants} from "../../constants/ErrorMessageConstants";

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
  // notifiers
  public transactionListScrolledToEndNotifier$ = new Subject<void>();
  public searchTransactionListScrolledToEndNotifier$ = new Subject<void>();

  //observable
  public searchText$ = new BehaviorSubject<string>("");
  public user$ = this._profileStateService.user$;
  public transactions$ = this._transactionsStateService.transactions$;
  public searchedTransactions$ = new BehaviorSubject<ITransactionDto[]>([]);

  //common
  public amount: string = '';
  public toUserId: string = '';
  public contacts: string[] = [];

  //state
  public transactionListLoader = false;

  //types
  protected readonly TransactionType = TransactionType;
  protected readonly TransactionState = TransactionState;
  protected readonly UserStatus = UserStatus;

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
    this.contacts = this._contactStateService.contacts$.getValue().map(x => `${this._textService.pullUsernameFromMail(x.contact.email)} - ${x.contactId}`);

    const date = new Date();
    if (this.transactions$.getValue().length === 0) await this._transactionsStateService.getTransactionsAsync(date, 20);

    this._changeDetectorRef.detectChanges();

    this.searchText$
      .pipe(
        filter(x => x.trim() !== ''),
        debounceTime(400))
      .subscribe(async x => {
        const getTransactionsBySearch$ = await this._transactionsApiService.getTransactionsBySearch(x, new Date(Date.now()), 20)
        const getTransactionsBySearchResult = await firstValueFrom(getTransactionsBySearch$);
        this.searchedTransactions$ = new BehaviorSubject<ITransactionDto[]>([]);
        setTimeout(() => {
          this.searchedTransactions$.next(getTransactionsBySearchResult.response);
        }, 10);
      });

    this.searchText$
      .pipe(filter(x => x === ''))
      .subscribe(_ => this.searchedTransactions$ = new BehaviorSubject<ITransactionDto[]>([]));

    this.transactionListScrolledToEndNotifier$
      .pipe(debounceTime(400))
      .subscribe(async _ => {
              this.transactionListLoader = true;
              const transactions = this._transactionsStateService.transactions$.getValue();
              const lastTransaction = transactions[transactions.length - 1];
              const date = new Date(lastTransaction.createdAt);
              await this._transactionsStateService.getTransactionsAsync(date, 20);
              this.transactionListLoader = false;
      });

    this.searchTransactionListScrolledToEndNotifier$
      .pipe(debounceTime(400))
      .subscribe(async _ => {
        this.transactionListLoader = true;
        const searchText = this.searchText$.getValue();
        const searchTransactions = this.searchedTransactions$.getValue();
        const lastSearchTransaction = searchTransactions[searchTransactions.length - 1];
        const date = new Date(lastSearchTransaction.createdAt);
        const getTransactionsBySearch$ = this._transactionsApiService.getTransactionsBySearch(searchText, date, 20);
        const result = await firstValueFrom(getTransactionsBySearch$);
        searchTransactions.push(...result.response);
        this.searchedTransactions$.next(searchTransactions);
        this.transactionListLoader = false;
      });
  }

  //events
  public onScrollTransactionsHandler(e: Event) {
    const target = e.target as HTMLElement;
    if (target.offsetHeight + target.scrollTop >= target.scrollHeight - 1) {
      this.transactionListScrolledToEndNotifier$.next(undefined);
    }
  }

  public onScrollSearchTransactionsHandler(e: Event) {
    const target = e.target as HTMLElement;
    if (target.offsetHeight + target.scrollTop >= target.scrollHeight - 1) {
      this.searchTransactionListScrolledToEndNotifier$.next(undefined);
    }
  }

  public onChangeDropDownHandler(event: any) {
    const value = event.value as string;
    const userId = value.split(" - ")[1];

    this.toUserId = userId ?? value;
    console.log(`new value: ${this.toUserId}`);
  }

  //common
  getTransactionColorClassName(transaction: ITransactionDto) : string {
    const userProfile = this._profileStateService.user$.getValue();

    if (!userProfile) throw new Error(ErrorMessageConstants.UserNotFound);
    if (transaction.transactionState === TransactionState.RolledBack) return "transactionItemColorRed";
    if (transaction.toUserId === userProfile.id) return 'transactionItemColorGreen';

    return 'transactionItemColorPurple'
  }

  async sendMoneyToUser() : Promise<void> {
    if (isNaN(+this.amount) || +this.amount <= 0) throw new Error(ErrorMessageConstants.AmountValueIsIncorrect);

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
