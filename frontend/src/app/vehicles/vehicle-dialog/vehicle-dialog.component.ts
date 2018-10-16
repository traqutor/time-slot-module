import {Component, Inject, OnDestroy, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material";
import {Subscription} from "rxjs";

import {CustomerService} from "../../custmer/customer.service";
import {ICustomer, IUserInfo, UserRoleNameEnum} from "../../users/user.model";
import {FleetService} from "../../fleets/fleet.service";
import {IVehicle} from "../vehicle.model";
import {IFleet} from "../../fleets/fleet.model";
import {AuthService} from "../../auth/auth.service";

@Component({
  selector: 'app-vehicle-dialog',
  templateUrl: './vehicle-dialog.component.html',
  styleUrls: ['./vehicle-dialog.component.css']
})
export class VehicleDialogComponent implements OnInit, OnDestroy {

  public userInfo: IUserInfo;
  public USER_ROLES = UserRoleNameEnum;
  public showCustomer: boolean;

  vehicleForm: FormGroup;
  customers: Array<ICustomer> = [];
  fleets: Array<IFleet> = [];

  private subscriptions: Array<Subscription> = [];

  constructor(public dialogRef: MatDialogRef<VehicleDialogComponent>,
              private formBuilder: FormBuilder,
              private customerService: CustomerService,
              private fleetService: FleetService,
              private auth: AuthService,
              @Inject(MAT_DIALOG_DATA) public vehicle: IVehicle) {
  }


  ngOnInit() {

    this.subscriptions.push(this.auth.currentUser.subscribe((res: IUserInfo) => {
      this.userInfo = res;

      // get customers if user Role Admin
      // invoke Customers get from db
      if (this.userInfo.role.name === this.USER_ROLES.Administrator) {

        // invoke Customers and fleet get from db
        this.customerService.getCustomers();

        // subscribe for Customers
        this.subscriptions.push(this.customerService.customersChanged
          .subscribe((res: Array<ICustomer>) => {
            this.customers = res;

            this.showCustomer = true;

            // after customers resolves the form needs to be filed with Customer related to customer in fleet
            // then selected customer related fleets needs to be fetched
            if (this.customers.length > 0 && this.vehicle.fleet.id > 0) {
              this.onCustomerChange(this.vehicle.fleet.customer);
            }
          }));

        this.vehicleForm = this.formBuilder.group({
          id: this.vehicle.id,
          rego: [this.vehicle.rego, [Validators.required,]],
          customer: [this.vehicle.fleet.customer, [Validators.required]],
          fleet: [this.vehicle.fleet, [Validators.required]],
          creationDate: this.vehicle.creationDate,
          modificationDate: this.vehicle.modificationDate,
          createdBy: this.vehicle.createdBy,
          modifiedBy: this.vehicle.modifiedBy,
          entityStatus: this.vehicle.entityStatus
        });


      } else if (this.userInfo.role.name === this.USER_ROLES.CustomerAdmin) {

        this.showCustomer = false;

        this.vehicleForm = this.formBuilder.group({
          id: this.vehicle.id,
          rego: [this.vehicle.rego, [Validators.required,]],
          customer: this.userInfo.customer,
          fleet: [this.vehicle.fleet, [Validators.required]],
          creationDate: this.vehicle.creationDate,
          modificationDate: this.vehicle.modificationDate,
          createdBy: this.vehicle.createdBy,
          modifiedBy: this.vehicle.modifiedBy,
          entityStatus: this.vehicle.entityStatus
        });

        this.onCustomerChange(this.userInfo.customer)

      }
    }));


  }

  onCustomerChange(customer: ICustomer) {
    this.fleetService.getFleetsById(customer.id).subscribe((res: Array<IFleet>) => {
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
