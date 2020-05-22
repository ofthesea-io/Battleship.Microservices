import { Component, OnInit, ViewEncapsulation } from "@angular/core";
import { Router } from "@angular/router";
import { Observable } from "rxjs";
import { AuthenticationService } from "./core/services/authentication.service";

@Component({
    selector: "app-root",
    templateUrl: "./app.component.html",
    styleUrls: ["./app.component.css"],
    encapsulation: ViewEncapsulation.None
})
export class AppComponent implements OnInit {

    isAuthenticated: Observable<boolean>;
    router: Router;

    /// Don't pass in router as private
    constructor(router: Router, private auth: AuthenticationService) {
        this.router = router;
    }

    ngOnInit() {
        this.router.navigateByUrl("/login");
        this.isAuthenticated = this.auth.isPlayerAuthenticated();
    }

    onExitGame(): void {
        this.auth.removeAuthHeader();
        this.router.navigateByUrl("/login");
    }
}
