import {Component, Inject, OnDestroy, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material";

import {ISite} from "../site.model";
import {ICustomer} from "../../users/user.model";
import {CustomerService} from "../../custmer/customer.service";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-site-dialog',
  templateUrl: './site-dialog.component.html',
  styleUrls: ['./site-dialog.component.css']
})
export class SiteDialogComponent implements OnInit, OnDestroy {

  siteForm: FormGroup;

  constructor(public dialogRef: MatDialogRef<SiteDialogComponent>,
              private formBuilder: FormBuilder,
              private customerService: CustomerService,
              @Inject(MAT_DIALOG_DATA) public site: ISite) {
  }

  private subscriptions: Array<Subscription> = [];
  public customers: Array<ICustomer> = [];

  ngOnInit() {

    this.siteForm = this.formBuilder.group({
      id: this.site.id,
      name: [this.site.name, [Validators.required,]],
      customer: [this.site.customer, [Validators.required]],
      creationDate: this.site.creationDate,
      modificationDate: this.site.modificationDate,
      createdBy: this.site.createdBy,
      modifiedBy: this.site.modifiedBy,
      entityStatus: this.site.entityStatus
    });

    // invoke Customers get from db
    this.customerService.getCustomers();

    // subscribe for Customers
    this.subscriptions.push(this.customerService.customersChanged
      .subscribe((res: Array<ICustomer>) => {
        this.customers = res;
      }));

  }

  compare(val1, val2) {
    if (val1 && val2) {
      return val1.id === val2.id;
    }
  }

  submit() {
    this.dialogRef.close(this.siteForm.value);
  }

  ngOnDestroy() {
    this.subscriptions.forEach((sub: Subscription) => {
      sub.unsubscribe();
    })

  }
}
