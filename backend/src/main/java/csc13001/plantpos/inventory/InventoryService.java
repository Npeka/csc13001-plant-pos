package csc13001.plantpos.inventory;

import csc13001.plantpos.inventory.dtos.InventoryDTO;
import csc13001.plantpos.inventory.dtos.InventoryDTO.InventoryItemDTO;
import csc13001.plantpos.inventory.exception.InventoryException;
import csc13001.plantpos.inventory.models.Inventory;
import csc13001.plantpos.inventory.models.InventoryItem;
import csc13001.plantpos.product.Product;
import csc13001.plantpos.product.ProductRepository;
import csc13001.plantpos.product.exception.ProductException;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Propagation;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.util.List;

@Service
@RequiredArgsConstructor
public class InventoryService {
    private final InventoryRepository inventoryRepository;
    private final InventoryItemRepository inventoryItemRepository;
    private final ProductRepository productRepository;

    public List<Inventory> getAllInventories() {
        return inventoryRepository.findAll();
    }

    @Transactional
    public Inventory createInventory(InventoryDTO inventoryDTO) {
        Inventory inventory = Inventory.builder()
                .supplier(inventoryDTO.getSupplier())
                .totalPrice(inventoryDTO.getTotalPrice())
                .purchaseDate(inventoryDTO.getPurchaseDate())
                .build();
        inventory = inventoryRepository.save(inventory);

        List<InventoryItemDTO> inventoryItemsDTO = inventoryDTO.getItems();
        BigDecimal totalPrice = BigDecimal.ZERO;

        for (InventoryItemDTO inventoryItemDTO : inventoryItemsDTO) {
            // check if product exists
            Product product = productRepository.findById(inventoryItemDTO.getProductId())
                    .orElseThrow(ProductException.ProductNotFoundException::new);

            // create inventory item
            InventoryItem inventoryItem = InventoryItem.builder()
                    .inventory(inventory)
                    .product(product)
                    .purchasePrice(inventoryItemDTO.getPurchasePrice())
                    .quantity(inventoryItemDTO.getQuantity())
                    .remainingQuantity(inventoryItemDTO.getQuantity())
                    .build();
            inventoryItemRepository.save(inventoryItem);

            // update stock
            product.setStock(product.getStock() + inventoryItem.getQuantity());
            productRepository.save(product);

            // calculate total price
            totalPrice = totalPrice
                    .add(inventoryItem.getPurchasePrice().multiply(BigDecimal.valueOf(inventoryItem.getQuantity())));
        }

        // update inventory
        inventory.setTotalPrice(totalPrice);
        return inventoryRepository.save(inventory);
    }

    @Transactional(propagation = Propagation.REQUIRES_NEW)
    public void updateStock(Product product, int quantityChange) {
        List<InventoryItem> inventoryItems;
        if (quantityChange > 0) {
            inventoryItems = inventoryItemRepository.findOldestInventoryItem(product.getProductId())
                    .orElseThrow(InventoryException.InventoryNotFoundException::new);
        } else {
            inventoryItems = inventoryItemRepository.findLatestInventoryItem(product.getProductId())
                    .orElseThrow(InventoryException.InventoryNotFoundException::new);
        }

        int remaining = quantityChange;

        for (InventoryItem inventoryItem : inventoryItems) {
            if (inventoryItem.getProduct().getProductId().equals(product.getProductId())) {
                int availableStock = inventoryItem.getRemainingQuantity();
                if (availableStock >= remaining) {
                    inventoryItem.setRemainingQuantity(availableStock - remaining);
                    remaining = 0;
                } else {
                    remaining -= availableStock;
                    inventoryItem.setRemainingQuantity(0);
                }
                inventoryItemRepository.save(inventoryItem);
            }
        }

        if (remaining > 0) {
            throw new InventoryException.InventoryNotEnoughStockException();
        }

        product.setStock(product.getStock() - quantityChange);
        productRepository.save(product);
    }

}
