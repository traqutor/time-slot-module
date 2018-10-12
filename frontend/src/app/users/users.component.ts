import {Component, OnInit} from '@angular/core';
import {EntityStatusEnum, IUser} from "./user.model";
import {ConfirmDialogService} from "../common/confirm-dialog/confirm-dialog.service";
import {MatDialog} from "@angular/material";
import {Subscription} from "rxjs";
import {UserService} from "./user.service";
import {UserDialogComponent} from "./user-dialog/user-dialog.component";

@Component({
  selector: 'app-user',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {

  public users: Array<IUser> = [];

  private voidUser: IUser = {
    id: 0,
    email: null,
    password: null,
    name: null,
    surname: null,
    role: {id: null, name: null},
    customer: {
      id: 0,
      name: null,
      creationDate: null,
      modificationDate: null,
      createdBy: null,
      modifiedBy: null,
      entityStatus: EntityStatusEnum.NORMAL
    },
    site: {
      id: 0,
      name: null,
      customer: {
        id: 0,
        name: null,
        creationDate: null,
        modificationDate: null,
        createdBy: null,
        modifiedBy: null,
        entityStatus: EntityStatusEnum.NORMAL
      },
      createdBy: null,
      creationDate: null,
      modifiedBy: null,
      modificationDate: null,
      entityStatus: EntityStatusEnum.NORMAL
    },
    fleet: {
      id: null,
      name: null,
      customer: {
        id: 0,
        name: null,
        creationDate: null,
        modificationDate: null,
        createdBy: null,
        modifiedBy: null,
        entityStatus: EntityStatusEnum.NORMAL
      },
      createdBy: null,
      creationDate: null,
      modifiedBy: null,
      modificationDate: null,
      entityStatus: EntityStatusEnum.NORMAL
    }
    ,
    vehicles: [],
    entityStatus: EntityStatusEnum.NORMAL


  };


  private subscriptions = [];

  constructor(private userService: UserService,
              private confirm: ConfirmDialogService,
              private dialog: MatDialog
  ) {
  }

  addUser() {
    this.editUser(this.voidUser, -1);
  }

  editUser(user: IUser, index: number) {
    const dialogRef = this.dialog.open(UserDialogComponent, {
      width: '45%',
      disableClose: true,
      data: user,
    });
    dialogRef.afterClosed()
      .subscribe((resolvedUser: IUser) => {

        if (resolvedUser) {
          this.userService.putUser(resolvedUser, index);
        }

      });
  }

  deleteUser(user: IUser, index: number) {
    this.confirm.confirm('Delete User', 'Are you sure you would like to delete the User?')
      .subscribe((res: boolean) => {
        if (res) {
          this.userService.deleteUser(user.id, index);
        }
      });
  }

  ngOnInit() {

    // invoke Customers get from db by Admin
    this.userService.getUsers();

    // subscribe for Customers
    this.subscriptions.push(this.userService.usersChanged
      .subscribe((res: Array<IUser>) => {
        this.users = res;
      }));
  }

  ngOnDestroy() {
    this.subscriptions.forEach((sub: Subscription) => {
      sub.unsubscribe();
    })
  }


}
