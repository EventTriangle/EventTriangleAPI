import {UserRole} from "../enums/UserRole";

export interface UpdateUserRoleRequest {
  userId: string;
  userRole: UserRole;
}
