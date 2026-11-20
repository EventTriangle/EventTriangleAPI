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
import {PaymentSubmissionService} from "../state/payment-submission.service";
import {ICreditCardDto} from "../../types/interfaces/consumer/ICreditCardDto";
import {ISupportTicketDto} from "../../types/interfaces/consumer/ISupportTicketDto";
import {CreditCardsStateService} from "../state/credit-cards-state.service";
import {SupportTicketStateService} from "../state/support-ticket-state.service";
import {ContactsStateService} from "../state/contacts-state.service";
import {ErrorMessageConstants} from "../../constants/ErrorMessageConstants";

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  public connection: signalR.HubConnection | undefined;
  private stateRevision = 0;

  constructor(
      private _messageService: MessageService,
      private _transactionStateService: TransactionsStateService,
      private _creditCardsStateService: CreditCardsStateService,
      private _supportTicketStateService: SupportTicketStateService,
      private _contactsStateService: ContactsStateService,
      private _profileStateService: ProfileStateService,
      private _configService: ConfigService,
      private paymentSubmission: PaymentSubmissionService
  ) { }

  public async build() {
    const serverUrl = this._configService.getServerUrl();

    const connectionBuilder = new signalR.HubConnectionBuilder();
    this.connection = connectionBuilder
        .configureLogging(signalR.LogLevel.Information)
        .withUrl(`${serverUrl}consumer` + "/notify")
        .withAutomaticReconnect({
          nextRetryDelayInMilliseconds: context => [0, 2000, 10000, 30000][Math.min(context.previousRetryCount, 3)]
        })
        .build();
  }

  public async start() {
    if (!this.connection) throw new Error(ErrorMessageConstants.SignalRConnectionIsUndefined);

    await this.connection.start();
  }

  public async refreshAfterReconnect() {
    try {
      // Retry if a live event arrived while an older snapshot was loading.
      let revision: number;
      do {
        revision = this.stateRevision;
        const requests: Promise<unknown>[] = [this._profileStateService.getProfileAsync()];
        if (this._transactionStateService.wasRequested) requests.push(this._transactionStateService.refreshTransactionsAsync());
        if (this._creditCardsStateService.wereCardsRequested) requests.push(this._creditCardsStateService.getCreditCardsAsync());
        if (this._supportTicketStateService.wasRequested) requests.push(this._supportTicketStateService.refreshSupportTicketsAsync());
        if (this._contactsStateService.wasContactsRequested) requests.push(this._contactsStateService.getContactsAsync());
        await Promise.all(requests);
      } while (revision !== this.stateRevision);
    } catch {
      this._messageService.add({severity: 'error', summary: 'Error', detail: 'Could not refresh data after reconnecting. Reload the page.'});
    }
  }

  public configure() {
    if (!this.connection) throw new Error(ErrorMessageConstants.SignalRConnectionIsUndefined);

    this.connection.onreconnected(() => { void this.refreshAfterReconnect(); });

    this.connection.on(SignalRMethodsName.TransactionSucceededAsync, (transactionDto: ITransactionDto, operationId?: string) => {
      this.stateRevision++;
      this.paymentSubmission.finish(operationId);
      this._transactionStateService.addTransactionInTransactions(transactionDto);
      void this._profileStateService.getProfileAsync().catch(() => {
        this._messageService.add({severity: 'error', summary: 'Error', detail: 'Could not refresh the balance. Reload the page.'});
      });
      this._messageService.add({ severity: 'success', summary: 'Success', detail: "Transaction succeeded." });
    });
    this.connection.on(SignalRMethodsName.CreditCardAddedAsync, (creditCardDto: ICreditCardDto) => {
      this.stateRevision++;
      this._creditCardsStateService.addCreditCard(creditCardDto);
      this._messageService.add({ severity: 'success', summary: 'Success', detail: "Credit card was added." });
    });
    this.connection.on(SignalRMethodsName.SupportTicketOpenedAsync, (supportTicketDto: ISupportTicketDto) => {
      this.stateRevision++;
      this._supportTicketStateService.addSupportTicket(supportTicketDto);
      this._messageService.add({ severity: 'success', summary: 'Success', detail: "Support ticket was opened." });
    });
    this.connection.on(SignalRMethodsName.SupportTicketResolvedAsync, (supportTicketDto: ISupportTicketDto) => {
      this.stateRevision++;
      this._supportTicketStateService.updateSupportTicket(supportTicketDto);
      this._messageService.add({ severity: 'success', summary: 'Success', detail: 'Your support ticket was resolved.' });
    });
    this.connection.on(SignalRMethodsName.ContactCreatingCanceledAsync, (notification: IContactCreatingCanceledNotification) => {
      this.stateRevision++;
      void this._contactsStateService.reconcileRejectedContact(notification.contactId, 'add').catch(() => {
        this._messageService.add({severity: 'error', summary: 'Error', detail: 'Could not refresh contacts. Reload the page.'});
      });
      this._messageService.add({ severity: 'error', summary: 'Error', detail: notification.reason });
    });
    this.connection.on(SignalRMethodsName.ContactDeletingCanceledAsync, (notification: IContactDeletingCanceledNotification) => {
      this.stateRevision++;
      void this._contactsStateService.reconcileRejectedContact(notification.contactId, 'delete').catch(() => {
        this._messageService.add({severity: 'error', summary: 'Error', detail: 'Could not refresh contacts. Reload the page.'});
      });
      this._messageService.add({ severity: 'error', summary: 'Error', detail: notification.reason });
    });
    this.connection.on(SignalRMethodsName.CreditCardAddingCanceledAsync, (notification: ICreditCardAddingCanceledNotification) => {
      this._messageService.add({ severity: 'error', summary: 'Error', detail: notification.reason });
    });
    this.connection.on(SignalRMethodsName.CreditCardChangingCanceledAsync, (notification: ICreditCardChangingCanceledNotification) => {
      this._messageService.add({ severity: 'error', summary: 'Error', detail: notification.reason });
    });
    this.connection.on(SignalRMethodsName.CreditCardDeletingCanceledAsync, (notification: ICreditCardDeletingCanceledNotification) => {
      this.stateRevision++;
      void this._creditCardsStateService.reconcileRejectedDeletion(notification.cardId).catch(() => {
        this._messageService.add({severity: 'error', summary: 'Error', detail: 'Could not refresh cards. Reload the page.'});
      });
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
      this.paymentSubmission.finish(notification.operationId);
      this._messageService.add({ severity: 'error', summary: 'Error', detail: notification.reason });
    });
  }
}
