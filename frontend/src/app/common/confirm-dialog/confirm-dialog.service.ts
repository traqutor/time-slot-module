import {Injectable} from '@angular/core';
import {MatDialogRef, MatDialog} from '@angular/material';
import {Observable} from "rxjs";


import {ConfirmDialogComponent} from './confirm-dialog.component';


@Injectable({
  providedIn: 'root'
})
export class ConfirmDialogService {

  constructor(private dialog: MatDialog) {
  }

  public confirm(title: string, message: string): Observable<boolean> {

    let dialogRef: MatDialogRef<ConfirmDialogComponent>;

    dialogRef = this.dialog.open(ConfirmDialogComponent);

    dialogRef.componentInstance.title = title;
    dialogRef.componentInstance.message = message;

    return dialogRef.afterClosed();
  }
}
