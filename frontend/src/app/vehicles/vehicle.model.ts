import {IFleet} from "../fleets/fleet.model";
import {EntityStatusEnum, IUser} from "../users/user.model";

export interface IVehicle {
  id: number,
  rego: string;
  fleet: IFleet
  createdBy: IUser;
  modifiedBy: IUser;
  creationDate: Date;
  modificationDate: Date;
  entityStatus: EntityStatusEnum
}

