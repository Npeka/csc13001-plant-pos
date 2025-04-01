package csc13001.plantpos.customer;

import csc13001.plantpos.customer.dtos.CustomerDTO;
import csc13001.plantpos.utils.http.HttpResponse;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.validation.BindingResult;
import org.springframework.validation.annotation.Validated;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/api/customers")
public class CustomerController {

    @Autowired
    private CustomerService customerService;

    @GetMapping
    public ResponseEntity<?> getAllCustomers() {
        List<CustomerDTO> customers = customerService.getAllCustomers();
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
        CustomerDTO customer = customerService.getCustomerById(id);
        return HttpResponse.ok("Get customer successful", customer);
    }

    @PutMapping("/{id}")
    public ResponseEntity<?> updateCustomer(
            @PathVariable Long customerId,
            @RequestBody @Validated Customer customer,
            BindingResult bindingResult) {
        if (bindingResult.hasErrors()) {
            return HttpResponse.badRequest(bindingResult);
        }
        customer.setCustomerId(customerId);
        customerService.updateCustomer(customer);
        return HttpResponse.ok("Cập nhật thông tin khách hàng thành công");
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<?> deleteCustomer(@PathVariable Long id) {
        customerService.deleteCustomer(id);
        return HttpResponse.ok("Xóa khách hàng thành công");
    }
}
