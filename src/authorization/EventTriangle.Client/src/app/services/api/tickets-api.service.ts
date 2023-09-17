import {Injectable} from "@angular/core";
import ApiBaseService from "./api-base.service";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {SupportTicketDto} from "../../types/models/consumer/SupportTicketDto";
import {OpenSupportTicketRequest} from "../../types/requests/OpenSupportTicketRequest";
import {SupportTicketOpenedEvent} from "../../types/models/sender/SupportTicketOpenedEvent";
import {SupportTicketResolved} from "../../types/models/sender/SupportTicketResolved";
import {ResolveSupportTickerRequest} from "../../types/requests/ResolveSupportTicketRequest";
import {Result} from "../../types/models/Result";

@Injectable({
  providedIn: 'root'
})
export class TicketsApiService extends ApiBaseService {
  private readonly consumerTicketsRoute = "consumer/tickets";
  private readonly senderTicketsRoute = "sender/tickets";
  private readonly baseUrl: string;

  constructor(private httpClient: HttpClient) {
    super()
    this.baseUrl = super.getUrl();
  }

  // requests

  // GET consumer/tickets
  public getTickets(fromDateTime: Date, limit: number) : Observable<Result<SupportTicketDto[]>> {
    return this.httpClient.get<Result<SupportTicketDto[]>>(
      this.baseUrl + this.consumerTicketsRoute + `?fromDateTime=${fromDateTime}&limit=${limit}`,
      { withCredentials: true }
    )
  }

  // GET consumer/tickets/support-ticket
  public getSupportTickets(fromDateTime: Date, limit: number) : Observable<Result<SupportTicketDto[]>> {
    return this.httpClient.get<Result<SupportTicketDto[]>>(
      this.baseUrl + this.consumerTicketsRoute + `support-tickets?fromDateTime=${fromDateTime}&limit=${limit}`,
      { withCredentials: true }
    )
  }

  // POST sender/tickets
  public openSupportTicket(walletId: string, transactionId: string, ticketReason: string)
    : Observable<Result<SupportTicketOpenedEvent>> {
    let command : OpenSupportTicketRequest = {
      walletId: walletId,
      transactionId: transactionId,
      ticketReason: ticketReason
    };

    return this.httpClient.post<Result<SupportTicketOpenedEvent>>(
      this.baseUrl + this.senderTicketsRoute,
      command, { withCredentials: true }
    );
  }

  // POST sender/tickets/support-ticket
  public resolveSupportTicket(ticketId: string, ticketJustification: string)
    : Observable<Result<SupportTicketResolved>> {
    let command : ResolveSupportTickerRequest = {
      ticketId: ticketId,
      ticketJustification: ticketJustification
    }

    return this.httpClient.post<Result<SupportTicketResolved>>(
      this.baseUrl + this.senderTicketsRoute + "sender-ticket",
      command, { withCredentials: true }
    );
  }
}
