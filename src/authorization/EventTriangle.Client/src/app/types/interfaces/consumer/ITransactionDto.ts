import {TransactionState} from "../../enums/TransactionState";
import {TransactionType} from "../../enums/TransactionType";

export interface ITransactionDto {
  id: string;
  fromUserId: string;
  toUserId: string;
  amount: number;
  transactionState: TransactionState;
  transactionType: TransactionType;
  createdAt: string;
}
