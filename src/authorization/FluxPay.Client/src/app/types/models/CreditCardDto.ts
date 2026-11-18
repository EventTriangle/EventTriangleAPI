import {ICreditCardDto} from "../interfaces/consumer/ICreditCardDto";
import {PaymentNetwork} from "../enums/PaymentNetwork";


export class CreditCardDto implements ICreditCardDto {
  id: string;
  userId: string;
  holderName: string;
  cardNumber: string;
  cvv: string;
  expiration: string;
  paymentNetwork: PaymentNetwork;

  constructor(id: string, userId: string, holderName: string, cardNumber: string, cvv: string,
              expiration: string, paymentNetwork: PaymentNetwork) {
    this.id = id;
    this.userId = userId;
    this.holderName = holderName;
    this.cardNumber = cardNumber;
    this.cvv = cvv;
    this.expiration = expiration;
    this.paymentNetwork = paymentNetwork;
  }
}