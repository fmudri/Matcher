import { Component, inject, OnInit } from '@angular/core';
import { RegisterComponent } from "../register/register.component";
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RegisterComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit{
  http = inject(HttpClient);
  registerMode = false;
  users: any;

  ngOnInit(): void {
    this.getUsers();
  }

  registerToggle(){
    this.registerMode = !this.registerMode
  }

  cancelRegisterMode(event: boolean) {
    this.registerMode = event;
  }
  
  getUsers() {
    // To make a new HTTP request we need to use this.class property
    this.http.get('https://localhost:5001/api/users').subscribe({
      // These are all callback functions
      // Response is the parametar, this.users = response(the thing I get back from my API server)
      next: response => this.users = response,

      // If I get an error, I can say error as the argument, then console.log and output the error
      error: error => console.log(error),

      // When we complete, we don't get anything back
      complete: () => console.log('Request has completed')
    })
  }
}
