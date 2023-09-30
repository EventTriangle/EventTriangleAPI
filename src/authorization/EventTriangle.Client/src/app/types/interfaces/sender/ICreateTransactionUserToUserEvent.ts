export interface ICreateTransactionUserToUserEvent {
  id: string;
  requesterId: string;
  toUserId: string;
  amount: number;
  createdAt: string;
}
