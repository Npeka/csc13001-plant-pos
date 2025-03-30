package csc13001.plantpos.discount;

import csc13001.plantpos.customer.Customer;
import csc13001.plantpos.customer.CustomerRepository;
import csc13001.plantpos.customer.exception.CustomerException;
import csc13001.plantpos.discount.exception.DiscountException;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

import java.util.Date;
import java.util.List;

@Service
@RequiredArgsConstructor
public class DiscountProgramService {
    private final DiscountProgramRepository discountProgramRepository;
    private final DiscountUsageRepository discountUsageRepository;
    private final CustomerRepository customerRepository;

    public List<DiscountProgram> getDiscountPrograms() {
        return discountProgramRepository.findAll();
    }

    public List<DiscountProgram> getDiscountProgramsByCustomerPhone(String phone) {
        Customer customer = customerRepository.findByPhone(phone)
                .orElseThrow(CustomerException.CustomerNotFoundException::new);

        List<DiscountProgram> discountPrograms = discountProgramRepository
                .findAll().stream()
                .filter(discountProgram -> discountProgram.isActive()
                        && customer.getLoyaltyCardType()
                                .isHigherThanOrEqualTo(discountProgram.getApplicableCustomerType())
                        && !discountUsageRepository.existsByCustomer_CustomerIdAndDiscountProgram_DiscountId(
                                customer.getCustomerId(),
                                discountProgram.getDiscountId()))
                .toList();

        return discountPrograms;
    }

    public DiscountProgram createDiscountProgram(DiscountProgram discountProgram) {
        Date startDate = discountProgram.getStartDate();
        Date endDate = discountProgram.getEndDate();
        if (startDate.compareTo(endDate) > 0) {
            throw new DiscountException.DiscountInvalidDateException();
        }
        return discountProgramRepository.save(discountProgram);
    }

    public DiscountProgram getDiscountProgramById(Long id) {
        return discountProgramRepository.findById(id)
                .orElseThrow(DiscountException.DiscountNotFoundException::new);
    }

    public void updateDiscountProgram(Long id, DiscountProgram discountProgram) {
        DiscountProgram existingDiscount = discountProgramRepository.findById(id)
                .orElseThrow(DiscountException.DiscountNotFoundException::new);

        Date startDate = discountProgram.getStartDate();
        Date endDate = discountProgram.getEndDate();
        if (startDate.compareTo(endDate) > 0) {
            throw new DiscountException.DiscountInvalidDateException();
        }

        discountProgramRepository.save(existingDiscount);
    }

    public void deleteDiscountProgram(Long id) {
        if (!discountProgramRepository.existsById(id)) {
            throw new DiscountException.DiscountNotFoundException();
        }
        discountProgramRepository.deleteById(id);
    }

}
