import { Component, OnInit, Input } from "@angular/core";

@Component({
    selector: "app-error-handler",
    templateUrl: "./error-handler.component.html",
})
export class ErrorHandlerComponent implements OnInit {
    @Input()
    errorMessageHandler: string;

    error: string;

    constructor() {}

    ngOnInit() {
        this.error = this.errorMessageHandler;
    }
}