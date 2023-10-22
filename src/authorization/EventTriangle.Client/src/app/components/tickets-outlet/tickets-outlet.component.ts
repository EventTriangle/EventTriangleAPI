import {Component, OnInit} from '@angular/core';
import {animate, style, transition, trigger} from "@angular/animations";
import {TicketStateService} from "../../services/state/ticket-state.service";
import {ISupportTicketDto} from "../../types/interfaces/consumer/ISupportTicketDto";

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
  ]
})
export class TicketsOutletComponent implements OnInit {
  //observable
  public tickets$ = this._ticketStateService.tickets$;

  constructor(
      protected _ticketStateService: TicketStateService
  ) {}

  async ngOnInit() {
    const date = new Date();
    await this._ticketStateService.getTicketsAsync(date, 25);
  }

  //events
  async onClickResolveTicketHandler(ticketId: string, ticketJustification: string) {
    await this._ticketStateService.resolveTicketAsync(ticketId, ticketJustification);
  }

  //other
  identifyTicketDto(index: number, item: ISupportTicketDto){
    return item.id;
  }
}
