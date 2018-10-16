import {Component, OnDestroy, OnInit} from '@angular/core';
import {CustomerService} from "./customer.service";
import {EntityStatusEnum, ICustomer} from "../users/user.model";
import {CustomerDialogComponent} from "./customer-dialog/customer-dialog.component";
import {MatDialog} from "@angular/material";
import {ConfirmDialogService} from "../common/confirm-dialog/confirm-dialog.service";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-custmer',
  templateUrl: './custmer.component.html',
  styleUrls: ['./custmer.component.css']
})
export class CustmerComponent implements OnInit, OnDestroy {

  public customers: Array<ICustomer> = [];

  private voidCustomer: ICustomer = {
    id: 0,
    name: '',
    creationDate: null,
    modificationDate: null,
    createdBy: null,
    modifiedBy: null,
    entityStatus: EntityStatusEnum.NORMAL
  };
  private subscriptions = [];

  constructor(private customerService: CustomerService,
              private confirm: ConfirmDialogService,
              private dialog: MatDialog) {
  }

  addCustomer() {
    this.editCustomer(this.voidCustomer, -1);
  }

  editCustomer(customer: ICustomer, index: number) {
    const dialogRef = this.dialog.open(CustomerDialogComponent, {
      width: '65%',
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
    this.confirm.confirm('Delete Customer', 'Are you sure you would like to delete the Customer?')
      .subscribe((res: boolean) => {
        if (res) {
          this.customerService.deleteOrder(customer.id, index);
        }
      });
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

  ngOnDestroy() {
    this.subscriptions.forEach((sub: Subscription) => {
      sub.unsubscribe();
    })
  }

}
