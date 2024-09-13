// Importing HttpClient module from Angular's @angular/common/http package.
// HttpClient is used to perform HTTP requests like GET, POST, PUT, DELETE.
import { HttpClient } from '@angular/common/http';

// Importing 'inject' and 'Injectable' from Angular's @angular/core package.
// 'inject' allows services to inject dependencies directly.
// 'Injectable' is used to mark the class as a service that can be injected into other components or services.
import { inject, Injectable, signal } from '@angular/core';
import { User } from '../_models/user';
import { map } from 'rxjs';

@Injectable({
  // 'providedIn: root' means this service is provided at the root level of the application.
  // This makes the service a singleton, meaning there will be only one instance of it across the app.
  providedIn: 'root'
})
export class AccountsService {

  // Injecting HttpClient as a dependency using the 'inject' method.
  // This allows the service to use HttpClient to make HTTP requests.
  private http = inject(HttpClient);

  // Base URL for the API endpoint.
  // This URL is used as the base for constructing full API URLs when making HTTP requests.
  baseUrl = 'https://localhost:5001/api/'

  // A Signal is a wrapper around a value that notifies interested consumers when that value changes.
  // Signals can contain any value, from primitives to complex data structures.
  // Signals are getter functions and calling them reads their value
  // This signal is used to store current user object
  currentUser = signal<User | null>(null);

  // Method to handle user login.
  // The 'model' parameter contains login credentials (typically a username and password).
  // The method sends a POST request to the API endpoint at 'account/login'.
  // The 'model' is sent in the body of the POST request.
  // The 'login' returns an observable
  login(model: any){
    // The 'this.http.post' method is used to send the HTTP POST request.
    // 'this.baseUrl + 'account/login'' constructs the full URL for the login API endpoint.
    // The 'model' is passed as the request body (this usually contains username and password).
    // The return value is an observable that will emit the response from the API.
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map(user => {
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUser.set(user);
        }
      })
    )
  }

  register(model: any){
    return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
      map(user => {
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUser.set(user);
        }
        return user;
      })
    )
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUser.set(null);
  }
}
