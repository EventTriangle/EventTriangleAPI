import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AuthorizationApiService } from 'src/app/services/api/authorization-api.service';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit {
  constructor(
    private httpClient: HttpClient,
    private authorozationApiService: AuthorizationApiService) { }

  public isAuthenticated = false;

  public ngOnInit(): void {
    const request = this.authorozationApiService.getIsAuthenticated();

    request.subscribe({
      next: (res) => this.isAuthenticated = res.authenticated,
      error: _ => window.location.href = this.authorozationApiService.getLoginPathForRedirection()
    });
  }
}
