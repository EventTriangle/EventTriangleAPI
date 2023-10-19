import { Injectable } from '@angular/core';
import * as signalR from "@microsoft/signalr";
import {SignalRMethodsName} from "../../constants/SignalRMethodNames";
import {ITransactionDto} from "../../types/interfaces/consumer/ITransactionDto";

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

    this.connection.on(SignalRMethodsName.TransactionSucceededAsync, () => );
    this.connection.on(SignalRMethodsName.ContactCreatingCanceledAsync, () => {});
    this.connection.on(SignalRMethodsName.ContactDeletingCanceledAsync, () => {});
    this.connection.on(SignalRMethodsName.CreditCardAddingCanceledAsync, () => {});
    this.connection.on(SignalRMethodsName.CreditCardChangingCanceledAsync, () => {});
    this.connection.on(SignalRMethodsName.CreditCardDeletingCanceledAsync, () => {});
    this.connection.on(SignalRMethodsName.SupportTicketOpeningCanceledAsync, () => {});
    this.connection.on(SignalRMethodsName.SupportTicketResolvingCanceledAsync, () => {});
    this.connection.on(SignalRMethodsName.TransactionRollBackCanceledAsync, () => {});
    this.connection.on(SignalRMethodsName.UserNotSuspendingCanceledAsync, () => {});
    this.connection.on(SignalRMethodsName.UserSuspendingCanceledAsync, () => {});
    this.connection.on(SignalRMethodsName.UserRoleUpdatingCanceledAsync, () => {});
    this.connection.on(SignalRMethodsName.TransactionCanceledAsync, () => {});
  }
}
