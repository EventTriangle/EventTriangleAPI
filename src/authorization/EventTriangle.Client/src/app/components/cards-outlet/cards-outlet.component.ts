import { Component } from '@angular/core';
import { animate, query, stagger, style, transition, trigger } from "@angular/animations";

@Component({
  selector: 'app-cards-outlet',
  templateUrl: './cards-outlet.component.html',
  styleUrls: ['./cards-outlet.component.scss'],
  animations: [
    trigger('cardListAnimation', [
      transition(':enter', [
        query(':enter', style({ transform: 'translateY(-5px)', opacity: 0 }), { optional: true }),
        query(':enter', stagger('100ms', [
          animate('200ms', style({ transform: 'translateY(0)', opacity: 1 }))
        ]), { optional: true })
      ])
    ]),
    trigger('rightBarAnimation', [
      transition(':enter', [
        style({ transform: 'translateY(10px)', opacity: 0 }),
        animate('.25s', style({ transform: 'translateY(0px)', opacity: 1 }))
      ])
    ])
  ]
})
export class CardsOutletComponent {
  cards = [1, 2, 1, 1, 1]
}
