import {PaymentNetwork} from "../../enums/PaymentNetwork";

export interface ICreditCardDto {
  id: string;
  userId: string;
  holderName: string;
  cardNumber: string;
  cvv: string;
  expiration: string;
  paymentNetwork: PaymentNetwork;
}
