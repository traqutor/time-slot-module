import {MatDialog, MatSnackBar} from "@angular/material";
import {Component, OnInit} from '@angular/core';
import {Subscription} from "rxjs";
import * as moment from 'moment';

import {ITimeSlotDelivery, IUniformViewTimeSlot} from "../time-slot.model";
import {EntityStatusEnum, ICustomer, IUserInfo, UserRoleNameEnum} from "../../users/user.model";
import {TimeSlotService} from "../time-slot.service";
import {ConfirmDialogService} from "../../common/confirm-dialog/confirm-dialog.service";
import {CustomerService} from "../../custmer/customer.service";
import {ISite} from "../../sites/site.model";
import {SiteService} from "../../sites/site.service";
import {TimeSlotDeliveryDialogComponent} from "../time-slot-delivery-dialog/time-slot-delivery-dialog.component";
import {AuthService} from "../../auth/auth.service";


@Component({
  selector: 'app-time-slots-user-view',
  templateUrl: './time-slots-user-view.component.html',
  styleUrls: ['./time-slots-user-view.component.css']
})
export class TimeSlotsUserViewComponent implements OnInit {

  public userInfo: IUserInfo;
  public USER_ROLES = UserRoleNameEnum;

  public timeSlots: Array<IUniformViewTimeSlot> = [];
  public customers: Array<ICustomer> = [];
  public sites: Array<ISite> = [];

  public customer: ICustomer;
  public site: ISite;
  public date: Date;
  public maxDate: Date;

  public showCustomer: boolean;

  private voidDeliveryTimeSlot: ITimeSlotDelivery = {
    id: 0,
    creationDate: null,
    modificationDate: null,
    createdBy: null,
    modifiedBy: null,
    entityStatus: EntityStatusEnum.NORMAL,
    timeSlot: {
      id: 0,
      startTime: '',
      endTime: '',
      creationDate: null,
      modificationDate: null,
      createdBy: null,
      modifiedBy: null,
      entityStatus: EntityStatusEnum.NORMAL
    },
    commodity: {
      id: 0,
      name: null,
      maxTonsPerDay: null,
      createdBy: null,
      creationDate: null,
      modifiedBy: null,
      modificationDate: null,
      entityStatus: EntityStatusEnum.NORMAL
    },
    contract: {
      id: 0,
      name: null,
      vendor: null,
      supplier: null,
      commodity: null,
      createdBy: null,
      creationDate: null,
      modifiedBy: null,
      modificationDate: null,
      entityStatus: EntityStatusEnum.NORMAL
    },
    customer: {
      id: 0,
      name: null,
      createdBy: null,
      creationDate: null,
      modifiedBy: null,
      modificationDate: null,
      entityStatus: EntityStatusEnum.NORMAL
    },
    deliveryDate: null,
    driver: {
      id: 0,
      name: null,
      fleet: null,
      customer: null,
      surname: null,
      role: null,
      vehicles: null,
      site: null,
      password: null,
      email: null,
      entityStatus: EntityStatusEnum.NORMAL
    },
    site: {
      id: 0, customer: null,
      name: null,
      createdBy: null,
      creationDate: null,
      modifiedBy: null,
      modificationDate: null,
      entityStatus: EntityStatusEnum.NORMAL
    },
    statusType: {
      id: 0,
      name: null,
      createdBy: null,
      creationDate: null,
      modifiedBy: null,
      modificationDate: null,
      entityStatus: EntityStatusEnum.NORMAL
    },
    supplier: {
      id: 0,
      name: null,
      createdBy: null,
      creationDate: null,
      modifiedBy: null,
      modificationDate: null,
      entityStatus: EntityStatusEnum.NORMAL
    },
    tons: null,
    vehicle: {
      id: 0,
      rego: null, fleet: null,
      createdBy: null,
      creationDate: null,
      modifiedBy: null,
      modificationDate: null,
      entityStatus: EntityStatusEnum.NORMAL
    },
    vendor: {
      id: 0,
      name: null,
      createdBy: null,
      creationDate: null,
      modifiedBy: null,
      modificationDate: null,
      entityStatus: EntityStatusEnum.NORMAL
    },
  };

  // subscriptions are only for cleanup after destroy
  private subscriptions = [];


  constructor(private timeSlotService: TimeSlotService,
              private customerService: CustomerService,
              private siteService: SiteService,
              private auth: AuthService,
              private snackBar: MatSnackBar,
              private confirm: ConfirmDialogService,
              private dialog: MatDialog) {
  }

  ngOnInit() {

    this.date = new Date();

    this.maxDate = new Date;
    this.maxDate.setDate(this.maxDate.getDate() + 7);

    this.subscriptions.push(this.auth.currentUser.subscribe((res: IUserInfo) => {
      this.userInfo = res;

      // get customers if user Role Admin
      // invoke Customers get from db
      if (this.userInfo) {
        if (this.userInfo.role && this.userInfo.role.name === this.USER_ROLES.Administrator) {

          this.showCustomer = true;
          this.customerService.getCustomers();

          // subscribe for Customers
          this.subscriptions.push(this.customerService.customersChanged
            .subscribe((res: Array<ICustomer>) => {
              this.customers = res;
            }));

        } else {

          this.showCustomer = false;
          this.customer = this.userInfo.customer;
          this.getSites(this.userInfo.customer);

        }

      }


      // subscribe for TimeSlots
      this.subscriptions.push(this.timeSlotService.deliveryTimeSlotsChanged
        .subscribe((res: Array<IUniformViewTimeSlot>) => {
          if (this.site) {
            this.timeSlots = res;
          } else {
            this.timeSlots.length = 0;
          }
        }));


    }));


  }

  getSites(customer: ICustomer) {
    this.timeSlots.length = 0;

    if (customer) {
      this.subscriptions.push(this.siteService.getSitesById(customer.id)
        .subscribe((res: Array<ISite>) => {
            this.sites = res;
          }
        ));
    }
  }

  getSlots(site: ISite) {
    if (site) {
      const tmpDate: string = moment(this.date).format('YYYY-MM-DD');
      this.timeSlotService.getTimeSlotDeliveryData(site.id, tmpDate);
    }
  }

  compare(val1, val2) {
    return val1 && val2 ? val1.id === val2.id : val1 === val2;
  }

  editDeliverySlot(timeSlot: IUniformViewTimeSlot, index: number) {

    let tmpSlot: IUniformViewTimeSlot = Object.assign({}, timeSlot);
    // the object needs to be created somehow ;)
    if (tmpSlot.deliveryTimeSlot) {
      tmpSlot.deliveryTimeSlot.timeSlot = timeSlot.timeSlot;
      tmpSlot.deliveryTimeSlot.customer = this.customer;
      tmpSlot.deliveryTimeSlot.site = this.site;
      tmpSlot.deliveryTimeSlot.deliveryDate = this.date;
    } else {
      tmpSlot.deliveryTimeSlot = this.voidDeliveryTimeSlot;
      tmpSlot.deliveryTimeSlot.timeSlot = timeSlot.timeSlot;
      tmpSlot.deliveryTimeSlot.customer = this.customer;
      tmpSlot.deliveryTimeSlot.site = this.site;
      tmpSlot.deliveryTimeSlot.deliveryDate = this.date;
    }

    const dialogRef = this.dialog.open(TimeSlotDeliveryDialogComponent, {
      width: '80%',
      disableClose: true,
      data: tmpSlot.deliveryTimeSlot,
    });
    this.subscriptions.push(dialogRef.afterClosed()
      .subscribe((resolvedTimeSlot: ITimeSlotDelivery) => {
        if (resolvedTimeSlot) {
          timeSlot.deliveryTimeSlot = resolvedTimeSlot;
        }
      }));
  }

  deleteDeliverySlot(timeSlot: IUniformViewTimeSlot, index: number) {
    this.subscriptions.push(this.confirm.confirm('Delete TimeSlot', 'Are you sure you would like to delete the TimeSlot?')
      .subscribe((res: boolean) => {
        if (res) {
          this.timeSlotService.deleteDeliveryTimseSlot(timeSlot.timeSlot.id, index);
        }
      }));
  }


  ngOnDestroy() {
    this.subscriptions.forEach((sub: Subscription) => {
      sub.unsubscribe();
    })
  }


}
