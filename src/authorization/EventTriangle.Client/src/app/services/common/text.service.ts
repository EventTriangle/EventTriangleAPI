import { Injectable } from '@angular/core';
import {ErrorMessageConstants} from "../../constants/ErrorMessageConstants";

@Injectable({
  providedIn: 'root'
})
export class TextService {

  public pullUsernameFromMail(mail: string) {
    const username = mail.split('@')[0];

    if (!username) throw new Error(ErrorMessageConstants.InvalidMail);

    return username;
  }

  public reformatCardNumbers(cardNumbers: string) {
    const array = cardNumbers.split('').map((x, i) => (i + 1) % 4 === 0 ? x + " " : x);

    return array.join('');
  }
}
