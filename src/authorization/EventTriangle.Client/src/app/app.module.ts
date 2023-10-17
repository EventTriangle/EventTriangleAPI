import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LayoutComponent } from './pages/layout/layout.component';
import { HeaderComponent } from './components/header/header.component';
import { HttpClient, HttpClientModule } from '@angular/common/http';
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
import { APP_BASE_HREF } from '@angular/common';
import {FormsModule} from "@angular/forms";
import { LoginWindowComponent } from './components/login-window/login-window.component';
import { SkeletonModule } from 'primeng/skeleton';
import {NgxMaskDirective, NgxMaskPipe, provideEnvironmentNgxMask} from "ngx-mask";

const initializeAppFactory = (): Promise<void> => {
  const configUrl = 'assets/config/config.json';
  const key = 'baseUrl';

  return fetch(configUrl, {
    method: 'GET'
  })
    .then((res) => res.json())
    .then((data) => {
      localStorage.setItem(key, data.baseUrl);
    });
};


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
    UsersOutletComponent,
    LoginWindowComponent
  ],
  imports: [
      BrowserModule,
      AppRoutingModule,
      HttpClientModule,
      BrowserAnimationsModule,
      AngularSvgIconModule.forRoot(),
      FormsModule,
      SkeletonModule,
      NgxMaskDirective,
      NgxMaskPipe
  ],
  providers: [
    {
      provide: APP_BASE_HREF,
      useValue: "/app"
    },
    {
      provide: APP_INITIALIZER,
      useFactory: () => initializeAppFactory,
      deps: [HttpClient],
      multi: true
    },
    provideEnvironmentNgxMask()
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
