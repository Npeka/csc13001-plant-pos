package csc13001.plantpos.application.services;

import csc13001.plantpos.adapters.repositories.InventoryRepository;
import csc13001.plantpos.domain.models.Inventory;
import org.springframework.stereotype.Service;

@Service
public class InventoryService {
    private final InventoryRepository inventoryRepository;

    public InventoryService(InventoryRepository inventoryRepository) {
        this.inventoryRepository = inventoryRepository;
    }

    public void updateStock(Long productId, int quantity) {
//        Inventory inventory = inventoryRepository.findByProductId(productId).orElseThrow();
//        inventory.decreaseStock(quantity);
//        inventoryRepository.save(inventory);
    }
}
