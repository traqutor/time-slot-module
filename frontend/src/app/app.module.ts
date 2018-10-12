import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {OverlayModule} from '@angular/cdk/overlay';
import {HttpClientModule} from '@angular/common/http';
import {FlexLayoutModule} from '@angular/flex-layout';
import {MomentDateAdapter} from '@angular/material-moment-adapter';
import {DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE} from '@angular/material/core';
import {HTTP_INTERCEPTORS} from '@angular/common/http';


import {AppComponent} from './app.component';
import {AppRoutingModule} from './app-routing.module';
import {AppMaterialModule} from './app-material.module';
import {LoginComponent} from './auth/login/login.component';
import {FrameComponent} from './common/frame/frame.component';
import {UserComponent} from './user/user.component';
import {AuthInterceptor} from './auth/auth.interceptor';
import {APP_DATE_FORMATS} from './auth/auth.model';
import {CustmerComponent} from './custmer/custmer.component';
import {CustomerDialogComponent} from './custmer/customer-dialog/customer-dialog.component';
import {VendorsComponent} from './vendors/vendors.component';
import {VendorDialogComponent} from './vendors/vendor-dialog/vendor-dialog.component';
import {ConfirmDialogComponent} from "./common/confirm-dialog/confirm-dialog.component";
import {SitesComponent} from './sites/sites.component';
import {SiteDialogComponent} from './sites/site-dialog/site-dialog.component';
import { SuppliersComponent } from './suppliers/suppliers.component';
import { SupplierDialogComponent } from './suppliers/supplier-dialog/supplier-dialog.component';
import { CommoditiesComponent } from './commodities/commodities.component';
import { CommodityDialogComponent } from './commodities/commodity-dialog/commodity-dialog.component';
import { ContractsComponent } from './contracts/contracts.component';
import { ContractDialogComponent } from './contracts/contract-dialog/contract-dialog.component';
import { StatusTypesComponent } from './status-types/status-types.component';
import { StatusTypeDialogComponent } from './status-types/status-type-dialog/status-type-dialog.component';
import { FleetsComponent } from './fleets/fleets.component';
import { FleetDialogComponent } from './fleets/fleet-dialog/fleet-dialog.component';
import { TimeSlotsComponent } from './time-slots/time-slots.component';
import { TimeSlotDialogComponent } from './time-slots/time-slot-dialog/time-slot-dialog.component';
import { VehiclesComponent } from './vehicles/vehicles.component';
import { VehicleDialogComponent } from './vehicles/vehicle-dialog/vehicle-dialog.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    FrameComponent,
    UserComponent,
    CustmerComponent,
    CustomerDialogComponent,
    VendorsComponent,
    VendorDialogComponent,
    ConfirmDialogComponent,
    VendorDialogComponent,
    SitesComponent,
    SiteDialogComponent,
    SuppliersComponent,
    SupplierDialogComponent,
    CommoditiesComponent,
    CommodityDialogComponent,
    ContractsComponent,
    ContractDialogComponent,
    StatusTypesComponent,
    StatusTypeDialogComponent,
    FleetsComponent,
    FleetDialogComponent,
    TimeSlotsComponent,
    TimeSlotDialogComponent,
    VehiclesComponent,
    VehicleDialogComponent,

  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    AppMaterialModule,
    FormsModule,
    HttpClientModule,
    FlexLayoutModule,
    ReactiveFormsModule,
    OverlayModule,
    AppMaterialModule,
    BrowserAnimationsModule,
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true},
    {provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE]},
    {provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS},

  ],
  entryComponents: [
    CustomerDialogComponent,
    ConfirmDialogComponent,
    VendorDialogComponent,
    SiteDialogComponent,
    SupplierDialogComponent,
    CommodityDialogComponent,
    ContractDialogComponent,
    StatusTypeDialogComponent,
    SiteDialogComponent,
    FleetDialogComponent,
    TimeSlotDialogComponent,
  ],
  bootstrap: [AppComponent]
})

export class AppModule {
}
