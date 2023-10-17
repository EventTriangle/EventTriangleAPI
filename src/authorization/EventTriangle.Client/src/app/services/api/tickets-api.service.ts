import {Injectable} from "@angular/core";
import ApiBaseService from "./api-base.service";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {ISupportTicketDto} from "../../types/interfaces/consumer/ISupportTicketDto";
import {OpenSupportTicketRequest} from "../../types/requests/OpenSupportTicketRequest";
import {ISupportTicketOpenedEvent} from "../../types/interfaces/sender/ISupportTicketOpenedEvent";
import {ISupportTicketResolved} from "../../types/interfaces/sender/ISupportTicketResolved";
import {ResolveSupportTickerRequest} from "../../types/requests/ResolveSupportTicketRequest";
import {IResult} from "../../types/interfaces/IResult";

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
  public getTickets(fromDateTime: Date, limit: number) : Observable<IResult<ISupportTicketDto[]>> {
    return this.httpClient.get<IResult<ISupportTicketDto[]>>(
      this.baseUrl + this.consumerTicketsRoute + `?fromDateTime=${fromDateTime.toJSON()}&limit=${limit}`,
      { withCredentials: true }
    )
  }

  // GET consumer/tickets/support-ticket
  public getSupportTickets(fromDateTime: Date, limit: number) : Observable<IResult<ISupportTicketDto[]>> {
    return this.httpClient.get<IResult<ISupportTicketDto[]>>(
      this.baseUrl + this.consumerTicketsRoute + `/support-tickets?fromDateTime=${fromDateTime.toJSON()}&limit=${limit}`,
      { withCredentials: true }
    )
  }

  // POST sender/tickets
  public openSupportTicket(walletId: string, transactionId: string, ticketReason: string)
    : Observable<IResult<ISupportTicketOpenedEvent>> {
    let command : OpenSupportTicketRequest = {
      walletId: walletId,
      transactionId: transactionId,
      ticketReason: ticketReason
    };

    return this.httpClient.post<IResult<ISupportTicketOpenedEvent>>(
      this.baseUrl + this.senderTicketsRoute + "/support-ticket",
      command, { withCredentials: true }
    );
  }

  // POST sender/tickets/support-ticket
  public resolveSupportTicket(ticketId: string, ticketJustification: string)
    : Observable<IResult<ISupportTicketResolved>> {
    let command : ResolveSupportTickerRequest = {
      ticketId: ticketId,
      ticketJustification: ticketJustification
    }

    return this.httpClient.put<IResult<ISupportTicketResolved>>(
      this.baseUrl + this.senderTicketsRoute + "/support-ticket",
      command, { withCredentials: true }
    );
  }
}
