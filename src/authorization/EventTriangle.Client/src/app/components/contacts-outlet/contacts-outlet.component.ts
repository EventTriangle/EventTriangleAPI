import {Component, OnInit} from '@angular/core';
import {animate, query, stagger, style, transition, trigger} from "@angular/animations";
import {ContactsStateService} from "../../services/state/contacts-state.service";
import {ProfileStateService} from "../../services/state/profile-state.service";
import {
  BehaviorSubject,
  debounceTime, filter
} from "rxjs";
import {TextService} from "../../services/common/text.service";
import {IContactDto} from "../../types/interfaces/consumer/IContactDto";

@Component({
  selector: 'app-contacts-outlet',
  templateUrl: './contacts-outlet.component.html',
  styleUrls: ['./contacts-outlet.component.scss'],
  animations: [
    trigger('contactListAnimation', [
      transition(':enter', [
        query(':enter', style({ marginTop: -5, opacity: 0 }), { optional: true }),
        query(':enter', stagger('50ms', [
          animate('150ms', style({ marginTop: 0, opacity: 1 }))
        ]), { optional: true })
      ])
    ])
  ]
})
export class ContactsOutletComponent implements OnInit{
  //observable
  public contacts$ = this._contactsStateService.contacts$;
  public searchedContacts$ = this._contactsStateService.searchedContacts$;

  public searchText$ = new BehaviorSubject<string>("");

  constructor(
      protected _contactsStateService: ContactsStateService,
      protected _profileStateService: ProfileStateService,
      protected _textService: TextService
  ) {}

  async ngOnInit() {
    if (!this._profileStateService.isAuthenticated) return;

    await this._contactsStateService.getContactsAsync();

    this.searchText$
        .pipe(
            filter(x => x.trim() !== ''),
            debounceTime(400))
        .subscribe(async x => await this._contactsStateService.getSearchContactsAsync(x));

    this.searchText$
        .pipe(filter(x => x === ''))
        .subscribe(_ => this._contactsStateService.clearSearchContacts())
  }

  //events
  async onClickAddContactHandler(contactId: string) {
    await this._contactsStateService.addContactAsync(contactId);
  }

  async onClickDeleteContactHandler(contactId: string) {
    await this._contactsStateService.deleteContactAsync(contactId);
  }

  //other
  identifyContactDto(index: number, item: IContactDto) {
    return item.contactId + item.userId;
  }
}
