import {PaymentNetwork} from "../enums/PaymentNetwork";

export interface ICreditCardChangingCanceledNotification {
    holderName: string;
    cardNumber: string;
    cvv: string;
    expiration: string;
    paymentNetwork: PaymentNetwork;
    reason: string;
}