import {Injectable} from "@angular/core";
import ApiBaseService from "./api-base.service";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Observable} from "rxjs";
import {AttachCreditCardToAccountRequest} from "../../types/requests/AttachCreditCardToAccountRequest";
import {PaymentNetwork} from "../../types/enums/PaymentNetwork";
import {ICreditCardAddedEvent} from "../../types/interfaces/sender/ICreditCardAddedEvent";
import {ICreditCardChangedEvent} from "../../types/interfaces/sender/ICreditCardChangedEvent";
import {EditCreditCardRequest} from "../../types/requests/EditCreditCardRequest";
import {ICreditCardDeletedEvent} from "../../types/interfaces/sender/ICreditCardDeletedEvent";
import {DeleteCreditCardRequest} from "../../types/requests/DeleteCreditCardRequest";
import {IResult} from "../../types/interfaces/IResult";
import {ICreditCardDto} from "../../types/interfaces/consumer/ICreditCardDto";

@Injectable({
  providedIn: 'root'
})
export class CreditCardApiService extends ApiBaseService {
  private readonly consumerCreditCardsRoute = "consumer/credit-cards";
  private readonly senderCreditCardsRoute = "sender/credit-cards";
  private readonly baseUrl: string;

  constructor(private httpClient: HttpClient) {
    super()
    this.baseUrl = super.getUrl();
  }

  // GET consumer/credit-cards
  public getCreditCards(): Observable<IResult<ICreditCardDto[]>> {
    return this.httpClient.get<IResult<ICreditCardDto[]>>(
      this.baseUrl + this.consumerCreditCardsRoute,
      { withCredentials: true }
    );
  }

  // POST sender/credit-cards
  public attachCreditCardToAccount(holderName: string, cardNumber: string,
                                   expiration: string, cvv: string, paymentNetwork: PaymentNetwork)
    : Observable<IResult<ICreditCardAddedEvent>> {
    let command : AttachCreditCardToAccountRequest = {
      holderName: holderName,
      cardNumber: cardNumber,
      expiration: expiration,
      cvv: cvv,
      paymentNetwork: paymentNetwork
    };

    return this.httpClient.post<IResult<ICreditCardAddedEvent>>(
      this.baseUrl + this.senderCreditCardsRoute,
      command, { withCredentials: true }
    );
  }

  // PUT sender/credit-cards
  public editCreditCard(holderName: string, creditCardNumber: string,
                        expiration: string, cvv: string, paymentNetwork: PaymentNetwork)
    : Observable<IResult<ICreditCardChangedEvent>> {
    let command : EditCreditCardRequest = {
      holderName: holderName,
      creditCardNumber: creditCardNumber,
      expiration: expiration,
      cvv: cvv,
      paymentNetwork: paymentNetwork
    }
    return this.httpClient.put<IResult<ICreditCardChangedEvent>>(
      this.baseUrl + this.senderCreditCardsRoute,
      command, { withCredentials: true }
    );
  }

  // DELETE sender/credit-cards
  public deleteCreditCard(creditCardId: string) : Observable<IResult<ICreditCardDeletedEvent>> {
    let command : DeleteCreditCardRequest = {
      creditCardId: creditCardId
    }
    let options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      }),
      withCredentials: true,
      body: command
    };

    return this.httpClient.delete<IResult<ICreditCardDeletedEvent>>(
      this.baseUrl + this.senderCreditCardsRoute,
      options
    );
  }
}
