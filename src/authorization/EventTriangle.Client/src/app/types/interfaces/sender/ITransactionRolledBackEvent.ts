export interface ITransactionRolledBackEvent {
  id: string;
  requesterId: string;
  transactionId: string;
  createdAt: string;
}
