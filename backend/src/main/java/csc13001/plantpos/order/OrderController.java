package csc13001.plantpos.order;

import csc13001.plantpos.order.dtos.CreateOrderDTO;
import csc13001.plantpos.order.models.Order;
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
            return HttpResponse.badRequest(bindingResult);
        }

        Order order = orderService.createOrder(createOrderDTO);
        return HttpResponse.ok("Order created successfully", order);
    }

    @GetMapping("/{orderId}")
    public ResponseEntity<?> getOrder(@PathVariable Long orderId) {
        Order order = orderService.getOrderById(orderId);
        return HttpResponse.ok("Order retrieved successfully", order);
    }

    @GetMapping("/customer/{customerId}")
    public ResponseEntity<?> getOrdersByCustomerId(@PathVariable Long customerId) {
        List<Order> orders = orderService.getOrdersByCustomerId(customerId);
        return HttpResponse.ok("Orders retrieved successfully for customer", orders);
    }

    @GetMapping("/staff/{staffId}")
    public ResponseEntity<?> getOrdersByStaffId(@PathVariable Long staffId) {
        List<Order> orders = orderService.getOrdersByStaffId(staffId);
        return HttpResponse.ok("Orders retrieved successfully for staff", orders);
    }

    @DeleteMapping("/{orderId}")
    public ResponseEntity<?> deleteOrder(@PathVariable Long orderId) {
        orderService.deleteOrder(orderId);
        return HttpResponse.ok("Order deleted successfully");
    }
}
