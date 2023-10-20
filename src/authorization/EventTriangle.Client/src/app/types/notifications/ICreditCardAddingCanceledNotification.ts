import {PaymentNetwork} from "../enums/PaymentNetwork";


export interface ICreditCardAddingCanceledNotification {
    HolderName: string,
    CardNumber: string,
    Cvv: string,
    Expiration: string,
    PaymentNetwork: PaymentNetwork,
    Reason: string
}