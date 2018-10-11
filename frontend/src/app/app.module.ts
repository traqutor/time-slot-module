import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {OverlayModule} from '@angular/cdk/overlay';
import { HttpClientModule } from '@angular/common/http';
import {FlexLayoutModule} from '@angular/flex-layout';
import {MomentDateAdapter} from '@angular/material-moment-adapter';
import {DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE} from '@angular/material/core';
import {HTTP_INTERCEPTORS} from '@angular/common/http';


import {AppComponent} from './app.component';
import {AppRoutingModule} from './app-routing.module';
import {AppMaterialModule} from './app-material.module';
import { LoginComponent } from './auth/login/login.component';
import { FrameComponent } from './common/frame/frame.component';
import { UserComponent } from './user/user.component';
import {AuthInterceptor} from './auth/auth.interceptor';
import {APP_DATE_FORMATS} from './auth/auth.model';
import { CustmerComponent } from './custmer/custmer.component';
import { CustomerDialogComponent } from './custmer/customer-dialog/customer-dialog.component';
import { VendorsComponent } from './vendors/vendors.component';
import { VendorDialogComponent } from './vendors/vendor-dialog/vendor-dialog.component';
import {ConfirmDialogComponent} from "./common/confirm-dialog/confirm-dialog.component";
import { SitessComponent } from './sitess/sitess.component';
import { SitesComponent } from './sites/sites.component';
import { SiteDialogComponent } from './sites/site-dialog/site-dialog.component';

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

  ],
  bootstrap: [AppComponent]
})

export class AppModule {
}
