import {Injectable} from '@angular/core';
import {ISite} from "../sites/site.model";
import {BehaviorSubject, Observable} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {MatSnackBar} from "@angular/material";
import {environment} from "../../environments/environment";
import {IRole, IRoleResult, IUser} from "./user.model";

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private users: Array<IUser> = [];
  public usersChanged: BehaviorSubject<Array<IUser>> = new BehaviorSubject<Array<IUser>>([]);

  private url: string;

  constructor(private http: HttpClient,
              private snackBar: MatSnackBar) {
    this.url = environment.url;
  }

  getUsers() {
    this.http.get(`${this.url}/api/WebUsers/GetUsers`)
      .subscribe((res: Array<IUser>) => {
        this.users = res;
        this.usersChanged.next(this.users);
      });
  }

  getDriversForCompany(companyId: number): Observable<Array<IUser>> {
    return this.http.request<Array<IUser>>('get', `${this.url}/api/WebUsers/GetDrivers`, {body: companyId});
  }

  getRoles(): Observable<IRoleResult> {
    return this.http.get<IRoleResult>(`${this.url}/api/Role/GetRoles`);
  }


  getUserById(userId: number): Observable<IUser> {
    return this.http.get<IUser>(`${this.url}/api/WebUsers/GetUser/${userId}`);
  }


  putUser(user: IUser, index: number) {

    this.http.put(`${this.url}/api/WebUsers/PutUser`, user)
      .subscribe((res: IUser) => {

        // in case when entry entity ID is 0 that means Add action
        // else is Edit so the object needs to be replaced in array

        if (user.id === 0) {

          this.users.push(res);
          this.snackBar.open('User Added', '', {
            duration: 2000,
          });

        } else {

          this.users[index] = res;
          this.snackBar.open('user Changed', '', {
            duration: 2000,
          });

        }

        this.usersChanged.next(this.users);
      });
  }

  deleteUser(userId: number, index: number) {
    this.http.delete(`${this.url}/api/WebUsers/DeleteUser/${userId}`)
      .subscribe(() => {
        this.users.splice(index, 1);
        this.usersChanged.next(this.users);
        this.snackBar.open('User Deleted!', '', {
          duration: 2000,
        });
      });
  }

}
