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
  public getTransactions(fromDateTime: Date, limit: number) : Observable<TransactionDto[]> {
    return this.httpClient.get<TransactionDto[]>(
      this.baseUrl + this.consumerTransactionsRoute + `?fromDateTime=${fromDateTime.toJSON()}&limit=${limit}`
    );
  }

  // POST sender/user-to-user
  public createTransactionUserToUser(toUserId: string, amount: number) : Observable<CreateTransactionUserToUserEvent> {
    let command = new CreateTransactionUserToUserRequest(toUserId, amount);

    return this.httpClient.post<CreateTransactionUserToUserEvent>(
      this.baseUrl + this.senderTransactionsRoute + "user-to-user",
      command
    );
  }

  // POST sender/card-to-user
  public topUpAccountBalance(creditCardId: string, amount: number) : Observable<CreateTransactionCardToUserEvent> {
    let command = new TopUpAccountBalanceRequest(creditCardId, amount);

    return this.httpClient.post<CreateTransactionCardToUserEvent>(
      this.baseUrl + this.senderTransactionsRoute + "card-to-user",
      command
    );
  }

  public rollBackTransaction(transactionId: string) : Observable<TransactionRolledBackEvent> {
    let command = new RollBackTransactionRequest(transactionId);

    return this.httpClient.post<TransactionRolledBackEvent>(
      this.baseUrl + this.senderTransactionsRoute + "rollback",
      command
    );
  }
}
