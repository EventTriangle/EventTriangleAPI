import {TicketStatus} from "../../enums/TicketStatus";

export interface SupportTicketDto {
  id: string;
  userId: string;
  wallerId: string;
  ticketReason: string;
  ticketJustification: string;
  ticketStatus: TicketStatus;
}
