package csc13001.plantpos.discount;

import csc13001.plantpos.customer.Customer;
import csc13001.plantpos.customer.CustomerRepository;
import csc13001.plantpos.customer.exception.CustomerException;
import csc13001.plantpos.discount.exception.DiscountException;
import lombok.RequiredArgsConstructor;

import org.springframework.context.ApplicationEventPublisher;
import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.stereotype.Service;

import java.time.LocalDate;
import java.util.List;

@Service
@RequiredArgsConstructor
public class DiscountProgramService {
    private final ApplicationEventPublisher eventPublisher;
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
        LocalDate startDate = discountProgram.getStartDate();
        LocalDate endDate = discountProgram.getEndDate();
        if (startDate.compareTo(endDate) > 0) {
            throw new DiscountException.DiscountInvalidDateException();
        }

        DiscountProgram savedDiscountProgram = discountProgramRepository.save(discountProgram);
        eventPublisher.publishEvent(savedDiscountProgram);
        return savedDiscountProgram;
    }

    public DiscountProgram getDiscountProgramById(Long id) {
        return discountProgramRepository.findById(id)
                .orElseThrow(DiscountException.DiscountNotFoundException::new);
    }

    public void updateDiscountProgram(DiscountProgram discountProgram) {
        if (!discountProgramRepository.existsById(discountProgram.getDiscountId())) {
            throw new DiscountException.DiscountNotFoundException();
        }

        LocalDate startDate = discountProgram.getStartDate();
        LocalDate endDate = discountProgram.getEndDate();
        if (startDate.compareTo(endDate) > 0) {
            throw new DiscountException.DiscountInvalidDateException();
        }

        discountProgramRepository.save(discountProgram);
    }

    public void deleteDiscountProgram(Long id) {
        if (!discountProgramRepository.existsById(id)) {
            throw new DiscountException.DiscountNotFoundException();
        }
        discountProgramRepository.deleteById(id);
    }

    // @Scheduled(cron = "0 0 0 * * *")
    @Scheduled(cron = "0 0 0 * * *")
    public void checkExpiredDiscounts() {
        LocalDate today = LocalDate.now().minusDays(1);
        List<DiscountProgram> expired = discountProgramRepository.findByEndDate(today);
        List<DiscountProgram> allDiscounts = discountProgramRepository.findAll();
        for (DiscountProgram discount : allDiscounts) {
            System.err.println(discount.getName() + " - " + discount.getEndDate());
        }
        for (DiscountProgram discount : expired) {
            eventPublisher.publishEvent(discount);
        }
    }
}
