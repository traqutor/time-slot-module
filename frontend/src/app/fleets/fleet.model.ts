import {EntityStatusEnum, ICustomer} from "../user/user.model";

export interface IFleet {
  id: 0;
  name: string;
  customer: ICustomer;
  createdBy: number;
  modifiedBy: number;
  creationDate: Date;
  modificationDate: Date;
  entityStatus: EntityStatusEnum;
}
