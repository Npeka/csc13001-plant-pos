package csc13001.plantpos.discount;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import java.util.List;
import java.time.LocalDate;

@Repository
public interface DiscountProgramRepository extends JpaRepository<DiscountProgram, Long> {
    List<DiscountProgram> findByEndDate(LocalDate endDate);
}
