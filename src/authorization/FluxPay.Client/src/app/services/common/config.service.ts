import { Injectable } from '@angular/core';
import {ErrorMessageConstants} from "../../constants/ErrorMessageConstants";

@Injectable({
  providedIn: 'root'
})
export class ConfigService {
  constructor() { }

  public getServerUrl = () => {
    const serverUrl = localStorage.getItem("serverUrl");
    if (!serverUrl) throw new Error(ErrorMessageConstants.ServerUrlIsUndefined);

    return serverUrl;
  };
}
