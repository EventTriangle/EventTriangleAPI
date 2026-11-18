import {UserRole} from "../../enums/UserRole";
import {UserStatus} from "../../enums/UserStatus";
import {IWalletDto} from "./IWalletDto";

export interface IUserDto {
  id: string;
  email: string;
  userRole: UserRole;
  userStatus: UserStatus;
  walletId: string;
  wallet: IWalletDto;
}
