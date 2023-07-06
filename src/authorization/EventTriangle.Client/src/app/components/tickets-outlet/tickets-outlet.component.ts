import { Component } from '@angular/core';
import {animate, style, transition, trigger} from "@angular/animations";

@Component({
  selector: 'app-tickets-outlet',
  templateUrl: './tickets-outlet.component.html',
  styleUrls: ['./tickets-outlet.component.scss'],
  animations: [
    trigger("ticketListAnimation", [
      transition(":enter", [
        style({transform: 'translateY(-10px)', opacity: 0}),
        animate(".3s", style({transform: 'translateY(0)', opacity: 1}))
      ])
    ])
  ]
})
export class TicketsOutletComponent {

}
