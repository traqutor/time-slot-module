import {Component, Inject, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material";
import {IContract} from "../contract.model";
import {VendorService} from "../../vendors/vendor.service";
import {SupplierService} from "../../suppliers/supplier.service";
import {CommodityService} from "../../commodities/commodity.service";
import {IVendor} from "../../vendors/vendor.model";
import {ISupplier} from "../../suppliers/supplier.model";
import {ICommodity} from "../../commodities/commodity.model";

@Component({
  selector: 'app-contract-dialog',
  templateUrl: './contract-dialog.component.html',
  styleUrls: ['./contract-dialog.component.css']
})
export class ContractDialogComponent implements OnInit {

  contractForm: FormGroup;
  vendors: IVendor[];
  suppliers: ISupplier[];
  commodities: ICommodity[];

  constructor(public dialogRef: MatDialogRef<ContractDialogComponent>,
              private formBuilder: FormBuilder,
              private vendorService: VendorService,
              private supplierService: SupplierService,
              private commodityService: CommodityService,
              @Inject(MAT_DIALOG_DATA) public contract: IContract) {
  }


  ngOnInit() {

    this.contractForm = this.formBuilder.group({
      id: this.contract.id,
      name: [this.contract.name, [Validators.required,]],
      vendor: [this.contract.vendor, [Validators.required]],
      supplier: [this.contract.supplier, [Validators.required]],
      commodity: [this.contract.commodity, [Validators.required]],
      creationDate: this.contract.creationDate,
      modificationDate: this.contract.modificationDate,
      createdBy: this.contract.createdBy,
      modifiedBy: this.contract.modifiedBy,
      entityStatus: this.contract.entityStatus
    });

    this.vendorService.getVendors();

    this.vendorService.vendorsChanged.subscribe((res: IVendor[]) => {
      this.vendors = res;
    });

    this.supplierService.getSuppliers();
    this.supplierService.suppliersChanged.subscribe((res: ISupplier[]) => {
      this.suppliers = res;
    });

    this.commodityService.getCommodities();
    this.commodityService.commoditiesChanged.subscribe((res: ICommodity[]) => {
      this.commodities = res;
    });

  }

  compare(val1, val2) {
    return val1 && val2 ? val1.id === val2.id : val1 === val2;
  }


  submit() {
    this.dialogRef.close(this.contractForm.value);
  }


}
