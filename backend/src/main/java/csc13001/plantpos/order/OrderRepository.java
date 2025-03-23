package csc13001.plantpos.order;

import csc13001.plantpos.order.models.Order;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.time.LocalDateTime;
import java.util.List;

@Repository
public interface OrderRepository extends JpaRepository<Order, Long> {
    List<Order> findByCustomer_CustomerId(Long customerId);

    List<Order> findByStaff_UserId(Long userId);

    List<Order> findAllByOrderDateBetween(LocalDateTime startDate, LocalDateTime endDate);
}
