import {Component, OnDestroy, OnInit} from '@angular/core';
import { animate, style, transition, trigger } from "@angular/animations";
import {TicketsApiService} from "../../services/api/tickets-api.service";
import {debounceTime, firstValueFrom, fromEvent, Subscription} from "rxjs";
import {SupportTicketStateService} from "../../services/state/support-ticket-state.service";
import {ISupportTicketDto} from "../../types/interfaces/consumer/ISupportTicketDto";
import {ProfileStateService} from "../../services/state/profile-state.service";

@Component({
  selector: 'app-support-outlet',
  templateUrl: './support-outlet.component.html',
  styleUrls: ['./support-outlet.component.scss'],
  animations: [
    trigger("supportTicketListAnimation", [
      transition(":enter", [
        style({ transform: 'translateY(-10px)', opacity: 0 }),
        animate(".3s", style({ transform: 'translateY(0)', opacity: 1 }))
      ])
    ]),
    trigger('supportTicketItemAnimation', [
      transition(':enter', [
        style({ height: 0, opacity: 0, "padding-top": 0, "padding-bottom": 0, margin: 0, "min-height": 0 }),
        animate('.4s', style({ height: "*", opacity: "*", "padding-top": "*", "padding-bottom": "*", margin: "*", "min-height": "*" }))
      ])
    ]),
    trigger('rightBarAnimation', [
      transition(':enter', [
        style({ transform: 'translateY(10px)', opacity: 0 }),
        animate('.25s', style({ transform: 'translateY(0px)', opacity: 1 }))
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
export class SupportOutletComponent implements OnInit, OnDestroy {
  //common
  public transactionId: string = "";
  public ticketReason: string = "";

  //state
  public ticketListLoaderAnimation = false;

  //observable
  public supportTickets$ = this._supportTicketStateService.supportTickets$;
  public documentScrollSub$: Subscription | undefined;

  constructor(
      private _ticketsApiService: TicketsApiService,
      protected _supportTicketStateService: SupportTicketStateService,
      protected _profileStateService : ProfileStateService
  ) {}

  async ngOnInit() {
    const date = new Date();
    if (this.supportTickets$.getValue().length === 0) await this._supportTicketStateService.getSupportTicketsAsync(date, 10);

    this.documentScrollSub$ = fromEvent(document, "scroll")
      .pipe(
        debounceTime(400))
      .subscribe(async e => {
        const event = e as Event;
        const target = event.target as Document;
        if (target.body.scrollHeight - window.innerHeight <= window.scrollY + 15) {
          this.ticketListLoaderAnimation = true;
          const tickets = this._supportTicketStateService.supportTickets$.getValue();
          const lastTicket = tickets[tickets.length - 1];
          const date = new Date(lastTicket.createdAt);
          await this._supportTicketStateService.getSupportTicketsAsync(date, 10);
          this.ticketListLoaderAnimation = false;
        }
      });
  }

  ngOnDestroy() {
    this.documentScrollSub$?.unsubscribe();
  }

  //events
  async onClickOpenSupportTicketHandler() {
    const walletId = this._profileStateService.user$.getValue()?.walletId ?? "";

    const openSupportTicket$ = this._ticketsApiService.openSupportTicket(walletId, this.transactionId, this.ticketReason);

    this.transactionId = "";
    this.ticketReason = "";

    await firstValueFrom(openSupportTicket$);
  }

  //other
  identifySupportTicketDto(index: number, item: ISupportTicketDto){
    return item.id;
  }
}
