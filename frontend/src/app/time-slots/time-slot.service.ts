import {Injectable} from '@angular/core';
import {BehaviorSubject, Observable} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {MatSnackBar} from "@angular/material";
import {environment} from "../../environments/environment";
import {ITimeSlotDelivery} from "./time-slot.model";

@Injectable({
  providedIn: 'root'
})
export class TimeSlotService {

  private timeSlots: Array<ITimeSlotDelivery> = [];
  public timeSlotsChanged: BehaviorSubject<Array<ITimeSlotDelivery>> = new BehaviorSubject<Array<ITimeSlotDelivery>>([]);

  private url: string;

  constructor(private http: HttpClient, private snackBar: MatSnackBar) {
    this.url = environment.url;
  }

  getTimeSlots() {
    this.http.get(`${this.url}/api/TimeSlots/GetTimeSlots`)
      .subscribe((res: Array<ITimeSlotDelivery>) => {
        this.timeSlots = res;
        this.timeSlotsChanged.next(this.timeSlots);
      });
  }

  getTimeSlotData(siteId: number, date: string): Observable<Array<ITimeSlotDelivery>> {
    return this.http.request<Array<ITimeSlotDelivery>>(`get`,
      `${this.url}/api/DeliveryTimeSlots/GetTimeSlotData?sid=${siteId}&day=${date}`);
  }

  getTimeSlotById(timeSlotId: number): Observable<ITimeSlotDelivery> {
    return this.http.get<ITimeSlotDelivery>(`${this.url}/api/TimeSlots/GetTimeSlot/${timeSlotId}`);
  }

  putTimeSlot(timeSlot: ITimeSlotDelivery, index: number) {

    this.http.put(`${this.url}/api/TimeSlots/PutTimeSlot`, timeSlot)
      .subscribe((res: ITimeSlotDelivery) => {

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
