import {Component, OnInit} from '@angular/core';
import {animate, query, stagger, style, transition, trigger} from "@angular/animations";
import {PaymentNetwork} from "../../types/enums/PaymentNetwork";
import {CreditCardsStateService} from "../../services/state/credit-cards-state.service";
import { ProfileStateService } from 'src/app/services/state/profile-state.service';
import {ICreditCardDto} from "../../types/interfaces/consumer/ICreditCardDto";
import {MenuItem} from "primeng/api";
import {ErrorMessageConstants} from "../../constants/ErrorMessageConstants";
import {TextService} from "../../services/common/text.service";

@Component({
  selector: 'app-cards-outlet',
  templateUrl: './cards-outlet.component.html',
  styleUrls: ['./cards-outlet.component.scss'],
  animations: [
    trigger('cardListAnimation', [
      transition(':enter', [
        query(':enter', style({ transform: 'translateY(-5px)', opacity: 0 }), { optional: true }),
        query(':enter', stagger('100ms', [animate('200ms', style({ transform: 'translateY(0)', opacity: 1 }))]), { optional: true })
      ])
    ]),
    trigger("cardItemAnimation", [
      transition(":enter", [
        style({opacity: 0}),
        animate(".3s", style({opacity: "*"}))
      ])
    ]),
    trigger('rightBarAnimation', [
      transition(':enter', [
        style({ transform: 'translateY(10px)', opacity: 0 }),
        animate('.25s', style({ transform: 'translateY(0px)', opacity: 1 }))
      ])
    ])
  ]
})
export class CardsOutletComponent implements OnInit {
  //observable
  public cards$ = this._creditCardsStateService.cards$;

  // input data
  public cardHolderName = '';
  public cardNumber = '';
  public expiration = '';
  public cvv = '';
  public paymentNetwork: PaymentNetwork = PaymentNetwork.Visa;
  public PaymentNetwork = PaymentNetwork;

  // state
  public selectedForContextMenuCard: ICreditCardDto | undefined;

  // context menu
  public contextMenuItems: MenuItem[] = [
    {label: 'Delete', command: async () => {
        const currentCardId = this.selectedForContextMenuCard?.id

        if (!currentCardId) throw new Error(ErrorMessageConstants.CardNotFound);

        await this._creditCardsStateService.deleteCreditCardAsync(currentCardId);
    }}
  ]

  constructor(
    protected _creditCardsStateService: CreditCardsStateService,
    protected _profileStateService: ProfileStateService,
    protected _textService: TextService) {

  }

  async ngOnInit() {
    if (!this._profileStateService.isAuthenticated) return;

    await this._creditCardsStateService.getCreditCardsAsync();
  }

  //events
  async onAttachCardOkClick() {
    if (this.expiration.length < 4) return;

    const expiration = this.expiration.slice(0, 2) + "/" + this.expiration.slice(2);

    const cardHolderName = this.cardHolderName;
    const cardNumber = this.cardNumber;
    const cvv = this.cvv;

    this.cardHolderName = "";
    this.cardNumber = "";
    this.expiration = "";
    this.cvv = "";

    await this._creditCardsStateService.attachCreditCardToAccountAsync(
      cardHolderName,
      cardNumber,
      expiration,
      cvv,
      this.paymentNetwork);
  }

  // action
  public setSelectedCardForContextMenu(card: ICreditCardDto) {
    this.selectedForContextMenuCard = card;
  }

  //other
  identifyCardDto(index: number, item: ICreditCardDto){
    return item.id;
  }
}
