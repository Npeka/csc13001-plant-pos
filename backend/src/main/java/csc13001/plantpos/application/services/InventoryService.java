package csc13001.plantpos.application.services;

import csc13001.plantpos.adapters.repositories.InventoryRepository;
import csc13001.plantpos.adapters.repositories.ProductRepository;
import csc13001.plantpos.domain.models.Inventory;
import csc13001.plantpos.domain.models.Product;
import csc13001.plantpos.exception.inventory.InventoryException;
import csc13001.plantpos.exception.product.ProductException;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Propagation;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;
import java.util.Optional;

@Service
public class InventoryService {

    @Autowired
    private InventoryRepository inventoryRepository;

    @Autowired
    private ProductRepository productRepository;

    public List<Inventory> getAllInventoryItems() {
        return inventoryRepository.findAll();
    }

    @Transactional
    public Inventory createInventoryItem(Inventory inventory) {
        Product product = productRepository.findById(inventory.getProduct().getProductId())
                .orElseThrow(ProductException.ProductNotFoundException::new);

        product.setStock(product.getStock() + inventory.getQuantity());
        productRepository.save(product);

        inventory.setProduct(product);
        return inventoryRepository.save(inventory);
    }

    public Inventory getInventoryItemById(Long id) {
        return inventoryRepository.findById(id)
                .orElseThrow(InventoryException.InventoryNotFoundException::new);
    }

    @Transactional(propagation = Propagation.REQUIRES_NEW)
    public void updateStock(Long productId, int quantityChange) {
        Product product = productRepository.findById(productId)
                .orElseThrow(ProductException.ProductNotFoundException::new);

        List<Inventory> inventories;
        if (quantityChange > 0) {
            inventories = inventoryRepository.findOldestInventory(productId)
                    .orElseThrow(InventoryException.InventoryNotFoundException::new);
        } else {
            inventories = inventoryRepository.findLatestInventory(productId)
                    .orElseThrow(InventoryException.InventoryNotFoundException::new);
        }

        int remaining = quantityChange;
        for (Inventory inventory : inventories) {
            int availableStock = inventory.getQuantity();
            if (availableStock >= remaining) {
                inventory.setQuantity(availableStock - remaining);
                remaining = 0;
            } else {
                remaining -= availableStock;
                inventory.setQuantity(0);
            }

            inventoryRepository.save(inventory);
        }

        if (remaining > 0) {
            throw new InventoryException.InventoryNotEnoughStockException();
        }

        product.setStock(product.getStock() - quantityChange);
        productRepository.save(product);
    }

    @Transactional
    public Inventory updateInventoryItem(Long id, Inventory inventoryDetails) {
        Inventory inventory = inventoryRepository.findById(id)
                .orElseThrow(InventoryException.InventoryNotFoundException::new);

        Product product = productRepository.findById(inventoryDetails.getProduct().getProductId())
                .orElseThrow(ProductException.ProductNotFoundException::new);

        int stockDiff = inventoryDetails.getQuantity() - inventory.getQuantity();
        if (stockDiff != 0) {
            product.setStock(product.getStock() + stockDiff);
            productRepository.save(product);
        }

        inventory.setSupplier(inventoryDetails.getSupplier());
        inventory.setQuantity(inventoryDetails.getQuantity());
        inventory.setPurchasePrice(inventoryDetails.getPurchasePrice());
        inventory.setPurchaseDate(inventoryDetails.getPurchaseDate());

        return inventoryRepository.save(inventory);
    }

    @Transactional
    public void deleteInventoryItem(Long id) {
        Inventory inventory = inventoryRepository.findById(id)
                .orElseThrow(InventoryException.InventoryNotFoundException::new);

        Product product = productRepository.findById(inventory.getProduct().getProductId())
                .orElseThrow(ProductException.ProductNotFoundException::new);

        product.setStock(product.getStock() - inventory.getQuantity());
        productRepository.save(product);

        inventoryRepository.deleteById(id);
    }
}
