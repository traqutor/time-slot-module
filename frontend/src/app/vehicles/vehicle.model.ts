import {IFleet} from "../fleets/fleet.model";
import {EntityStatusEnum} from "../user/user.model";

export interface IVehicle {
  id: number,
  rego: string;
  fleet: IFleet
  createdBy: number;
  modifiedBy: number;
  creationDate: Date;
  modificationDate: Date;
  entityStatus: EntityStatusEnum
}
