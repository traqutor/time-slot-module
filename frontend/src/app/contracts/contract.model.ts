import {IUser} from "../users/user.model";

export interface IContract {
  id: number;
  name: string;
  creationDate: Date;
  modificationDate: Date;
  createdBy: IUser;
  modifiedBy: IUser;
  entityStatus: number;
}
