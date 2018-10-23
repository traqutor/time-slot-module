import {Component, Inject, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material";
import {IContract} from "../../contracts/contract.model";
import {IStatusType} from "../status-type.model";

@Component({
  selector: 'app-status-type-dialog',
  templateUrl: './status-type-dialog.component.html',
  styleUrls: ['./status-type-dialog.component.css']
})
export class StatusTypeDialogComponent implements OnInit {

  statusTypeForm: FormGroup;

  constructor(public dialogRef: MatDialogRef<StatusTypeDialogComponent>,
              private formBuilder: FormBuilder,
              @Inject(MAT_DIALOG_DATA) public statusType: IStatusType) {
  }


  ngOnInit() {

    this.statusTypeForm = this.formBuilder.group({
      id: this.statusType.id,
      name: [this.statusType.name, [Validators.required,]],
      creationDate: this.statusType.creationDate,
      modificationDate: this.statusType.modificationDate,
      createdBy: this.statusType.createdBy,
      modifiedBy: this.statusType.modifiedBy,
      entityStatus: this.statusType.entityStatus
    });

  }


  submit() {
    this.dialogRef.close(this.statusTypeForm.value);
  }


}
