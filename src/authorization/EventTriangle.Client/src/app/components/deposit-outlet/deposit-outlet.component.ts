import { Component } from '@angular/core';
import { animate, query, stagger, state, style, transition, trigger } from "@angular/animations";

@Component({
  selector: 'app-deposit-outlet',
  templateUrl: './deposit-outlet.component.html',
  styleUrls: ['./deposit-outlet.component.scss'],
  animations: [
    trigger('depositFormAnimation', [
      transition(':enter', [
        style({ transform: 'translateY(10px)', opacity: 0 }),
        animate('.25s', style({ transform: 'translateY(0px)', opacity: 1 }))
      ])
    ])
  ]
})
export class DepositOutletComponent {

}
