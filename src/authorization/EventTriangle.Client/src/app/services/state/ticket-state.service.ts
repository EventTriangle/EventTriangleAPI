import { Injectable } from '@angular/core';
import {BehaviorSubject, firstValueFrom} from "rxjs";
import {ISupportTicketDto} from "../../types/interfaces/consumer/ISupportTicketDto";
import {TicketsApiService} from "../api/tickets-api.service";

@Injectable({
  providedIn: 'root'
})
export class TicketStateService {
  public wasRequested = false;

  public tickets$: BehaviorSubject<ISupportTicketDto[]> = new BehaviorSubject<ISupportTicketDto[]>([]);

  constructor(
      private _ticketsApiService: TicketsApiService
  ) { }

  //requests
  public async getTicketsAsync(fromDateTime: Date, limit: number) {
    const getTickets$ = this._ticketsApiService.getTickets(fromDateTime, limit);
    const getTicketsResult = await firstValueFrom(getTickets$);

    this.tickets$.next(getTicketsResult.response);
    this.wasRequested = true;

    return getTicketsResult;
  }

  public async resolveTicketAsync(ticketId: string, ticketJustification: string) {
    const resolveTicket$ = this._ticketsApiService.resolveSupportTicket(ticketId, ticketJustification);
    const resolveTicketResult = await firstValueFrom(resolveTicket$);

    const tickets = this.tickets$.getValue();
    const newTicketsArray = tickets.filter(x => x.id !== ticketId);

    this.tickets$.next(newTicketsArray);

    return resolveTicketResult;
  }
}