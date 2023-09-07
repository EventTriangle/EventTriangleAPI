export interface CreateTransactionUserToUserEvent {
  id: string;
  requesterId: string;
  toUserId: string;
  amount: number;
  createdAt: string;
}
