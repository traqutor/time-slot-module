import {EntityStatusEnum, ICustomer, IUser} from "../users/user.model";

export interface IFleet {
  id: 0;
  name: string;
  customer: ICustomer;
  createdBy: IUser;
  modifiedBy: IUser;
  creationDate: Date;
  modificationDate: Date;
  entityStatus: EntityStatusEnum;
}
