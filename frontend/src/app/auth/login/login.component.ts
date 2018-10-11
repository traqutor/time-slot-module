import {Component, OnInit} from '@angular/core';
import {NgForm} from "@angular/forms";
import {Router} from "@angular/router";

import {AuthService} from "../auth.service";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  isLogging: boolean;

  constructor(private auth: AuthService, private router: Router) {
  }

  ngOnInit() {
  }

  onSignIn(form: NgForm) {
    this.isLogging = true;
    const login = form.value.login;
    const password = form.value.password;
    this.auth.login({login, password})
      .subscribe(() => {
        this.isLogging = false;
        this.router.navigateByUrl('frame');
      }, err => {
        this.isLogging = false;
        console.log('error', err);
      });
  }


}
