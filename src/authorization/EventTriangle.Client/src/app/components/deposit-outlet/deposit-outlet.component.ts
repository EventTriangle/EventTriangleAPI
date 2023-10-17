import { Component } from '@angular/core';
import { animate, style, transition, trigger } from "@angular/animations";
import {TransactionsApiService} from "../../services/api/transactions-api.service";
import {firstValueFrom} from "rxjs";

@Component({
  selector: 'app-deposit-outlet',
  templateUrl: './deposit-outlet.component.html',
  styleUrls: ['./deposit-outlet.component.scss'],
  animations: [
    trigger('depositFormAnimation', [
      transition(':enter', [
        style({ transform: 'translateY(10px)', opacity: 0 }),
        animate('.25s', style({ transform: 'translateY(0px)', opacity: 1 }))
      ])
    ])
  ]
})
export class DepositOutletComponent {
  public creditCardId: string = "";
  public amount: string = "";

  constructor(
      private _transactionsApiService: TransactionsApiService
  ) {}

  //events
  public async onClickTopUpAccountBalanceHandler() {
    console.log(this.amount);
    const topUpAccountBalance$ = this._transactionsApiService.topUpAccountBalance(this.creditCardId, +this.amount);
    await firstValueFrom(topUpAccountBalance$);
  }
}
