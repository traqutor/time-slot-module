import {Component, OnInit} from '@angular/core';
import {NgForm} from "@angular/forms";
import {Router} from "@angular/router";

import {AuthService} from "../auth.service";
import {tap} from "rxjs/operators";
import {AuthResponse, IUserInfo} from "../../users/user.model";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  public errMessage: string;
  public isLogging: boolean;
  public isError: boolean;

  constructor(private auth: AuthService, private router: Router) {
  }

  ngOnInit() {
  }

  onSignIn(form: NgForm) {
    this.errMessage = '';
    this.isLogging = true;
    this.isError = false;
    const login = form.value.login;
    const password = form.value.password;
    this.auth.login({login, password})
      .subscribe((authResponse: AuthResponse) => {
        this.auth.saveCredentialsToStorage(authResponse.access_token);

        this.auth.getUserInfo()
          .subscribe((user: IUserInfo) => {
            this.isLogging = false;
            this.auth.saveUserInfoToStorage(user);
            this.router.navigateByUrl('frame/timeSlots');
          });

      }, err => {
        this.isLogging = false;
        this.isError = true;
        this.errMessage = err.error.error_description;
      });
  }

}
