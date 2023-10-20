import { Injectable } from '@angular/core';
import * as signalR from "@microsoft/signalr";
import {SignalRMethodsName} from "../../constants/SignalRMethodNames";
import {ITransactionDto} from "../../types/interfaces/consumer/ITransactionDto";
import {IContactCreatingCanceledNotification} from "../../types/notifications/IContactCreatingCanceledNotification";
import {IContactDeletingCanceledNotification} from "../../types/notifications/IContactDeletingCanceledNotification";
import {ICreditCardAddingCanceledNotification} from "../../types/notifications/ICreditCardAddingCanceledNotification";
import {
  ICreditCardChangingCanceledNotification
} from "../../types/notifications/ICreditCardChangingCanceledNotification";
import {
  ICreditCardDeletingCanceledNotification
} from "../../types/notifications/ICreditCardDeletingCanceledNotification";
import {
  ISupportTicketOpeningCanceledNotification
} from "../../types/notifications/ISupportTicketOpeningCanceledNotification";
import {
  ISupportTicketResolvingCanceledNotification
} from "../../types/notifications/ISupportTicketResolvingCanceledNotification";
import {
  ITransactionRollBackCanceledNotification
} from "../../types/notifications/ITransactionRollBackCanceledNotification";
import {IUserNotSuspendingCanceledNotification} from "../../types/notifications/IUserNotSuspendingCanceledNotification";
import {IUserSuspendingCanceledNotification} from "../../types/notifications/IUserSuspendingCanceledNotification";
import {IUserRoleUpdatingCanceledNotification} from "../../types/notifications/IUserRoleUpdatingCanceledNotification";
import {ITransactionCanceledNotification} from "../../types/notifications/ITransactionCanceledNotification";

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  public connection: signalR.HubConnection | undefined;

  constructor() { }

  public async build() {
    const connectionBuilder = new signalR.HubConnectionBuilder();
    this.connection = connectionBuilder
        .configureLogging(signalR.LogLevel.Information)
        .withUrl("https://localhost:7000/consumer" + "/notify")
        .build();
  }

  public async start() {
    if (!this.connection) throw new Error("SignalR connection is undefined");

    await this.connection.start();
  }

  public configure() {
    if (!this.connection) throw new Error("SignalR connection is undefined");

    this.connection.on(SignalRMethodsName.TransactionSucceededAsync, (transactionDto: ITransactionDto) => {

    });
    this.connection.on(SignalRMethodsName.ContactCreatingCanceledAsync, (notification: IContactCreatingCanceledNotification) => {

    });
    this.connection.on(SignalRMethodsName.ContactDeletingCanceledAsync, (notification: IContactDeletingCanceledNotification) => {

    });
    this.connection.on(SignalRMethodsName.CreditCardAddingCanceledAsync, (notification: ICreditCardAddingCanceledNotification) => {

    });
    this.connection.on(SignalRMethodsName.CreditCardChangingCanceledAsync, (notification: ICreditCardChangingCanceledNotification) => {

    });
    this.connection.on(SignalRMethodsName.CreditCardDeletingCanceledAsync, (notification: ICreditCardDeletingCanceledNotification) => {

    });
    this.connection.on(SignalRMethodsName.SupportTicketOpeningCanceledAsync, (notification: ISupportTicketOpeningCanceledNotification) => {

    });
    this.connection.on(SignalRMethodsName.SupportTicketResolvingCanceledAsync, (notification: ISupportTicketResolvingCanceledNotification) => {

    });
    this.connection.on(SignalRMethodsName.TransactionRollBackCanceledAsync, (notification: ITransactionRollBackCanceledNotification) => {

    });
    this.connection.on(SignalRMethodsName.UserNotSuspendingCanceledAsync, (notification: IUserNotSuspendingCanceledNotification) => {

    });
    this.connection.on(SignalRMethodsName.UserSuspendingCanceledAsync, (notification: IUserSuspendingCanceledNotification) => {

    });
    this.connection.on(SignalRMethodsName.UserRoleUpdatingCanceledAsync, (notification: IUserRoleUpdatingCanceledNotification) => {

    });
    this.connection.on(SignalRMethodsName.TransactionCanceledAsync, (notification: ITransactionCanceledNotification) => {

    });
  }
}
