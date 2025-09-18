// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $('#propietariosTable').DataTable({
        "columnDefs": [
            { "orderable": false, "targets": 7 },
            { "width": "150px", "targets": 7 }
        ],
        "language": {
            "emptyTable": "No hay datos",
            "info": "Mostrando _START_ a _END_ de _TOTAL_ Propietarios",
            "infoEmpty": "Mostrando 0 a 0 de 0 Propietarios",
            "infoFiltered": "(Filtrado de _MAX_ total Propietarios)",
            "search": "Buscador:",
            "zeroRecords": "Sin resultados encontrados",
            "lengthMenu": "Mostrar _MENU_ Propietarios",
            "paginate": {
                "first": "Primero",
                "last": "Ultimo",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        }
    });

    $('#inquilinosTable').DataTable({
        "columnDefs": [
            { "orderable": false, "targets": 6 },
            { "width": "150px", "targets": 6 }
        ],
        "language": {
            "emptyTable": "No hay datos",
            "info": "Mostrando _START_ a _END_ de _TOTAL_ Inquilinos",
            "infoEmpty": "Mostrando 0 a 0 de 0 Inquilinos",
            "infoFiltered": "(Filtrado de _MAX_ total Inquilinos)",
            "search": "Buscador:",
            "zeroRecords": "Sin resultados encontrados",
            "lengthMenu": "Mostrar _MENU_ Inquilinos",
            "paginate": {
                "first": "Primero",
                "last": "Ultimo",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        }
    });

    $('#inmueblesTable').DataTable({
        "columnDefs": [
            { "orderable": false, "targets": 9 },
            { "width": "150px", "targets": 9 }
        ],
        "language": {
            "emptyTable": "No hay datos",
            "info": "Mostrando _START_ a _END_ de _TOTAL_ Inmuebles",
            "infoEmpty": "Mostrando 0 a 0 de 0 Inmuebles",
            "infoFiltered": "(Filtrado de _MAX_ total Inmuebles)",
            "search": "Buscador:",
            "zeroRecords": "Sin resultados encontrados",
            "lengthMenu": "Mostrar _MENU_ Inmuebles",
            "paginate": {
                "first": "Primero",
                "last": "Ultimo",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        }
    });

    $('#contratosTable').DataTable({
        "columnDefs": [
            { "orderable": false, "targets": -1 },
            { "width": "150px", "targets": -1 }
        ],
        "language": {
            "emptyTable": "No hay datos",
            "info": "Mostrando _START_ a _END_ de _TOTAL_ Contratos",
            "infoEmpty": "Mostrando 0 a 0 de 0 Contratos",
            "infoFiltered": "(Filtrado de _MAX_ total Contratos)",
            "search": "Buscador:",
            "zeroRecords": "Sin resultados encontrados",
            "lengthMenu": "Mostrar _MENU_ Contratos",
            "paginate": {
                "first": "Primero",
                "last": "Ultimo",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        }
    });

    
});

document.addEventListener('DOMContentLoaded', function() {
    const inicio = document.getElementById('inicio');
    const fin = document.getElementById('fin');

    // Solo ejecutar si existen los elementos en la vista
    if (inicio && fin) {
        // Actualizar el min del campo "Hasta" cuando se selecciona una fecha en "Desde"
        inicio.addEventListener('change', function() {
            var inicioValue = inicio.value;

            if (inicioValue) {
                // Convertir el valor de "Desde" a una fecha de JavaScript
                var fechaInicio = new Date(inicioValue);
                
                // Sumar 1 día
                fechaInicio.setDate(fechaInicio.getDate() + 1);

                // Formatear la nueva fecha en formato YYYY-MM-DD
                var fechaMin = fechaInicio.toISOString().split('T')[0];

                // Establecer el min en el campo "Hasta" para que sea al menos la fecha de "Desde" + 1
                fin.setAttribute('min', fechaMin);
            } else {
                // Si "Desde" está vacío, eliminar el min de "Hasta"
                fin.removeAttribute('min');
            }
        });


        // Limpiar fechas
        document.getElementById('sinFechasBtn').addEventListener('click', function() {
            inicio.value = '';
            fin.value = '';
            fin.removeAttribute('min'); // Limpiar el min cuando se limpian los campos
        });
    }
});