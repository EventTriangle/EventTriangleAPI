import {Component, OnInit} from '@angular/core';
import {animate, query, stagger, style, transition, trigger} from "@angular/animations";
import {UsersStateService} from "../../services/state/users-state.service";
import {BehaviorSubject} from "rxjs";
import {TextService} from "../../services/common/text.service";
import {IUserDto} from "../../types/interfaces/consumer/IUserDto";

@Component({
  selector: 'app-users-outlet',
  templateUrl: './users-outlet.component.html',
  styleUrls: ['./users-outlet.component.scss'],
  animations: [
    trigger('userListAnimation', [
      transition(':enter', [
        query(':enter', style({ marginTop: -5, opacity: 0 }), { optional: true }),
        query(':enter', stagger('50ms', [
          animate('300ms', style({ marginTop: 0, opacity: 1 }))
        ]), { optional: true })
      ])
    ]),
  ]
})
export class UsersOutletComponent implements OnInit {
  //observable
  public users$ = this._usersStateService.users$;
  public searchedUsers$ = this._usersStateService.searchedUsers$;

  public searchText$ = new BehaviorSubject<string>("");

  constructor(
      protected _usersStateService: UsersStateService,
      protected _textService: TextService
  ) {}

  async ngOnInit() {
    await this._usersStateService.getUsersAsync(25, 1);
  }

  //other
  identifyUserDto(index: number, item: IUserDto){
    return item.id;
  }
}
