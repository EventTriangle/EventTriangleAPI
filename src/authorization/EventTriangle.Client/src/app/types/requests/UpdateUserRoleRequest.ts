import {UserRole} from "../enums/UserRole";

export class UpdateUserRoleRequest {
  userId: string;
  userRole: UserRole;

  constructor(userId: string, userRole: UserRole) {
    this.userId = userId;
    this.userRole = userRole;
  }
}
