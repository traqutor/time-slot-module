import {Component, Inject, OnDestroy, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material";
import {CustomerService} from "../../custmer/customer.service";
import {Subscription} from "rxjs";
import {ICustomer} from "../../user/user.model";
import {IFleet} from "../fleet.model";

@Component({
  selector: 'app-fleet-dialog',
  templateUrl: './fleet-dialog.component.html',
  styleUrls: ['./fleet-dialog.component.css']
})
export class FleetDialogComponent implements OnInit, OnDestroy {

  fleetForm: FormGroup;

  constructor(public dialogRef: MatDialogRef<FleetDialogComponent>,
              private formBuilder: FormBuilder,
              private customerService: CustomerService,
              @Inject(MAT_DIALOG_DATA) public fleet: IFleet) {
  }

  private subscriptions: Array<Subscription> = [];
  public customers: Array<ICustomer> = [];

  ngOnInit() {

    this.fleetForm = this.formBuilder.group({
      id: this.fleet.id,
      name: [this.fleet.name, [Validators.required,]],
      customer: [this.fleet.customer, [Validators.required]],
      creationDate: this.fleet.creationDate,
      modificationDate: this.fleet.modificationDate,
      createdBy: this.fleet.createdBy,
      modifiedBy: this.fleet.modifiedBy,
      entityStatus: this.fleet.entityStatus
    });

    // invoke Customers get from db
    this.customerService.getCustomers();

    // subscribe for Customers
    this.subscriptions.push(this.customerService.customersChanged
      .subscribe((res: Array<ICustomer>) => {
        this.customers = res;
      }));

  }

  compare(val1, val2) {
    if (val1 && val2) {
      return val1.id === val2.id;
    }
  }

  submit() {
    this.dialogRef.close(this.fleetForm.value);
  }

  ngOnDestroy() {
    this.subscriptions.forEach((sub: Subscription) => {
      sub.unsubscribe();
    })

  }

}
