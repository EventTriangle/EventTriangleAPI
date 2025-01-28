import {TransactionType} from "../enums/TransactionType";

export interface ITransactionCanceledNotification {
    fromUserId: string;
    toUserId: string;
    amount: number;
    transactionType: TransactionType;
    createdAt: Date;
    reason: string;
}
