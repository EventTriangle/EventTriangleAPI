export interface SupportTicketOpenedEvent {
  id: string;
  requesterId: string;
  walletId: string;
  transactionId: string;
  ticketReason: string;
  createdAt: string;
}
