import {PaymentNetwork} from "../enums/PaymentNetwork";

export class AttachCreditCardToAccountRequest {
  holderName: string;
  creditCardNumber: string;
  expiration: string;
  cvv: string;
  paymentNetwork: PaymentNetwork;

  constructor(holderName: string, creditCardNumber: string,
              expiration: string, cvv: string, paymentNetwork: PaymentNetwork) {
    this.holderName = holderName;
    this.creditCardNumber = creditCardNumber;
    this.expiration = expiration;
    this.cvv = cvv;
    this.paymentNetwork = paymentNetwork;
  }
}
