package csc13001.plantpos.adapters.repositories;

import csc13001.plantpos.domain.models.DiscountProgram;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface DiscountProgramRepository extends JpaRepository<DiscountProgram, Long> {
}
