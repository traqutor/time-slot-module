import {Component, Inject, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material";
import {CustomerService} from "../../custmer/customer.service";
import {EntityStatusEnum, ICustomer} from "../../users/user.model";
import {IVendor} from "../vendor.model";

@Component({
  selector: 'app-vendor-dialog',
  templateUrl: './vendor-dialog.component.html',
  styleUrls: ['./vendor-dialog.component.css']
})
export class VendorDialogComponent implements OnInit {

  vendorForm: FormGroup;

  constructor(public dialogRef: MatDialogRef<VendorDialogComponent>,
              private formBuilder: FormBuilder,
              @Inject(MAT_DIALOG_DATA) public vendor: IVendor) {
  }


  ngOnInit() {

    this.vendorForm = this.formBuilder.group({
      id: this.vendor.id,
      name: [this.vendor.name, [Validators.required,]],
      creationDate: this.vendor.creationDate,
      modificationDate: this.vendor.modificationDate,
      createdBy: this.vendor.createdBy,
      modifiedBy: this.vendor.modifiedBy,
      entityStatus: this.vendor.entityStatus
    });

  }


  submit() {
    this.dialogRef.close(this.vendorForm.value);
  }

}
