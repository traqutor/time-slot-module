import { Injectable } from '@angular/core';
import {BehaviorSubject, Observable} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {MatSnackBar} from "@angular/material";
import {environment} from "../../environments/environment";
import {ISite} from "./site.model";

@Injectable({
  providedIn: 'root'
})
export class SiteService {

  private sites: Array<ISite> = [];
  public sitesChanged: BehaviorSubject<Array<ISite>> = new BehaviorSubject<Array<ISite>>([]);

  private url: string;

  constructor(private http: HttpClient,
              private snackBar: MatSnackBar) {
    this.url = environment.url;
  }

  getSites() {
    this.http.get(`${this.url}/api/Sites/GetSites`)
      .subscribe((res: Array<ISite>) => {
        this.sites = res;
        this.sitesChanged.next(this.sites);
      });
  }

  getSitesById(siteId: number): Observable<Array<ISite>> {
    return this.http.get<Array<ISite>>(`${this.url}/api/Sites/GetSites/${siteId}`);
  }

  getSiteById(siteId: number): Observable<ISite> {
    return this.http.get<ISite>(`${this.url}/api/Sites/GetSite/${siteId}`);
  }

  putSite(site: ISite, index: number)  {

    this.http.put(`${this.url}/api/Sites/PutSite`, site)
      .subscribe((res: ISite) => {

        // in case when entry entity ID is 0 that means Add action
        // else is Edit so the object needs to be replaced in array

        if (site.id === 0) {

          this.sites.push(res);
          this.snackBar.open('Site Added', '', {
            duration: 2000,
          });

        } else {

          this.sites[index] = res;
          this.snackBar.open('Site Changed', '', {
            duration: 2000,
          });

        }

        this.sitesChanged.next(this.sites);
      });
  }

  deleteSite(siteId: number, index: number) {
    this.http.delete(`${this.url}/api/Sites/DeleteSite/${siteId}`)
      .subscribe(() => {
        this.sites.splice(index, 1);
        this.sitesChanged.next(this.sites);
        this.snackBar.open('Site Deleted!', '', {
          duration: 2000,
        });
      });
  }

}
