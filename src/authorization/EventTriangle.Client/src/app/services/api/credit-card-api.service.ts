import {Injectable} from "@angular/core";
import ApiBaseService from "./api-base.service";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Observable} from "rxjs";
import {CreditCardDto} from "../../types/models/consumer/CreditCardDto";
import {AttachCreditCardToAccountRequest} from "../../types/requests/AttachCreditCardToAccountRequest";
import {PaymentNetwork} from "../../types/enums/PaymentNetwork";
import {CreditCardAddedEvent} from "../../types/models/sender/CreditCardAddedEvent";
import {CreditCardChangedEvent} from "../../types/models/sender/CreditCardChangedEvent";
import {EditCreditCardRequest} from "../../types/requests/EditCreditCardRequest";
import {CreditCardDeletedEvent} from "../../types/models/sender/CreditCardDeletedEvent";
import {DeleteCreditCardRequest} from "../../types/requests/DeleteCreditCardRequest";

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
  public getCreditCards(): Observable<CreditCardDto[]> {
    return this.httpClient.get<CreditCardDto[]>(
      this.baseUrl + this.consumerCreditCardsRoute
    );
  }

  // POST sender/credit-cards
  public attachCreditCardToAccount(holderName: string, creditCardNumber: string,
                                   expiration: string, cvv: string, paymentNetwork: PaymentNetwork)
    : Observable<CreditCardAddedEvent> {
    let command =
      new AttachCreditCardToAccountRequest(holderName, creditCardNumber, expiration, cvv, paymentNetwork);

    return this.httpClient.post<CreditCardAddedEvent>(
      this.baseUrl + this.senderCreditCardsRoute,
      command
    );
  }

  // PUT sender/credit-cards
  public editCreditCard(holderName: string, creditCardNumber: string,
                        expiration: string, cvv: string, paymentNetwork: PaymentNetwork)
    : Observable<CreditCardChangedEvent> {
    let command =
      new EditCreditCardRequest(holderName, creditCardNumber, expiration, cvv, paymentNetwork);

    return this.httpClient.put<CreditCardChangedEvent>(
      this.baseUrl + this.senderCreditCardsRoute,
      command
    );
  }

  // DELETE sender/credit-cards
  public deleteCreditCard(creditCardId: string) : Observable<CreditCardDeletedEvent> {
    let command = new DeleteCreditCardRequest(creditCardId);
    let options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      }),
      body: command
    };

    return this.httpClient.delete<CreditCardDeletedEvent>(
      this.baseUrl + this.senderCreditCardsRoute,
      options
    );
  }
}
