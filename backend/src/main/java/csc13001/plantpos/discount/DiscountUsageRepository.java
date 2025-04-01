package csc13001.plantpos.discount;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import csc13001.plantpos.discount.models.DiscountUsage;

@Repository
public interface DiscountUsageRepository extends JpaRepository<DiscountUsage, Long> {
    boolean existsByCustomer_CustomerIdAndDiscountProgram_DiscountId(Long customerId, Long discountId);

    List<DiscountUsage> findByCustomer_Phone(String phone);
}
