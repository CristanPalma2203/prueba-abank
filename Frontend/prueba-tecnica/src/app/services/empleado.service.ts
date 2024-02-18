import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface Empleado {
  id: number;
  nombres: string;
  apellidos: string;
  telefono: string;
  correo: string;
  nombreDepartamento: string;
}

const EMPLEADOS_API = 'https://localhost:7170/Empleados';

@Injectable({
  providedIn: 'root'
})
export class EmpleadoService {

  constructor(private http: HttpClient) { }

  getEmpleados(): Observable<Empleado[]> {
    return this.http.get<Empleado[]>(EMPLEADOS_API);
  }
}
