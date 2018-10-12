import {Component, Inject, OnDestroy, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {ICustomer, IRole, IRoleResult, IUser} from "../user.model";
import {IFleet} from "../../fleets/fleet.model";
import {Subscription} from "rxjs";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material";
import {CustomerService} from "../../custmer/customer.service";
import {FleetService} from "../../fleets/fleet.service";
import {UserService} from "../user.service";
import {IVehicle} from "../../vehicles/vehicle.model";
import {VehicleService} from "../../vehicles/vehicle.service";

@Component({
  selector: 'app-user-dialog',
  templateUrl: './user-dialog.component.html',
  styleUrls: ['./user-dialog.component.css']
})
export class UserDialogComponent implements OnInit, OnDestroy {

  userForm: FormGroup;
  customers: Array<ICustomer> = [];
  fleets: Array<IFleet> = [];
  roles: Array<IRole> = [];
  fleetsVehicles: Array<IVehicle> = [];

  private subscriptions: Array<Subscription> = [];

  constructor(public dialogRef: MatDialogRef<UserDialogComponent>,
              private formBuilder: FormBuilder,
              private customerService: CustomerService,
              private userService: UserService,
              private fleetService: FleetService,
              private vehicleService: VehicleService,
              @Inject(MAT_DIALOG_DATA) public user: IUser) {
  }


  ngOnInit() {

    // invoke Customers and fleet get from db
    this.customerService.getCustomers();

    // subscribe for Customers
    this.subscriptions.push(this.customerService.customersChanged
      .subscribe((res: Array<ICustomer>) => {
        this.customers = res;
      }));

    // take roles

    this.userService.getRoles()
      .subscribe((res: IRoleResult) => {
        this.roles = res.results;
      });

    this.userForm = this.formBuilder.group({
      id: this.user.id,
      email: [this.user.email, [Validators.required]],
      password: this.user.password,
      name: [this.user.name, [Validators.required]],
      surname: [this.user.surname, [Validators.required]],
      role: [this.user.role, [Validators.required]],
      customer: this.user.customer,
      site: this.user.site,
      fleet: this.user.fleet,
      vehicles: this.user.vehicles,
      entityStatus: this.user.entityStatus
    });

  }

  onRoleChange(role: IRole) {

  }

  onCustomerChange(customer: ICustomer) {
    this.fleetService.getFleetsById(customer.id).subscribe((res: Array<IFleet>) => {
        this.fleets = res;
      }
    );
  }

  onFleetChange(fleet: IFleet) {
    console.log('szukam vehicli dla', fleet);
    this.vehicleService.getVehiclesForFleetId(fleet.id)
      .subscribe((res: Array<IVehicle>) => {
        this.fleetsVehicles = res;
        console.log('this.fleetsVehicles',this.fleetsVehicles);
      });
  }

  compare(val1, val2) {
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
