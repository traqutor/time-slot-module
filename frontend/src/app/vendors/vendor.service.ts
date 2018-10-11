import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {MatSnackBar} from "@angular/material";
import {BehaviorSubject, Observable} from "rxjs";

import {environment} from "../../environments/environment";
import {IVendor} from "./vendor.model";

@Injectable({
  providedIn: 'root'
})
export class VendorService {
  private vendors: Array<IVendor> = [];
  public vendorsChanged: BehaviorSubject<Array<IVendor>> = new BehaviorSubject<Array<IVendor>>([]);

  private url: string;

  constructor(private http: HttpClient,
              private snackBar: MatSnackBar) {
    this.url = environment.url;
  }

  getVendors() {
    this.http.get(`${this.url}/api/Vendors/GetVendors`)
      .subscribe((res: Array<IVendor>) => {
        this.vendors = res;
        this.vendorsChanged.next(this.vendors);
      });
  }

  getVendorList() {
    this.http.get(`${this.url}/api/Vendors/GetVendorList`)
      .subscribe((res: Array<IVendor>) => {
        this.vendors = res;
        this.vendorsChanged.next(this.vendors);
      });
  }

  getVendorById(vendorId: number): Observable<IVendor> {
    // todo different data model not IVendor
    return this.http.get<IVendor>(`${this.url}/api/Vendors/GetVendor/${vendorId}`);
  }

  putVendor(vendor: IVendor, index: number)  {

    console.log('vendor', vendor);
    console.log('index', index);

    this.http.put(`${this.url}/api/Vendors/PutVendor`, vendor)
      .subscribe((res: IVendor) => {

        // in case when entry entity ID is 0 that means Add action
        // else is Edit so the object needs to be replaced in array

        if (vendor.id === 0) {

          this.vendors.push(res);
          this.snackBar.open('Vendor Added', '', {
            duration: 2000,
          });

        } else {

          this.vendors[index] = res;
          this.snackBar.open('Vendor Changed', '', {
            duration: 2000,
          });

        }

        this.vendorsChanged.next(this.vendors);
      });
  }

  deleteOrder(vendorId: number, index: number) {
    this.http.delete(`${this.url}/api/Vendors/DeleteVendor/${vendorId}`)
      .subscribe(() => {
        this.vendors.splice(index, 1);
        this.vendorsChanged.next(this.vendors);
        this.snackBar.open('Vendor Deleted!', '', {
          duration: 2000,
        });
      });
  }
}
