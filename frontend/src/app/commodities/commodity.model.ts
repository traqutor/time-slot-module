import {IUser} from "../users/user.model";

export interface ICommodity {
  id: number;
  name: string;
  creationDate: Date;
  modificationDate: Date;
  createdBy: IUser;
  modifiedBy: IUser;
  entityStatus: number;
}
