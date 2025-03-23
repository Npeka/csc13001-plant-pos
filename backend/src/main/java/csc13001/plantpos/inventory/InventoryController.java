package csc13001.plantpos.inventory;

import csc13001.plantpos.inventory.dtos.InventoryDTO;
import csc13001.plantpos.inventory.models.Inventory;
import csc13001.plantpos.utils.http.HttpResponse;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.validation.BindingResult;
import org.springframework.validation.annotation.Validated;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/api/inventories")
public class InventoryController {

    @Autowired
    private InventoryService inventoryService;

    @GetMapping
    public ResponseEntity<?> getAllInventoryItems() {
        List<Inventory> inventories = inventoryService.getAllInventories();
        return HttpResponse.ok("Get all inventory items successful", inventories);
    }

    @PostMapping
    public ResponseEntity<?> createInventoryItem(
            @RequestBody @Validated InventoryDTO inventoryDTO,
            BindingResult bindingResult) {
        if (bindingResult.hasErrors()) {
            return HttpResponse.badRequest(bindingResult);
        }
        Inventory createdInventory = inventoryService.createInventory(inventoryDTO);
        return HttpResponse.ok("Create inventory item successful", createdInventory);
    }
}
