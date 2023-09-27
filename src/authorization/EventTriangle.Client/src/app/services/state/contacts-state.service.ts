import { Injectable } from '@angular/core';
import {ContactDto} from "../../types/models/consumer/ContactDto";
import {ContactsApiService} from "../api/contacts-api.service";
import {BehaviorSubject, firstValueFrom} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ContactsStateService {
  public wasContactsRequested = false;

  public contacts: BehaviorSubject<ContactDto[]> = new BehaviorSubject<ContactDto[]>([]);
  public searchedContacts: BehaviorSubject<ContactDto[]> = new BehaviorSubject<ContactDto[]>([]);

  constructor(
      private _contactsApiService: ContactsApiService
  ) {}

  async getContactsAsync() {
    const getContacts$ = this._contactsApiService.getContacts();
    const getContactsResult = await firstValueFrom(getContacts$);

    this.wasContactsRequested = true;

    this.contacts.next(getContactsResult.response);
  }

  async getSearchContactsAsync(email: string) {
    const getSearchContacts$ = this._contactsApiService.getSearchContacts(email);
    const getSearchContactsResult = await firstValueFrom(getSearchContacts$);

    this.searchedContacts.next(getSearchContactsResult.response);
  }

  async addContactAsync(contactId: string) {
    const addContact$ = this._contactsApiService.addContact(contactId);
    await firstValueFrom(addContact$);

    const contactIndex = this.searchedContacts.getValue().findIndex(x => x.contactId === contactId);
    const contact = this.searchedContacts.getValue()[contactIndex];

    const contactsForDeleteContact = this.searchedContacts.getValue();
    contactsForDeleteContact.splice(contactIndex, 1);
    this.searchedContacts.next(contactsForDeleteContact);

    const contactsForAddContact = this.contacts.getValue();
    contactsForAddContact.unshift(contact)
    this.contacts.next(contactsForAddContact);
  }

  async deleteContactAsync(contactId: string) {
    const addContact$ = this._contactsApiService.deleteContact(contactId);
    await firstValueFrom(addContact$);

    const contactIndex = this.contacts.getValue().findIndex(x => x.contactId === contactId);
    this.contacts.getValue().splice(contactIndex, 1);
  }
}