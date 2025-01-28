import { Injectable } from '@angular/core';
import {ITransactionDto} from "../../types/interfaces/consumer/ITransactionDto";
import {BehaviorSubject, firstValueFrom} from "rxjs";
import {TransactionsApiService} from "../api/transactions-api.service";

@Injectable({
  providedIn: 'root'
})
export class TransactionsStateService {
  public wasRequested = false;

  public transactions$: BehaviorSubject<ITransactionDto[]> = new BehaviorSubject<ITransactionDto[]>([]);

  constructor(
      private _transactionsApiService: TransactionsApiService
  ) { }

  //actions
  public addTransactionInTransactions(transaction: ITransactionDto) {
    const transactions = this.transactions$.getValue();
    transactions.unshift(transaction);
    this.transactions$.next(transactions);
  }

  //requests
  public async getTransactionsAsync(fromDateTime: Date, limit: number) {
    const getTransactions$ = this._transactionsApiService.getTransactions(fromDateTime, limit);
    const getTransactionsResult = await firstValueFrom(getTransactions$);

    const transactions = this.transactions$.getValue();
    transactions.push(...getTransactionsResult.response);

    this.transactions$.next(transactions);
    this.wasRequested = true;

    return getTransactionsResult;
  }
}
