import {NgModule} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';


import {LoginComponent} from "./auth/login/login.component";
import {FrameComponent} from "./common/frame/frame.component";
import {CustmerComponent} from "./custmer/custmer.component";
import {VendorsComponent} from "./vendors/vendors.component";

const routes: Routes = [
  {path: '', redirectTo: 'login', pathMatch: 'full'},
  {path: 'login', component: LoginComponent},
  {
    path: 'frame', component: FrameComponent, children: [
      {path: 'customers', component: CustmerComponent},
      {path: 'vendors', component: VendorsComponent},
    ]
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
