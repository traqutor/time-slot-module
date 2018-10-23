import {Injectable} from '@angular/core';
import {BehaviorSubject, observable, Observable} from "rxjs";
import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {MatSnackBar} from "@angular/material";
import {environment} from "../../environments/environment";
import {ITimeSlot, ITimeSlotDelivery, IUniformViewTimeSlot} from "./time-slot.model";
import {Moment} from "moment";

@Injectable({
  providedIn: 'root'
})
export class TimeSlotService {

  private timeSlots: Array<ITimeSlot> = [];
  public timeSlotsChanged: BehaviorSubject<Array<ITimeSlot>> = new BehaviorSubject<Array<ITimeSlot>>([]);


  private deliveryTimeSlots: Array<IUniformViewTimeSlot> = [];
  public deliveryTimeSlotsChanged: BehaviorSubject<Array<IUniformViewTimeSlot>> = new BehaviorSubject<Array<IUniformViewTimeSlot>>([]);

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

  getTimeSlotDeliveryData(siteId: number, date: string) {
    return this.http.request(`get`,
      `${this.url}/api/DeliveryTimeSlots/GetTimeSlotData?sid=${siteId}&day=${date}`)
      .subscribe((res: Array<IUniformViewTimeSlot>) => {
        this.deliveryTimeSlots = res;
        this.deliveryTimeSlotsChanged.next(this.deliveryTimeSlots);
      });
  }

  getUsedCommodityAmount(commodityId: number, siteId: number, date: string): Observable<{ usedAmount: number }> {
    return this.http.get<{ usedAmount: number }>(`${this.url}/api/DeliveryTimeSlots/GetUsedCommodityAmount?cid=${commodityId}&sid=${siteId}&day=${date}`);
  }


  getTimeSlotById(timeSlotId: number): Observable<ITimeSlot> {
    return this.http.get<ITimeSlot>(`${this.url}/api/TimeSlots/GetTimeSlot/${timeSlotId}`);
  }

  putTimeSlot(timeSlot: ITimeSlot, index: number) {

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

  putDeliveryTimeSlot(deliveryTimeSlot: ITimeSlotDelivery, index: number): Observable<any> {

    const slotObservable = new Observable((observer) => {

      if (deliveryTimeSlot.deliveryDate instanceof Date) {

      }
      else {
        let tmpMoment: Moment = <Moment>deliveryTimeSlot.deliveryDate;
        let tmpDate: Date = new Date(Date.UTC(tmpMoment.year(), tmpMoment.month(), tmpMoment.date()));
        deliveryTimeSlot.deliveryDate = tmpDate;
      }

      this.http.put(`${this.url}/api/DeliveryTimeSlots/PutTimeSlot`, deliveryTimeSlot)
        .subscribe((res: ITimeSlotDelivery) => {

          // there is only Edit so the object needs to be replaced in array

          deliveryTimeSlot = res;

          this.deliveryTimeSlots[index].deliveryTimeSlot = deliveryTimeSlot;
          this.snackBar.open('TimeSlot Defined', '', {
            duration: 2000,
          });

          this.deliveryTimeSlotsChanged.next(this.deliveryTimeSlots);

          observer.next('');
        }, error => {
          observer.error(error.error.message);
    });
      observer.complete();

    });

    return slotObservable;
  }

  directputDeliveryTimeSlot(deliveryTimeSlot: ITimeSlotDelivery): Observable<ITimeSlotDelivery> {
    return this.http.put<ITimeSlotDelivery>(`${this.url}/api/DeliveryTimeSlots/PutTimeSlot`, deliveryTimeSlot);
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

  deleteDeliveryTimseSlot(timeSlotId: number, index: number) {
    this.http.delete(`${this.url}/api/DeliveryTimeSlots/DeleteTimeSlot/${timeSlotId}`)
      .subscribe(() => {
        this.timeSlots.splice(index, 1);
        this.timeSlotsChanged.next(this.timeSlots);
        this.snackBar.open('TimeSlot Deleted!', '', {
          duration: 2000,
        });
      });
  }


}
