import { Injectable } from '@angular/core';
import {ITransactionDto} from "../../types/interfaces/consumer/ITransactionDto";
import {BehaviorSubject, firstValueFrom} from "rxjs";
import {TransactionsApiService} from "../api/transactions-api.service";

@Injectable({
  providedIn: 'root'
})
export class TransactionsStateService {
  public wasRequested = false;
  private snapshotVersion = 0;

  public transactions$: BehaviorSubject<ITransactionDto[]> = new BehaviorSubject<ITransactionDto[]>([]);

  constructor(
      private _transactionsApiService: TransactionsApiService
  ) { }

  //actions
  public addTransactionInTransactions(transaction: ITransactionDto) {
    this.transactions$.next([transaction, ...this.transactions$.getValue().filter(item => item.id !== transaction.id)]);
  }

  //requests
  public async refreshTransactionsAsync() {
    const version = ++this.snapshotVersion;
    const result = await firstValueFrom(this._transactionsApiService.getTransactions(new Date(), 20));
    if (version === this.snapshotVersion) this.transactions$.next(result.response);
    this.wasRequested = true;
  }

  public async getTransactionsAsync(fromDateTime: Date | string, limit: number, cursorId?: string) {
    const version = this.snapshotVersion;
    const getTransactions$ = this._transactionsApiService.getTransactions(fromDateTime, limit, cursorId);
    const getTransactionsResult = await firstValueFrom(getTransactions$);

    if (version === this.snapshotVersion) {
      const transactions = this.transactions$.getValue();
      const ids = new Set(transactions.map(item => item.id));
      this.transactions$.next([...transactions, ...getTransactionsResult.response.filter(item => !ids.has(item.id))]);
    }
    this.wasRequested = true;

    return getTransactionsResult;
  }
}
