import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavComponent } from "./nav/nav.component";
import { AccountsService } from './_services/accounts.service';
import { HomeComponent } from "./home/home.component";

@Component({
  // The 'selector' property specifies the custom HTML tag for this component.
  // This tag can be used in the template of other components to include this component.
  // Here, 'app-root' means that wherever you see <app-root></app-root> in the HTML,
  // Angular will render the template associated with this component.
  selector: 'app-root',

  // The 'standalone' property indicates whether the component is a standalone component
  // that does not rely on Angular modules. If set to true, the component can be used
  // independently without being declared in an NgModule.
  standalone: true,

  // The 'imports' property allows you to specify other Angular features or modules
  // that this component requires. 'RouterOutlet' is a directive from Angular's
  // RouterModule that acts as a placeholder for dynamically loaded views
  // based on the current route.
  imports: [RouterOutlet, NavComponent, HomeComponent],

  // The 'templateUrl' property specifies the path to an external HTML file
  // that defines the view for this component. The content of this HTML file
  // will be displayed wherever this component is used.
  templateUrl: './app.component.html',

  // The 'styleUrls' property (note the correct property name should be 'styleUrls', not 'styleUrl')
  // is an array of paths to CSS files that contain styles for this component.
  // These styles are scoped to this component and will not affect other components.
  styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit {

  // Injection for making HTTP requests possible
  private accountService = inject(AccountsService);

  // This came from implements OnInit quick fix
  ngOnInit(): void {
    this.setCurrentUser();
  }

  setCurrentUser(){
    const userString = localStorage.getItem('user');
    if (!userString) return;
    const user =JSON.parse(userString);
    this.accountService.currentUser.set(user);
  }

  
}
