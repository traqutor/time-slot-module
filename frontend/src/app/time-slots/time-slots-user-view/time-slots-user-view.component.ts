import {MatDialog} from "@angular/material";
import {Component, OnInit} from '@angular/core';
import {Subscription} from "rxjs";


import {ITimeSlotDelivery} from "../time-slot.model";
import {EntityStatusEnum, ICustomer} from "../../users/user.model";
import {TimeSlotService} from "../time-slot.service";
import {ConfirmDialogService} from "../../common/confirm-dialog/confirm-dialog.service";
import {TimeSlotDialogComponent} from "../time-slot-dialog/time-slot-dialog.component";
import {CustomerService} from "../../custmer/customer.service";
import {ISite} from "../../sites/site.model";
import {SiteService} from "../../sites/site.service";

@Component({
  selector: 'app-time-slots-user-view',
  templateUrl: './time-slots-user-view.component.html',
  styleUrls: ['./time-slots-user-view.component.css']
})
export class TimeSlotsUserViewComponent implements OnInit {

  public timeSlots: Array<ITimeSlotDelivery> = [];
  public customers: Array<ICustomer> = [];
  public sites: Array<ISite> = [];

  public customer: ICustomer;
  public site: ISite;
  public date: Date;

  private voidTimeSlot: ITimeSlotDelivery = {
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
              private confirm: ConfirmDialogService,
              private dialog: MatDialog) {
  }

  ngOnInit() {

    this.date = new Date();

    // get customers
    // invoke Customers get from db
    this.customerService.getCustomers();

    // subscribe for Customers
    this.subscriptions.push(this.customerService.customersChanged
      .subscribe((res: Array<ICustomer>) => {
        this.customers = res;
      }));

    // get sites
    // get slots

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
      this.timeSlotService.getTimeSlotData(site.id, this.date)
        .subscribe((res: Array<ITimeSlotDelivery>) => {
          this.timeSlots = res;
        });
    }
  }

  compare(val1, val2) {
    return val1 && val2 ? val1.id === val2.id : val1 === val2;
  }

  addTimseSlot() {
    this.editDeliverySlot(this.voidTimeSlot, -1);
  }

  editDeliverySlot(timeSlot: ITimeSlotDelivery, index: number) {
    const dialogRef = this.dialog.open(TimeSlotDialogComponent, {
      width: '45%',
      disableClose: true,
      data: timeSlot,
    });
    dialogRef.afterClosed()
      .subscribe((resolvedTimeSlot: ITimeSlotDelivery) => {

        if (resolvedTimeSlot) {
          this.timeSlotService.putTimeSlot(resolvedTimeSlot, index);
        }

      });
  }

  deleteDeliverySlot(timeSlot: ITimeSlotDelivery, index: number) {
    this.confirm.confirm('Delete TimeSlot', 'Are you sure you would like to delete the TimeSlot?')
      .subscribe((res: boolean) => {
        if (res) {
          this.timeSlotService.deleteTimseSlot(timeSlot.id, index);
        }
      });
  }


  ngOnDestroy() {
    this.subscriptions.forEach((sub: Subscription) => {
      sub.unsubscribe();
    })
  }


}
