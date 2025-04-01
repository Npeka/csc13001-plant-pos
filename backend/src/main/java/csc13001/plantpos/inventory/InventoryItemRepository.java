package csc13001.plantpos.inventory;

import csc13001.plantpos.inventory.models.InventoryItem;
import io.lettuce.core.dynamic.annotation.Param;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Optional;

@Repository
public interface InventoryItemRepository extends JpaRepository<InventoryItem, Long> {
    @Query("SELECT i FROM InventoryItem i WHERE i.product.productId = :productId ORDER BY i.inventory.purchaseDate ASC")
    Optional<List<InventoryItem>> findOldestInventoryItem(@Param("productId") Long productId);

    @Query("SELECT i FROM InventoryItem i WHERE i.product.productId = :productId ORDER BY i.inventory.purchaseDate DESC")
    Optional<List<InventoryItem>> findLatestInventoryItem(@Param("productId") Long productId);
}