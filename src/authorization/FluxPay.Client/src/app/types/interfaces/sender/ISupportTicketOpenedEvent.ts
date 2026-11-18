export interface ISupportTicketOpenedEvent {
  id: string;
  requesterId: string;
  walletId: string;
  transactionId: string;
  ticketReason: string;
  createdAt: string;
}
