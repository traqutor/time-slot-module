import {EntityStatusEnum, ICustomer} from "../user/user.model";

export interface ISite {
  id: 0;
  name: string;
  customer: ICustomer;
  createdBy: number;
  modifiedBy: number;
  creationDate: Date;
  modificationDate: Date;
  entityStatus: EntityStatusEnum;
}


