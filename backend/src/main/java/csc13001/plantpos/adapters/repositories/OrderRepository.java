package csc13001.plantpos.adapters.repositories;

import org.springframework.stereotype.Repository;
import org.springframework.data.jpa.repository.JpaRepository;
import java.util.Optional;
import csc13001.plantpos.domain.models.Order;

@Repository
public interface OrderRepository extends JpaRepository<Order, Long> {
}
