import {ChangeDetectorRef, Component, OnDestroy, OnInit} from '@angular/core';
import {animate, animateChild, query, stagger, style, transition, trigger} from "@angular/animations";
import {ContactsStateService} from "../../services/state/contacts-state.service";
import {ProfileStateService} from "../../services/state/profile-state.service";
import {
  BehaviorSubject,
  debounceTime, filter, Subscription
} from "rxjs";
import {TextService} from "../../services/common/text.service";
import {IContactDto} from "../../types/interfaces/consumer/IContactDto";
import {NavigationStart, Router} from "@angular/router";

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
      ]),
      transition(":leave", [
          query("@contactAnimation", animateChild({duration: "0.25s"}), {optional: true}),
      ])
    ]),
    trigger("contactAnimation", [
      transition(':leave', [
          animate('0.25s ease', style({ height: 0, opacity: 0, padding: 0, margin: 0 })),
      ])
    ])
  ]
})
export class ContactsOutletComponent implements OnInit, OnDestroy{
  //observable
  public contacts$ = this._contactsStateService.contacts$;
  public searchedContacts$ = this._contactsStateService.searchedContacts$;

  public searchText$ = new BehaviorSubject<string>("");

  //subscriptions
  public routerSubscription: Subscription;

  //common
  public contactListAnimContinues = false;
  public searchedContactListAnimContinues = false;
  public isAnimationAllowed = true;

  constructor(
      protected _contactsStateService: ContactsStateService,
      protected _profileStateService: ProfileStateService,
      protected _textService: TextService,
      protected _router: Router,
      protected _changeDetectorRef: ChangeDetectorRef
  ) {
    this.routerSubscription = this._router.events
        .pipe(filter((event): event is NavigationStart => event instanceof NavigationStart))
        .subscribe(x => {
          this.isAnimationAllowed = false;
          this._changeDetectorRef.detectChanges();
        })
  }

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

  ngOnDestroy() {
    this.routerSubscription.unsubscribe();
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

  setContactListAnimContinues = (v: boolean) => this.contactListAnimContinues = v;
  setSearchedContactListAnimContinues = (v: boolean) => this.searchedContactListAnimContinues = v;
}
