import { Component } from '@angular/core';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  username: string = '';
  password: string = '';

  login() {
    // Aquí puedes agregar la lógica para enviar los datos de inicio de sesión al servidor
    console.log('Username:', this.username);
    console.log('Password:', this.password);
  }
}