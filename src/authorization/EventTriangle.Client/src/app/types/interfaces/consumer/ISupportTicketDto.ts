import {TicketStatus} from "../../enums/TicketStatus";

export interface ISupportTicketDto {
  id: string;
  userId: string;
  wallerId: string;
  ticketReason: string;
  ticketJustification: string;
  ticketStatus: TicketStatus;
}
