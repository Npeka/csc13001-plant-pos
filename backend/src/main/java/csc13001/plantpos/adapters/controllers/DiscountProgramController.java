package csc13001.plantpos.adapters.controllers;

import csc13001.plantpos.application.services.DiscountProgramService;
import csc13001.plantpos.domain.models.DiscountProgram;
import csc13001.plantpos.utils.http.HttpResponse;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.validation.BindingResult;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/api/discounts")
public class DiscountProgramController {
    @Autowired
    private DiscountProgramService discountProgramService;

    @GetMapping
    public ResponseEntity<?> getDiscountPrograms() {
        List<DiscountProgram> discountPrograms = discountProgramService.getDiscountPrograms();
        return HttpResponse.ok("get discount programs successfully", discountPrograms);
    }

    @PostMapping
    public ResponseEntity<?> createDiscountProgram(
            @RequestBody DiscountProgram discountProgram,
            BindingResult bindingResult) {
        if (bindingResult.hasErrors()) {
            return HttpResponse.invalidInputData();
        }

        DiscountProgram createdDiscountProgram = discountProgramService.createDiscountProgram(discountProgram);
        return HttpResponse.ok("create discount program successfully", createdDiscountProgram);
    }

    @PutMapping("/{id}")
    public ResponseEntity<?> updateDiscountProgram(
            @PathVariable Long id,
            @RequestBody DiscountProgram discountProgram,
            BindingResult bindingResult) {
        if (bindingResult.hasErrors()) {
            return HttpResponse.invalidInputData();
        }

        discountProgramService.updateDiscountProgram(id, discountProgram);
        return HttpResponse.ok("update discount program successfully");
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<?> deleteDiscountProgram(@PathVariable Long id) {
        discountProgramService.deleteDiscountProgram(id);
        return HttpResponse.ok("delete discount program successfully");
    }
}
