import {UserRole} from "../enums/UserRole";

export interface UpdateUserRoleEvent {
  id: string;
  requesterId: string;
  userId: string;
  userRole: UserRole;
  createdAt: string;
}
