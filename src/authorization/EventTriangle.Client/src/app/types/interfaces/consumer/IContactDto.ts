import {IUserDto} from "./IUserDto";

export interface IContactDto {
  userId: string;
  contactId: string;
  contact: IUserDto;
}
