import { Injectable } from '@angular/core';
import {IContactDto} from "../../types/interfaces/consumer/IContactDto";
import {ContactsApiService} from "../api/contacts-api.service";
import {BehaviorSubject, firstValueFrom} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ContactsStateService {
  public wasContactsRequested = false;

  public contacts$: BehaviorSubject<IContactDto[]> = new BehaviorSubject<IContactDto[]>([]);
  public searchedContacts$: BehaviorSubject<IContactDto[]> = new BehaviorSubject<IContactDto[]>([]);

  constructor(
      private _contactsApiService: ContactsApiService
  ) {}

  //action
  clearSearchContacts() {
    this.searchedContacts$.next([]);
  }

  //requests
  async getContactsAsync() {
    const getContacts$ = this._contactsApiService.getContacts();
    const getContactsResult = await firstValueFrom(getContacts$);

    this.wasContactsRequested = true;

    this.contacts$.next(getContactsResult.response);

    return getContactsResult;
  }

  async getSearchContactsAsync(email: string) {
    const getSearchContacts$ = this._contactsApiService.getSearchContacts(email);
    const getSearchContactsResult = await firstValueFrom(getSearchContacts$);

    this.searchedContacts$.next(getSearchContactsResult.response);

    return getSearchContactsResult;
  }

  async addContactAsync(contactId: string) {
    const addContact$ = this._contactsApiService.addContact(contactId);
    const addContactResult = await firstValueFrom(addContact$);

    const contactIndex = this.searchedContacts$.getValue().findIndex(x => x.contactId === contactId);
    const contact = this.searchedContacts$.getValue()[contactIndex];

    const contactsForDeleteContact = this.searchedContacts$.getValue();
    contactsForDeleteContact.splice(contactIndex, 1);
    this.searchedContacts$.next(contactsForDeleteContact);

    const contactsForAddContact = this.contacts$.getValue();
    contactsForAddContact.unshift(contact)
    this.contacts$.next(contactsForAddContact);

    return addContactResult;
  }

  async deleteContactAsync(contactId: string) {
    const deleteContact$ = this._contactsApiService.deleteContact(contactId);
    const deleteContactResult = await firstValueFrom(deleteContact$);

    const contactIndex = this.contacts$.getValue().findIndex(x => x.contactId === contactId);
    this.contacts$.getValue().splice(contactIndex, 1);

    return deleteContactResult;
  }
}