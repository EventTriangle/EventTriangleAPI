import {Component, OnInit} from '@angular/core';
import { animate, style, transition, trigger } from "@angular/animations";
import {TransactionsApiService} from "../../services/api/transactions-api.service";
import {firstValueFrom} from "rxjs";
import {CreditCardsStateService} from "../../services/state/credit-cards-state.service";

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
export class DepositOutletComponent implements OnInit{
  //common
  public creditCardId: string = "";
  public amount: string = "";
  public creditCardIdArray: string[] = [];

  constructor(
      private _transactionsApiService: TransactionsApiService,
      private _creditCardStateService: CreditCardsStateService
  ) {}

  async ngOnInit() {
    await this._creditCardStateService.getCreditCardsAsync();
    this.creditCardIdArray = this._creditCardStateService.cards$.getValue().map(x => x.id);
  }

  //events
  public async onClickTopUpAccountBalanceHandler() {
    const topUpAccountBalance$ = this._transactionsApiService.topUpAccountBalance(this.creditCardId, +this.amount);

    this.creditCardId = "";
    this.amount = "";

    await firstValueFrom(topUpAccountBalance$);
  }
}
