import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LayoutComponent } from './pages/layout/layout.component';
import { HeaderComponent } from './components/header/header.component';
import { HttpClientModule } from '@angular/common/http';
import { AngularSvgIconModule } from 'angular-svg-icon';
import { MainComponent } from './components/main/main.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { TransactionsOutletComponent } from './components/transactions-outlet/transactions-outlet.component';
import { CardsOutletComponent } from './components/cards-outlet/cards-outlet.component';
import { DepositOutletComponent } from './components/deposit-outlet/deposit-outlet.component';
import { ContactsOutletComponent } from './components/contacts-outlet/contacts-outlet.component';
import { SupportOutletComponent } from './components/support-outlet/support-outlet.component';
import { TicketsOutletComponent } from './components/tickets-outlet/tickets-outlet.component';
import { UsersOutletComponent } from './components/users-outlet/users-outlet.component';

@NgModule({
  declarations: [
    AppComponent,
    LayoutComponent,
    HeaderComponent,
    MainComponent,
    NavbarComponent,
    TransactionsOutletComponent,
    CardsOutletComponent,
    DepositOutletComponent,
    ContactsOutletComponent,
    SupportOutletComponent,
    TicketsOutletComponent,
    UsersOutletComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    AngularSvgIconModule.forRoot(),
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
