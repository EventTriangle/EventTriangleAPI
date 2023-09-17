import {Injectable} from "@angular/core";
import ApiBaseService from "./api-base.service";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {ContactDto} from "../../types/models/consumer/ContactDto";
import {Observable} from "rxjs";
import {ContactCreatedEvent} from "../../types/models/sender/ContactCreatedEvent";
import {AddContactRequest} from "../../types/requests/AddContactRequest";
import {DeleteContactRequest} from "../../types/requests/DeleteContactRequest";
import {ContactDeletedEvent} from "../../types/models/sender/ContactDeletedEvent";
import {Result} from "../../types/models/Result";

@Injectable({
  providedIn: 'root'
})
export class ContactsApiService extends ApiBaseService {
  private readonly consumerContactsRoute = "consumer/contacts";
  private readonly senderContactsRoute = "sender/contacts";
  private readonly baseUrl: string;

  constructor(private httpClient: HttpClient) {
    super()
    this.baseUrl = super.getUrl();
  }

  // requests

  // GET consumer/contacts
  public getContacts() : Observable<Result<ContactDto[]>> {
    return this.httpClient.get<Result<ContactDto[]>>(
      this.baseUrl + this.consumerContactsRoute,
      { withCredentials: true }
    );
  }

  // POST sender/contacts
  public addContact(contactId: string) : Observable<Result<ContactCreatedEvent>> {
    let command : AddContactRequest = {
      contactId: contactId
    };

    return this.httpClient.post<Result<ContactCreatedEvent>>(
      this.baseUrl + this.senderContactsRoute,
      command, { withCredentials: true }
    );
  }

  // DELETE sender/contacts
  public deleteContact(contactId: string) : Observable<Result<ContactDeletedEvent>> {
    let command : DeleteContactRequest = {
      contactId: contactId
    };
    let options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      }),
      withCredentials: true,
      body: command
    };

    return this.httpClient.delete<Result<ContactDeletedEvent>>(
      this.baseUrl + this.senderContactsRoute,
      options
    );
  }
}
