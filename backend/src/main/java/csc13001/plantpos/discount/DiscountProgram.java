package csc13001.plantpos.discount;

import com.fasterxml.jackson.annotation.JsonAutoDetect;
import com.fasterxml.jackson.annotation.JsonFormat;
import com.fasterxml.jackson.annotation.JsonInclude;

import csc13001.plantpos.customer.CustomerType;
import jakarta.persistence.*;
import jakarta.validation.constraints.Max;
import jakarta.validation.constraints.Min;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.time.LocalDate;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
@JsonAutoDetect(fieldVisibility = JsonAutoDetect.Visibility.ANY)
@JsonInclude(JsonInclude.Include.NON_NULL)
@Entity
@Table(name = "discount_programs")
public class DiscountProgram {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "discount_id")
    private Long discountId;

    @NotBlank(message = "Tên chương trình khuyến mãi là bắt buộc")
    @Column(name = "name")
    private String name;

    @NotNull(message = "Tỷ lệ giảm giá là bắt buộc")
    @Min(value = 0, message = "Tỷ lệ giảm giá không được nhỏ hơn 0")
    @Max(value = 100, message = "Tỷ lệ giảm giá không được lớn hơn 100")
    @Column(name = "discount_rate")
    private Double discountRate;

    @NotNull(message = "Ngày bắt đầu là bắt buộc")
    @JsonFormat(shape = JsonFormat.Shape.STRING)
    @Column(name = "start_date", columnDefinition = "DATE")
    private LocalDate startDate;

    @NotNull(message = "Ngày kết thúc là bắt buộc")
    @JsonFormat(shape = JsonFormat.Shape.STRING)
    @Column(name = "end_date", columnDefinition = "DATE")
    private LocalDate endDate;

    @Column(name = "applicable_customer_type")
    private CustomerType applicableCustomerType;

    public boolean isActive() {
        LocalDate today = LocalDate.now();
        return (startDate == null || !startDate.isAfter(today)) && (endDate == null || !endDate.isBefore(today));
    }

    public boolean isApplicableToCustomerType(CustomerType customerType) {
        return applicableCustomerType == null || applicableCustomerType == customerType;
    }
}