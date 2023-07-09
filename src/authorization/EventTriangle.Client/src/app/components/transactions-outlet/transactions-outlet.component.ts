import { Component, AfterViewInit } from '@angular/core';
import {animate, query, stagger, state, style, transition, trigger} from "@angular/animations";

@Component({
  selector: 'app-transactions-outlet',
  templateUrl: './transactions-outlet.component.html',
  styleUrls: ['./transactions-outlet.component.scss'],
  animations: [
    trigger('walletAnimation', [
      transition(':enter', [
        style({ opacity: 0}),
        animate('.5s', style({opacity: 1}))
      ])
    ]),
    trigger('transactionItemAnimation', [
      transition(':enter', [
        query(':enter', style({ transform: 'translateY(-5px)', opacity: 0 }), { optional: true }),
        query(':enter', stagger('50ms', [
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
export class TransactionsOutletComponent {
  transactions = [1, 1, 2, 3, 2];
}
