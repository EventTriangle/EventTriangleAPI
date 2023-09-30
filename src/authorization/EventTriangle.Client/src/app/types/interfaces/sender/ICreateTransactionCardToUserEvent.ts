export interface ICreateTransactionCardToUserEvent {
  id: string;
  requesterId: string;
  creditCardId: string;
  amount: number;
  createdAt: string;
}
