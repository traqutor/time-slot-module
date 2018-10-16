import {Component, Inject, OnDestroy, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material";
import {ITimeSlotDelivery} from "../time-slot.model";
import {IUser, IUserInfo, UserRoleNameEnum} from "../../users/user.model";
import {DriverService} from "selenium-webdriver/remote";
import {UserService} from "../../users/user.service";
import {IVehicle} from "../../vehicles/vehicle.model";
import {VehicleService} from "../../vehicles/vehicle.service";
import {Subscription} from "rxjs";
import {IContract} from "../../contracts/contract.model";
import {ContractService} from "../../contracts/contract.service";
import {IVendor} from "../../vendors/vendor.model";
import {VendorService} from "../../vendors/vendor.service";
import {ISupplier} from "../../suppliers/supplier.model";
import {ICommodity} from "../../commodities/commodity.model";
import {IStatusType} from "../../status-types/status-type.model";
import {SupplierService} from "../../suppliers/supplier.service";
import {CommodityService} from "../../commodities/commodity.service";
import {StatusTypeService} from "../../status-types/status-type.service";
import {AuthService} from "../../auth/auth.service";

@Component({
  selector: 'app-time-slot-delivery-dialog',
  templateUrl: './time-slot-delivery-dialog.component.html',
  styleUrls: ['./time-slot-delivery-dialog.component.css']
})
export class TimeSlotDeliveryDialogComponent implements OnInit, OnDestroy {

  public userInfo: IUserInfo;
  public USER_ROLES = UserRoleNameEnum;

  timeSlotForm: FormGroup;
  drivers: IUser[] = [];
  vehicles: IVehicle[] = [];
  contracts: IContract[] = [];
  vendors: IVendor[] = [];
  suppliers: ISupplier[] = [];
  commodities: ICommodity[] = [];
  statusTypes: IStatusType[] = [];

  // subscriptions are only for cleanup after destroy
  private subscriptions = [];

  constructor(public dialogRef: MatDialogRef<TimeSlotDeliveryDialogComponent>,
              private userService: UserService,
              private vehicleService: VehicleService,
              private contractService: ContractService,
              private vendorService: VendorService,
              private supplierService: SupplierService,
              private commodityService: CommodityService,
              private statusTypeService: StatusTypeService,
              private formBuilder: FormBuilder,
              private auth: AuthService,
              @Inject(MAT_DIALOG_DATA) public timeSlot: ITimeSlotDelivery) {
  }


  ngOnInit() {

    this.subscriptions.push(this.auth.currentUser.subscribe((res: IUserInfo) => {

      this.userInfo = res;

      this.timeSlotForm = this.formBuilder.group({
        id: this.timeSlot.id,

        driver: [this.timeSlot.driver, [Validators.required]],
        vehicle: [this.timeSlot.vehicle, [Validators.required]],
        contract: [this.timeSlot.contract, [Validators.required]],
        vendor: [this.timeSlot.vendor, [Validators.required]],
        supplier: [this.timeSlot.supplier, [Validators.required]],
        commodity: [this.timeSlot.commodity, [Validators.required]],
        statusType: [this.timeSlot.statusType, [Validators.required]],
        tons: [this.timeSlot.tons, [Validators.required]],

        timeSlot: this.timeSlot.timeSlot,
        deliveryDate: this.timeSlot.deliveryDate,
        customer: this.timeSlot.customer,
        site: this.timeSlot.site,

        creationDate: this.timeSlot.creationDate,
        modificationDate: this.timeSlot.modificationDate,
        createdBy: this.timeSlot.createdBy,
        modifiedBy: this.timeSlot.modifiedBy,
        entityStatus: this.timeSlot.entityStatus
      });

      this.subscriptions.push(this.userService.getDriversForCompany(this.timeSlot.customer.id)
        .subscribe((res: IUser[]) => {
          this.drivers = res;
        }));

      // invoke Contracts get from db by Admin
      this.contractService.getContracts();

      // subscribe for Contracts
      this.subscriptions.push(this.contractService.contractsChanged
        .subscribe((res: Array<IContract>) => {
          this.contracts = res;
        }));

      // invoke Vendors get from db by Admin
      this.vendorService.getVendors();

      // subscribe for Vendors
      this.subscriptions.push(this.vendorService.vendorsChanged
        .subscribe((res: Array<IVendor>) => {
          this.vendors = res;
        }));

      // invoke Supplies get from db by Admin
      this.supplierService.getSuppliers();

      // subscribe for Suppliers
      this.subscriptions.push(this.supplierService.suppliersChanged
        .subscribe((res: Array<ISupplier>) => {
          this.suppliers = res;
        }));

      // invoke Commodities get from db by Admin
      this.commodityService.getCommodities();

      // subscribe for Commodities
      this.subscriptions.push(this.commodityService.commoditiesChanged
        .subscribe((res: Array<ICommodity>) => {
          this.commodities = res;
        }));

      // Depends on user Role get Statuses
      if (this.userInfo.role.name === this.USER_ROLES.Administrator) {
        // invoke StatusTypes get from db by Admin
        this.statusTypeService.getStatusTypes();
      } else {
        this.statusTypeService.getStatusTypesList();
      }

      // subscribe for StatusTypes
      this.subscriptions.push(this.statusTypeService.statusTypesChanged
        .subscribe((res: Array<IStatusType>) => {
          this.statusTypes = res;
        }));

      if (this.timeSlot.driver && this.timeSlot.driver.id !== 0) {
        this.onDriverChange(this.timeSlot.driver);
      }


    }));

  }

  onDriverChange(driver: IUser) {
    this.getVehicles(driver);
  }

  getVehicles(driver: IUser) {
    if (driver) {
      this.subscriptions.push(this.vehicleService.getVehiclesForSpecificDriver(driver.id)
        .subscribe((res: IVehicle[]) => {
          this.vehicles = res;
        }));
    }
  }

  compare(val1, val2) {
    return val1 && val2 ? val1.id === val2.id : val1 === val2;
  }

  submit() {
    this.dialogRef.close(this.timeSlotForm.value);
  }

  ngOnDestroy() {
    this.subscriptions.forEach((sub: Subscription) => {
      sub.unsubscribe();
    })
  }
}
