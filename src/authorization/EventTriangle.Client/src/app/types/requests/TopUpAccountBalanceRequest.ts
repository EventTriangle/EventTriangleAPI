export class TopUpAccountBalanceRequest {
  creditCardId: string;
  amount: number;

  constructor(creditCardId: string, amount: number) {
    this.creditCardId = creditCardId;
    this.amount = amount;
  }
}
