# TAREA: EJERCICIO 2 - SISTEMA DE ALERTAS DE INVENTARIO CRÍTICO

## REQUERIMIENTOS TÉCNICOS:
* **Capa:** UseCases (Aplicación)
* **Objetivo:** Identificar productos cerca de agotarse para evitar pérdida de ventas.
* **Clase/Interfaz:** Interfaz 'ILowStockAlertUseCase' e implementación 'LowStockAlertUseCaseImpl'.
* **Detalle Clave:** El método debe recibir un entero 'threshold' y devolver un 'IAsyncEnumerable<Product>'.
* **Registro:** Configurar la inyección de dependencias en el archivo 'Program.cs'.

---
Genera el código en C# 14 (.NET 10) asegurando que el flujo sea asíncrono y eficiente en memoria para el proyecto UTM Market.