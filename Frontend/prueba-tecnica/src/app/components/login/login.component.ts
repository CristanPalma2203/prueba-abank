import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  isLoggedIn = false;
  isLoginFailed = false;
  isBusy = false;
  errorMessage = '';
  roles: string[] = [];

  constructor(private authService: AuthService, private formBuilder: FormBuilder, private router: Router) {
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    if (this.authService.isLoggedIn()) {
      this.router.navigate(['/']);
    }
  }

  onSubmit(): void {
    const username = this.loginForm.value.username;
    const password = this.loginForm.value.password;

    this.isBusy = true;
    this.authService.login(username, password).subscribe({
      next: data => {
        console.log(data);
        this.authService.saveUser(data);
        this.isBusy = false;
        this.isLoginFailed = false;
        this.isLoggedIn = true;
        this.router.navigate(['/']);
      },
      error: err => {
        console.log(err);
        this.isBusy = false;
        this.errorMessage = err.error.message;
        this.isLoginFailed = true;
      }
    });
  }
}