import {Component, OnInit} from '@angular/core';
import {CustomerService} from "./customer.service";
import {ICustomer} from "../user/user.model";
import {CustomerDialogComponent} from "./customer-dialog/customer-dialog.component";
import {MatDialog} from "@angular/material";

@Component({
  selector: 'app-custmer',
  templateUrl: './custmer.component.html',
  styleUrls: ['./custmer.component.css']
})
export class CustmerComponent implements OnInit {

  public customers: Array<ICustomer> = [];

  private voidCustomer: ICustomer = {id: 0, name: '', creationDate: null, modificationDate: null};
  private subscriptions = [];

  constructor(private customerService: CustomerService, public dialog: MatDialog) {
  }

  addCustomer() {
    this.editCustomer(this.voidCustomer, -1);
  }

  editCustomer(customer: ICustomer, index: number) {
    const dialogRef = this.dialog.open(CustomerDialogComponent, {
      width: '45%',
      disableClose: true,
      data: customer,
    });
    dialogRef.afterClosed()
      .subscribe((resolvedCustomer: ICustomer) => {

        if (resolvedCustomer) {
          this.customerService.putCustomer(resolvedCustomer, index);
        }

      });
  }

  deleteCustomer(customer: ICustomer, index: number) {
    if (this.customerService.deleteOrder(customer.id, index)) {

    }
  }

  ngOnInit() {

    // invoke Customers get from db
    this.customerService.getCustomers();

    // subscribe for Customers
    this.subscriptions.push(this.customerService.customersChanged
      .subscribe((res: Array<ICustomer>) => {
        this.customers = res;
      }));
  }


}
