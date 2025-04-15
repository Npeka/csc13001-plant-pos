package csc13001.plantpos.staff;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.validation.BindingResult;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestPart;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.multipart.MultipartFile;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.JsonMappingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.datatype.jsr310.JavaTimeModule;

import csc13001.plantpos.staff.dtos.StaffDTO;
import csc13001.plantpos.user.User;
import csc13001.plantpos.utils.http.HttpResponse;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.PostMapping;

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
    public ResponseEntity<?> getStaffById(@PathVariable Long staffId) {
        StaffDTO staffDTO = staffService.getStaffById(staffId);
        return HttpResponse.ok("Lấy thông tin nhân viên thành công", staffDTO);
    }

    @PostMapping(consumes = { MediaType.MULTIPART_FORM_DATA_VALUE })
    public ResponseEntity<?> createStaff(
            @RequestPart("staff") String staffJson,
            @RequestPart(value = "image", required = false) MultipartFile image,
            BindingResult bindingResult) throws JsonMappingException, JsonProcessingException {
        if (bindingResult.hasErrors()) {
            return HttpResponse.badRequest(bindingResult);
        }

        ObjectMapper objectMapper = new ObjectMapper();
        objectMapper.registerModule(new JavaTimeModule());
        User staff = objectMapper.readValue(staffJson, User.class);
        staffService.createStaff(staff, image);

        return HttpResponse.ok("Thêm nhân viên thành công");
    }

    @PutMapping(path = "/{staffId}", consumes = { MediaType.MULTIPART_FORM_DATA_VALUE })
    public ResponseEntity<?> updateStaff(
            @PathVariable Long staffId,
            @RequestPart("staff") String staffJson,
            @RequestPart(value = "image", required = false) MultipartFile image,
            BindingResult bindingResult) throws JsonMappingException, JsonProcessingException {
        if (bindingResult.hasErrors()) {
            return HttpResponse.badRequest(bindingResult);
        }

        ObjectMapper objectMapper = new ObjectMapper();
        objectMapper.registerModule(new JavaTimeModule());
        User staff = objectMapper.readValue(staffJson, User.class);

        staff.setUserId(staffId);
        staffService.updateStaff(staff, image);

        return HttpResponse.ok("Cập nhật thông tin nhân viên thành công");
    }
}
