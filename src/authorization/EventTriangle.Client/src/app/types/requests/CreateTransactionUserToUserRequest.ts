export class CreateTransactionUserToUserRequest {
  toUserId: string;
  amount: number;

  constructor(toUserId: string, amount: number) {
    this.toUserId = toUserId;
    this.amount = amount;
  }
}
