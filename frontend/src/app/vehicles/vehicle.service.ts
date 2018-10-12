import {Injectable} from '@angular/core';
import {ICommodity} from "../commodities/commodity.model";
import {BehaviorSubject, Observable} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {MatSnackBar} from "@angular/material";
import {environment} from "../../environments/environment";
import {IVehicle} from "./vehicle.model";

@Injectable({
  providedIn: 'root'
})
export class VehicleService {

  private vehicles: Array<IVehicle> = [];
  public vehiclesChanged: BehaviorSubject<Array<IVehicle>> = new BehaviorSubject<Array<IVehicle>>([]);

  private readonly url: string;

  constructor(private http: HttpClient,
              private snackBar: MatSnackBar) {
    this.url = environment.url;
  }

  // gets vehicles from all fleets with customer id based on the logged user gets all vehicles for admin
  getVehicles() {
    this.http.get(`${this.url}/api/Vehicles/GetVehicles`)
      .subscribe((res: Array<IVehicle>) => {
        this.vehicles = res;
        this.vehiclesChanged.next(this.vehicles);
      });
  }

  // get vehicles for specific fleet Id
  getVehiclesForFleetId(fleetId: number): Observable<Array<IVehicle>> {
    return this.http.get <Array<IVehicle>>(`${this.url}/api/Vehicles/GetVehicles/${fleetId}`);
  }

  // get vehicles for specific fleet Id
  getVehiclesForSpecificDriver(driverId: number): Observable<Array<IVehicle>> {
    return this.http.get <Array<IVehicle>>(`${this.url}/api/Vehicles/GetDriverVehicles`);
  }


  getVehicleById(vehicleId: number): Observable<IVehicle> {
    return this.http.get<IVehicle>(`${this.url}/api/Vehicles/GetVehicle/${vehicleId}`);
  }

  putVehicle(vehicle: IVehicle, index: number) {

    this.http.put(`${this.url}/api/Vehicles/PutVehicle`, vehicle)
      .subscribe((res: IVehicle) => {

        // in case when entry entity ID is 0 that means Add action
        // else is Edit so the object needs to be replaced in array

        if (vehicle.id === 0) {

          this.vehicles.push(res);
          this.snackBar.open('Vehicle Added', '', {
            duration: 2000,
          });

        } else {

          this.vehicles[index] = res;
          this.snackBar.open('Vehicle Changed', '', {
            duration: 2000,
          });

        }

        this.vehiclesChanged.next(this.vehicles);
      });
  }

  deleteVehicle(vehicleId: number, index: number) {
    this.http.delete(`${this.url}/api/Vehicles/DeleteVehicle/${vehicleId}`)
      .subscribe(() => {
        this.vehicles.splice(index, 1);
        this.vehiclesChanged.next(this.vehicles);
        this.snackBar.open('Vehicle Deleted!', '', {
          duration: 2000,
        });
      });
  }
}
