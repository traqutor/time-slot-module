import {Component, OnInit} from '@angular/core';
import {MatDialog} from "@angular/material";
import {Subscription} from "rxjs";

import {EntityStatusEnum} from "../user/user.model";
import {ConfirmDialogService} from "../common/confirm-dialog/confirm-dialog.service";
import {IVehicle} from "./vehicle.model";
import {VehicleService} from "./vehicle.service";
import {VehicleDialogComponent} from "./vehicle-dialog/vehicle-dialog.component";

@Component({
  selector: 'app-vehicles',
  templateUrl: './vehicles.component.html',
  styleUrls: ['./vehicles.component.css']
})
export class VehiclesComponent implements OnInit {

  public vehicles: Array<IVehicle> = [];

  private voidVehicle: IVehicle = {
    id: 0,
    rego: '',
    fleet: {
      id: null,
      name: null,
      createdBy: null,
      creationDate: null,
      modifiedBy: null,
      modificationDate: null,
      entityStatus: EntityStatusEnum.NORMAL,
      customer: {
        id: null,
        name: null,
        creationDate: null,
        createdBy: null,
        modifiedBy: null,
        modificationDate: null,
        entityStatus: EntityStatusEnum.NORMAL
      }
    },
    creationDate: null,
    modificationDate: null,
    createdBy: null,
    modifiedBy: null,
    entityStatus: EntityStatusEnum.NORMAL
  };
  private subscriptions = [];

  constructor(private vehicleService: VehicleService,
              private confirm: ConfirmDialogService,
              private dialog: MatDialog) {
  }

  addVehicle() {
    this.editVehicle(this.voidVehicle, -1);
  }

  editVehicle(vehicle: IVehicle, index: number) {
    const dialogRef = this.dialog.open(VehicleDialogComponent, {
      width: '45%',
      disableClose: true,
      data: vehicle,
    });
    dialogRef.afterClosed()
      .subscribe((resolvedVehicle: IVehicle) => {

        if (resolvedVehicle) {
          this.vehicleService.putVehicle(resolvedVehicle, index);
        }

      });
  }

  deleteVehicle(vehicle: IVehicle, index: number) {
    this.confirm.confirm('Delete Vehicle', 'Are you sure you would like to delete the Vehicle?')
      .subscribe((res: boolean) => {
        if (res) {
          this.vehicleService.deleteVehicle(vehicle.id, index);
        }
      });
  }

  ngOnInit() {

    // invoke Customers get from db
    this.vehicleService.getVehicles();

    // subscribe for Customers
    this.subscriptions.push(this.vehicleService.vehiclesChanged
      .subscribe((res: Array<IVehicle>) => {
        this.vehicles = res;
      }));
  }

  ngOnDestroy() {
    this.subscriptions.forEach((sub: Subscription) => {
      sub.unsubscribe();
    })
  }

}
