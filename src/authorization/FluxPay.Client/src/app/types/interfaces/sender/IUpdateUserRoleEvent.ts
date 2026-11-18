import {UserRole} from "../../enums/UserRole";

export interface IUpdateUserRoleEvent {
  id: string;
  requesterId: string;
  userId: string;
  userRole: UserRole;
  createdAt: string;
}
