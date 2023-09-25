import { Injectable } from '@angular/core';
import {TransactionDto} from "../../types/models/consumer/TransactionDto";

@Injectable({
  providedIn: 'root'
})
export class TransactionsStateService {
  public wasRequested = false;

  public transactions: TransactionDto[] = [];

  constructor() { }
}
