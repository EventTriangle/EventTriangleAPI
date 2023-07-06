import { Component } from '@angular/core';
import {animate, style, transition, trigger} from "@angular/animations";

@Component({
  selector: 'app-support-outlet',
  templateUrl: './support-outlet.component.html',
  styleUrls: ['./support-outlet.component.scss'],
  animations: [
    trigger("supportTicketListAnimation", [
      transition(":enter", [
        style({transform: 'translateY(-10px)', opacity: 0}),
        animate(".3s", style({transform: 'translateY(0)', opacity: 1}))
      ])
    ])
  ]
})
export class SupportOutletComponent {

}
