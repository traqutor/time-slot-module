import {MatDialog} from "@angular/material";
import {Component, OnDestroy, OnInit} from '@angular/core';

import {EntityStatusEnum, ICustomer} from "../user/user.model";
import {CustomerService} from "../custmer/customer.service";
import {ConfirmDialogService} from "../common/confirm-dialog/confirm-dialog.service";
import {CustomerDialogComponent} from "../custmer/customer-dialog/customer-dialog.component";
import {IVendor} from "./vendor.model";
import {VendorDialogComponent} from "./vendor-dialog/vendor-dialog.component";
import {VendorService} from "./vendor.service";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-vendors',
  templateUrl: './vendors.component.html',
  styleUrls: ['./vendors.component.css']
})
export class VendorsComponent implements OnInit, OnDestroy {


  public vendors: Array<IVendor> = [];

  private voidVendor: IVendor = {
    id: 0,
    name: '',
    creationDate: null,
    modificationDate: null,
    createdBy: null,
    modifiedBy: null,
    entityStatus: EntityStatusEnum.NORMAL
  };
  private subscriptions = [];

  constructor(private vendorService: VendorService,
              private confirm: ConfirmDialogService,
              private dialog: MatDialog) {
  }

  addVendor() {
    this.editVendor(this.voidVendor, -1);
  }

  editVendor(vendor: IVendor, index: number) {
    const dialogRef = this.dialog.open(VendorDialogComponent, {
      width: '45%',
      disableClose: true,
      data: vendor,
    });
    dialogRef.afterClosed()
      .subscribe((resolvedVendor: IVendor) => {

        if (resolvedVendor) {
          this.vendorService.putVendor(resolvedVendor, index);
        }

      });
  }

  deleteVendor(vendor: IVendor, index: number) {
    this.confirm.confirm('Delete Vendor', 'Are you sure you would like to delete the Vendor?')
      .subscribe((res: boolean) => {
        if (res) {
          this.vendorService.deleteOrder(vendor.id, index);
        }
      });
  }

  ngOnInit() {

    // invoke Customers get from db by Admin
    this.vendorService.getVendors();

    // subscribe for Customers
    this.subscriptions.push(this.vendorService.vendorsChanged
      .subscribe((res: Array<IVendor>) => {
        this.vendors = res;
      }));
  }

  ngOnDestroy() {
    this.subscriptions.forEach( (sub: Subscription) => {
      sub.unsubscribe();
    })
  }

}
