package csc13001.plantpos.adapters.repositories;

import csc13001.plantpos.domain.models.Inventory;
import io.lettuce.core.dynamic.annotation.Param;

import java.util.List;
import java.util.Optional;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;

@Repository
public interface InventoryRepository extends JpaRepository<Inventory, Long> {
    @Query("SELECT i FROM Inventory i WHERE i.product.id = :productId ORDER BY i.purchaseDate ASC")
    Optional<List<Inventory>> findOldestInventory(@Param("productId") Long productId);

    @Query("SELECT i FROM Inventory i WHERE i.product.id = :productId ORDER BY i.purchaseDate DESC")
    Optional<List<Inventory>> findLatestInventory(@Param("productId") Long productId);
}
