import {Component, OnInit} from '@angular/core';
import { animate, style, transition, trigger } from "@angular/animations";
import {TicketsApiService} from "../../services/api/tickets-api.service";
import {firstValueFrom} from "rxjs";
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
    trigger('rightBarAnimation', [
      transition(':enter', [
        style({ transform: 'translateY(10px)', opacity: 0 }),
        animate('.25s', style({ transform: 'translateY(0px)', opacity: 1 }))
      ])
    ])
  ]
})
export class SupportOutletComponent implements OnInit{
  public transactionId: string = "";
  public ticketReason: string = "";

  //observable
  public supportTickets$ = this._supportTicketStateService.supportTickets$;

  constructor(
      private _ticketsApiService: TicketsApiService,
      protected _supportTicketStateService: SupportTicketStateService,
      protected _profileStateService : ProfileStateService
  ) {}

  async ngOnInit() {
    const date = new Date();
    await this._supportTicketStateService.getSupportTicketsAsync(date, 25);
  }

  //events
  async onClickOpenSupportTicketHandler() {
    const walletId = this._profileStateService.user$.getValue()?.walletId ?? "";
    this.transactionId = "";
    this.ticketReason = "";

    const openSupportTicket$ = this._ticketsApiService.openSupportTicket(walletId, this.transactionId, this.ticketReason);
    await firstValueFrom(openSupportTicket$);
  }

  //other
  identifySupportTicketDto(index: number, item: ISupportTicketDto){
    return item.id;
  }
}
