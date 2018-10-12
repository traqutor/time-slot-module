import { Component, OnInit } from '@angular/core';
import {ISite} from "../sites/site.model";
import {EntityStatusEnum} from "../users/user.model";
import {SiteService} from "../sites/site.service";
import {ConfirmDialogService} from "../common/confirm-dialog/confirm-dialog.service";
import {MatDialog} from "@angular/material";
import {SiteDialogComponent} from "../sites/site-dialog/site-dialog.component";
import {Subscription} from "rxjs";
import {IFleet} from "./fleet.model";
import {FleetService} from "./fleet.service";
import {FleetDialogComponent} from "./fleet-dialog/fleet-dialog.component";

@Component({
  selector: 'app-fleets',
  templateUrl: './fleets.component.html',
  styleUrls: ['./fleets.component.css']
})
export class FleetsComponent implements OnInit {

  public fleets: Array<IFleet> = [];

  private voidFleet: IFleet = {
    id: 0,
    name: '',
    customer: {
      id: 0,
      name: null,
      creationDate: null,
      modificationDate: null,
      createdBy: null,
      modifiedBy: null,
      entityStatus: EntityStatusEnum.NORMAL
    },
    creationDate: null,
    modificationDate: null,
    createdBy: null,
    modifiedBy: null,
    entityStatus: EntityStatusEnum.NORMAL
  };


  private subscriptions = [];

  constructor(private fleetService: FleetService,
              private confirm: ConfirmDialogService,
              private dialog: MatDialog) {
  }

  addFleet() {
    this.editFleet(this.voidFleet, -1);
  }

  editFleet(fleet: IFleet, index: number) {
    const dialogRef = this.dialog.open(FleetDialogComponent, {
      width: '45%',
      disableClose: true,
      data: fleet,
    });
    dialogRef.afterClosed()
      .subscribe((resolvedFleet: IFleet) => {

        if (resolvedFleet) {
          this.fleetService.putFleet(resolvedFleet, index);
        }

      });
  }

  deleteFleet(fleet: IFleet, index: number) {
    this.confirm.confirm('Delete Fleet', 'Are you sure you would like to delete the Fleet?')
      .subscribe((res: boolean) => {
        if (res) {
          this.fleetService.deleteFleet(fleet.id, index);
        }
      });
  }

  ngOnInit() {

    // invoke Customers get from db by Admin
    this.fleetService.getFleets();

    // subscribe for Customers
    this.subscriptions.push(this.fleetService.fleetsChanged
      .subscribe((res: Array<IFleet>) => {
        this.fleets = res;
      }));
  }

  ngOnDestroy() {
    this.subscriptions.forEach((sub: Subscription) => {
      sub.unsubscribe();
    })
  }

}
