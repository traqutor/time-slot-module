import { Component, OnInit } from '@angular/core';
import {IContract} from "../contracts/contract.model";
import {EntityStatusEnum} from "../user/user.model";
import {ContractService} from "../contracts/contract.service";
import {ConfirmDialogService} from "../common/confirm-dialog/confirm-dialog.service";
import {MatDialog} from "@angular/material";
import {ContractDialogComponent} from "../contracts/contract-dialog/contract-dialog.component";
import {Subscription} from "rxjs";
import {IStatusType} from "./status-type.model";
import {StatusTypeService} from "./status-type.service";
import {StatusTypeDialogComponent} from "./status-type-dialog/status-type-dialog.component";

@Component({
  selector: 'app-status-types',
  templateUrl: './status-types.component.html',
  styleUrls: ['./status-types.component.css']
})
export class StatusTypesComponent implements OnInit {

  public statusTypes: Array<IStatusType> = [];

  private voidStatusType: IStatusType = {
    id: 0,
    name: '',
    creationDate: null,
    modificationDate: null,
    createdBy: null,
    modifiedBy: null,
    entityStatus: EntityStatusEnum.NORMAL
  };


  private subscriptions = [];

  constructor(private statusTypeService: StatusTypeService,
              private confirm: ConfirmDialogService,
              private dialog: MatDialog) {
  }

  addStatusType() {
    this.editStatusType(this.voidStatusType, -1);
  }

  editStatusType(statusType: IStatusType, index: number) {
    const dialogRef = this.dialog.open(StatusTypeDialogComponent, {
      width: '45%',
      disableClose: true,
      data: statusType,
    });
    dialogRef.afterClosed()
      .subscribe((resolvedStatusType: IStatusType) => {

        if (resolvedStatusType) {
          this.statusTypeService.putStatusType(resolvedStatusType, index);
        }

      });
  }

  deleteStatusType(statusType: IStatusType, index: number) {
    this.confirm.confirm('Delete Status Type', 'Are you sure you would like to delete the Status Type?')
      .subscribe((res: boolean) => {
        if (res) {
          this.statusTypeService.deleteStatusType(statusType.id, index);
        }
      });
  }

  ngOnInit() {

    // invoke Customers get from db by Admin
    this.statusTypeService.getStatusTypes();

    // subscribe for Customers
    this.subscriptions.push(this.statusTypeService.statusTypesChanged
      .subscribe((res: Array<IStatusType>) => {
        this.statusTypes = res;
      }));
  }

  ngOnDestroy() {
    this.subscriptions.forEach((sub: Subscription) => {
      sub.unsubscribe();
    })
  }


}
