import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {OverlayModule} from "@angular/cdk/overlay";
import { HttpClientModule } from '@angular/common/http';
import { HttpModule } from '@angular/http';


import {AppComponent} from './app.component';
import {AppRoutingModule} from './app-routing.module';
import {AppMaterialModule} from './app-material.module';
import { LoginComponent } from './auth/login/login.component';
import { FrameComponent } from './common/frame/frame.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    FrameComponent
  ],
  imports: [

    BrowserModule,
    AppRoutingModule,
    AppMaterialModule,
    FormsModule,
    HttpClientModule,
    HttpModule,
    ReactiveFormsModule,
    OverlayModule,
    AppMaterialModule,
    BrowserAnimationsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})

export class AppModule {
}
