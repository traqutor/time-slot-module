import {Component, OnDestroy, OnInit} from '@angular/core';
import {EntityStatusEnum, ICustomer} from "../users/user.model";
import {CustomerService} from "../custmer/customer.service";
import {ConfirmDialogService} from "../common/confirm-dialog/confirm-dialog.service";
import {MatDialog} from "@angular/material";
import {CustomerDialogComponent} from "../custmer/customer-dialog/customer-dialog.component";
import {Subscription} from "rxjs";
import {ITimeSlot} from "./time-slot.model";
import {TimeSlotService} from "./time-slot.service";
import {TimeSlotDialogComponent} from "./time-slot-dialog/time-slot-dialog.component";

@Component({
  selector: 'app-time-slots',
  templateUrl: './time-slots.component.html',
  styleUrls: ['./time-slots.component.css']
})
export class TimeSlotsComponent implements OnInit, OnDestroy {

  public timeSlots: Array<ITimeSlot> = [];

  private voidTimeSlot: ITimeSlot = {
    id: 0,
    startTime: '',
    endTime: '',
    creationDate: null,
    modificationDate: null,
    createdBy: null,
    modifiedBy: null,
    entityStatus: EntityStatusEnum.NORMAL
  };
  private subscriptions = [];

  constructor(private timeSlotService: TimeSlotService,
              private confirm: ConfirmDialogService,
              private dialog: MatDialog) {
  }

  addTimseSlot() {
    this.editTimeSlot(this.voidTimeSlot, -1);
  }

  editTimeSlot(timeSlot: ITimeSlot, index: number) {
    const dialogRef = this.dialog.open(TimeSlotDialogComponent, {
      width: '45%',
      disableClose: true,
      data: timeSlot,
    });
    dialogRef.afterClosed()
      .subscribe((resolvedTimeSlot: ITimeSlot) => {

        if (resolvedTimeSlot) {
          this.timeSlotService.putTimeSlot(resolvedTimeSlot, index);
        }

      });
  }

  deleteTimseSlot(timseslot: ITimeSlot, index: number) {
    this.confirm.confirm('Delete TimeSlot', 'Are you sure you would like to delete the TimeSlot?')
      .subscribe((res: boolean) => {
        if (res) {
          this.timeSlotService.deleteTimseSlot(timseslot.id, index);
        }
      });
  }

  ngOnInit() {

    // invoke Customers get from db
    this.timeSlotService.getTimeSlots();

    // subscribe for Customers
    this.subscriptions.push(this.timeSlotService.timeSlotsChanged
      .subscribe((res: Array<ITimeSlot>) => {
        this.timeSlots = res;
      }));
  }

  ngOnDestroy() {
    this.subscriptions.forEach((sub: Subscription) => {
      sub.unsubscribe();
    })
  }


}
