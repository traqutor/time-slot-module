import {Component, Inject, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material";
import {ICommodity} from "../commodity.model";

@Component({
  selector: 'app-commodity-dialog',
  templateUrl: './commodity-dialog.component.html',
  styleUrls: ['./commodity-dialog.component.css']
})
export class CommodityDialogComponent implements OnInit {

  commodityForm: FormGroup;

  constructor(public dialogRef: MatDialogRef<CommodityDialogComponent>,
              private formBuilder: FormBuilder,
              @Inject(MAT_DIALOG_DATA) public commodity: ICommodity) {
  }


  ngOnInit() {

    this.commodityForm = this.formBuilder.group({
      id: this.commodity.id,
      name: [this.commodity.name, [Validators.required,]],
      maxTonsPerDay: [this.commodity.maxTonsPerDay, [Validators.required,]],
      creationDate: this.commodity.creationDate,
      modificationDate: this.commodity.modificationDate,
      createdBy: this.commodity.createdBy,
      modifiedBy: this.commodity.modifiedBy,
      entityStatus: this.commodity.entityStatus
    });

  }


  submit() {
    this.dialogRef.close(this.commodityForm.value);
  }

}
