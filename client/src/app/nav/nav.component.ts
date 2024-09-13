// Importing necessary Angular modules and components.
import { Component, inject } from '@angular/core'; // Importing 'Component' for creating a new Angular component and 'inject' for dependency injection.
import { FormsModule } from '@angular/forms'; // Importing 'FormsModule' to use Angular forms functionality like two-way data binding.
import { AccountsService } from '../_services/accounts.service'; // Importing 'AccountsService' to handle user login and other account-related services.
import { BsDropdownModule } from 'ngx-bootstrap/dropdown'; // Importing 'BsDropdownModule' from 'ngx-bootstrap' to use Bootstrap-styled dropdown functionality.

@Component({
  // Component metadata
  selector: 'app-nav', // This defines the custom HTML tag for this component. It can be used as <app-nav> in other templates.
  standalone: true, // The component is a standalone component, meaning it can be used independently without needing to be declared in a module.
  
  // Declaring the external modules required by this component.
  imports: [FormsModule, BsDropdownModule], // Importing 'FormsModule' for form handling and 'BsDropdownModule' to use Bootstrap's dropdown functionality in the template.
  
  // Path to the HTML template and CSS file for this component.
  templateUrl: './nav.component.html', // Path to the HTML file that defines the structure and layout of the component's UI.
  styleUrl: './nav.component.css' // Path to the CSS file that defines the styles for this component.
})
export class NavComponent {
  
  // Dependency Injection
  // Using Angular's 'inject' function to inject the 'AccountsService' instance into this component.
  // 'AccountsService' contains methods for handling user login and authentication.
  accountService = inject(AccountsService)

  // 'model' is an object used to hold form data (e.g., login credentials such as username and password).
  // It will be bound to the input fields in the HTML template using Angular's two-way binding.
  model: any = {};

  // 'login' method handles the login logic when the user submits the login form.
  // It sends a POST request to the server through 'AccountsService.login()' and listens for the response.
  login() {
    // 'this.accountService.login(this.model)' sends the login data (contained in 'model') to the backend API.
    // 'subscribe' is used to handle the response asynchronously.
    this.accountService.login(this.model).subscribe({
      
      // 'next' is called when the login is successful, receiving the response from the server.
      // 'response' contains the server's response, typically authentication tokens or user data.
      // Once login is successful, 'loggedIn' is set to 'true' to indicate the user is now logged in.
      next: response => {
        console.log(response); // Logging the response to the console for debugging purposes.
      },
      
      // 'error' is called if there's an error in the login request, such as incorrect credentials.
      // The error is logged to the console for debugging purposes.
      error: error => console.log(error)
    })
  }

  // 'logout' method sets the 'loggedIn' flag to 'false' when the user logs out.
  // This can be called when the user clicks a "Logout" button in the template.
  logout() {
    this.accountService.logout(); // Marking the user as logged out.
  }
}
