import { Component } from '@angular/core';
import {ProfileStateService} from "../../services/state/profile-state.service";
import {TextService} from "../../services/common/text.service";
import {Router} from "@angular/router";

interface AutoCompleteCompleteEvent {
  originalEvent: Event;
  query: string;
}

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent {
  // p-autocomplete
  selectedItem: any;
  suggestions: any[] = [];

  // target words for p-autocomplete
  private readonly targetWordsForTransactions = ['transactions', 'transfer', 'payment', 'currency', 'account', 'balance', 'money', 'finances'];
  private readonly targetWordsForCards = ['credit card', 'payment', 'top up'];
  private readonly targetWordsForDeposit = ['deposit ', 'payment', 'top up', 'money'];
  private readonly targetWordsForContacts = ['contacts', 'payee', 'payer', 'sender', 'recipient', 'details'];
  private readonly targetWordsForSupport = ['support', 'customer service', 'assistance', 'query', 'issue resolution', 'helpline'];

  // page names
  private readonly TransactionsPageName = "Transactions";
  private readonly CreditCardsPageName = "Cards";
  private readonly DepositPageName = "Deposit";
  private readonly ContactsPageName = "Contacts";
  private readonly SupportPageName = "Support";

  //observable
  public user$ = this._profileStateService.user$;

  constructor(
    protected _profileStateService: ProfileStateService,
    protected _textService: TextService,
    private router: Router) {}

  search(event: AutoCompleteCompleteEvent) {
    const suggestionArr = []
    for (let item of this.targetWordsForTransactions) {
      if (item.includes(event.query.toLowerCase())) { suggestionArr.push(this.TransactionsPageName); break; }
    }
    for (let item of this.targetWordsForCards) {
      if (item.includes(event.query.toLowerCase())) { suggestionArr.push(this.CreditCardsPageName); break; }
    }
    for (let item of this.targetWordsForDeposit) {
      if (item.includes(event.query.toLowerCase())) { suggestionArr.push(this.DepositPageName); break; }
    }
    for (let item of this.targetWordsForContacts) {
      if (item.includes(event.query.toLowerCase())) { suggestionArr.push(this.ContactsPageName); break; }
    }
    for (let item of this.targetWordsForSupport) {
      if (item.includes(event.query.toLowerCase())) { suggestionArr.push(this.SupportPageName); break; }
    }

    this.suggestions = suggestionArr;
  }

  //events
  onSelectAutocompletedItemHandler(e: string) {
    switch (e) {
      case this.TransactionsPageName:
        this.router.navigate([`/${this.TransactionsPageName.toLowerCase()}`]);
        break;
      case this.CreditCardsPageName:
        this.router.navigate([`/${this.CreditCardsPageName.toLowerCase()}`]);
        break;
      case this.DepositPageName:
        this.router.navigate([`/${this.DepositPageName.toLowerCase()}`]);
        break;
      case this.ContactsPageName:
        this.router.navigate([`/${this.ContactsPageName.toLowerCase()}`]);
        break;
      case this.SupportPageName:
        this.router.navigate([`/${this.SupportPageName.toLowerCase()}`]);
        break;
    }
  }
}
