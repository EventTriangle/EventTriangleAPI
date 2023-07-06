import { Component } from '@angular/core';
import {animate, query, stagger, style, transition, trigger} from "@angular/animations";

@Component({
  selector: 'app-contacts-outlet',
  templateUrl: './contacts-outlet.component.html',
  styleUrls: ['./contacts-outlet.component.scss'],
  animations: [
    trigger('contactListAnimation', [
      transition(':enter', [
        query(':enter', style({ marginTop: -5, opacity: 0 }), { optional: true }),
        query(':enter', stagger('50ms', [
          animate('150ms', style({ marginTop: 0, opacity: 1 }))
        ]), { optional: true })
      ])
    ])
  ]
})
export class ContactsOutletComponent {
  contacts = [1, 1, 1, 1, 1]
}
