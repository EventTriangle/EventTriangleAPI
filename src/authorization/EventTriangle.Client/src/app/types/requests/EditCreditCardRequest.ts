import {PaymentNetwork} from "../enums/PaymentNetwork";

export interface EditCreditCardRequest {
  holderName: string;
  creditCardNumber: string;
  expiration: string;
  cvv: string;
  paymentNetwork: PaymentNetwork;
}
