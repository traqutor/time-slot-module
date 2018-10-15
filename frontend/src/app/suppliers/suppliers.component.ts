import {Component, OnDestroy, OnInit} from '@angular/core';
import {EntityStatusEnum} from "../users/user.model";
import {ConfirmDialogService} from "../common/confirm-dialog/confirm-dialog.service";
import {MatDialog} from "@angular/material";
import {Subscription} from "rxjs";
import {ISupplier} from "./supplier.model";
import {SupplierService} from "./supplier.service";
import {SupplierDialogComponent} from "./supplier-dialog/supplier-dialog.component";

@Component({
  selector: 'app-suppliers',
  templateUrl: './suppliers.component.html',
  styleUrls: ['./suppliers.component.css']
})
export class SuppliersComponent implements OnInit, OnDestroy {

  public suppliers: Array<ISupplier> = [];

  private voidSupplier: ISupplier = {
    id: 0,
    name: '',
    creationDate: null,
    modificationDate: null,
    createdBy: null,
    modifiedBy: null,
    entityStatus: EntityStatusEnum.NORMAL
  };


  private subscriptions = [];

  constructor(private supplierService: SupplierService,
              private confirm: ConfirmDialogService,
              private dialog: MatDialog) {
  }

  addSupplier() {
    this.editSupplier(this.voidSupplier, -1);
  }

  editSupplier(supplier: ISupplier, index: number) {
    const dialogRef = this.dialog.open(SupplierDialogComponent, {
      width: '45%',
      disableClose: true,
      data: supplier,
    });
    dialogRef.afterClosed()
      .subscribe((resolvedSupplier: ISupplier) => {

        if (resolvedSupplier) {
          this.supplierService.putSupplier(resolvedSupplier, index);
        }

      });
  }

  deleteSupplier(supplier: ISupplier, index: number) {
    this.confirm.confirm('Delete Supplier', 'Are you sure you would like to delete the Supplier?')
      .subscribe((res: boolean) => {
        if (res) {
          this.supplierService.deleteSupplier(supplier.id, index);
        }
      });
  }

  ngOnInit() {

    // invoke Supplies get from db by Admin
    this.supplierService.getSuppliers();

    // subscribe for Suppliers
    this.subscriptions.push(this.supplierService.suppliersChanged
      .subscribe((res: Array<ISupplier>) => {
        this.suppliers = res;
      }));
  }

  ngOnDestroy() {
    this.subscriptions.forEach((sub: Subscription) => {
      sub.unsubscribe();
    })
  }


}
