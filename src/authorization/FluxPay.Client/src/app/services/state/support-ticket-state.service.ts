import { Injectable } from '@angular/core';
import {BehaviorSubject, firstValueFrom} from "rxjs";
import {ISupportTicketDto} from "../../types/interfaces/consumer/ISupportTicketDto";
import {TicketsApiService} from "../api/tickets-api.service";

@Injectable({
  providedIn: 'root'
})
export class SupportTicketStateService {
  public wasRequested = false;

  public supportTickets$: BehaviorSubject<ISupportTicketDto[]> = new BehaviorSubject<ISupportTicketDto[]>([]);

  constructor(
      private _ticketsApiService: TicketsApiService
  ) { }

  //actions
  public addSupportTicket(supportTicketDto: ISupportTicketDto) {
    const supportTickets = this.supportTickets$.getValue();
    supportTickets.unshift(supportTicketDto);
    this.supportTickets$.next(supportTickets);
  }

  //requests
  public async getSupportTicketsAsync(fromDateTime: Date, limit: number) {
    const getSupportTickets$ = this._ticketsApiService.getSupportTickets(fromDateTime, limit);
    const getSupportTicketsResult = await firstValueFrom(getSupportTickets$);

    const supportTickets = this.supportTickets$.getValue();
    supportTickets.push(...getSupportTicketsResult.response);

    this.supportTickets$.next(supportTickets);
    this.wasRequested = true;

    return getSupportTicketsResult;
  }
}
