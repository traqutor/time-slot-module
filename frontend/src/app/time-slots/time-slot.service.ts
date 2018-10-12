import { Injectable } from '@angular/core';
import {ICustomer} from "../user/user.model";
import {BehaviorSubject, Observable} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {MatSnackBar} from "@angular/material";
import {environment} from "../../environments/environment";
import {ITimeSlot} from "./time-slot.model";

@Injectable({
  providedIn: 'root'
})
export class TimeSlotService {

  private timeSlots: Array<ITimeSlot> = [];
  public timeSlotsChanged: BehaviorSubject<Array<ITimeSlot>> = new BehaviorSubject<Array<ITimeSlot>>([]);

  private url: string;

  constructor(private http: HttpClient, private snackBar: MatSnackBar) {
    this.url = environment.url;
  }

  getTimeSlots() {
    this.http.get(`${this.url}/api/TimeSlots/GetTimeSlots`)
      .subscribe((res: Array<ITimeSlot>) => {
        this.timeSlots = res;
        this.timeSlotsChanged.next(this.timeSlots);
      });
  }

  getTimeSlotById(timeSlotId: number): Observable<ITimeSlot> {
    return this.http.get<ITimeSlot>(`${this.url}/api/TimeSlots/GetTimeSlot/${timeSlotId}`);
  }

  putTimeSlot(timeSlot: ITimeSlot, index: number) {

    console.log('timeSlot', timeSlot);
    console.log('index', index);

    this.http.put(`${this.url}/api/TimeSlots/PutTimeSlot`, timeSlot)
      .subscribe((res: ITimeSlot) => {

        // in case when entry entity ID is 0 that means Add action
        // else is Edit so the object needs to be replaced in array

        if (timeSlot.id === 0) {

          this.timeSlots.push(res);
          this.snackBar.open('TimeSlot Added', '', {
            duration: 2000,
          });

        } else {

          this.timeSlots[index] = res;
          this.snackBar.open('TimeSlot Changed', '', {
            duration: 2000,
          });

        }

        this.timeSlotsChanged.next(this.timeSlots);
      });
  }

  deleteTimseSlot(timeSlotId: number, index: number) {
    this.http.delete(`${this.url}/api/TimeSlots/DeleteTimeSlot/${timeSlotId}`)
      .subscribe(() => {
        this.timeSlots.splice(index, 1);
        this.timeSlotsChanged.next(this.timeSlots);
        this.snackBar.open('TimeSlot Deleted!', '', {
          duration: 2000,
        });
      });
  }

}
