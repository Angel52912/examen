# TAREA: EJERCICIO 3 - CONSULTA DE HISTORIAL DE VENTAS POR RANGO DE FECHAS

## REQUERIMIENTOS TÉCNICOS:
* **Capa:** UI (Consola) e Interfaz de Usuario (UX).
* **Objetivo:** Consultar cuánto se ha vendido en periodos específicos.
* **Captura de Datos:** Solicitar 'Fecha de Inicio' y 'Fecha de Fin' validando con 'DateTime.TryParse'.
* **Orquestación:** Consumir el caso de uso 'IFetchSalesByFilterUseCase' enviando un objeto 'SaleFilter'.
* **Restricción de Arquitectura:** Crear un 'IServiceScope' manual para cada ciclo de ejecución en la consola.

---
Genera el código en C# 14 (.NET 10) mostrando los resultados en una tabla formateada con Folio, Fecha y Monto Total.