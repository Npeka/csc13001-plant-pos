package csc13001.plantpos.event_listeners;

//import csc13001.plantpos.application.services.EmailService;
//import csc13001.plantpos.application.services.StaffService;

import org.springframework.stereotype.Component;

@Component
public class OrderCreatedListener {
    // private final EmailService emailService;
    // private final StaffService staffService;

    // public OrderCreatedListener(EmailService emailService, StaffService
    // staffService) {
    // this.emailService = emailService;
    // this.staffService = staffService;
    // }
    //
    // @EventListener
    // public void handleOrderCreated(OrderCreatedEvent event) {
    // // Cập nhật doanh số của nhân viên bán hàng
    // staffService.updateSales(event.getStaffId());
    //
    // // Gửi email thông báo
    // emailService.sendEmail("admin@plantpos.com", "New Order Created", "Order ID:
    // " + event.getOrderId());
    // }
}
