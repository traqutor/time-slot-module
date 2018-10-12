import { Injectable } from '@angular/core';
import {IContract} from "../contracts/contract.model";
import {BehaviorSubject, Observable} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {MatSnackBar} from "@angular/material";
import {environment} from "../../environments/environment";
import {IStatusType} from "./status-type.model";

@Injectable({
  providedIn: 'root'
})
export class StatusTypeService {

  private statusTypes: Array<IStatusType> = [];
  public statusTypesChanged: BehaviorSubject<Array<IStatusType>> = new BehaviorSubject<Array<IStatusType>>([]);

  private url: string;

  constructor(private http: HttpClient,
              private snackBar: MatSnackBar) {
    this.url = environment.url;
  }

  getStatusTypes() {
    this.http.get(`${this.url}/api/StatusTypes/GetStatusTypes`)
      .subscribe((res: Array<IStatusType>) => {
        this.statusTypes = res;
        this.statusTypesChanged.next(this.statusTypes);
      });
  }

  getStatusTypesList() {
    this.http.get(`${this.url}/api/StatusTypes/GetStatusTypeList`)
      .subscribe((res: Array<IStatusType>) => {
        this.statusTypes = res;
        this.statusTypesChanged.next(this.statusTypes);
      });
  }

  getStatusTypeById(statusTypeId: number): Observable<IStatusType> {
    return this.http.get<IStatusType>(`${this.url}/api/StatusTypes/GetStatusType/${statusTypeId}`);
  }

  putStatusType(statusType: IStatusType, index: number) {

    this.http.put(`${this.url}/api/StatusTypes/PutStatusType`, statusType)
      .subscribe((res: IStatusType) => {

        // in case when entry entity ID is 0 that means Add action
        // else is Edit so the object needs to be replaced in array

        if (statusType.id === 0) {

          this.statusTypes.push(res);
          this.snackBar.open('Status Type Added', '', {
            duration: 2000,
          });

        } else {

          this.statusTypes[index] = res;
          this.snackBar.open('Status Type Changed', '', {
            duration: 2000,
          });

        }

        this.statusTypesChanged.next(this.statusTypes);
      });
  }

  deleteStatusType(statusTypeId: number, index: number) {
    this.http.delete(`${this.url}/api/StatusTypes/DeleteStatusType/${statusTypeId}`)
      .subscribe(() => {
        this.statusTypes.splice(index, 1);
        this.statusTypesChanged.next(this.statusTypes);
        this.snackBar.open('Status Type Deleted!', '', {
          duration: 2000,
        });
      });
  }

}
