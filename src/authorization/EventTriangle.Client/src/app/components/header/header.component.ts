import { Component } from '@angular/core';
import {ProfileStateService} from "../../services/state/profile-state.service";
import {TextService} from "../../services/common/text.service";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent {
  //observable
  public user$ = this._profileStateService.user$;

  constructor(
    protected _profileStateService: ProfileStateService,
    protected _textService: TextService) {}
}
