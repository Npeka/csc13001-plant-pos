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
                            orders.stream().map(Order::getTotalPrice).reduce(BigDecimal.ZERO, BigDecimal::add));
                })
                .toList();
    }

    public Customer createCustomer(Customer customer) {
        if (customerRepository.existsByPhone(customer.getPhone())) {
            throw new CustomerException.CustomerPhoneExistsException();
        }
        return customerRepository.save(customer);
    }

    public Customer getCustomerById(Long id) {
        return customerRepository.findById(id)
                .orElseThrow(CustomerException.CustomerNotFoundException::new);
    }

    public void updateCustomer(Customer customer) {
        Customer existingCustomer = customerRepository.findByPhone(customer.getPhone())
                .orElseThrow(CustomerException.CustomerNotFoundException::new);

        existingCustomer.setName(customer.getName());
        existingCustomer.setEmail(customer.getEmail());
        existingCustomer.setGender(customer.getGender());

        customerRepository.save(existingCustomer);
    }

    public void deleteCustomer(Long id) {
        if (!customerRepository.existsById(id)) {
            throw new CustomerException.CustomerNotFoundException();
        }
        customerRepository.deleteById(id);
    }
}
