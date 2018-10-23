import {Component, Inject, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material";
import {ISupplier} from "../supplier.model";

@Component({
  selector: 'app-supplier-dialog',
  templateUrl: './supplier-dialog.component.html',
  styleUrls: ['./supplier-dialog.component.css']
})
export class SupplierDialogComponent implements OnInit {

  supplierForm: FormGroup;

  constructor(public dialogRef: MatDialogRef<SupplierDialogComponent>,
              private formBuilder: FormBuilder,
              @Inject(MAT_DIALOG_DATA) public supplier: ISupplier) {
  }


  ngOnInit() {

    this.supplierForm = this.formBuilder.group({
      id: this.supplier.id,
      name: [this.supplier.name, [Validators.required,]],
      creationDate: this.supplier.creationDate,
      modificationDate: this.supplier.modificationDate,
      createdBy: this.supplier.createdBy,
      modifiedBy: this.supplier.modifiedBy,
      entityStatus: this.supplier.entityStatus
    });

  }


  submit() {
    this.dialogRef.close(this.supplierForm.value);
  }


}
