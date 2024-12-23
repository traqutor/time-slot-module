import {ISite} from "../sites/site.model";
import {IFleet} from "../fleets/fleet.model";
import {IVehicle} from "../vehicles/vehicle.model";


export interface IUser {

  id: number;
  email: string;
  password: string;
  name: string;
  surname: string;
  role: IRole;
  customer: ICustomer,
  site: ISite,
  fleet: IFleet,
  vehicles: Array<IVehicle>;
  entityStatus: EntityStatusEnum;

}

export interface AuthResponse {
  '.expires': string;
  '.issued': string;
  access_token: string;
  expires_in: number;
  token_type: string;
  userName: string;
}


export interface IUserInfo {
  id: number;
  email: string;
  password: string;
  name: string;
  surname: string;
  role: IRole;
  customer: ICustomer;
  site: ISite
  entityStatus: EntityStatusEnum
}


export interface IRole {
  id: string;
  name: string;
}

export interface IRoleResult {
  resultsCount: number;
  results: Array<IRole>;
}

export interface ICustomer {
  id: number;
  name: string;
  createdBy: IUser;
  modifiedBy: IUser;
  creationDate: Date,
  modificationDate: Date
  entityStatus: EntityStatusEnum
}


export enum EntityStatusEnum {
  NORMAL = 0, DELETED = 1, DISABLED = 2
}

// User roles
export enum UserRoleNameEnum {
  Administrator = 'Administrator', // see menu, can select Customer on main page
  CustomerAdmin = 'CustomerAdmin', // see menu,
  CustomerUser = 'CustomerUser', //
  Driver = 'Driver', // see all sites related to linked to customer One customer
  SiteUser = 'SiteUser' // see the same as driver
}
