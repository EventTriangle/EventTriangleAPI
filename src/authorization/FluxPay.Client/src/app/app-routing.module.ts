import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LayoutComponent } from './pages/layout/layout.component';

const routes: Routes = [
  {
    path: '', component: LayoutComponent,
    children: [
      { path: 'transactions', loadComponent: () => import('./components/transactions-outlet/transactions-outlet.component').then(m => m.TransactionsOutletComponent) },
      { path: 'cards', loadComponent: () => import('./components/cards-outlet/cards-outlet.component').then(m => m.CardsOutletComponent) },
      { path: 'deposit', loadComponent: () => import('./components/deposit-outlet/deposit-outlet.component').then(m => m.DepositOutletComponent) },
      { path: 'contacts', loadComponent: () => import('./components/contacts-outlet/contacts-outlet.component').then(m => m.ContactsOutletComponent) },
      { path: 'support', loadComponent: () => import('./components/support-outlet/support-outlet.component').then(m => m.SupportOutletComponent) },
      { path: 'tickets', loadComponent: () => import('./components/tickets-outlet/tickets-outlet.component').then(m => m.TicketsOutletComponent) },
      { path: 'users', loadComponent: () => import('./components/users-outlet/users-outlet.component').then(m => m.UsersOutletComponent) }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
