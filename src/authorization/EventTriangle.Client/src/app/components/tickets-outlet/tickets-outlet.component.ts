import {Component, OnDestroy, OnInit} from '@angular/core';
import {animate, style, transition, trigger} from "@angular/animations";
import {TicketStateService} from "../../services/state/ticket-state.service";
import {ISupportTicketDto} from "../../types/interfaces/consumer/ISupportTicketDto";
import {debounceTime, firstValueFrom, fromEvent, Subscription} from "rxjs";
import {TransactionsApiService} from "../../services/api/transactions-api.service";

@Component({
  selector: 'app-tickets-outlet',
  templateUrl: './tickets-outlet.component.html',
  styleUrls: ['./tickets-outlet.component.scss'],
  animations: [
    trigger("ticketListAnimation", [
      transition(":enter", [
        style({transform: 'translateY(-10px)', opacity: 0}),
        animate(".3s", style({transform: 'translateY(0)', opacity: 1}))
      ])
    ]),
    trigger('ticketItemAnimation', [
      transition(':leave', [
        style({margin: 0}),
        animate('.3s', style({ height: 0, opacity: 0, "padding-top": 0, "padding-bottom": 0, "min-height": 0 }))
      ])
    ]),
    trigger("ticketListLoaderAnimation", [
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
export class TicketsOutletComponent implements OnInit, OnDestroy {
  //common
  public rollBackedTransactionIds: string[] = [];

  //state
  public ticketListLoaderAnimation = false;

  //observable
  public tickets$ = this._ticketStateService.tickets$;
  public documentScrollSub$: Subscription | undefined;

  constructor(
    protected _ticketStateService: TicketStateService,
    private _transactionApiService: TransactionsApiService
  ) {}

  async ngOnInit() {
    const date = new Date();
    if (this.tickets$.getValue().length === 0) await this._ticketStateService.getTicketsAsync(date, 10);

    this.documentScrollSub$ = fromEvent(document, "scroll")
      .pipe(
        debounceTime(400))
      .subscribe(async e => {
        const event = e as Event;
        const target = event.target as Document;
        if (target.body.scrollHeight - window.innerHeight <= window.scrollY + 15) {
          this.ticketListLoaderAnimation = true;
          const tickets = this._ticketStateService.tickets$.getValue();
          const lastTicket = tickets[tickets.length - 1];
          const date = new Date(lastTicket.createdAt);
          await this._ticketStateService.getTicketsAsync(date, 10);
          this.ticketListLoaderAnimation = false;
        }
      });
  }

  ngOnDestroy() {
    this.documentScrollSub$?.unsubscribe();
  }

  //events
  async onClickResolveTicketHandler(ticketId: string, ticketJustification: string) {
    await this._ticketStateService.resolveTicketAsync(ticketId, ticketJustification);
  }

  async onClickRollBackHandler(transactionId: string) {
    this.rollBackedTransactionIds.push(transactionId);
    const rollBackTransaction$ = this._transactionApiService.rollBackTransaction(transactionId);
    await firstValueFrom(rollBackTransaction$);
  }

  //common
  isTransactionRollBacked(transactionId: string) {
    return this.rollBackedTransactionIds.includes(transactionId);
  }

  //other
  identifyTicketDto(index: number, item: ISupportTicketDto){
    return item.id;
  }
}
