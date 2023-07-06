import { Component } from '@angular/core';
import {animate, style, transition, trigger} from "@angular/animations";

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
  animations: [
    trigger("navbarAnimation", [
      transition(":enter", [
        style({opacity: 0}),
        animate(".3s", style({opacity: 1}))
      ])
    ])
  ]
})
export class NavbarComponent {

}
