import {EntityStatusEnum} from "../users/user.model";

export interface IVendor {
  id: number;
  name: string;
  creationDate: Date;
  modificationDate: Date;
  createdBy: number;
  modifiedBy: number;
  entityStatus: EntityStatusEnum
}
