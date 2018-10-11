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

export interface ICustomer {
  id: number;
  name: string;
  creationDate: Date,
  modificationDate: Date
}

export interface ISite {
  id: 0;
  name: string;
}


export enum EntityStatusEnum {
  NORMAL = 0, DELETED = 1, DISABLED = 2
}
