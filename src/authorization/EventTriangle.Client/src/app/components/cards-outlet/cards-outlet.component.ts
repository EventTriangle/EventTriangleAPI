import {Component, OnInit} from '@angular/core';
import {animate, query, stagger, style, transition, trigger} from "@angular/animations";
import {CreditCardApiService} from "../../services/api/credit-card-api.service";
import {PaymentNetwork} from "../../types/enums/PaymentNetwork";
import {firstValueFrom} from "rxjs";
import {CreditCardAddedEvent} from "../../types/models/sender/CreditCardAddedEvent";
import {CreditCardDto} from "../../types/models/consumer/CreditCardDto";
import {Result} from "../../types/models/Result";

@Component({
  selector: 'app-cards-outlet',
  templateUrl: './cards-outlet.component.html',
  styleUrls: ['./cards-outlet.component.scss'],
  animations: [
    trigger('cardListAnimation', [
      transition(':enter', [
        query(':enter', style({ transform: 'translateY(-5px)', opacity: 0 }), { optional: true }),
        query(':enter', stagger('100ms', [
          animate('200ms', style({ transform: 'translateY(0)', opacity: 1 }))
        ]), { optional: true })
      ])
    ]),
    trigger('rightBarAnimation', [
      transition(':enter', [
        style({ transform: 'translateY(10px)', opacity: 0 }),
        animate('.25s', style({ transform: 'translateY(0px)', opacity: 1 }))
      ])
    ])
  ]
})
export class CardsOutletComponent implements OnInit {
  cards: CreditCardDto[] = [];

  // input data
  public cardHolderName = '';
  public cardNumber = '';
  public expiration = '';
  public cvv = '';
  public paymentNetwork: PaymentNetwork = PaymentNetwork.Visa;
  public PaymentNetwork = PaymentNetwork;

  constructor(private _creditCardApiService: CreditCardApiService) {

  }

  async ngOnInit() {
    const getCreditCardsSub$ = this._creditCardApiService.getCreditCards();
    const getCreditCardsResult = await firstValueFrom<Result<CreditCardDto[]>>(getCreditCardsSub$);
    this.cards = getCreditCardsResult.response;
  }

  async onAttachCardOkClick() {
    const attachCardSub$ = this._creditCardApiService.attachCreditCardToAccount(
      this.cardHolderName, this.cardNumber,
      this.expiration, this.cvv, this.paymentNetwork
    );

    await firstValueFrom<Result<CreditCardAddedEvent>>(attachCardSub$);

    const getCreditCardsSub$ = this._creditCardApiService.getCreditCards();
    const getCreditCardsResponse = await firstValueFrom<Result<CreditCardDto[]>>(getCreditCardsSub$);
    this.cards = getCreditCardsResponse.response;
  }
}
