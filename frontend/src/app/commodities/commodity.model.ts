import {IUser} from "../users/user.model";

export interface ICommodity {
  id: number;
  name: string;
  maxTonsPerDay: number;
  creationDate: Date;
  modificationDate: Date;
  createdBy: IUser;
  modifiedBy: IUser;
  entityStatus: number;
}
