import {Component, OnDestroy, OnInit} from '@angular/core';
import {IVendor} from "../vendors/vendor.model";
import {EntityStatusEnum} from "../user/user.model";
import {VendorService} from "../vendors/vendor.service";
import {ConfirmDialogService} from "../common/confirm-dialog/confirm-dialog.service";
import {MatDialog} from "@angular/material";
import {VendorDialogComponent} from "../vendors/vendor-dialog/vendor-dialog.component";
import {Subscription} from "rxjs";
import {ISite} from "./site.model";
import {SiteService} from "./site.service";
import {SiteDialogComponent} from "./site-dialog/site-dialog.component";

@Component({
  selector: 'app-sites',
  templateUrl: './sites.component.html',
  styleUrls: ['./sites.component.css']
})
export class SitesComponent implements OnInit , OnDestroy {

  public sites: Array<ISite> = [];

  private voidSite: ISite = {
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

  constructor(private siteService: SiteService,
              private confirm: ConfirmDialogService,
              private dialog: MatDialog) {
  }

  addSite() {
    this.editSite(this.voidSite, -1);
  }

  editSite(site: ISite, index: number) {
    const dialogRef = this.dialog.open(SiteDialogComponent, {
      width: '45%',
      disableClose: true,
      data: site,
    });
    dialogRef.afterClosed()
      .subscribe((resolvedSite: ISite) => {

        if (resolvedSite) {
          this.siteService.putSite(resolvedSite, index);
        }

      });
  }

  deleteSite(site: ISite, index: number) {
    this.confirm.confirm('Delete Site', 'Are you sure you would like to delete the Site?')
      .subscribe((res: boolean) => {
        if (res) {
          this.siteService.deleteSite(site.id, index);
        }
      });
  }

  ngOnInit() {

    // invoke Customers get from db by Admin
    this.siteService.getSites();

    // subscribe for Customers
    this.subscriptions.push(this.siteService.sitesChanged
      .subscribe((res: Array<ISite>) => {
        this.sites = res;
      }));
  }

  ngOnDestroy() {
    this.subscriptions.forEach((sub: Subscription) => {
      sub.unsubscribe();
    })
  }

}
