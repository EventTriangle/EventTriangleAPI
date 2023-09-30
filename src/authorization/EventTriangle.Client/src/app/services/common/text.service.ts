import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TextService {

  public pullUsernameFromMail(mail: string) {
    const username = mail.split('@')[0];

    if (!username) throw new Error("Invalid mail");

    return username;
  }
}
