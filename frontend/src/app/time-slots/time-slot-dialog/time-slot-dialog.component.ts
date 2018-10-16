import {Component, Inject, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material";
import {ITimeSlot} from "../time-slot.model";

@Component({
  selector: 'app-time-slot-dialog',
  templateUrl: './time-slot-dialog.component.html',
  styleUrls: ['./time-slot-dialog.component.css']
})
export class TimeSlotDialogComponent implements OnInit {

  timeSlotForm: FormGroup;

  constructor(public dialogRef: MatDialogRef<TimeSlotDialogComponent>,
              private formBuilder: FormBuilder,
              @Inject(MAT_DIALOG_DATA) public timeSlot: ITimeSlot) {
  }


  ngOnInit() {
    console.log('timeSlot', this.timeSlot);

    this.timeSlotForm = this.formBuilder.group({
      id: this.timeSlot.id,
      startTime: [this.timeSlot.startTime, [Validators.required,]],
      endTime: [this.timeSlot.endTime, [Validators.required,]],
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


}
