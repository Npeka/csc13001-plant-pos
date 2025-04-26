package csc13001.plantpos.customer;

import csc13001.plantpos.customer.dtos.CustomerDTO;
import csc13001.plantpos.customer.exception.CustomerException;
import csc13001.plantpos.order.OrderRepository;
import csc13001.plantpos.order.models.Order;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.util.List;

@Service
@RequiredArgsConstructor
public class CustomerService {
    private final CustomerRepository customerRepository;
    private final OrderRepository orderRepository;

    public List<CustomerDTO> getAllCustomers() {
        return customerRepository.findAll().stream()
                .map(customer -> {
                    List<Order> orders = orderRepository.findByCustomer_CustomerId(customer.getCustomerId());
                    return new CustomerDTO(customer, orders.size(),
                            orders.stream().map(Order::getFinalPrice).reduce(BigDecimal.ZERO, BigDecimal::add));
                })
                .toList();
    }

    public Customer createCustomer(Customer customer) {
        if (customerRepository.existsByPhone(customer.getPhone())) {
            throw new CustomerException.CustomerPhoneExistsException();
        }
        if (customerRepository.existsByEmail(customer.getEmail())) {
            throw new RuntimeException("Email already exists");
        }
        return customerRepository.save(customer);
    }

    public CustomerDTO getCustomerById(Long id) {
        Customer customer = customerRepository.findById(id)
                .orElseThrow(CustomerException.CustomerNotFoundException::new);

        List<Order> orders = orderRepository.findByCustomer_CustomerId(customer.getCustomerId());
        return new CustomerDTO(customer, orders.size(),
                orders.stream().map(Order::getFinalPrice).reduce(BigDecimal.ZERO, BigDecimal::add));
    }

    public void updateCustomer(Customer customer) {
        Customer existingCustomer = customerRepository.findById(customer.getCustomerId())
                .orElseThrow(CustomerException.CustomerNotFoundException::new);

        existingCustomer.setName(customer.getName());
        if (existingCustomer.getEmail() != null && !existingCustomer.getEmail().equals(customer.getEmail())) {
            if (customerRepository.existsByEmail(customer.getEmail())) {
                throw new RuntimeException("Email đã tồn tại");
            }
            existingCustomer.setEmail(customer.getEmail());
        }
        if (existingCustomer.getPhone() != null && !existingCustomer.getPhone().equals(customer.getPhone())) {
            if (customerRepository.existsByPhone(customer.getPhone())) {
                throw new CustomerException.CustomerPhoneExistsException();
            }
            existingCustomer.setPhone(customer.getPhone());
        }
        existingCustomer.setGender(customer.getGender());
        existingCustomer.setAddress(customer.getAddress());

        customerRepository.save(existingCustomer);
    }

    public void deleteCustomer(Long id) {
        if (!customerRepository.existsById(id)) {
            throw new CustomerException.CustomerNotFoundException();
        }
        try {
            customerRepository.deleteById(id);
        } catch (Exception e) {
            throw new RuntimeException("Không thể xóa khách hàng này vì có đơn hàng liên quan", e);
        }
    }
}
