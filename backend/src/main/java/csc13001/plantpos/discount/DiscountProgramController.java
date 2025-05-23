package csc13001.plantpos.discount;

import csc13001.plantpos.utils.http.HttpResponse;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.validation.BindingResult;
import org.springframework.validation.annotation.Validated;
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
        return HttpResponse.ok("Lấy danh sách chương trình giảm giá thành công", discountPrograms);
    }

    @GetMapping("/customer/{phone}")
    public ResponseEntity<?> getDiscountProgramsByCustomerPhone(@PathVariable String phone) {
        List<DiscountProgram> discountPrograms = discountProgramService.getDiscountProgramsByCustomerPhone(phone);
        return HttpResponse.ok("Lấy danh sách chương trình giảm giá theo số điện thoại khách hàng thành công",
                discountPrograms);
    }

    @PostMapping
    public ResponseEntity<?> createDiscountProgram(
            @RequestBody DiscountProgram discountProgram,
            BindingResult bindingResult) {
        if (bindingResult.hasErrors()) {
            return HttpResponse.badRequest(bindingResult);
        }

        DiscountProgram createdDiscountProgram = discountProgramService.createDiscountProgram(discountProgram);
        return HttpResponse.ok("Tạo chương trình giảm giá thành công", createdDiscountProgram);
    }

    @PutMapping("/{id}")
    public ResponseEntity<?> updateDiscountProgram(
            @PathVariable Long id,
            @RequestBody @Validated DiscountProgram discountProgram,
            BindingResult bindingResult) {
        if (bindingResult.hasErrors()) {
            return HttpResponse.badRequest(bindingResult);
        }
        discountProgram.setDiscountId(id);
        discountProgramService.updateDiscountProgram(discountProgram);
        return HttpResponse.ok("Cập nhật chương trình giảm giá thành công");
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<?> deleteDiscountProgram(@PathVariable Long id) {
        discountProgramService.deleteDiscountProgram(id);
        return HttpResponse.ok("Xóa chương trình giảm giá thành công");
    }
}
