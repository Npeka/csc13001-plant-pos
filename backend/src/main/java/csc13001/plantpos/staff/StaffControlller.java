package csc13001.plantpos.staff;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.validation.BindingResult;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import csc13001.plantpos.staff.dtos.StaffDTO;
import csc13001.plantpos.user.User;
import csc13001.plantpos.utils.http.HttpResponse;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;

@RestController
@RequestMapping("/api/staff")
public class StaffControlller {
    @Autowired
    StaffService staffService;

    @GetMapping
    public ResponseEntity<?> getAllStaff() {
        return HttpResponse.ok("Lấy danh sách nhân viên thành công", staffService.getAllStaff());
    }

    @GetMapping("/{staffId}")
    public ResponseEntity<?> getAllProducts(@PathVariable Long staffId) {
        StaffDTO staffDTO = staffService.getStaffById(staffId);
        return HttpResponse.ok("Lấy thông tin nhân viên thành công", staffDTO);
    }

    @PutMapping("/{staffId}")
    public ResponseEntity<?> putMethodName(
            @PathVariable Long staffId,
            @RequestBody User staff, BindingResult bindingResult) {
        if (bindingResult.hasErrors()) {
            return HttpResponse.badRequest(bindingResult);
        }

        staff.setUserId(staffId);
        staffService.updateStaff(staff);
        return HttpResponse.ok("Cập nhật thông tin nhân viên thành công");
    }
}
