package csc13001.plantpos.discount;

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
        return HttpResponse.ok("Get discount programs successfully", discountPrograms);
    }

    @GetMapping("/customer/{phone}")
    public ResponseEntity<?> getDiscountProgramsByCustomerPhone(@PathVariable String phone) {
        List<DiscountProgram> discountPrograms = discountProgramService.getDiscountProgramsByCustomerPhone(phone);
        return HttpResponse.ok("Get discount programs by customer phone successfully", discountPrograms);
    }

    @PostMapping
    public ResponseEntity<?> createDiscountProgram(
            @RequestBody DiscountProgram discountProgram,
            BindingResult bindingResult) {
        if (bindingResult.hasErrors()) {
            return HttpResponse.badRequest(bindingResult);
        }

        DiscountProgram createdDiscountProgram = discountProgramService.createDiscountProgram(discountProgram);
        return HttpResponse.ok("Create discount program successfully", createdDiscountProgram);
    }

    @PutMapping("/{id}")
    public ResponseEntity<?> updateDiscountProgram(
            @PathVariable Long id,
            @RequestBody DiscountProgram discountProgram,
            BindingResult bindingResult) {
        if (bindingResult.hasErrors()) {
            return HttpResponse.badRequest(bindingResult);
        }

        discountProgramService.updateDiscountProgram(id, discountProgram);
        return HttpResponse.ok("Update discount program successfully");
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<?> deleteDiscountProgram(@PathVariable Long id) {
        discountProgramService.deleteDiscountProgram(id);
        return HttpResponse.ok("Delete discount program successfully");
    }
}
