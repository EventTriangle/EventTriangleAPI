export interface CreateTransactionCardToUserEvent {
  id: string;
  requesterId: string;
  creditCardId: string;
  amount: number;
  createdAt: string;
}
