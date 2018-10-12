import {Component, Inject, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material";
import {IContract} from "../contract.model";

@Component({
  selector: 'app-contract-dialog',
  templateUrl: './contract-dialog.component.html',
  styleUrls: ['./contract-dialog.component.css']
})
export class ContractDialogComponent implements OnInit {

  contractForm: FormGroup;

  constructor(public dialogRef: MatDialogRef<ContractDialogComponent>,
              private formBuilder: FormBuilder,
              @Inject(MAT_DIALOG_DATA) public contract: IContract) {
  }


  ngOnInit() {

    this.contractForm = this.formBuilder.group({
      id: this.contract.id,
      name: [this.contract.name, [Validators.required,]],
      creationDate: this.contract.creationDate,
      modificationDate: this.contract.modificationDate,
      createdBy: this.contract.createdBy,
      modifiedBy: this.contract.modifiedBy,
      entityStatus: this.contract.entityStatus
    });

  }


  submit() {
    this.dialogRef.close(this.contractForm.value);
  }


}
