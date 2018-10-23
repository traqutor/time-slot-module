import {EntityStatusEnum, IUser} from "../users/user.model";

export interface IVendor {
  id: number;
  name: string;
  creationDate: Date;
  modificationDate: Date;
  createdBy: IUser;
  modifiedBy: IUser;
  entityStatus: EntityStatusEnum
}
