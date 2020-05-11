import { HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { throwError } from 'rxjs';
import { ConfirmationDialogComponent } from 'src/app/modules/child-components/confirmation-dialog/confirmation-dialog.component';
import { MatDialogConfig, MatDialog, MatDialogRef } from '@angular/material';
import { Injectable } from '@angular/core';

@Injectable()
export class Configuration {
  shipCounter = [
    { Id: 9, option: 'One' },
    { Id: 8, option: 'Two' },
    { Id: 7, option: 'Three' },
    { Id: 6, option: 'Four' },
    { Id: 5, option: 'Five' },
    { Id: 4, option: 'Six' },
    { Id: 3, option: 'Seven' },
    { Id: 2, option: 'Eight' },
    { Id: 1, option: 'Nine' }
  ];

    readonly hit: string = 'You\'ve hit a ship!';
    readonly miss: string = 'Sorry you missed, please try again...';
    readonly sunk: string = 'You\'ve sunk a ship!';
    readonly tried: string = 'Coordinate already tried. Please try a different cell.';
    readonly incorrect: string = 'Incorrect input, please try again!';
    readonly completed: string = 'Game completed!';
    readonly close: string = 'Close';
    readonly start: string = 'Start Game';
    readonly applicationError: string = 'Application Error. Please contain your administrator!';
    readonly demoLoginError: string = 'No demo users found. Please create an account.';
    readonly gameNotStarted: string = 'To start the game, select your Battleship gaming level';
    readonly gameStarted: string = 'Let the games begin!';
    readonly demoAccountSaveError: string = 'Can\'t save demo account game!';
    readonly gameIncomplete: string = 'Continue previous game?';
    readonly somethingWentWrongError: string = 'Something went wrong, please try again';
    readonly serviceError: string = 'we are currently experiencing technical issues. We will have it issue resolved shortly!';

    readonly hitClass: string = 'col-1 bg-danger text-white border text-center';
    readonly missClass: string = 'col-1 bg-warning text-white border text-center';

    getHeaders(): HttpHeaders {
      const headers =  new HttpHeaders({
        'Content-Type': 'application/json'
      });
      return headers;
    }

    getAuthHeaders(): HttpHeaders {
      const authorization = localStorage.getItem('authToken');
      const authHeaders = new HttpHeaders({
        'Content-Type': 'application/json',
        Authorization: authorization
      });
      return authHeaders;
    }

    setAuthHeader(sessionToken: string): void  {
      localStorage.setItem('authDate', Date.now().toString());
      localStorage.setItem('authToken', sessionToken);
    }

    removeAuthHeader(sessionToken: string): void {
      localStorage.removeItem('authDate');
      localStorage.removeItem('authToken');
    }

    setGameCompleted(isCompleted: string) {
      localStorage.setItem('isCompleted', isCompleted);
    }

    getGameCompeted(): string {
      return localStorage.getItem('isCompleted');
    }

    getLastAuthDate(): string {
      return localStorage.getItem('authDate');
    }

    handleError(error: HttpErrorResponse) {
      if (error.error instanceof ErrorEvent) {
        console.error('An error occurred:', error.error.message);
      } else {
        console.error(`Backend returned code ${error.status}, ` + `body was: ${error.error}`);
      }
      return throwError('Error');
    }

    openDialog(dialog: MatDialog, message: string , buttonText: string ): MatDialogRef<ConfirmationDialogComponent, any> {
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
