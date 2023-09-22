import { Injectable } from '@angular/core';
import {TransactionDto} from "../../types/models/consumer/TransactionDto";

@Injectable({
  providedIn: 'root'
})
export class TransactionsStateService {
  public transactions: TransactionDto[] = [];

  constructor() { }
}
