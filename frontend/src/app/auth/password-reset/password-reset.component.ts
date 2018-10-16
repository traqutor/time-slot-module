import {Component, OnInit} from '@angular/core';
import {SharedService} from "../shared/shared.service";
import {MatSnackBar} from "@angular/material";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {AuthService} from "../auth.service";


@Component({
  selector: 'app-password-reset',
  templateUrl: './password-reset.component.html',
  styleUrls: ['./password-reset.component.css']
})
export class PasswordResetComponent implements OnInit {

  public isEmailSend = false;
  public email: string;
  resetForm: FormGroup;


  constructor(private auth: AuthService,
              private formBuilder: FormBuilder,
              private snackBar: MatSnackBar) {
  }

  ngOnInit() {
    this.resetForm = this.formBuilder.group({
      email: [null, [
        Validators.required,
        Validators.email,
      ]]
    });

  }

  resetPassword() {
    this.auth.forgotPassword(this.resetForm.value)
      .subscribe(() => {
        this.isEmailSend = true;
      }, () => {
        this.isEmailSend = true;
      });
  }

}
