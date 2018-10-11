import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../environments/environment";
import {BehaviorSubject, Observable} from "rxjs";
import {ICustomer} from "../user/user.model";

@Injectable({
  providedIn: 'root'
})
export class CustomerService {

  private customers: Array<ICustomer> = [];
  public customersChanged: BehaviorSubject<Array<ICustomer>> = new BehaviorSubject<Array<ICustomer>>([]);

  private url: string;

  constructor(private http: HttpClient) {
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

  putCustomer(customer: ICustomer, index: number): boolean {
    this.http.put(`${this.url}/api/Customers/PutCustomer`, customer)
      .subscribe((res: ICustomer) => {
        if (index) {
          this.customers[index] = res;
        } else {
          this.customers.push(res);
        }
        this.customersChanged.next(this.customers);
      }, error => {
        return false;
      });
    return true;
  }

  deleteOrder(customerId: number, index: number): boolean {
    this.http.delete(`${this.url}/api/Customers/DeleteCustomer/${customerId}`)
      .subscribe(() => {
        this.customers.splice(index, 1);
        this.customersChanged.next(this.customers);
      }, error1 => {
        return false;
      });
    return true;
  }
}
