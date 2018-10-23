import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material";

@Component({
  selector: 'app-slot-hour-dialog',
  templateUrl: './slot-hour-dialog.component.html',
  styleUrls: ['./slot-hour-dialog.component.css']
})
export class SlotHourDialogComponent implements OnInit {

  public hours: string[] = ['00', '01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12', '13', '14', '15', '16', '17', '18', '19', '20', '21', '22', '23'];
  public minutes: string[] = ['00', '05', '10', '15', '20', '25', '30', '35', '40', '45', '50', '55'];

  hour: string;
  minute: string;

  constructor(public dialogRef: MatDialogRef<SlotHourDialogComponent>,
              @Inject(MAT_DIALOG_DATA) public time: string) {
  }

  ngOnInit() {

    console.log('this.time', this.time);

    if (this.time.indexOf(':') > 0) {
      this.hour = this.time.slice(0, this.time.indexOf(':'));
      this.minute = this.time.slice(this.time.indexOf(':'));
    } else {

    }
    this.hour = '00';
    this.minute = '00';
  }

  submit() {

    this.dialogRef.close(this.hour + ':' + this.minute);
  }

}
