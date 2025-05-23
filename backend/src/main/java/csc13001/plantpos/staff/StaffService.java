package csc13001.plantpos.staff;

import csc13001.plantpos.authentication.AuthService;
import csc13001.plantpos.global.MinIOService;
import csc13001.plantpos.global.enums.MinioBucket;
import csc13001.plantpos.order.OrderRepository;
import csc13001.plantpos.order.models.Order;
import csc13001.plantpos.staff.dtos.StaffDTO;
import csc13001.plantpos.staff.exception.StaffException;
import csc13001.plantpos.user.User;
import csc13001.plantpos.user.UserRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.multipart.MultipartFile;

import java.math.BigDecimal;
import java.util.List;

@Service
@RequiredArgsConstructor
public class StaffService {
    private final AuthService authService;
    private final UserRepository userRepository;
    private final OrderRepository orderRepository;
    private final MinIOService minIOService;

    public List<User> getAllStaff() {
        return userRepository.findAllByIsAdmin(false);
    }

    public StaffDTO getStaffById(Long staffId) {
        User user = userRepository.findById(staffId)
                .orElseThrow(StaffException.StaffNotFoundException::new);

        List<Order> orders = orderRepository.findByStaff_UserId(staffId);
        int totalOrders = orders.size();
        BigDecimal totalRevenue = orders.stream()
                .map(Order::getFinalPrice)
                .reduce(BigDecimal.ZERO, BigDecimal::add);

        StaffDTO staffDTO = new StaffDTO(user, totalOrders, totalRevenue);
        return staffDTO;
    }

    @Transactional
    public void createStaff(User staff, MultipartFile image) {
        User user = authService.register(staff);
        if (image != null) {
            String imageurl = minIOService.uploadFile(image, MinioBucket.STAFF);
            user.setImageUrl(imageurl);
        }

        userRepository.save(user);
    }

    public void updateStaff(User staff, MultipartFile image) {
        User user = userRepository.findById(staff.getUserId())
                .orElseThrow(StaffException.StaffNotFoundException::new);

        user.setFullname(staff.getFullname());
        if (staff.getEmail() != null && !user.getEmail().equals(staff.getEmail())
                && userRepository.existsByEmail(staff.getEmail())) {
            throw new RuntimeException("Email đã tồn tại trong hệ thống");
        }
        if (staff.getPhone() != null && !user.getPhone().equals(staff.getPhone())
                && userRepository.existsByPhone(staff.getPhone())) {
            throw new RuntimeException("Số điện thoại đã tồn tại trong hệ thống");
        }
        user.setEmail(staff.getEmail());
        user.setPhone(staff.getPhone());
        user.setStartDate(staff.getStartDate());
        user.setStatus(staff.getStatus());
        user.setGender(staff.getGender());
        user.setCanManageDiscounts(staff.isCanManageDiscounts());
        user.setCanManageInventory(staff.isCanManageInventory());
        System.out.println(image);
        if (image != null) {
            if (user.getImageUrl() != null) {
                minIOService.deleteFile(user.getImageUrl());
            }
            String imageurl = minIOService.uploadFile(image, MinioBucket.STAFF);
            user.setImageUrl(imageurl);
        }

        userRepository.save(user);
    }
}
