import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { Auth } from "./core/utilities/auth";

@Component({
    selector: "app-root",
    templateUrl: "./app.component.html",
    styleUrls: ["./app.component.css"]
})
export class AppComponent implements OnInit {

    private auth: Auth;
    isAuthenticated: boolean;

    constructor(private router: Router) {
        this.auth = new Auth();
    }

    ngOnInit() {
        this.router.navigateByUrl("/login");
        this.getPlayerStatus();
    }

    onExitGame() {
        this.auth.removeAuthHeader();
        this.isAuthenticated = false;
        this.router.navigateByUrl("/login");
    }

    private getPlayerStatus() {
        this.isAuthenticated = false;
        const token = this.auth.isAuthenticated();
        if (token) {
            this.isAuthenticated = true;
        }
    }
}