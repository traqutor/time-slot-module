import {Component, OnDestroy, OnInit} from '@angular/core';
import {EntityStatusEnum} from "../users/user.model";
import {ConfirmDialogService} from "../common/confirm-dialog/confirm-dialog.service";
import {MatDialog} from "@angular/material";
import {Subscription} from "rxjs";
import {ICommodity} from "./commodity.model";
import {CommodityService} from "./commodity.service";
import {CommodityDialogComponent} from "./commodity-dialog/commodity-dialog.component";

@Component({
  selector: 'app-commodities',
  templateUrl: './commodities.component.html',
  styleUrls: ['./commodities.component.css']
})
export class CommoditiesComponent implements OnInit, OnDestroy {

  public commodities: Array<ICommodity> = [];

  private voidCommodity: ICommodity = {
    id: 0,
    name: '',
    creationDate: null,
    modificationDate: null,
    createdBy: null,
    modifiedBy: null,
    entityStatus: EntityStatusEnum.NORMAL
  };


  private subscriptions = [];

  constructor(private commodityService: CommodityService,
              private confirm: ConfirmDialogService,
              private dialog: MatDialog) {
  }

  addCommodity() {
    this.editCommodity(this.voidCommodity, -1);
  }

  editCommodity(commodity: ICommodity, index: number) {
    const dialogRef = this.dialog.open(CommodityDialogComponent, {
      width: '65%',
      disableClose: true,
      data: commodity,
    });
    dialogRef.afterClosed()
      .subscribe((resolvedCommodity: ICommodity) => {

        if (resolvedCommodity) {
          this.commodityService.putCommodity(resolvedCommodity, index);
        }

      });
  }

  deleteCommodity(commodity: ICommodity, index: number) {
    this.confirm.confirm('Delete Commodity', 'Are you sure you would like to delete the Commodity?')
      .subscribe((res: boolean) => {
        if (res) {
          this.commodityService.deleteCommodity(commodity.id, index);
        }
      });
  }

  ngOnInit() {

    // invoke Commodities get from db by Admin
    this.commodityService.getCommodities();

    // subscribe for Commodities
    this.subscriptions.push(this.commodityService.commoditiesChanged
      .subscribe((res: Array<ICommodity>) => {
        this.commodities = res;
      }));
  }

  ngOnDestroy() {
    this.subscriptions.forEach((sub: Subscription) => {
      sub.unsubscribe();
    })
  }


}
