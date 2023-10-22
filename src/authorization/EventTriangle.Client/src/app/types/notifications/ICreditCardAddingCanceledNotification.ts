import {PaymentNetwork} from "../enums/PaymentNetwork";


export interface ICreditCardAddingCanceledNotification {
    holderName: string,
    cardNumber: string,
    cvv: string,
    expiration: string,
    paymentNetwork: PaymentNetwork,
    reason: string
}