import {Injectable} from "@angular/core";
import {UserDto} from "../../types/models/consumer/UserDto";
import {WalletDto} from "../../types/models/consumer/WalletDto";
import {UserRole} from "../../types/enums/UserRole";
import {UserStatus} from "../../types/enums/UserStatus";

@Injectable({
  providedIn: 'root'
})
export class ProfileStateService {
  public user: UserDto;

  constructor() {
    const emptyWallet : WalletDto = {
      id: '',
      balance: 0,
      lastTransactionId: ''
    };

    this.user = {
      id: '',
      email: '',
      userRole: UserRole.User,
      userStatus: UserStatus.Active,
      walletId: '',
      wallet: emptyWallet
    };
  }
}
