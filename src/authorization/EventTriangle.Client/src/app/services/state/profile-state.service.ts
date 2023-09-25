import {Injectable} from "@angular/core";
import {UserDto} from "../../types/models/consumer/UserDto";

@Injectable({
  providedIn: 'root'
})
export class ProfileStateService {
  public wasAuthenticationCheck = false;
  public isAuthenticated = false;

  public user: UserDto | null = null;
}
