import {Injectable} from "@angular/core";
import ApiBaseService from "./api-base.service";
import {HttpClient} from "@angular/common/http";
import {TransactionDto} from "../../types/models/consumer/TransactionDto";
import {CreateTransactionUserToUserRequest} from "../../types/requests/CreateTransactionUserToUserRequest";
import {Observable} from "rxjs";
import {CreateTransactionUserToUserEvent} from "../../types/models/sender/CreateTransactionUserToUserEvent";
import {CreateTransactionCardToUserEvent} from "../../types/models/sender/CreateTransactionCardToUserEvent";
import {TopUpAccountBalanceRequest} from "../../types/requests/TopUpAccountBalanceRequest";
import {RollBackTransactionRequest} from "../../types/requests/RollBackTransactionRequest";
import {TransactionRolledBackEvent} from "../../types/models/sender/TransactionRolledBackEvent";
import {Result} from "../../types/models/Result";

@Injectable({
  providedIn: 'root'
})
export class TransactionsApiService extends ApiBaseService {
  private readonly consumerTransactionsRoute = "consumer/transactions";
  private readonly senderTransactionsRoute = "sender/transactions";
  private readonly baseUrl: string;

  constructor(private httpClient: HttpClient) {
    super()
    this.baseUrl = super.getUrl();
  }

  // requests

  // GET consumer/transactions
  public getTransactions(fromDateTime: Date, limit: number) : Observable<Result<TransactionDto[]>> {
    return this.httpClient.get<Result<TransactionDto[]>>(
      this.baseUrl + this.consumerTransactionsRoute + `?fromDateTime=${fromDateTime.toJSON()}&limit=${limit}`,
      { withCredentials: true }
    );
  }

  // POST sender/user-to-user
  public createTransactionUserToUser(toUserId: string, amount: number)
    : Observable<Result<CreateTransactionUserToUserEvent>> {
    let command : CreateTransactionUserToUserRequest = {
      toUserId: toUserId,
      amount: amount
    };

    return this.httpClient.post<Result<CreateTransactionUserToUserEvent>>(
      this.baseUrl + this.senderTransactionsRoute + "user-to-user",
      command, { withCredentials: true }
    );
  }

  // POST sender/card-to-user
  public topUpAccountBalance(creditCardId: string, amount: number) : Observable<Result<CreateTransactionCardToUserEvent>> {
    let command : TopUpAccountBalanceRequest = {
      creditCardId: creditCardId,
      amount: amount
    };

    return this.httpClient.post<Result<CreateTransactionCardToUserEvent>>(
      this.baseUrl + this.senderTransactionsRoute + "card-to-user",
      command, { withCredentials: true }
    );
  }

  // POST sender/transactions/rollback
  public rollBackTransaction(transactionId: string) : Observable<Result<TransactionRolledBackEvent>> {
    let command : RollBackTransactionRequest = {
      transactionId: transactionId
    };

    return this.httpClient.post<Result<TransactionRolledBackEvent>>(
      this.baseUrl + this.senderTransactionsRoute + "rollback",
      command, { withCredentials: true }
    );
  }
}
