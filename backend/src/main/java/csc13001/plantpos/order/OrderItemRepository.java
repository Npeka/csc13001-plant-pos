package csc13001.plantpos.order;

import csc13001.plantpos.order.models.OrderItem;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface OrderItemRepository extends JpaRepository<OrderItem, Long> {
    List<OrderItem> findByOrder_OrderId(Long orderId);

    List<OrderItem> findByProduct_ProductId(Long productId);
}
