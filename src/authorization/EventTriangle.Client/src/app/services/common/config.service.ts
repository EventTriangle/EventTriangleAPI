import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ConfigService {
  constructor() { }

  public getServerUrl = () => {
    const serverUrl = localStorage.getItem("serverUrl");

    if (!serverUrl) throw new Error("Server url is not defined");

    return serverUrl;
  };
}
