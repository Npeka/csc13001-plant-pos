package csc13001.plantpos.application.services;

import csc13001.plantpos.adapters.repositories.CustomerRepository;
import csc13001.plantpos.domain.models.Customer;
import csc13001.plantpos.exception.customer.CustomerException;

import org.springframework.stereotype.Service;
import java.util.List;

@Service
public class CustomerService {

    private final CustomerRepository customerRepository;

    public CustomerService(CustomerRepository customerRepository) {
        this.customerRepository = customerRepository;
    }

    public List<Customer> getAllCustomers() {
        return customerRepository.findAll();
    }

    public Customer createCustomer(Customer customer) {
        if (customerRepository.findByPhone(customer.getPhone()).isPresent()) {
            throw new CustomerException.CustomerExistsException();
        }
        return customerRepository.save(customer);
    }

    public Customer createCustomerIfNotExists(Customer customer) {
        return customerRepository.findByPhone(customer.getPhone()).orElseGet(() -> customerRepository.save(customer));
    }

    public Customer getCustomerById(Long id) {
        return customerRepository.findById(id).orElse(null);
    }

    public Customer updateCustomer(Long id, Customer customerDetails) {
        return customerRepository.findById(id)
                .map(existingCustomer -> {
                    existingCustomer.setName(customerDetails.getName());
                    existingCustomer.setEmail(customerDetails.getEmail());
                    return customerRepository.save(existingCustomer);
                })
                .orElse(null);
    }

    public void deleteCustomer(Long id) {
        if (!customerRepository.existsById(id)) {
            throw new CustomerException.CustomerNotFoundException();
        }
        customerRepository.deleteById(id);
    }
}
