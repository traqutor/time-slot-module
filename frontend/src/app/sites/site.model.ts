import {EntityStatusEnum, ICustomer} from "../users/user.model";

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


