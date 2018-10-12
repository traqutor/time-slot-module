import { Injectable } from '@angular/core';
import {ISupplier} from "../suppliers/supplier.model";
import {BehaviorSubject, Observable} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {MatSnackBar} from "@angular/material";
import {environment} from "../../environments/environment";
import {ICommodity} from "./commodity.model";

@Injectable({
  providedIn: 'root'
})
export class CommodityService {

  private commodities: Array<ICommodity> = [];
  public commoditiesChanged: BehaviorSubject<Array<ICommodity>> = new BehaviorSubject<Array<ICommodity>>([]);

  private url: string;

  constructor(private http: HttpClient,
              private snackBar: MatSnackBar) {
    this.url = environment.url;
  }

  getCommodities() {
    this.http.get(`${this.url}/api/Commodities/GetCommodities`)
      .subscribe((res: Array<ICommodity>) => {
        this.commodities = res;
        this.commoditiesChanged.next(this.commodities);
      });
  }

  getCommoditysList() {
    this.http.get(`${this.url}/api/Commodities/GetCommodityList`)
      .subscribe((res: Array<ICommodity>) => {
        this.commodities = res;
        this.commoditiesChanged.next(this.commodities);
      });
  }

  getCommodityById(commodityId: number): Observable<ICommodity> {
    return this.http.get<ICommodity>(`${this.url}/api/Commodities/GetCommodity/${commodityId}`);
  }

  putCommodity(commodity: ICommodity, index: number)  {

    this.http.put(`${this.url}/api/Commodities/PutCommodity`, commodity)
      .subscribe((res: ICommodity) => {

        // in case when entry entity ID is 0 that means Add action
        // else is Edit so the object needs to be replaced in array

        if (commodity.id === 0) {

          this.commodities.push(res);
          this.snackBar.open('Commodity Added', '', {
            duration: 2000,
          });

        } else {

          this.commodities[index] = res;
          this.snackBar.open('Commodity Changed', '', {
            duration: 2000,
          });

        }

        this.commoditiesChanged.next(this.commodities);
      });
  }

  deleteCommodity(commodityId: number, index: number) {
    this.http.delete(`${this.url}/api/Commodities/DeleteCommodity/${commodityId}`)
      .subscribe(() => {
        this.commodities.splice(index, 1);
        this.commoditiesChanged.next(this.commodities);
        this.snackBar.open('Commodity Deleted!', '', {
          duration: 2000,
        });
      });
  }

}
