import { Component } from '@angular/core';
import { EmpleadoService } from '../../services/empleado.service';

@Component({
  selector: 'app-empleado-list',
  templateUrl: './empleado-list.component.html',
  styleUrl: './empleado-list.component.css'
})
export class EmpleadoListComponent {
  empleados: any[] = []; // Arreglo para almacenar los datos de empleados

  constructor(private empleadoService: EmpleadoService) { }

  ngOnInit(): void {
    this.obtenerEmpleados(); // Llama al método para obtener empleados al inicializar el componente
  }

  obtenerEmpleados(): void {
    this.empleadoService.getEmpleados().subscribe(
      (data: any) => {
        this.empleados = data; // Asigna los datos obtenidos del servicio al arreglo de empleados
      },
      (error) => {
        console.error('Error al obtener los empleados:', error);
      }
    );
  }
}
