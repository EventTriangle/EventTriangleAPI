import {Injectable} from "@angular/core";
import {UserDto} from "../../types/models/consumer/UserDto";
import {UserRole} from "../../types/enums/UserRole";
import {UserStatus} from "../../types/enums/UserStatus";
import {WalletDto} from "../../types/models/consumer/WalletDto";

@Injectable({
  providedIn: 'root'
})
export class TransactionsOutletHelper {
  public generateEmptyUser() : UserDto {
    const emptyWallet : WalletDto = {
      id: '',
      balance: 0,
      lastTransactionId: ''
    };

    const emptyUser : UserDto = {
      id: '',
      email: '',
      userRole: UserRole.User,
      userStatus: UserStatus.Active,
      walletId: '',
      wallet: emptyWallet
    };

    return emptyUser;
  }
}
