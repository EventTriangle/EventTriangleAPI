import {UserDto} from "./UserDto";

export interface ContactDto {
  userId: string;
  contactId: string;
  contact: UserDto;
}
