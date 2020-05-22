import { Component, ComponentFactoryResolver } from "@angular/core";
import { Observable } from "rxjs";
import { AuthenticationService } from "src/app/core/services/authentication.service";

@Component({
  selector: "app-navigation",
  templateUrl: "./navigation.component.html",
  styleUrls: ["./navigation.component.css"]
})
export class NavigationComponent {

  isDemoAccount: boolean;
  isAuthenticated: boolean;
  authenticationService: AuthenticationService;

  constructor(authenticationService: AuthenticationService) {
    this.authenticationService = authenticationService;
    authenticationService.isPlayerAuthenticated().subscribe((result) => {
        if (result != null) {
          this.isAuthenticated = true;
          this.isDemoAccount = result.isDemoAccount;
        } else {
          this.isAuthenticated = false;
        }
      }
    );
  }

  onLogoutClicked(): void {
    this.authenticationService.logout();
  }
}
