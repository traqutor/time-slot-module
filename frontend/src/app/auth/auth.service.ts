import {Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient, HttpParams} from "@angular/common/http";
import {Router} from "@angular/router";
import {BehaviorSubject, Observable, ReplaySubject} from "rxjs";
import {distinctUntilChanged} from "rxjs/operators";
import {IUserInfo} from "../users/user.model";

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  public url: string;

  private currentUserSubject = new BehaviorSubject<IUserInfo>({} as IUserInfo);
  public currentUser = this.currentUserSubject.asObservable().pipe(distinctUntilChanged());

  private isAuthenticatedSubject = new ReplaySubject<boolean>(1);
  public isAuthenticated = this.isAuthenticatedSubject.asObservable();

  constructor(private http: HttpClient,
              private router: Router) {
    this.url = environment.url;
  }

  saveCredentialsToStorage(login: string, token: string) {
    sessionStorage.setItem('login', login);
    sessionStorage.setItem('token', token);
  }

  removeCredentialsFromStorage() {
    sessionStorage.removeItem('login');
    sessionStorage.removeItem('token');
  }

  getTokenFromStorage(): string {
    return sessionStorage.getItem('token');
  }

  setUser(user?: IUserInfo) {
    sessionStorage.setItem('user', JSON.stringify(user));
    this.currentUserSubject.next(user);
  }

  setAuth(auth: boolean) {
    this.isAuthenticatedSubject.next(auth);
  }

  getUser(): IUserInfo {
    return JSON.parse(sessionStorage.getItem('user'));
  }

  isUserAuthenticated() {
    const token: string = this.getTokenFromStorage();
    if (token) {
      return true;
    }
    return false;
  }

  login(credentials): Observable<Object> {
    const {login, password} = credentials;
    const body = new HttpParams()
      .set('grant_type', 'password')
      .set('userName', login)
      .set('password', password);
    return this.http.post(this.url + '/token', body.toString());
  }

  logout() {
    this.removeCredentialsFromStorage();
    this.setUser({} as IUserInfo);
    this.setAuth(false);
    this.router.navigateByUrl('/login');
  }

  getUserInfo(): Observable<IUserInfo> {
    return this.http.get<IUserInfo>(`${this.url}/api/WebUsers/GetUserInfo`);
  }

}
