import { Injectable } from '@angular/core';
import {IContactDto} from "../../types/interfaces/consumer/IContactDto";
import {ContactsApiService} from "../api/contacts-api.service";
import {BehaviorSubject, firstValueFrom} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ContactsStateService {
  public wasContactsRequested = false;
  private searchText = '';
  private searchGeneration = 0;
  private pending = new Set<string>();
  private rejected = new Set<string>();

  public contacts$: BehaviorSubject<IContactDto[]> = new BehaviorSubject<IContactDto[]>([]);
  public searchedContacts$: BehaviorSubject<IContactDto[]> = new BehaviorSubject<IContactDto[]>([]);

  constructor(
      private _contactsApiService: ContactsApiService
  ) {}

  //action
  clearSearchContacts() {
    this.searchText = '';
    this.searchGeneration++;
    this.searchedContacts$.next([]);
  }

  async reconcileRejectedContact(contactId: string, action: 'add' | 'delete') {
    const key = `${action}:${contactId}`;
    if (this.pending.has(key)) this.rejected.add(key);
    await this.getContactsAsync();
    if (this.searchText.trim()) await this.getSearchContactsAsync(this.searchText);
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
    this.searchText = email;
    const generation = ++this.searchGeneration;
    const getSearchContacts$ = this._contactsApiService.getSearchContacts(email);
    const getSearchContactsResult = await firstValueFrom(getSearchContacts$);

    if (generation === this.searchGeneration) this.searchedContacts$.next(getSearchContactsResult.response);

    return getSearchContactsResult;
  }

  async addContactAsync(contactId: string) {
    const key = `add:${contactId}`;
    if (this.pending.has(key)) return;
    this.pending.add(key);
    const contact = this.searchedContacts$.getValue().find(x => x.contactId === contactId);
    try {
      const result = await firstValueFrom(this._contactsApiService.addContact(contactId));
      if (!this.rejected.has(key)) {
        this.searchedContacts$.next(this.searchedContacts$.getValue().filter(x => x.contactId !== contactId));
        if (contact && !this.contacts$.getValue().some(x => x.contactId === contactId)) {
          this.contacts$.next([contact, ...this.contacts$.getValue()]);
        }
      }
      return result;
    } finally {
      this.pending.delete(key);
      this.rejected.delete(key);
    }
  }

  async deleteContactAsync(contactId: string) {
    const key = `delete:${contactId}`;
    if (this.pending.has(key)) return;
    this.pending.add(key);
    try {
      const result = await firstValueFrom(this._contactsApiService.deleteContact(contactId));
      if (!this.rejected.has(key)) {
        this.contacts$.next(this.contacts$.getValue().filter(x => x.contactId !== contactId));
      }
      return result;
    } finally {
      this.pending.delete(key);
      this.rejected.delete(key);
    }
  }

}
