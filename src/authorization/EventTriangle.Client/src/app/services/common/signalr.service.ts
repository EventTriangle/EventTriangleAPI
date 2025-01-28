import {Injectable} from '@angular/core';
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
import {MessageService} from "primeng/api";
import {TransactionsStateService} from "../state/transactions-state.service";
import {ConfigService} from "./config.service";
import {ProfileStateService} from "../state/profile-state.service";
import {TransactionType} from "../../types/enums/TransactionType";
import {ICreditCardDto} from "../../types/interfaces/consumer/ICreditCardDto";
import {ISupportTicketDto} from "../../types/interfaces/consumer/ISupportTicketDto";
import {CreditCardsStateService} from "../state/credit-cards-state.service";
import {SupportTicketStateService} from "../state/support-ticket-state.service";
import {ErrorMessageConstants} from "../../constants/ErrorMessageConstants";

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  public connection: signalR.HubConnection | undefined;

  constructor(
      private _messageService: MessageService,
      private _transactionStateService: TransactionsStateService,
      private _creditCardsStateService: CreditCardsStateService,
      private _supportTicketStateService: SupportTicketStateService,
      private _profileStateService: ProfileStateService,
      private _configService: ConfigService
  ) { }

  public async build() {
    const serverUrl = this._configService.getServerUrl();

    const connectionBuilder = new signalR.HubConnectionBuilder();
    this.connection = connectionBuilder
        .configureLogging(signalR.LogLevel.Information)
        .withUrl(`${serverUrl}consumer` + "/notify")
        .build();
  }

  public async start() {
    if (!this.connection) throw new Error(ErrorMessageConstants.SignalRConnectionIsUndefined);

    await this.connection.start();
  }

  public configure() {
    if (!this.connection) throw new Error(ErrorMessageConstants.SignalRConnectionIsUndefined);

    this.connection.on(SignalRMethodsName.TransactionSucceededAsync, (transactionDto: ITransactionDto) => {
      const user = this._profileStateService.user$.getValue();

      if (!user) throw new Error(ErrorMessageConstants.UserNotFound);

      this._transactionStateService.addTransactionInTransactions(transactionDto);

      if (transactionDto.fromUserId === user.id && transactionDto.transactionType === TransactionType.FromUserToUser)
        this._profileStateService.minusFromBalance(transactionDto.amount);

      if (transactionDto.toUserId === user.id && transactionDto.transactionType === TransactionType.FromUserToUser)
        this._profileStateService.plusToBalance(transactionDto.amount);

      if (transactionDto.transactionType === TransactionType.FromCardToUser)
        this._profileStateService.plusToBalance(transactionDto.amount);

      this._messageService.add({ severity: 'success', summary: 'Success', detail: "Transaction succeeded." });
    });
    this.connection.on(SignalRMethodsName.CreditCardAddedAsync, (creditCardDto: ICreditCardDto) => {
      this._creditCardsStateService.addCreditCard(creditCardDto);
      this._messageService.add({ severity: 'success', summary: 'Success', detail: "Credit card was added." });
    });
    this.connection.on(SignalRMethodsName.SupportTicketOpenedAsync, (supportTicketDto: ISupportTicketDto) => {
      this._supportTicketStateService.addSupportTicket(supportTicketDto);
      this._messageService.add({ severity: 'success', summary: 'Success', detail: "Support ticket was opened." });
    });
    this.connection.on(SignalRMethodsName.ContactCreatingCanceledAsync, (notification: IContactCreatingCanceledNotification) => {
      this._messageService.add({ severity: 'error', summary: 'Error', detail: notification.reason });
    });
    this.connection.on(SignalRMethodsName.ContactDeletingCanceledAsync, (notification: IContactDeletingCanceledNotification) => {
      this._messageService.add({ severity: 'error', summary: 'Error', detail: notification.reason });
    });
    this.connection.on(SignalRMethodsName.CreditCardAddingCanceledAsync, (notification: ICreditCardAddingCanceledNotification) => {
      this._messageService.add({ severity: 'error', summary: 'Error', detail: notification.reason });
    });
    this.connection.on(SignalRMethodsName.CreditCardChangingCanceledAsync, (notification: ICreditCardChangingCanceledNotification) => {
      this._messageService.add({ severity: 'error', summary: 'Error', detail: notification.reason });
    });
    this.connection.on(SignalRMethodsName.CreditCardDeletingCanceledAsync, (notification: ICreditCardDeletingCanceledNotification) => {
      this._messageService.add({ severity: 'error', summary: 'Error', detail: notification.reason });
    });
    this.connection.on(SignalRMethodsName.SupportTicketOpeningCanceledAsync, (notification: ISupportTicketOpeningCanceledNotification) => {
      this._messageService.add({ severity: 'error', summary: 'Error', detail: notification.reason });
    });
    this.connection.on(SignalRMethodsName.SupportTicketResolvingCanceledAsync, (notification: ISupportTicketResolvingCanceledNotification) => {
      this._messageService.add({ severity: 'error', summary: 'Error', detail: notification.reason });
    });
    this.connection.on(SignalRMethodsName.TransactionRollBackCanceledAsync, (notification: ITransactionRollBackCanceledNotification) => {
      this._messageService.add({ severity: 'error', summary: 'Error', detail: notification.reason });
    });
    this.connection.on(SignalRMethodsName.UserNotSuspendingCanceledAsync, (notification: IUserNotSuspendingCanceledNotification) => {
      this._messageService.add({ severity: 'error', summary: 'Error', detail: notification.reason });
    });
    this.connection.on(SignalRMethodsName.UserSuspendingCanceledAsync, (notification: IUserSuspendingCanceledNotification) => {
      this._messageService.add({ severity: 'error', summary: 'Error', detail: notification.reason });
    });
    this.connection.on(SignalRMethodsName.UserRoleUpdatingCanceledAsync, (notification: IUserRoleUpdatingCanceledNotification) => {
      this._messageService.add({ severity: 'error', summary: 'Error', detail: notification.reason });
    });
    this.connection.on(SignalRMethodsName.TransactionCanceledAsync, (notification: ITransactionCanceledNotification) => {
      this._messageService.add({ severity: 'error', summary: 'Error', detail: notification.reason });
    });
  }
}
