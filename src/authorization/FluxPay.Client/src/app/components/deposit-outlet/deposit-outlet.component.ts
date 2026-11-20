import { NgxMaskDirective } from 'ngx-mask';
import { SelectModule } from 'primeng/select';
import { AngularSvgIconModule } from 'angular-svg-icon';
import { FormsModule } from '@angular/forms';
import {Component, OnInit} from '@angular/core';
import { animate, style, transition, trigger } from "@angular/animations";
import {TransactionsApiService} from "../../services/api/transactions-api.service";
import {firstValueFrom} from "rxjs";
import {CreditCardsStateService} from "../../services/state/credit-cards-state.service";
import {MessageService} from "primeng/api";

@Component({
  standalone: true,
  imports: [FormsModule, AngularSvgIconModule, SelectModule, NgxMaskDirective],
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
      private _creditCardStateService: CreditCardsStateService,
      private _messageService: MessageService
  ) {}

  async ngOnInit() {
    await this._creditCardStateService.getCreditCardsAsync();
    this.creditCardIdArray = this._creditCardStateService.cards$.getValue().map(x => x.id);
  }

  //events
  public async onClickTopUpAccountBalanceHandler() {
    if (!this.creditCardId.trim() || !Number.isFinite(+this.amount) || +this.amount <= 0) {
      this._messageService.add({severity: 'error', summary: 'Error', detail: 'Select a card and enter an amount greater than zero.'});
      return;
    }
    const topUpAccountBalance$ = this._transactionsApiService.topUpAccountBalance(this.creditCardId, +this.amount);

    try {
      await firstValueFrom(topUpAccountBalance$);
      this.creditCardId = "";
      this.amount = "";
    } catch {
      this._messageService.add({severity: 'error', summary: 'Error', detail: 'Could not submit the deposit.'});
    }
  }
}
