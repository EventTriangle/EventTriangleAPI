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

  public async getTransactionsAsync(fromDateTime: Date, limit: number) {
    const getTransactions$ = this._transactionsApiService.getTransactions(fromDateTime, limit);
    const getTransactionsResult = await firstValueFrom(getTransactions$);

    this.transactions$.next(getTransactionsResult.response);
    this.wasRequested = true;

    return getTransactionsResult;
  }
}
