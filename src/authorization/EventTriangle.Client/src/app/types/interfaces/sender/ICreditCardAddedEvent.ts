import {PaymentNetwork} from "../../enums/PaymentNetwork";

export interface ICreditCardAddedEvent {
  id: string;
  requesterId: string;
  holderName: string;
  cardNumber: string;
  cvv: string;
  expiration: string;
  paymentNetwork: PaymentNetwork;
  createdAt: string;
}
