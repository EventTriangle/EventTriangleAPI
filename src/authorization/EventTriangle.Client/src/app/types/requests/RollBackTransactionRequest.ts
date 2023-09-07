export class RollBackTransactionRequest {
  transactionId: string;

  constructor(transactionId: string) {
    this.transactionId = transactionId;
  }
}
