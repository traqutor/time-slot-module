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

  saveCredentialsToStorage(token: string) {
    sessionStorage.setItem('token', token);
  }

  saveUserInfoToStorage(userInfo: IUserInfo) {
    sessionStorage.setItem('userInfo', JSON.stringify(userInfo));
    this.currentUserSubject.next(userInfo);
  }

  removeCredentialsFromStorage() {
    sessionStorage.removeItem('userInfo');
    sessionStorage.removeItem('token');
    this.currentUserSubject.next(null);
  }

  getTokenFromStorage(): string {
    return sessionStorage.getItem('token');
  }

  getUserFromStorage(): IUserInfo {
    return JSON.parse(sessionStorage.getItem('userInfo'));
  }


  isUserAuthenticated() {
    const token: string = this.getTokenFromStorage();
    if (token) {
      const userInfo: IUserInfo = this.getUserFromStorage();
      this.currentUserSubject.next(userInfo);
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
    this.saveUserInfoToStorage({} as IUserInfo);
    this.router.navigateByUrl('/login');
    this.currentUserSubject.next(null);
  }

  getUserInfo(): Observable<IUserInfo> {
    return this.http.get<IUserInfo>(`${this.url}/api/WebUsers/GetUserInfo`);
  }

  forgotPassword(email: string) {
    return this.http.post(`${this.url}/ForgotPassword`, email);
  }

  onPasswordRecovery(token: string, email: string,  password: string, confirmPassword: string) {
    return this.http.post(`${this.url}/ForgotPasswordReset`, {
      token: token,
      email: email,
      password: password,
      confirmPassword: confirmPassword
    });
  }

}
