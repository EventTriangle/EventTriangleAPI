import {Injectable} from "@angular/core";
import {BehaviorSubject, first, firstValueFrom} from "rxjs";
import {CreditCardApiService} from "../api/credit-card-api.service";
import {IResult} from "../../types/interfaces/IResult";
import {ICreditCardAddedEvent} from "../../types/interfaces/sender/ICreditCardAddedEvent";
import {PaymentNetwork} from "../../types/enums/PaymentNetwork";
import {ICreditCardDto} from "../../types/interfaces/consumer/ICreditCardDto";

@Injectable({
  providedIn: 'root'
})
export class CreditCardsStateService {
  public wereCardsRequested = false;

  public cards$: BehaviorSubject<ICreditCardDto[]> = new BehaviorSubject<ICreditCardDto[]>([]);

  constructor(
      private _creditCardApiService: CreditCardApiService
  ) { }

  //actions
  public addCreditCard(creditCardDto: ICreditCardDto) {
    const cards = this.cards$.getValue();
    cards.push(creditCardDto);
    this.cards$.next(cards);
  }

  //requests
  public async getCreditCardsAsync() {
    const getCreditCardsSub$ = this._creditCardApiService.getCreditCards();
    const getCreditCardsResult = await firstValueFrom<IResult<ICreditCardDto[]>>(getCreditCardsSub$);
    this.cards$.next(getCreditCardsResult.response);

    this.wereCardsRequested = true;

    return getCreditCardsResult;
  }

  public async attachCreditCardToAccountAsync(cardHolderName: string, cardNumber: string, expiration: string,
                                              cvv: string, paymentNetwork: PaymentNetwork) {
    const attachCard$ = this._creditCardApiService.attachCreditCardToAccount(
        cardHolderName,
        cardNumber,
        expiration,
        cvv,
        paymentNetwork
    );
    return await firstValueFrom<IResult<ICreditCardAddedEvent>>(attachCard$);
  }

  public async deleteCreditCardAsync(creditCardId: string) {
    const deleteCreditCard$ = this._creditCardApiService.deleteCreditCard(creditCardId);

    const cards = this.cards$.getValue().filter(x => x.id !== creditCardId);
    this.cards$.next(cards);

    return await firstValueFrom(deleteCreditCard$);
  }
}
