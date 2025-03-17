package csc13001.plantpos.adapters.controllers;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.validation.BindingResult;
import org.springframework.validation.annotation.Validated;
import org.springframework.web.bind.annotation.*;
import csc13001.plantpos.domain.models.Inventory;
import csc13001.plantpos.utils.http.HttpResponse;
import csc13001.plantpos.application.services.InventoryService;
import java.util.List;

@RestController
@RequestMapping("/api/inventory")
public class InventoryController {

    @Autowired
    private InventoryService inventoryService;

    @GetMapping
    public ResponseEntity<?> getAllInventoryItems() {
        List<Inventory> inventoryItems = inventoryService.getAllInventoryItems();
        return HttpResponse.ok("Get all inventory items successful", inventoryItems);
    }

    @PostMapping
    public ResponseEntity<?> createInventoryItem(
            @RequestBody @Validated Inventory inventory,
            BindingResult bindingResult) {
        if (bindingResult.hasErrors()) {
            return HttpResponse.badRequest(bindingResult);
        }
        Inventory createdInventory = inventoryService.createInventoryItem(inventory);
        return HttpResponse.ok("Create inventory item successful", createdInventory);
    }

    @GetMapping("/{id}")
    public ResponseEntity<?> getInventoryItemById(@PathVariable Long id) {
        Inventory inventory = inventoryService.getInventoryItemById(id);
        if (inventory == null) {
            return HttpResponse.notFound("Inventory item not found");
        }
        return HttpResponse.ok("Get inventory item successful", inventory);
    }

    @PutMapping("/{id}")
    public ResponseEntity<?> updateInventoryItem(
            @PathVariable Long id,
            @RequestBody @Validated Inventory inventoryDetails,
            BindingResult bindingResult) {
        if (bindingResult.hasErrors()) {
            return HttpResponse.badRequest(bindingResult);
        }
        Inventory inventory = inventoryService.updateInventoryItem(id, inventoryDetails);
        if (inventory == null) {
            return HttpResponse.notFound("Inventory item not found");
        }
        return HttpResponse.ok("Update inventory item successful");
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<?> deleteInventoryItem(@PathVariable Long id) {
        inventoryService.deleteInventoryItem(id);
        return HttpResponse.ok("Delete inventory item successful");
    }
}
