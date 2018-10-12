import {EntityStatusEnum} from "../users/user.model";

export interface ITimeSlot {
  id: number;
  startTime: string;
  endTime: string;
  creationDate: Date;
  modificationDate: Date;
  createdBy: number;
  modifiedBy: number;
  entityStatus: EntityStatusEnum;
}
