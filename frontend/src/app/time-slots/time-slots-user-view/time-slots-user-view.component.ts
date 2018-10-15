import {MatDialog} from "@angular/material";
import { Component, OnInit } from '@angular/core';
import {Subscription} from "rxjs";


import {ITimeSlot} from "../time-slot.model";
import {EntityStatusEnum} from "../../users/user.model";
import {TimeSlotService} from "../time-slot.service";
import {ConfirmDialogService} from "../../common/confirm-dialog/confirm-dialog.service";
import {TimeSlotDialogComponent} from "../time-slot-dialog/time-slot-dialog.component";

@Component({
  selector: 'app-time-slots-user-view',
  templateUrl: './time-slots-user-view.component.html',
  styleUrls: ['./time-slots-user-view.component.css']
})
export class TimeSlotsUserViewComponent implements OnInit {

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
