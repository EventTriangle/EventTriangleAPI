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
}
