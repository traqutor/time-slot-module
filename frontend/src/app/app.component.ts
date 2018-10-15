import {Component, OnDestroy, OnInit} from '@angular/core';
import {AuthService} from "./auth/auth.service";
import {IUserInfo, UserRoleNameEnum} from "./users/user.model";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, OnDestroy {
  user: IUserInfo;
  USER_ROLES = UserRoleNameEnum;

  private subscriptions: Subscription[] = [];

  constructor(private auth: AuthService) {
  }

  logOut() {
    this.auth.logout();
  }

  ngOnInit() {
    this.subscriptions.push(this.auth.currentUser.subscribe((res: IUserInfo) => {
      this.user = res;
    }));
  }

  ngOnDestroy() {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

}
