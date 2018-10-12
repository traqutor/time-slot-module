import {Component, Inject, OnDestroy, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material";
import {CustomerService} from "../../custmer/customer.service";
import {ISite} from "../../sites/site.model";
import {Subscription} from "rxjs";
import {ICustomer} from "../../user/user.model";
import {FleetService} from "../../fleets/fleet.service";
import {IVehicle} from "../vehicle.model";
import {IFleet} from "../../fleets/fleet.model";

@Component({
  selector: 'app-vehicle-dialog',
  templateUrl: './vehicle-dialog.component.html',
  styleUrls: ['./vehicle-dialog.component.css']
})
export class VehicleDialogComponent implements OnInit, OnDestroy {

  vehicleForm: FormGroup;
  customers: Array<ICustomer> = [];
  fleets: Array<IFleet> = [];

  private subscriptions: Array<Subscription> = [];

  constructor(public dialogRef: MatDialogRef<VehicleDialogComponent>,
              private formBuilder: FormBuilder,
              private customerService: CustomerService,
              private fleetService: FleetService,
              @Inject(MAT_DIALOG_DATA) public vehicle: IVehicle) {
  }


  ngOnInit() {

    this.vehicleForm = this.formBuilder.group({
      id: this.vehicle.id,
      rego: [this.vehicle.rego, [Validators.required,]],
      customer: [null, [Validators.required]],
      fleet: [this.vehicle.fleet, [Validators.required]],
      creationDate: this.vehicle.creationDate,
      modificationDate: this.vehicle.modificationDate,
      createdBy: this.vehicle.createdBy,
      modifiedBy: this.vehicle.modifiedBy,
      entityStatus: this.vehicle.entityStatus
    });

    // invoke Customers and fleet get from db
    this.customerService.getCustomers();

    // subscribe for Customers
    this.subscriptions.push(this.customerService.customersChanged
      .subscribe((res: Array<ICustomer>) => {
        this.customers = res;
      }));

  }

  onCustomerChange(customer: ICustomer) {
    this.fleetService.getFleetsById(customer.id).subscribe( (res: Array<IFleet>) => {
      this.fleets = res;
      }
    );
  }

  compare(val1, val2) {
    if (val1 && val2) {
      return val1.id === val2.id;
    }
  }

  submit() {
    this.dialogRef.close(this.vehicleForm.value);
  }

  ngOnDestroy() {
    this.subscriptions.forEach((sub: Subscription) => {
      sub.unsubscribe();
    })

  }

}
