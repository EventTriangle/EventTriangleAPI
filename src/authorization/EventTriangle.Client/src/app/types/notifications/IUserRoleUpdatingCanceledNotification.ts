import {UserRole} from "../enums/UserRole";

export interface IUserRoleUpdatingCanceledNotification {
    userId: string;
    userRole: UserRole;
    reason: string;
}