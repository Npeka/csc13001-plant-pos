package csc13001.plantpos.discount;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface DiscountProgramRepository extends JpaRepository<DiscountProgram, Long> {
}
