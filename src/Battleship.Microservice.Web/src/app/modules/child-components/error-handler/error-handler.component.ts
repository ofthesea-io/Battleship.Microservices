import { Component, OnInit, Input } from '@angular/core';

@Component({
    selector: 'app-error-handler',
    templateUrl: './error-handler.component.html',
})
export class ErrorHandlerComponent {
    @Input()
    errorMessageHandler: string;

    error: string;

    constructor() {
        this.error = this.errorMessageHandler;
    }
}
