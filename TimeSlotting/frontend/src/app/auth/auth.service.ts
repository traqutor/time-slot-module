import {Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient, HttpParams} from "@angular/common/http";
import {Router} from "@angular/router";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  public url: string;

  constructor(private http: HttpClient,
              private router: Router) {
    this.url = environment.url;
  }

  login(credentials): Observable<Object> {
    const { login, password } = credentials;
    const body = new HttpParams()
      .set('grant_type', 'password')
      .set('userName', login)
      .set('password', password);
    return this.http.post(this.url + '/token', body.toString());
  }

}
