import {TicketStatus} from "../../enums/TicketStatus";

export interface ISupportTicketDto {
  id: string;
  transactionId: string;
  userId: string;
  wallerId: string;
  ticketReason: string;
  ticketJustification: string;
  ticketStatus: TicketStatus;
}
