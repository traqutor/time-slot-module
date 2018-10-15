import {NgModule} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';


import {LoginComponent} from "./auth/login/login.component";
import {FrameComponent} from "./common/frame/frame.component";
import {CustmerComponent} from "./custmer/custmer.component";
import {VendorsComponent} from "./vendors/vendors.component";
import {SitesComponent} from "./sites/sites.component";
import {SuppliersComponent} from "./suppliers/suppliers.component";
import {CommoditiesComponent} from "./commodities/commodities.component";
import {ContractsComponent} from "./contracts/contracts.component";
import {StatusTypesComponent} from "./status-types/status-types.component";
import {FleetsComponent} from "./fleets/fleets.component";
import {TimeSlotsComponent} from "./time-slots/time-slots.component";
import {VehiclesComponent} from "./vehicles/vehicles.component";
import {UsersComponent} from "./users/users.component";
import {AdminComponent} from "./common/admin/admin.component";
import {TimeSlotsUserViewComponent} from "./time-slots/time-slots-user-view/time-slots-user-view.component";
import {AuthGuard} from "./auth/auth-guard.service";

const routes: Routes = [
  {path: '', redirectTo: 'login', pathMatch: 'full'},
  {path: 'login', component: LoginComponent},
  {
    path: 'admin', component: AdminComponent,  canActivate: [AuthGuard], children: [
      {path: 'customers', component: CustmerComponent},

      {path: 'vendors', component: VendorsComponent},
      {path: 'suppliers', component: SuppliersComponent},
      {path: 'commodities', component: CommoditiesComponent},
      {path: 'contracts', component: ContractsComponent},

      {path: 'timeSlots', component: TimeSlotsComponent},
      {path: 'statusTypes', component: StatusTypesComponent},

      {path: 'sites', component: SitesComponent},
      {path: 'fleets', component: FleetsComponent},
      {path: 'vehicles', component: VehiclesComponent},

      {path: 'users', component: UsersComponent},
    ]
  }, {
    path: 'frame', component: FrameComponent, canActivate: [AuthGuard], children: [
      {path: 'timeSlots', component: TimeSlotsUserViewComponent},
    ],
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {

}
