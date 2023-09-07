import {PaymentNetwork} from "../../enums/PaymentNetwork";

export interface CreditCardChangedEvent {
  id: string;
  requesterId: string;
  holderName: string;
  cardNumber: string;
  cvv: string;
  expiration: string;
  paymentNetwork: PaymentNetwork;
  createdAt: string;
}
