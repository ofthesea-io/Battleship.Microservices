import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Authentication } from './core/utilities/authentication';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  constructor(private router: Router, private auth: Authentication) {}

  ngOnInit() {
    this.router.navigateByUrl('/login');
  }
}
