import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LayoutComponent } from './pages/layout/layout.component';
import { TransactionsOutletComponent } from './components/transactions-outlet/transactions-outlet.component';
import { CardsOutletComponent } from './components/cards-outlet/cards-outlet.component';
import { DepositOutletComponent } from './components/deposit-outlet/deposit-outlet.component';
import { ContactsOutletComponent } from './components/contacts-outlet/contacts-outlet.component';
import { SupportOutletComponent } from './components/support-outlet/support-outlet.component';
import { TicketsOutletComponent } from './components/tickets-outlet/tickets-outlet.component';
import { UsersOutletComponent } from './components/users-outlet/users-outlet.component';

const routes: Routes = [
  {
    path: '', component: LayoutComponent,
    children: [
      { path: 'transactions', component: TransactionsOutletComponent },
      { path: 'cards', component: CardsOutletComponent },
      { path: 'deposit', component: DepositOutletComponent },
      { path: 'contacts', component: ContactsOutletComponent },
      { path: 'support', component: SupportOutletComponent },
      { path: 'tickets', component: TicketsOutletComponent },
      { path: 'users', component: UsersOutletComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
