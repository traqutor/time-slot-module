import {Component, Inject, OnDestroy, OnInit} from '@angular/core';
import {AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators} from "@angular/forms";
import {Subscription} from "rxjs";

import {ICustomer, IRole, IRoleResult, IUser, UserRoleNameEnum} from "../user.model";
import {IFleet} from "../../fleets/fleet.model";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material";
import {CustomerService} from "../../custmer/customer.service";
import {FleetService} from "../../fleets/fleet.service";
import {UserService} from "../user.service";
import {IVehicle} from "../../vehicles/vehicle.model";
import {VehicleService} from "../../vehicles/vehicle.service";
import {SiteService} from "../../sites/site.service";
import {ISite} from "../../sites/site.model";

@Component({
  selector: 'app-user-dialog',
  templateUrl: './user-dialog.component.html',
  styleUrls: ['./user-dialog.component.css']
})
export class UserDialogComponent implements OnInit, OnDestroy {

  userForm: FormGroup;
  customers: Array<ICustomer> = [];
  sites: Array<ISite> = [];
  fleets: Array<IFleet> = [];
  roles: Array<IRole> = [];
  fleetsVehicles: Array<IVehicle> = [];
  USER_ROLES = UserRoleNameEnum;

  // dynamic validations variables
  isCustomerValidationRequired = false;
  isSiteValidationRequired = false;
  isFleetValidationRequired = false;
  isvehicleValidationRequired = false;

  private subscriptions: Array<Subscription> = [];

  constructor(public dialogRef: MatDialogRef<UserDialogComponent>,
              private formBuilder: FormBuilder,
              private customerService: CustomerService,
              private userService: UserService,
              private siteService: SiteService,
              private fleetService: FleetService,
              private vehicleService: VehicleService,
              @Inject(MAT_DIALOG_DATA) public user: IUser) {
  }


  ngOnInit() {

    console.log('this.user', this.user);

    // invoke Customers and fleet get from db
    this.customerService.getCustomers();

    // subscribe for Customers
    this.subscriptions.push(this.customerService.customersChanged
      .subscribe((res: Array<ICustomer>) => {
        this.customers = res;
      }));

    // take roles
    this.subscriptions.push(this.userService.getRoles()
      .subscribe((res: IRoleResult) => {
        this.roles = res.results;
      }));

    // Reactive form
    this.userForm = this.formBuilder.group({
      id: this.user.id,
      email: [this.user.email, [Validators.required, Validators.email]],
      password: this.user.password,
      name: [this.user.name, [Validators.required]],
      surname: [this.user.surname, [Validators.required]],
      role: [this.user.role, [Validators.required]],
      customer: this.user.customer,
      site: this.user.site,
      fleet: this.user.fleet,
      vehicles: [],
      entityStatus: this.user.entityStatus
    });

    // depends on the User role select related properties to display in mat-select options
    // get role dependent settings Customer,
    if (this.user.role) {
      this.onRoleChange(this.user.role);
    }

    // get customer dependent dictionaries Sites and Fleets
    if (this.user.customer && this.user.customer.id > 0) {
      this.onCustomerChange(this.user.customer);
    }

    // get fleet Vehicles
    if (this.user.id > 0 && this.user.fleet && this.user.fleet.id > 0) {
      this.onFleetChange(this.user.fleet);
    }

  }

  // todo conditional validation of form required fielsd
  conditionalValidator(condition: (() => boolean), validator: ValidatorFn): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } => {
      if (!condition()) {
        return null;
      }
      return validator(control);
    }
  }

  onRoleChange(role: IRole) {
    this.isCustomerValidationRequired = role.name !== this.USER_ROLES.Administrator;
    this.isSiteValidationRequired = role.name === this.USER_ROLES.SiteUser;
    this.isFleetValidationRequired = role.name === this.USER_ROLES.Driver;
    this.isvehicleValidationRequired = role.name === this.USER_ROLES.Driver;
  }

  onCustomerChange(customer: ICustomer) {
    this.subscriptions.push(this.siteService.getSitesById(customer.id)
      .subscribe((res: Array<ISite>) => {
          this.sites = res;
        }
      ));

    this.subscriptions.push(this.fleetService.getFleetsById(customer.id)
      .subscribe((res: Array<IFleet>) => {
          this.fleets = res;
        }
      ));
  }


  onFleetChange(fleet: IFleet) {
    this.vehicleService.getVehiclesForFleetId(fleet.id)
      .subscribe((res: Array<IVehicle>) => {
        this.fleetsVehicles = res;

        // ToDO some worlkaround to make multiple selection on Vehicles work
        let tmp: IVehicle[] = [];
        this.fleetsVehicles.forEach((recivedVehilce: IVehicle) => {
          this.user.vehicles.forEach((userVehicle) => {
            if (recivedVehilce.id === userVehicle.id) {
              tmp.push(recivedVehilce);
            }
          });
        });
        this.userForm.controls['vehicles'].setValue(tmp);

      });
  }

  compare(val1, val2) {
    return val1 && val2 ? val1.id === val2.id : val1 === val2;
  }

  compareArray(val1, val2) {
    console.log('val1', val1);
    console.log('val2', val2);
    if (val1 && val2) {
      return val1.id === val2.id;
    }
  }


  submit() {
    this.dialogRef.close(this.userForm.value);
  }

  ngOnDestroy() {
    this.subscriptions.forEach((sub: Subscription) => {
      sub.unsubscribe();
    })

  }


}
