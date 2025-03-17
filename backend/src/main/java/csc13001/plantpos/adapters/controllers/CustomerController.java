package csc13001.plantpos.adapters.controllers;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.validation.BindingResult;
import org.springframework.validation.annotation.Validated;
import org.springframework.web.bind.annotation.*;
import csc13001.plantpos.domain.models.Customer;
import csc13001.plantpos.utils.http.HttpResponse;
import csc13001.plantpos.application.services.CustomerService;
import java.util.List;

@RestController
@RequestMapping("/api/customers")
public class CustomerController {

    @Autowired
    private CustomerService customerService;

    @GetMapping
    public ResponseEntity<?> getAllCustomers() {
        List<Customer> customers = customerService.getAllCustomers();
        return HttpResponse.ok("Get all customers successful", customers);
    }

    @PostMapping
    public ResponseEntity<?> createCustomer(
            @RequestBody @Validated Customer customer,
            BindingResult bindingResult) {
        if (bindingResult.hasErrors()) {
            return HttpResponse.badRequest(bindingResult);
        }
        Customer createdCustomer = customerService.createCustomer(customer);
        return HttpResponse.ok("Create customer successful", createdCustomer);
    }

    @GetMapping("/{id}")
    public ResponseEntity<?> getCustomerById(@PathVariable Long id) {
        Customer customer = customerService.getCustomerById(id);
        if (customer == null) {
            return HttpResponse.notFound("Customer not found");
        }
        return HttpResponse.ok("Get customer successful", customer);
    }

    @PutMapping("/{id}")
    public ResponseEntity<?> updateCustomer(
            @PathVariable Long id,
            @RequestBody @Validated Customer customerDetails,
            BindingResult bindingResult) {
        if (bindingResult.hasErrors()) {
            return HttpResponse.badRequest(bindingResult);
        }
        Customer customer = customerService.updateCustomer(id, customerDetails);
        if (customer == null) {
            return HttpResponse.notFound("Customer not found");
        }
        return HttpResponse.ok("Update customer successful");
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<?> deleteCustomer(@PathVariable Long id) {
        customerService.deleteCustomer(id);
        return HttpResponse.ok("Delete customer successful");
    }
}
