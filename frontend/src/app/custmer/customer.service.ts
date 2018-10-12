import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../environments/environment";
import {BehaviorSubject, Observable} from "rxjs";
import {ICustomer} from "../users/user.model";
import {MatSnackBar} from "@angular/material";

@Injectable({
  providedIn: 'root'
})
export class CustomerService {

  private customers: Array<ICustomer> = [];
  public customersChanged: BehaviorSubject<Array<ICustomer>> = new BehaviorSubject<Array<ICustomer>>([]);

  private url: string;

  constructor(private http: HttpClient, private snackBar: MatSnackBar) {
    this.url = environment.url;
  }

  getCustomers() {
    this.http.get(`${this.url}/api/Customers/GetCustomers`)
      .subscribe((res: Array<ICustomer>) => {
        this.customers = res;
        this.customersChanged.next(this.customers);
      });
  }

  getCustomerById(customerId: number): Observable<ICustomer> {
    return this.http.get<ICustomer>(`${this.url}/api/Customers/GetCustomer/${customerId}`);
  }

  putCustomer(customer: ICustomer, index: number) {

    console.log('customer', customer);
    console.log('index', index);

    this.http.put(`${this.url}/api/Customers/PutCustomer`, customer)
      .subscribe((res: ICustomer) => {

        // in case when entry entity ID is 0 that means Add action
        // else is Edit so the object needs to be replaced in array

        if (customer.id === 0) {

          this.customers.push(res);
          this.snackBar.open('Customer Added', '', {
            duration: 2000,
          });

        } else {

          this.customers[index] = res;
          this.snackBar.open('Customer Changed', '', {
            duration: 2000,
          });

        }

        this.customersChanged.next(this.customers);
      });
  }

  deleteOrder(customerId: number, index: number) {
    this.http.delete(`${this.url}/api/Customers/DeleteCustomer/${customerId}`)
      .subscribe(() => {
        this.customers.splice(index, 1);
        this.customersChanged.next(this.customers);
        this.snackBar.open('Customer Deleted!', '', {
          duration: 2000,
        });
      });
  }

}
