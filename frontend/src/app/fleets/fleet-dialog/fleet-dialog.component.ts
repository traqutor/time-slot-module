import {Component, Inject, OnDestroy, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material";
import {CustomerService} from "../../custmer/customer.service";
import {Subscription} from "rxjs";
import {ICustomer, IUserInfo, UserRoleNameEnum} from "../../users/user.model";
import {IFleet} from "../fleet.model";
import {AuthService} from "../../auth/auth.service";

@Component({
  selector: 'app-fleet-dialog',
  templateUrl: './fleet-dialog.component.html',
  styleUrls: ['./fleet-dialog.component.css']
})
export class FleetDialogComponent implements OnInit, OnDestroy {

  public userInfo: IUserInfo;
  public USER_ROLES = UserRoleNameEnum;
  public showCustomer: boolean;

  fleetForm: FormGroup;

  constructor(public dialogRef: MatDialogRef<FleetDialogComponent>,
              private formBuilder: FormBuilder,
              private customerService: CustomerService,
              private auth: AuthService,
              @Inject(MAT_DIALOG_DATA) public fleet: IFleet) {
  }

  private subscriptions: Array<Subscription> = [];
  public customers: Array<ICustomer> = [];

  ngOnInit() {

    this.subscriptions.push(this.auth.currentUser.subscribe((res: IUserInfo) => {
      this.userInfo = res;

      // get customers if user Role Admin
      // invoke Customers get from db
      if (this.userInfo.role.name === this.USER_ROLES.Administrator) {
        // invoke Customers get from db
        this.customerService.getCustomers();

        // subscribe for Customers
        this.subscriptions.push(this.customerService.customersChanged
          .subscribe((res: Array<ICustomer>) => {
            this.customers = res;
            this.showCustomer = true;

          }));

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


      } else if (this.userInfo.role.name === this.USER_ROLES.CustomerAdmin) {

        this.fleetForm = this.formBuilder.group({
          id: this.fleet.id,
          name: [this.fleet.name, [Validators.required,]],
          customer: this.userInfo.customer,
          creationDate: this.fleet.creationDate,
          modificationDate: this.fleet.modificationDate,
          createdBy: this.fleet.createdBy,
          modifiedBy: this.fleet.modifiedBy,
          entityStatus: this.fleet.entityStatus
        });

      }

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
