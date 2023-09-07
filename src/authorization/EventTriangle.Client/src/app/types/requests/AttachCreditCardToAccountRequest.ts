import {PaymentNetwork} from "../enums/PaymentNetwork";

export interface AttachCreditCardToAccountRequest {
  holderName: string;
  creditCardNumber: string;
  expiration: string;
  cvv: string;
  paymentNetwork: PaymentNetwork;
}
