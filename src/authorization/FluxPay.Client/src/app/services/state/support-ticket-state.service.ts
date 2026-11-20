import { Injectable } from '@angular/core';
import {BehaviorSubject, firstValueFrom} from "rxjs";
import {ISupportTicketDto} from "../../types/interfaces/consumer/ISupportTicketDto";
import {TicketsApiService} from "../api/tickets-api.service";

@Injectable({
  providedIn: 'root'
})
export class SupportTicketStateService {
  public wasRequested = false;
  private snapshotVersion = 0;

  public supportTickets$: BehaviorSubject<ISupportTicketDto[]> = new BehaviorSubject<ISupportTicketDto[]>([]);

  constructor(
      private _ticketsApiService: TicketsApiService
  ) { }

  //actions
  public addSupportTicket(supportTicketDto: ISupportTicketDto) {
    this.supportTickets$.next([supportTicketDto, ...this.supportTickets$.getValue().filter(item => item.id !== supportTicketDto.id)]);
  }

  public updateSupportTicket(ticket: ISupportTicketDto) {
    this.supportTickets$.next(this.supportTickets$.getValue().map(item => item.id === ticket.id ? ticket : item));
  }

  //requests
  public async refreshSupportTicketsAsync() {
    const version = ++this.snapshotVersion;
    const result = await firstValueFrom(this._ticketsApiService.getSupportTickets(new Date(), 10));
    if (version === this.snapshotVersion) this.supportTickets$.next(result.response);
    this.wasRequested = true;
  }

  public async getSupportTicketsAsync(fromDateTime: Date | string, limit: number, cursorId?: string) {
    const version = this.snapshotVersion;
    const getSupportTickets$ = this._ticketsApiService.getSupportTickets(fromDateTime, limit, cursorId);
    const getSupportTicketsResult = await firstValueFrom(getSupportTickets$);

    if (version === this.snapshotVersion) {
      const supportTickets = this.supportTickets$.getValue();
      const ids = new Set(supportTickets.map(item => item.id));
      this.supportTickets$.next([...supportTickets, ...getSupportTicketsResult.response.filter(item => !ids.has(item.id))]);
    }
    this.wasRequested = true;

    return getSupportTicketsResult;
  }
}
