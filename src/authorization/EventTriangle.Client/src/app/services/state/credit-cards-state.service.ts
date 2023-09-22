import {Injectable} from "@angular/core";
import {CreditCardDto} from "../../types/models/consumer/CreditCardDto";

@Injectable({
  providedIn: 'root'
})
export class CreditCardsStateService {
  public cards: CreditCardDto[] = [];

  constructor() { }
}
