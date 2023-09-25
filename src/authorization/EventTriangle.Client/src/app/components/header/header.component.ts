import { Component } from '@angular/core';
import {ProfileStateService} from "../../services/state/profile-state.service";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent {
  constructor(protected _profileStateService: ProfileStateService) {}
}
