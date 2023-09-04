import {UserRole} from "../enums/UserRole";
import {UserStatus} from "../enums/UserStatus";
import {WalletDto} from "./WalletDto";

export interface UserDto {
  id: string;
  email: string;
  userRole: UserRole;
  userStatus: UserStatus;
  walletId: string;
  wallet: WalletDto;
}
