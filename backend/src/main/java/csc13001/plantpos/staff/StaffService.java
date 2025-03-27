package csc13001.plantpos.staff;

import java.math.BigDecimal;
import java.util.List;

import org.springframework.stereotype.Service;

import csc13001.plantpos.order.OrderRepository;
import csc13001.plantpos.order.models.Order;
import csc13001.plantpos.staff.dtos.StaffDTO;
import csc13001.plantpos.staff.exception.StaffException;
import csc13001.plantpos.user.User;
import csc13001.plantpos.user.UserRepository;
import lombok.RequiredArgsConstructor;

@Service
@RequiredArgsConstructor
public class StaffService {
    private final UserRepository userRepository;
    private final OrderRepository orderRepository;

    public List<User> getAllStaff() {
        return userRepository.findByIsAdmin(false).get();
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

    public void updateStaff(User staff) {
        User user = userRepository.findById(staff.getUserId())
                .orElseThrow(StaffException.StaffNotFoundException::new);

        user.setFullname(staff.getFullname());
        user.setEmail(staff.getEmail());
        user.setPhone(staff.getPhone());
        user.setStartDate(staff.getStartDate());
        user.setStatus(staff.getStatus());
        user.setGender(staff.getGender());

        userRepository.save(user);
    }
}
