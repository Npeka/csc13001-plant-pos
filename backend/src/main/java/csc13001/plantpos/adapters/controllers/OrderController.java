package csc13001.plantpos.adapters.controllers;

import csc13001.plantpos.application.dtos.order.CreateOrderDTO;
import csc13001.plantpos.application.dtos.order.UpdateOrderDTO;
import csc13001.plantpos.application.services.OrderService;
import csc13001.plantpos.domain.models.Order;
import csc13001.plantpos.utils.http.HttpResponse;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.validation.BindingResult;
import org.springframework.validation.annotation.Validated;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/api/orders")
public class OrderController {

    @Autowired
    private OrderService orderService;

    @GetMapping()
    public ResponseEntity<?> getAllOrders() {
        List<Order> orders = orderService.getAllOrders();
        return HttpResponse.ok("Orders retrieved successfully", orders);
    }

    @PostMapping()
    public ResponseEntity<?> createOrder(
            @RequestBody @Validated CreateOrderDTO createOrderDTO,
            BindingResult bindingResult) {
        if (bindingResult.hasErrors()) {
            return HttpResponse.invalidInputData();
        }

        Order order = orderService.createOrder(createOrderDTO);
        return HttpResponse.ok("Order created successfully", order);
    }

    @GetMapping("/{orderId}")
    public ResponseEntity<?> getOrder(@PathVariable Long orderId) {
        Order order = orderService.getOrderById(orderId);
        return HttpResponse.ok("Order retrieved successfully", order);
    }

    @PutMapping("/{orderId}")
    public ResponseEntity<?> updateOrder(
            @RequestBody @Validated UpdateOrderDTO updateOrderDTO,
            BindingResult bindingResult) {
        if (bindingResult.hasErrors()) {
            return HttpResponse.invalidInputData();
        }

        Order order = orderService.updateOrder(updateOrderDTO);

        return HttpResponse.ok("Order updated successfully", order);
    }

    @DeleteMapping("/{orderId}")
    public ResponseEntity<?> deleteOrder(@PathVariable Long orderId) {
        orderService.deleteOrder(orderId);
        return HttpResponse.ok("Order deleted successfully");
    }
}
