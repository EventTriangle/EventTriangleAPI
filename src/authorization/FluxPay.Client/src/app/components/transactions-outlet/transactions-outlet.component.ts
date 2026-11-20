import {PaymentSubmissionService} from "../../services/state/payment-submission.service";
import { CopyTextDirective } from '../../directives/copy-text.directive';
import { TooltipModule } from 'primeng/tooltip';
import { SelectModule } from 'primeng/select';
import { SkeletonModule } from 'primeng/skeleton';
import { AngularSvgIconModule } from 'angular-svg-icon';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import {
  ChangeDetectorRef,
  Component,
  OnDestroy,
  OnInit,
} from '@angular/core';
import {animate, query, stagger, style, transition, trigger} from "@angular/animations";
import {BehaviorSubject, catchError, debounceTime, EMPTY, firstValueFrom, map, Subject, switchMap, takeUntil, timer} from "rxjs";
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
import {MessageService} from "primeng/api";

@Component({
  standalone: true,
  imports: [CommonModule, FormsModule, AngularSvgIconModule, SkeletonModule, SelectModule, TooltipModule, CopyTextDirective],
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
export class TransactionsOutletComponent implements OnInit, OnDestroy {
  private readonly destroyed$ = new Subject<void>();
  private searchRevision = 0;
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
  public contacts$ = this._contactStateService.contacts$.pipe(
    map(contacts => contacts.map(x => `${this._textService.pullUsernameFromMail(x.contact.email)} - ${x.contactId}`))
  );

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
    protected _dateService: DateService,
    private _messageService: MessageService,
    protected paymentSubmission: PaymentSubmissionService) {}

  async ngOnInit() {
    if (!this._profileStateService.isAuthenticated) return;

    await this._contactStateService.getContactsAsync();

    const date = new Date();
    if (this.transactions$.getValue().length === 0) await this._transactionsStateService.getTransactionsAsync(date, 20);

    this._changeDetectorRef.detectChanges();

    this.searchText$.pipe(
      switchMap(search => {
        this.searchRevision++;
        this.searchedTransactions$.next([]);
        if (!search.trim()) return EMPTY;
        return timer(400).pipe(switchMap(() =>
          this._transactionsApiService.getTransactionsBySearch(search, new Date(), 20).pipe(
            catchError(() => {
              this._messageService.add({severity: 'error', summary: 'Error', detail: 'Could not search transactions.'});
              return EMPTY;
            })
          )
        ));
      }),
      takeUntil(this.destroyed$)
    ).subscribe(result => this.searchedTransactions$.next(result.response));

    this.transactionListScrolledToEndNotifier$
      .pipe(debounceTime(400), takeUntil(this.destroyed$))
      .subscribe(async _ => {
              if (this.transactionListLoader) return;
              this.transactionListLoader = true;
              try {
                const transactions = this._transactionsStateService.transactions$.getValue();
                const lastTransaction = transactions[transactions.length - 1];
                if (lastTransaction) await this._transactionsStateService.getTransactionsAsync(lastTransaction.createdAt, 20, lastTransaction.id);
              } catch {
                this._messageService.add({severity: 'error', summary: 'Error', detail: 'Could not load more transactions.'});
              } finally {
                this.transactionListLoader = false;
              }
      });

    this.searchTransactionListScrolledToEndNotifier$
      .pipe(debounceTime(400), takeUntil(this.destroyed$))
      .subscribe(async _ => {
        if (this.transactionListLoader) return;
        this.transactionListLoader = true;
        try {
          const revision = this.searchRevision;
          const searchText = this.searchText$.getValue();
          const searchTransactions = this.searchedTransactions$.getValue();
          const lastSearchTransaction = searchTransactions[searchTransactions.length - 1];
          if (!lastSearchTransaction) return;
          const result = await firstValueFrom(this._transactionsApiService.getTransactionsBySearch(searchText, lastSearchTransaction.createdAt, 20, lastSearchTransaction.id));
          if (revision === this.searchRevision) this.searchedTransactions$.next([...searchTransactions, ...result.response]);
        } catch {
          this._messageService.add({severity: 'error', summary: 'Error', detail: 'Could not load more search results.'});
        } finally {
          this.transactionListLoader = false;
        }
      });
  }

  ngOnDestroy() {
    this.destroyed$.next();
    this.destroyed$.complete();
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
    if (!this.toUserId.trim()) {
      this._messageService.add({severity: 'error', summary: 'Error', detail: 'Select a recipient.'});
      return;
    }
    if (!/^[0-9]{1,13}$/.test(this.amount) || +this.amount <= 0) {
      this._messageService.add({severity: 'error', summary: 'Error', detail: ErrorMessageConstants.AmountValueIsIncorrect});
      return;
    }

    const operationId = this.paymentSubmission.start('transfer');
    if (!operationId) return;

    const submittedAmount = this.amount;
    const sendMoneyToUserSub$ = this._transactionsApiService.createTransactionUserToUser(this.toUserId.trim(), +submittedAmount, operationId);

    try {
      await firstValueFrom<IResult<ICreateTransactionUserToUserEvent>>(sendMoneyToUserSub$);
      if (this.amount === submittedAmount) this.amount = "";
    } catch {
      this.paymentSubmission.finish(operationId);
      this._messageService.add({severity: 'error', summary: 'Error', detail: 'Could not send the transfer.'});
    }
  }

  //other
  identifyTransactionDto(index: number, item: ITransactionDto){
    return item.id;
  }
}
