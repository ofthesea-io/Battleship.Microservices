import { HttpErrorResponse } from "@angular/common/http";
import { throwError } from "rxjs";
import { MatDialogConfig, MatDialog, MatDialogRef } from "@angular/material";
import { Injectable } from "@angular/core";
import { ConfirmationDialogComponent } from "src/app/modules/child-components/confirmation-dialog/confirmation-dialog.component";

@Injectable()
export class Configuration {
    shipCounter = [
        { Id: 9, option: "One" },
        { Id: 8, option: "Two" },
        { Id: 7, option: "Three" },
        { Id: 6, option: "Four" },
        { Id: 5, option: "Five" },
        { Id: 4, option: "Six" },
        { Id: 3, option: "Seven" },
        { Id: 2, option: "Eight" },
        { Id: 1, option: "Nine" }
    ];

    readonly hit = "You've hit a ship!";
    readonly miss = "Sorry you missed, please try again...";
    readonly sunk = "You've sunk a ship!";
    readonly tried = "Coordinate already tried. Please try a different cell.";
    readonly incorrect = "Incorrect input, please try again!";
    readonly completed = "Game completed!";
    readonly close = "Close";
    readonly start = "Start Game";
    readonly loginFailed = "Unable to log you in.";
    readonly applicationError = "Application Error. Please contain your administrator!";
    readonly demoLoginError = "No demo users found. Please create an account.";
    readonly gameNotStarted = "To start the game, select your Battleship gaming level";
    readonly gameStarted = "Let the games begin!";
    readonly demoAccountSaveError = "Can't save demo account game!";
    readonly gameIncomplete = "Continue previous game?";
    readonly somethingWentWrongError = "Something went wrong, please try again";
    readonly serviceError = "we are currently experiencing technical issues. We will have it issue resolved shortly!";

    readonly hitClass = "col-1 bg-danger text-white border text-center";
    readonly missClass = "col-1 bg-warning text-white border text-center";

    handleError(error: HttpErrorResponse) {
        if (error.error instanceof ErrorEvent) {
            console.error("An error occurred:", error.error.message);
        } else {
            console.error(`Backend returned code ${error.status}, ` + `body was: ${error.error}`);
        }
        return throwError("Error");
    }

    openDialog(dialog: MatDialog, message: string, buttonText: string): MatDialogRef<ConfirmationDialogComponent, any> {
        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = true;
        dialogConfig.autoFocus = true;
        dialogConfig.hasBackdrop = true;
        dialogConfig.data = { message, buttonText };
        const dialogRef = dialog.open(
            ConfirmationDialogComponent,
            dialogConfig
        );
        return dialogRef;
    }
}
