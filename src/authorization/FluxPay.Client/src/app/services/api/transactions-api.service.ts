import {Injectable} from "@angular/core";
import ApiBaseService from "./api-base.service";
import {HttpClient} from "@angular/common/http";
import {ITransactionDto} from "../../types/interfaces/consumer/ITransactionDto";
import {CreateTransactionUserToUserRequest} from "../../types/requests/CreateTransactionUserToUserRequest";
import {Observable} from "rxjs";
import {ICreateTransactionUserToUserEvent} from "../../types/interfaces/sender/ICreateTransactionUserToUserEvent";
import {ICreateTransactionCardToUserEvent} from "../../types/interfaces/sender/ICreateTransactionCardToUserEvent";
import {TopUpAccountBalanceRequest} from "../../types/requests/TopUpAccountBalanceRequest";
import {RollBackTransactionRequest} from "../../types/requests/RollBackTransactionRequest";
import {ITransactionRolledBackEvent} from "../../types/interfaces/sender/ITransactionRolledBackEvent";
import {IResult} from "../../types/interfaces/IResult";
import {pageParams} from './page-params';
import {ConfigService} from "../common/config.service";

@Injectable({
  providedIn: 'root'
})
export class TransactionsApiService extends ApiBaseService {
  private readonly consumerTransactionsRoute = "consumer/transactions";
  private readonly senderTransactionsRoute = "sender/transactions";
  private readonly baseUrl: string;

  constructor(
      private httpClient: HttpClient,
      private _configService: ConfigService
  ) {
    super()
    this.baseUrl = _configService.getServerUrl();
  }

  // requests

  // GET consumer/transactions
  public getTransactions(fromDateTime: Date | string, limit: number, cursorId?: string) : Observable<IResult<ITransactionDto[]>> {
    return this.httpClient.get<IResult<ITransactionDto[]>>(
      this.baseUrl + this.consumerTransactionsRoute,
      { withCredentials: true, params: pageParams(fromDateTime, limit, cursorId) }
    );
  }

  // GET consumer/transactions/search/{searchText}
  public getTransactionsBySearch(searchText: string, fromDateTime: Date | string, limit: number, cursorId?: string) : Observable<IResult<ITransactionDto[]>> {
    return this.httpClient.get<IResult<ITransactionDto[]>>(
      this.baseUrl + this.consumerTransactionsRoute + `/search/${encodeURIComponent(searchText)}`,
      { withCredentials: true, params: pageParams(fromDateTime, limit, cursorId) }
    );
  }

  // GET consumer/transactions/{userId}
  public getTransactionsByUserId(userId: string, fromDateTime: Date | string, limit: number, cursorId?: string) : Observable<IResult<ITransactionDto[]>> {
    return this.httpClient.get<IResult<ITransactionDto[]>>(
      this.baseUrl + this.consumerTransactionsRoute + `/${encodeURIComponent(userId)}`,
      { withCredentials: true, params: pageParams(fromDateTime, limit, cursorId) }
    );
  }

  // POST sender/user-to-user
  public createTransactionUserToUser(toUserId: string, amount: number, operationId?: string)
    : Observable<IResult<ICreateTransactionUserToUserEvent>> {
    let command : CreateTransactionUserToUserRequest = {
      toUserId: toUserId,
      amount,
      operationId
    };

    return this.httpClient.post<IResult<ICreateTransactionUserToUserEvent>>(
      this.baseUrl + this.senderTransactionsRoute + "/user-to-user",
      command, { withCredentials: true }
    );
  }

  // POST sender/card-to-user
  public topUpAccountBalance(creditCardId: string, amount: number, operationId?: string) : Observable<IResult<ICreateTransactionCardToUserEvent>> {
    let command : TopUpAccountBalanceRequest = {
      creditCardId: creditCardId,
      amount,
      operationId
    };

    return this.httpClient.post<IResult<ICreateTransactionCardToUserEvent>>(
      this.baseUrl + this.senderTransactionsRoute + "/card-to-user",
      command, { withCredentials: true }
    );
  }

  // POST sender/transactions/rollback
  public rollBackTransaction(transactionId: string) : Observable<IResult<ITransactionRolledBackEvent>> {
    let command : RollBackTransactionRequest = {
      transactionId: transactionId
    };

    return this.httpClient.post<IResult<ITransactionRolledBackEvent>>(
      this.baseUrl + this.senderTransactionsRoute + "/rollback",
      command, { withCredentials: true }
    );
  }
}
