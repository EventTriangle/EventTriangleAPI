import {PaymentNetwork} from "../enums/PaymentNetwork";

export interface AttachCreditCardToAccountRequest {
  holderName: string;
  cardNumber: string;
  expiration: string;
  cvv: string;
  paymentNetwork: PaymentNetwork;
}
