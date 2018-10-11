import {Component, OnInit} from '@angular/core';
import {CustomerService} from "./customer.service";
import {ICustomer} from "../user/user.model";

@Component({
  selector: 'app-custmer',
  templateUrl: './custmer.component.html',
  styleUrls: ['./custmer.component.css']
})
export class CustmerComponent implements OnInit {

  public customers: Array<ICustomer> = [];

  private voidCustomer: ICustomer = {id: 0, name: 'Pusty customer', creationDate: null, modificationDate: null};
  private subscriptions = [];

  constructor(private customerService: CustomerService) {
  }

  addCustomer() {

    if (this.customerService.putCustomer(this.voidCustomer, null)) {

    }

  }

  editCustomer() {

  }

  deleteCusomer() {

  }

  ngOnInit() {

    // invoke Customers get from db
    this.customerService.getCustomers();

    // subscribe for Customers
    this.subscriptions.push(this.customerService.customersChanged
      .subscribe((res: Array<ICustomer>) => {
        this.subscriptions = res;
      }));
  }


}
