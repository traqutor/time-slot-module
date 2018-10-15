import {EntityStatusEnum, ICustomer, IUser} from "../users/user.model";
import {IStatusType} from "../status-types/status-type.model";
import {IContract} from "../contracts/contract.model";
import {ICommodity} from "../commodities/commodity.model";
import {ISupplier} from "../suppliers/supplier.model";
import {IVendor} from "../vendors/vendor.model";
import {ISite} from "../sites/site.model";
import {IVehicle} from "../vehicles/vehicle.model";

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


export interface ITimeSlotDelivery {
  id: number;
  tons: number;
  timeSlot: ITimeSlot;
  deliveryDate: Date;
  statusType: IStatusType;
  contract: IContract,
  commodity: ICommodity,
  supplier: ISupplier,
  vendor: IVendor,
  customer: ICustomer,
  site: ISite,
  vehicle: IVehicle,
  driver: IUser,
  creationDate: Date,
  modificationDate: Date,
  createdBy: IUser;
  modifiedBy: IUser;
  entityStatus: EntityStatusEnum;
}
