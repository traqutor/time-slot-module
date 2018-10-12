import { Component, OnInit } from '@angular/core';
import {EntityStatusEnum} from "../user/user.model";
import {ConfirmDialogService} from "../common/confirm-dialog/confirm-dialog.service";
import {MatDialog} from "@angular/material";
import {Subscription} from "rxjs";
import {IContract} from "./contract.model";
import {ContractService} from "./contract.service";
import {ContractDialogComponent} from "./contract-dialog/contract-dialog.component";

@Component({
  selector: 'app-contracts',
  templateUrl: './contracts.component.html',
  styleUrls: ['./contracts.component.css']
})
export class ContractsComponent implements OnInit {

  public contracts: Array<IContract> = [];

  private voidContract: IContract = {
    id: 0,
    name: '',
    creationDate: null,
    modificationDate: null,
    createdBy: null,
    modifiedBy: null,
    entityStatus: EntityStatusEnum.NORMAL
  };


  private subscriptions = [];

  constructor(private contractService: ContractService,
              private confirm: ConfirmDialogService,
              private dialog: MatDialog) {
  }

  addContract() {
    this.editContract(this.voidContract, -1);
  }

  editContract(contract: IContract, index: number) {
    const dialogRef = this.dialog.open(ContractDialogComponent, {
      width: '45%',
      disableClose: true,
      data: contract,
    });
    dialogRef.afterClosed()
      .subscribe((resolvedContract: IContract) => {

        if (resolvedContract) {
          this.contractService.putContract(resolvedContract, index);
        }

      });
  }

  deleteContract(contract: IContract, index: number) {
    this.confirm.confirm('Delete Contract', 'Are you sure you would like to delete the Contract?')
      .subscribe((res: boolean) => {
        if (res) {
          this.contractService.deleteContract(contract.id, index);
        }
      });
  }

  ngOnInit() {

    // invoke Customers get from db by Admin
    this.contractService.getContracts();

    // subscribe for Customers
    this.subscriptions.push(this.contractService.contractsChanged
      .subscribe((res: Array<IContract>) => {
        this.contracts = res;
      }));
  }

  ngOnDestroy() {
    this.subscriptions.forEach((sub: Subscription) => {
      sub.unsubscribe();
    })
  }

}
