import {Injectable} from "@angular/core";
import ApiBaseService from "./api-base.service";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {IContactDto} from "../../types/interfaces/consumer/IContactDto";
import {Observable} from "rxjs";
import {IContactCreatedEvent} from "../../types/interfaces/sender/IContactCreatedEvent";
import {AddContactRequest} from "../../types/requests/AddContactRequest";
import {DeleteContactRequest} from "../../types/requests/DeleteContactRequest";
import {IContactDeletedEvent} from "../../types/interfaces/sender/IContactDeletedEvent";
import {IResult} from "../../types/interfaces/IResult";
import {ConfigService} from "../common/config.service";

@Injectable({
  providedIn: 'root'
})
export class ContactsApiService extends ApiBaseService {
  private readonly consumerContactsRoute = "consumer/contacts";
  private readonly senderContactsRoute = "sender/contacts";
  private readonly baseUrl: string;

  constructor(
      private httpClient: HttpClient,
      private _configService: ConfigService
  ) {
    super()
    this.baseUrl = _configService.getServerUrl();
  }

  // requests

  // GET consumer/contacts
  public getContacts() : Observable<IResult<IContactDto[]>> {
    return this.httpClient.get<IResult<IContactDto[]>>(
      this.baseUrl + this.consumerContactsRoute,
      { withCredentials: true }
    );
  }

  // GET consumer/contacts/search
  public getSearchContacts(email: string) : Observable<IResult<IContactDto[]>> {
    return this.httpClient.get<IResult<IContactDto[]>>(
        this.baseUrl + this.consumerContactsRoute + `/search?email=${email}`,
        { withCredentials: true }
    );
  }

  // POST sender/contacts
  public addContact(contactId: string) : Observable<IResult<IContactCreatedEvent>> {
    let command : AddContactRequest = {
      contactId: contactId
    };

    return this.httpClient.post<IResult<IContactCreatedEvent>>(
      this.baseUrl + this.senderContactsRoute,
      command, { withCredentials: true }
    );
  }

  // DELETE sender/contacts
  public deleteContact(contactId: string) : Observable<IResult<IContactDeletedEvent>> {
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

    return this.httpClient.delete<IResult<IContactDeletedEvent>>(
      this.baseUrl + this.senderContactsRoute,
      options
    );
  }
}
