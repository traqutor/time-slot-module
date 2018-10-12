import {Injectable} from '@angular/core';
import {ISite} from "../sites/site.model";
import {BehaviorSubject, Observable} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {MatSnackBar} from "@angular/material";
import {environment} from "../../environments/environment";
import {IFleet} from "./fleet.model";

@Injectable({
  providedIn: 'root'
})
export class FleetService {

  private fleets: Array<IFleet> = [];
  public fleetsChanged: BehaviorSubject<Array<IFleet>> = new BehaviorSubject<Array<IFleet>>([]);

  private url: string;

  constructor(private http: HttpClient,
              private snackBar: MatSnackBar) {
    this.url = environment.url;
  }

  getFleets() {
    this.http.get(`${this.url}/api/Fleets/GetFleets`)
      .subscribe((res: Array<IFleet>) => {
        this.fleets = res;
        this.fleetsChanged.next(this.fleets);
      });
  }

  getFleetsById(fleetId: number): Observable<Array<IFleet>> {
    return this.http.get<Array<IFleet>>(`${this.url}/api/Fleets/GetFleets/${fleetId}`);
  }

  getFleetById(fleetId: number): Observable<IFleet> {
    return this.http.get<IFleet>(`${this.url}/api/Fleets/GetFleet/${fleetId}`);
  }

  putFleet(fleet: IFleet, index: number) {

    this.http.put(`${this.url}/api/Fleets/PutFleet`, fleet)
      .subscribe((res: IFleet) => {

        // in case when entry entity ID is 0 that means Add action
        // else is Edit so the object needs to be replaced in array

        if (fleet.id === 0) {

          this.fleets.push(res);
          this.snackBar.open('Fleet Added', '', {
            duration: 2000,
          });

        } else {

          this.fleets[index] = res;
          this.snackBar.open('Fleet Changed', '', {
            duration: 2000,
          });

        }

        this.fleetsChanged.next(this.fleets);
      });
  }

  deleteFleet(fleetId: number, index: number) {
    this.http.delete(`${this.url}/api/Fleets/DeleteFleet/${fleetId}`)
      .subscribe(() => {
        this.fleets.splice(index, 1);
        this.fleetsChanged.next(this.fleets);
        this.snackBar.open('Fleet Deleted!', '', {
          duration: 2000,
        });
      });
  }

}
