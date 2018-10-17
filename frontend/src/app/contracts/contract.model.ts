import {IUser} from "../users/user.model";
import {IVendor} from "../vendors/vendor.model";
import {ISupplier} from "../suppliers/supplier.model";
import {ICommodity} from "../commodities/commodity.model";

export interface IContract {
  id: number;
  name: string;
  vendor: IVendor
  supplier: ISupplier
  commodity: ICommodity
  creationDate: Date;
  modificationDate: Date;
  createdBy: IUser;
  modifiedBy: IUser;
  entityStatus: number;
}
