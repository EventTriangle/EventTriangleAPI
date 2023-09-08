export interface TransactionRolledBackEvent {
  id: string;
  requesterId: string;
  transactionId: string;
  createdAt: string;
}
