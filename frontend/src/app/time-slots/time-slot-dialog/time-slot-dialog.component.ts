import {Component, Inject, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {MAT_DIALOG_DATA, MatDialog, MatDialogRef} from "@angular/material";
import {ITimeSlot} from "../time-slot.model";
import {SlotHourDialogComponent} from "../../common/slot-hour-dialog/slot-hour-dialog.component";

@Component({
  selector: 'app-time-slot-dialog',
  templateUrl: './time-slot-dialog.component.html',
  styleUrls: ['./time-slot-dialog.component.css']
})
export class TimeSlotDialogComponent implements OnInit {

  timeSlotForm: FormGroup;

  constructor(public dialogRef: MatDialogRef<TimeSlotDialogComponent>,
              private formBuilder: FormBuilder,
              private dialog: MatDialog,
              @Inject(MAT_DIALOG_DATA) public timeSlot: ITimeSlot) {
  }


  ngOnInit() {
    console.log('timeSlot', this.timeSlot);

    this.timeSlotForm = this.formBuilder.group({
      id: this.timeSlot.id,
      startTime: [this.timeSlot.startTime, [Validators.required, Validators.pattern(/^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$/)]],
      endTime: [this.timeSlot.endTime, [Validators.required, , Validators.pattern(/^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$/)]],
      creationDate: this.timeSlot.creationDate,
      modificationDate: this.timeSlot.modificationDate,
      createdBy: this.timeSlot.createdBy,
      modifiedBy: this.timeSlot.modifiedBy,
      entityStatus: this.timeSlot.entityStatus
    });

  }

  submit() {
    this.dialogRef.close(this.timeSlotForm.value);
  }

  timePickerOpen(time: string, controlName: string) {
    const dialogRef = this.dialog.open(SlotHourDialogComponent, {
      width: '45%',
      disableClose: true,
      data: time,
    });
    dialogRef.afterClosed()
      .subscribe((resolvedTime: string) => {

        console.log('resolvedTime', resolvedTime);

        if (resolvedTime) {

          this.timeSlotForm.controls[controlName].setValue(resolvedTime);
        }

      });

  }

}
