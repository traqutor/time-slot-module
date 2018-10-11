import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";

import {CustomerService} from "../customer.service";
import {ICustomer} from "../../user/user.model";

@Component({
  selector: 'app-customer-dialog',
  templateUrl: './customer-dialog.component.html',
  styleUrls: ['./customer-dialog.component.css']
})
export class CustomerDialogComponent implements OnInit {

  customerForm: FormGroup;

  constructor(public dialogRef: MatDialogRef<CustomerDialogComponent>,
              private customerService: CustomerService,
              private formBuilder: FormBuilder,
              @Inject(MAT_DIALOG_DATA) public customer: ICustomer) {
  }


  ngOnInit() {

    this.customerForm = this.formBuilder.group({
      id: this.customer.id,
      name: [this.customer.name, [Validators.required,]],
      creationDate: this.customer.creationDate,
      modificationDate: this.customer.modificationDate,
    });
  }

  submit() {
    this.dialogRef.close(this.customerForm.value);
  }

}
