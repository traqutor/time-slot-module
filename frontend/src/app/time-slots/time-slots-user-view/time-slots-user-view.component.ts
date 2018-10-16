import {MatDialog} from "@angular/material";
import {Component, OnInit} from '@angular/core';
import {Subscription} from "rxjs";


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
    commodity: null,
    contract: null,
    customer: null,
    deliveryDate: null,
    driver: null,
    site: null,
    statusType: null,
    supplier: null,
    tons: null,
    vehicle: null,
    vendor: null
  };

  // subscriptions are only for cleanup after destroy
  private subscriptions = [];


  constructor(private timeSlotService: TimeSlotService,
              private customerService: CustomerService,
              private siteService: SiteService,
              private auth: AuthService,
              private confirm: ConfirmDialogService,
              private dialog: MatDialog) {
  }

  ngOnInit() {

    this.date = new Date();


    this.subscriptions.push(this.auth.currentUser.subscribe((res: IUserInfo) => {
      this.userInfo = res;

      // get customers if user Role Admin
      // invoke Customers get from db
      if (this.userInfo.role.name === this.USER_ROLES.Administrator) {

        this.showCustomer = true;
        this.customerService.getCustomers();

        // subscribe for Customers
        this.subscriptions.push(this.timeSlotService.deliveryTimeSlotsChanged
          .subscribe((res: Array<IUniformViewTimeSlot>) => {
            this.timeSlots = res;
          }));

        // subscribe for Customers
        this.subscriptions.push(this.customerService.customersChanged
          .subscribe((res: Array<ICustomer>) => {
            this.customers = res;
          }));

      } else {
        this.showCustomer = false;
        this.getSites(this.userInfo.customer);
      }

    }));


  }

  getSites(customer: ICustomer) {
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
      const tmpDate = this.date.toISOString();
      this.timeSlotService.getTimeSlotDeliveryData(site.id, tmpDate);
    }
  }

  compare(val1, val2) {
    return val1 && val2 ? val1.id === val2.id : val1 === val2;
  }

  editDeliverySlot(timeSlot: IUniformViewTimeSlot, index: number) {
    if (timeSlot.deliveryTimeSlot) {
      timeSlot.deliveryTimeSlot.customer = this.customer;
      timeSlot.deliveryTimeSlot.site = this.site;
    } else {
      timeSlot.deliveryTimeSlot = this.voidDeliveryTimeSlot;
      timeSlot.deliveryTimeSlot.customer = this.customer;
      timeSlot.deliveryTimeSlot.site = this.site;
    }

    const dialogRef = this.dialog.open(TimeSlotDeliveryDialogComponent, {
      width: '45%',
      disableClose: true,
      data: timeSlot.deliveryTimeSlot,
    });
    this.subscriptions.push(dialogRef.afterClosed()
      .subscribe((resolvedTimeSlot: ITimeSlotDelivery) => {
        if (resolvedTimeSlot) {
          timeSlot.deliveryTimeSlot = resolvedTimeSlot;
          this.timeSlotService.putDeliveryTimeSlot(timeSlot, index);
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
