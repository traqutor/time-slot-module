import {IUser} from "../users/user.model";

export interface IStatusType {
  id: number;
  name: string;
  creationDate: Date;
  modificationDate: Date;
  createdBy: IUser;
  modifiedBy: IUser;
  entityStatus: number;
}
