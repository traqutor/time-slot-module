import { Injectable } from '@angular/core';
import {BehaviorSubject, Observable} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {MatSnackBar} from "@angular/material";
import {environment} from "../../environments/environment";
import {ISupplier} from "./supplier.model";

@Injectable({
  providedIn: 'root'
})
export class SupplierService {

  private suppliers: Array<ISupplier> = [];
  public suppliersChanged: BehaviorSubject<Array<ISupplier>> = new BehaviorSubject<Array<ISupplier>>([]);

  private url: string;

  constructor(private http: HttpClient,
              private snackBar: MatSnackBar) {
    this.url = environment.url;
  }

  getSuppliers() {
    this.http.get(`${this.url}/api/Suppliers/GetSuppliers`)
      .subscribe((res: Array<ISupplier>) => {
        this.suppliers = res;
        this.suppliersChanged.next(this.suppliers);
      });
  }

  getSuppliersList() {
    this.http.get(`${this.url}/api/Suppliers/GetSupplierList`)
      .subscribe((res: Array<ISupplier>) => {
        this.suppliers = res;
        this.suppliersChanged.next(this.suppliers);
      });
  }

  getSupplierById(supplierId: number): Observable<ISupplier> {
    return this.http.get<ISupplier>(`${this.url}/api/Suppliers/GetSupplier/${supplierId}`);
  }

  putSupplier(supplier: ISupplier, index: number)  {

    this.http.put(`${this.url}/api/Suppliers/PutSupplier`, supplier)
      .subscribe((res: ISupplier) => {

        // in case when entry entity ID is 0 that means Add action
        // else is Edit so the object needs to be replaced in array

        if (supplier.id === 0) {

          this.suppliers.push(res);
          this.snackBar.open('Supplier Added', '', {
            duration: 2000,
          });

        } else {

          this.suppliers[index] = res;
          this.snackBar.open('Supplier Changed', '', {
            duration: 2000,
          });

        }

        this.suppliersChanged.next(this.suppliers);
      });
  }

  deleteSupplier(supplierId: number, index: number) {
    this.http.delete(`${this.url}/api/Suppliers/DeleteSupplier/${supplierId}`)
      .subscribe(() => {
        this.suppliers.splice(index, 1);
        this.suppliersChanged.next(this.suppliers);
        this.snackBar.open('Supplier Deleted!', '', {
          duration: 2000,
        });
      });
  }

}
